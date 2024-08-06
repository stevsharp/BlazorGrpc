using System.ComponentModel.DataAnnotations;

namespace BlazorGrpc.Model;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }
}
