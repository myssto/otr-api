﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("player_id")]
    public int PlayerId { get; set; }

    [Column("last_login", TypeName = "timestamp without time zone")]
    public DateTime? LastLogin { get; set; }

    [Column("created", TypeName = "timestamp without time zone")]
    public DateTime Created { get; set; }

    /// <summary>
    /// Comma-delimited list of roles (e.g. user, admin, etc.)
    /// </summary>
    [Column("roles")]
    public string? Roles { get; set; }

    [Column("session_token")]
    public string? SessionToken { get; set; }

    [Column("updated", TypeName = "timestamp without time zone")]
    public DateTime? Updated { get; set; }

    [Column("session_expiration", TypeName = "timestamp without time zone")]
    public DateTime? SessionExpiration { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("Users")]
    public virtual Player Player { get; set; } = null!;
}
