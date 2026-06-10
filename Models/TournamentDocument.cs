using System.ComponentModel.DataAnnotations;

namespace League_of_Legends_Tournament_Hosting.Models;

public class TournamentDocument
{
    [Key]
    public int Id { get; set; }

    public int TournamentId { get; set; }
    public virtual Tournament Tournament { get; set; } = null!;

    [Required]
    [StringLength(500)]
    public string FileName { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string FilePath { get; set; } = string.Empty;

    [StringLength(200)]
    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
