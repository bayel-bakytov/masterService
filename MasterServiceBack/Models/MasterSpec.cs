using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServiceBack.Models;

public class MasterSpec
{
    [Key]
    public int Id { get; set; }
    public double? Sum { get; set; }
    public int? AppId { get; set; }
    public int? MasterId { get; set; }
    [NotMapped]
    public Client? Master { get; set; }
}