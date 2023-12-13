﻿using System.ComponentModel.DataAnnotations;

namespace MasterServiceBack.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Inn { get; set; }
    public string? Password { get; set; }
}