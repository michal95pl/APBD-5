using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Animal;

public class AnimalDTO
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    
    [StringLength(200)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string Category { get; set; }

    [Required]
    [StringLength(200)]
    public string Area { get; set; }
}