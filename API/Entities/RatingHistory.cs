﻿using API.Entities.Bases;
using Dapper;

namespace API.Entities;

[Table("ratinghistories")]
public class RatingHistory : EntityBase
{
	[Column("player_id")]
	public int PlayerId { get; set; }
	[Column("mu")]
	public double Mu { get; set; }
	[Column("sigma")]
	public double Sigma { get; set; }
	[Column("match_data_id")]
	public int MatchDataId { get; set; }
	[Column("mode")]
	public string Mode { get; set; } = "Standard"; // TODO: Remove hardcoded mode
}