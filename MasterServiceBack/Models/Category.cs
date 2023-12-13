
using System.ComponentModel.DataAnnotations;

namespace MasterServiceBack.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<Application>? Applications { get; set; }
}