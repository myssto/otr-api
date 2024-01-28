using API.DTOs;
using API.Entities;
using API.Enums;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;

namespace API.Services.Implementations;

public class MatchesService : IMatchesService
{
	private readonly ILogger<MatchesService> _logger;
	private readonly IMatchesRepository _matchesRepository;
	private readonly ITournamentsRepository _tournamentsRepository;
	private readonly IMatchDuplicateXRefRepository _duplicateXRefRepository;
	private readonly IMapper _mapper;

	public MatchesService(ILogger<MatchesService> logger, IMatchesRepository matchesRepository,
		ITournamentsRepository tournamentsRepository, IMatchDuplicateXRefRepository duplicateXRefRepository, IMapper mapper)
	{
		_logger = logger;
		_matchesRepository = matchesRepository;
		_tournamentsRepository = tournamentsRepository;
		_duplicateXRefRepository = duplicateXRefRepository;
		_mapper = mapper;
	}

	public async Task<MatchDTO?> GetAsync(int id, bool filterInvalid = true) => _mapper.Map<MatchDTO?>(await _matchesRepository.GetAsync(id, filterInvalid));

	public async Task<IEnumerable<MatchDTO>> GetAllForPlayerAsync(long osuPlayerId, int mode, DateTime start, DateTime end)
	{
		var matches = await _matchesRepository.GetPlayerMatchesAsync(osuPlayerId, mode, start, end);
		return _mapper.Map<IEnumerable<MatchDTO>>(matches);
	}

	public async Task BatchInsertOrUpdateAsync(MatchWebSubmissionDTO matchWebSubmissionDto, bool verified, int? verifier)
	{
		var existingMatches = (await _matchesRepository.GetByMatchIdsAsync(matchWebSubmissionDto.Ids)).ToList();
		var tournament = await _tournamentsRepository.CreateOrUpdateAsync(matchWebSubmissionDto, verified);

		// Update the matches that already exist, if we are verified
		if (verified)
		{
			foreach (var match in existingMatches)
			{
				match.NeedsAutoCheck = true;
				match.IsApiProcessed = false;
				match.VerificationStatus = (int)MatchVerificationStatus.Verified;
				match.VerificationSource = verifier;
				match.VerifierUserId = matchWebSubmissionDto.SubmitterId;
				match.TournamentId = tournament.Id;
				match.SubmitterUserId = matchWebSubmissionDto.SubmitterId;

				await _matchesRepository.UpdateAsync(match);
			}
		}

		// Matches that don't exist yet
		var newMatchIds = matchWebSubmissionDto.Ids.Except(existingMatches.Select(x => x.MatchId)).ToList();
		var verificationStatus = verified ? MatchVerificationStatus.Verified : MatchVerificationStatus.PendingVerification;

		var newMatches = newMatchIds.Select(id => new Match
		{
			MatchId = id,
			VerificationStatus = (int)verificationStatus,
			SubmitterUserId = matchWebSubmissionDto.SubmitterId,
			NeedsAutoCheck = true,
			IsApiProcessed = false,
			VerificationSource = verifier,
			VerifierUserId = verified ? matchWebSubmissionDto.SubmitterId : null,
			TournamentId = tournament.Id
		});

		int? result = await _matchesRepository.BatchInsertAsync(newMatches);
		if (result > 0)
		{
			_logger.LogInformation("Successfully inserted {Matches} new matches as {Status}", result, verificationStatus);
		}
	}

	public async Task<Dictionary<long, int>> GetIdMappingAsync() => await _matchesRepository.GetIdMappingAsync();
	public async Task<IEnumerable<MatchDTO>> ConvertAsync(IEnumerable<int> ids) => _mapper.Map<IEnumerable<MatchDTO>>(await _matchesRepository.GetAsync(ids, true));

	public async Task VerifyDuplicatesAsync(int verifierUserId, int matchRootId, bool confirmedDuplicate)
	{
		// Mark the items as confirmed / denied duplicates
		await _matchesRepository.VerifyDuplicatesAsync(matchRootId, verifierUserId, confirmedDuplicate);

		// If confirmedDuplicate, trigger the update process.
		if (confirmedDuplicate)
		{
			await _matchesRepository.MergeDuplicatesAsync(matchRootId);
		}
	}

	public async Task<IEnumerable<MatchDuplicateCollectionDTO>> GetAllDuplicatesAsync()
	{
		var collections = new List<MatchDuplicateCollectionDTO>();
		var duplicateGroups = (await _duplicateXRefRepository.GetAllAsync()).GroupBy(x => x.SuspectedDuplicateOf);
		foreach (var dupeGroup in duplicateGroups)
		{
			var root = await GetAsync(dupeGroup.First().SuspectedDuplicateOf);
			var collection = new MatchDuplicateCollectionDTO
			{
				IdRoot = root.Id,
				MatchTitleRoot = root.Name ?? string.Empty,
				OsuMatchIdRoot = root.MatchId,
				SuspectedDuplicates = new List<MatchDuplicateDTO>()
			};

			foreach (var item in dupeGroup)
			{
				if (root == null)
				{
					continue;
				}

				var duplicateMatchData = await GetByOsuIdAsync(item.OsuMatchId);
				if (duplicateMatchData == null)
				{
					continue;
				}

				collection.SuspectedDuplicates.Add(new MatchDuplicateDTO
				{
					MatchTitle = duplicateMatchData.Name ?? string.Empty,
					OsuMatchId = duplicateMatchData.MatchId,
					VerifiedByUsername = item.Verifier?.Player.Username,
					VerifiedAsDuplicate = item.VerifiedAsDuplicate,
					VerifiedByUserId = item.VerifiedBy
				});
			}

			collections.Add(collection);
		}

		return collections;
	}

	public async Task RefreshAutomationChecks(bool invalidOnly = true) => await _matchesRepository.SetRequireAutoCheckAsync(invalidOnly);
	public async Task<IEnumerable<int>> GetAllIdsAsync(bool onlyIncludeFiltered) { return await _matchesRepository.GetAllAsync(onlyIncludeFiltered); }

	public async Task<MatchDTO?> GetByOsuIdAsync(long osuMatchId)
	{
		var match = await _matchesRepository.GetByMatchIdAsync(osuMatchId);
		return _mapper.Map<MatchDTO?>(match);
	}
}