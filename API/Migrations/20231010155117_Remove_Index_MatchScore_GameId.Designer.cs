﻿// <auto-generated />
using System;
using API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(OtrContext))]
    [Migration("20231010155117_Remove_Index_MatchScore_GameId")]
    partial class Remove_Index_MatchScore_GameId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.Beatmap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double?>("AimDiff")
                        .HasColumnType("double precision")
                        .HasColumnName("aim_diff");

                    b.Property<double>("Ar")
                        .HasColumnType("double precision")
                        .HasColumnName("ar");

                    b.Property<string>("Artist")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("artist");

                    b.Property<long>("BeatmapId")
                        .HasColumnType("bigint")
                        .HasColumnName("beatmap_id");

                    b.Property<double?>("Bpm")
                        .HasColumnType("double precision")
                        .HasColumnName("bpm");

                    b.Property<int>("CircleCount")
                        .HasColumnType("integer")
                        .HasColumnName("circle_count");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<double>("Cs")
                        .HasColumnType("double precision")
                        .HasColumnName("cs");

                    b.Property<string>("DiffName")
                        .HasColumnType("text")
                        .HasColumnName("diff_name");

                    b.Property<double>("DrainTime")
                        .HasColumnType("double precision")
                        .HasColumnName("drain_time");

                    b.Property<int>("GameMode")
                        .HasColumnType("integer")
                        .HasColumnName("game_mode");

                    b.Property<double>("Hp")
                        .HasColumnType("double precision")
                        .HasColumnName("hp");

                    b.Property<double>("Length")
                        .HasColumnType("double precision")
                        .HasColumnName("length");

                    b.Property<long>("MapperId")
                        .HasColumnType("bigint")
                        .HasColumnName("mapper_id");

                    b.Property<string>("MapperName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("mapper_name");

                    b.Property<int?>("MaxCombo")
                        .HasColumnType("integer")
                        .HasColumnName("max_combo");

                    b.Property<double>("Od")
                        .HasColumnType("double precision")
                        .HasColumnName("od");

                    b.Property<int>("SliderCount")
                        .HasColumnType("integer")
                        .HasColumnName("slider_count");

                    b.Property<double?>("SpeedDiff")
                        .HasColumnType("double precision")
                        .HasColumnName("speed_diff");

                    b.Property<int>("SpinnerCount")
                        .HasColumnType("integer")
                        .HasColumnName("spinner_count");

                    b.Property<double>("Sr")
                        .HasColumnType("double precision")
                        .HasColumnName("sr");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("beatmaps_pk");

                    b.HasIndex(new[] { "BeatmapId" }, "beatmaps_beatmapid")
                        .IsUnique();

                    b.ToTable("beatmaps");
                });

            modelBuilder.Entity("API.Entities.BeatmapModSr", b =>
                {
                    b.Property<int>("BeatmapId")
                        .HasColumnType("integer")
                        .HasColumnName("beatmap_id");

                    b.Property<int>("Mods")
                        .HasColumnType("integer")
                        .HasColumnName("mods");

                    b.Property<double>("PostModSr")
                        .HasColumnType("double precision")
                        .HasColumnName("post_mod_sr");

                    b.HasKey("BeatmapId", "Mods");

                    b.HasIndex("BeatmapId");

                    b.ToTable("beatmap_mod_sr");
                });

            modelBuilder.Entity("API.Entities.Config", b =>
                {
                    b.Property<DateTime?>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.ToTable("config");
                });

            modelBuilder.Entity("API.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("BeatmapId")
                        .HasColumnType("integer")
                        .HasColumnName("beatmap_id");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<long>("GameId")
                        .HasColumnType("bigint")
                        .HasColumnName("game_id");

                    b.Property<int>("MatchId")
                        .HasColumnType("integer")
                        .HasColumnName("match_id");

                    b.Property<int>("MatchType")
                        .HasColumnType("integer")
                        .HasColumnName("match_type");

                    b.Property<int>("Mods")
                        .HasColumnType("integer")
                        .HasColumnName("mods");

                    b.Property<int>("PlayMode")
                        .HasColumnType("integer")
                        .HasColumnName("play_mode");

                    b.Property<double>("PostModSr")
                        .HasColumnType("double precision")
                        .HasColumnName("post_mod_sr");

                    b.Property<int?>("RejectionReason")
                        .HasColumnType("integer")
                        .HasColumnName("rejection_reason");

                    b.Property<int>("ScoringType")
                        .HasColumnType("integer")
                        .HasColumnName("scoring_type");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<int>("TeamType")
                        .HasColumnType("integer")
                        .HasColumnName("team_type");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<int?>("VerificationStatus")
                        .HasColumnType("integer")
                        .HasColumnName("verification_status");

                    b.HasKey("Id")
                        .HasName("osugames_pk");

                    b.HasIndex("BeatmapId");

                    b.HasIndex("GameId");

                    b.HasIndex("MatchId");

                    b.HasIndex(new[] { "GameId" }, "osugames_gameid")
                        .IsUnique();

                    b.ToTable("games");
                });

            modelBuilder.Entity("API.Entities.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Abbreviation")
                        .HasColumnType("text")
                        .HasColumnName("abbreviation");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<string>("Forum")
                        .HasColumnType("text")
                        .HasColumnName("forum");

                    b.Property<bool?>("IsApiProcessed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_api_processed");

                    b.Property<long>("MatchId")
                        .HasColumnType("bigint")
                        .HasColumnName("match_id");

                    b.Property<int?>("Mode")
                        .HasColumnType("integer")
                        .HasColumnName("mode");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<bool?>("NeedsAutoCheck")
                        .HasColumnType("boolean")
                        .HasColumnName("needs_auto_check");

                    b.Property<int?>("RankRangeLowerBound")
                        .HasColumnType("integer")
                        .HasColumnName("rank_range_lower_bound");

                    b.Property<DateTime?>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<int?>("SubmitterUserId")
                        .HasColumnType("integer")
                        .HasColumnName("submitted_by_user");

                    b.Property<int?>("TeamSize")
                        .HasColumnType("integer")
                        .HasColumnName("team_size");

                    b.Property<string>("TournamentName")
                        .HasColumnType("text")
                        .HasColumnName("tournament_name");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<string>("VerificationInfo")
                        .HasColumnType("text")
                        .HasColumnName("verification_info");

                    b.Property<int?>("VerificationSource")
                        .HasColumnType("integer")
                        .HasColumnName("verification_source");

                    b.Property<int?>("VerificationStatus")
                        .HasColumnType("integer")
                        .HasColumnName("verification_status");

                    b.Property<int?>("VerifierUserId")
                        .HasColumnType("integer")
                        .HasColumnName("verified_by_user");

                    b.HasKey("Id")
                        .HasName("matches_pk");

                    b.HasIndex("MatchId")
                        .IsUnique();

                    b.HasIndex("SubmitterUserId");

                    b.HasIndex("VerifierUserId");

                    b.HasIndex(new[] { "MatchId" }, "osumatches_matchid")
                        .IsUnique();

                    b.ToTable("matches");
                });

            modelBuilder.Entity("API.Entities.MatchScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Count100")
                        .HasColumnType("integer")
                        .HasColumnName("count_100");

                    b.Property<int>("Count300")
                        .HasColumnType("integer")
                        .HasColumnName("count_300");

                    b.Property<int>("Count50")
                        .HasColumnType("integer")
                        .HasColumnName("count_50");

                    b.Property<int>("CountGeki")
                        .HasColumnType("integer")
                        .HasColumnName("count_geki");

                    b.Property<int>("CountKatu")
                        .HasColumnType("integer")
                        .HasColumnName("count_katu");

                    b.Property<int>("CountMiss")
                        .HasColumnType("integer")
                        .HasColumnName("count_miss");

                    b.Property<int?>("EnabledMods")
                        .HasColumnType("integer")
                        .HasColumnName("enabled_mods");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<bool?>("IsValid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_valid");

                    b.Property<int>("MaxCombo")
                        .HasColumnType("integer")
                        .HasColumnName("max_combo");

                    b.Property<bool>("Pass")
                        .HasColumnType("boolean")
                        .HasColumnName("pass");

                    b.Property<bool>("Perfect")
                        .HasColumnType("boolean")
                        .HasColumnName("perfect");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    b.Property<long>("Score")
                        .HasColumnType("bigint")
                        .HasColumnName("score");

                    b.Property<int>("Team")
                        .HasColumnType("integer")
                        .HasColumnName("team");

                    b.HasKey("Id")
                        .HasName("match_scores_pk");

                    b.HasIndex("PlayerId");

                    b.HasIndex(new[] { "GameId", "PlayerId" }, "match_scores_gameid_playerid")
                        .IsUnique();

                    b.ToTable("match_scores");
                });

            modelBuilder.Entity("API.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .HasColumnType("text")
                        .HasColumnName("country");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int?>("EarliestCatchGlobalRank")
                        .HasColumnType("integer")
                        .HasColumnName("earliest_catch_global_rank");

                    b.Property<DateTime?>("EarliestCatchGlobalRankDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("earliest_catch_global_rank_date");

                    b.Property<int?>("EarliestManiaGlobalRank")
                        .HasColumnType("integer")
                        .HasColumnName("earliest_mania_global_rank");

                    b.Property<DateTime?>("EarliestManiaGlobalRankDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("earliest_mania_global_rank_date");

                    b.Property<int?>("EarliestOsuGlobalRank")
                        .HasColumnType("integer")
                        .HasColumnName("earliest_osu_global_rank");

                    b.Property<DateTime?>("EarliestOsuGlobalRankDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("earliest_osu_global_rank_date");

                    b.Property<int?>("EarliestTaikoGlobalRank")
                        .HasColumnType("integer")
                        .HasColumnName("earliest_taiko_global_rank");

                    b.Property<DateTime?>("EarliestTaikoGlobalRankDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("earliest_taiko_global_rank_date");

                    b.Property<long>("OsuId")
                        .HasColumnType("bigint")
                        .HasColumnName("osu_id");

                    b.Property<int?>("RankCatch")
                        .HasColumnType("integer")
                        .HasColumnName("rank_catch");

                    b.Property<int?>("RankMania")
                        .HasColumnType("integer")
                        .HasColumnName("rank_mania");

                    b.Property<int?>("RankStandard")
                        .HasColumnType("integer")
                        .HasColumnName("rank_standard");

                    b.Property<int?>("RankTaiko")
                        .HasColumnType("integer")
                        .HasColumnName("rank_taiko");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("Player_pk");

                    b.HasIndex("OsuId");

                    b.HasIndex(new[] { "OsuId" }, "Players_osuid")
                        .IsUnique();

                    b.ToTable("players");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("Mode")
                        .HasColumnType("integer")
                        .HasColumnName("mode");

                    b.Property<double>("Mu")
                        .HasColumnType("double precision")
                        .HasColumnName("mu");

                    b.Property<double>("MuInitial")
                        .HasColumnType("double precision")
                        .HasColumnName("mu_initial");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    b.Property<double>("Sigma")
                        .HasColumnType("double precision")
                        .HasColumnName("sigma");

                    b.Property<double>("SigmaInitial")
                        .HasColumnType("double precision")
                        .HasColumnName("sigma_initial");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("Ratings_pk");

                    b.HasIndex("PlayerId");

                    b.HasIndex(new[] { "PlayerId", "Mode" }, "ratings_playerid_mode")
                        .IsUnique();

                    b.ToTable("ratings");
                });

            modelBuilder.Entity("API.Entities.RatingHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityAlwaysColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("MatchId")
                        .HasColumnType("integer")
                        .HasColumnName("match_id");

                    b.Property<int>("Mode")
                        .HasColumnType("integer")
                        .HasColumnName("mode");

                    b.Property<double>("Mu")
                        .HasColumnType("double precision")
                        .HasColumnName("mu");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    b.Property<double>("Sigma")
                        .HasColumnType("double precision")
                        .HasColumnName("sigma");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("RatingHistories_pk");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.HasIndex(new[] { "PlayerId", "MatchId" }, "ratinghistories_pk")
                        .IsUnique();

                    b.ToTable("ratinghistories");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_login");

                    b.Property<int?>("PlayerId")
                        .HasColumnType("integer")
                        .HasColumnName("player_id");

                    b.Property<string[]>("Roles")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("roles");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated");

                    b.HasKey("Id")
                        .HasName("User_pk");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("API.Entities.BeatmapModSr", b =>
                {
                    b.HasOne("API.Entities.Beatmap", null)
                        .WithMany("BeatmapModSrs")
                        .HasForeignKey("BeatmapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.Game", b =>
                {
                    b.HasOne("API.Entities.Beatmap", "Beatmap")
                        .WithMany("Games")
                        .HasForeignKey("BeatmapId")
                        .HasConstraintName("games_beatmaps_id_fk");

                    b.HasOne("API.Entities.Match", "Match")
                        .WithMany("Games")
                        .HasForeignKey("MatchId")
                        .IsRequired()
                        .HasConstraintName("games_matches_id_fk");

                    b.Navigation("Beatmap");

                    b.Navigation("Match");
                });

            modelBuilder.Entity("API.Entities.Match", b =>
                {
                    b.HasOne("API.Entities.User", "SubmittedBy")
                        .WithMany("SubmittedMatches")
                        .HasForeignKey("SubmitterUserId");

                    b.HasOne("API.Entities.User", "VerifiedBy")
                        .WithMany("VerifiedMatches")
                        .HasForeignKey("VerifierUserId");

                    b.Navigation("SubmittedBy");

                    b.Navigation("VerifiedBy");
                });

            modelBuilder.Entity("API.Entities.MatchScore", b =>
                {
                    b.HasOne("API.Entities.Game", "Game")
                        .WithMany("MatchScores")
                        .HasForeignKey("GameId")
                        .IsRequired()
                        .HasConstraintName("match_scores_games_id_fk");

                    b.HasOne("API.Entities.Player", "Player")
                        .WithMany("MatchScores")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("match_scores_players_id_fk");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("API.Entities.Rating", b =>
                {
                    b.HasOne("API.Entities.Player", "Player")
                        .WithMany("Ratings")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("Ratings___fkplayerid");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("API.Entities.RatingHistory", b =>
                {
                    b.HasOne("API.Entities.Match", "Match")
                        .WithMany("RatingHistories")
                        .HasForeignKey("MatchId")
                        .IsRequired()
                        .HasConstraintName("ratinghistories_matches_id_fk");

                    b.HasOne("API.Entities.Player", "Player")
                        .WithMany("RatingHistories")
                        .HasForeignKey("PlayerId")
                        .IsRequired()
                        .HasConstraintName("RatingHistories___fkplayerid");

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.HasOne("API.Entities.Player", "Player")
                        .WithOne("User")
                        .HasForeignKey("API.Entities.User", "PlayerId")
                        .HasConstraintName("Users___fkplayerid");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("API.Entities.Beatmap", b =>
                {
                    b.Navigation("BeatmapModSrs");

                    b.Navigation("Games");
                });

            modelBuilder.Entity("API.Entities.Game", b =>
                {
                    b.Navigation("MatchScores");
                });

            modelBuilder.Entity("API.Entities.Match", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("RatingHistories");
                });

            modelBuilder.Entity("API.Entities.Player", b =>
                {
                    b.Navigation("MatchScores");

                    b.Navigation("RatingHistories");

                    b.Navigation("Ratings");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Entities.User", b =>
                {
                    b.Navigation("SubmittedMatches");

                    b.Navigation("VerifiedMatches");
                });
#pragma warning restore 612, 618
        }
    }
}
