using System.ComponentModel.DataAnnotations;

namespace Lab6.DTOs;

public class AddAnimal
{

    [Required]
    [MaxLength(200)]
    [MinLength(3)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [MaxLength(200)]
    public string? Area { get; set; }
    [MaxLength(200)]
    public string? Category { get; set; }
}