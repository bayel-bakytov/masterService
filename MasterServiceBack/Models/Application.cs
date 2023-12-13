using System.ComponentModel.DataAnnotations;

namespace MasterServiceBack.Models;

public class Application
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DateStart { get; set; } // Используем DateTime?
    public DateTime? DateEnd { get; set; }   // Используем DateTime?
    public string? LinkImg { get; set; }
    public int? CategoryId { get; set; }      // Используем int?
    public int? ClientId { get; set; }        // Используем int?
    public Category? Category { get; set; }
}