namespace BlazorGrpc.Shared;

public record ProductDto
{
    public required int Id { get; set; }

    public required string Name { get; set; }

    public required decimal Price { get; set; }
}
