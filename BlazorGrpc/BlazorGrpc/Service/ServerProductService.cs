using BlazorGrpc.gRPC;
using BlazorGrpc.Model;

using Grpc.Core;
using Microsoft.EntityFrameworkCore;


namespace BlazorGrpc.Service;

public class ServerProductService : ProductService.ProductServiceBase
{
    private readonly ProductContext _context;
    public ServerProductService(ProductContext context)
    {
        _context = context;
    }

    public override async Task<ProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Id);
        
        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var product = new Product
        {
            Name = request.Name,
            Price = (decimal)request.Price
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        product.Name = request.Name;
        product.Price = (decimal)request.Price;

        await _context.SaveChangesAsync();

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = (float)product.Price
        };
    }

    public override async Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
    {
        var product = await _context.Products.FindAsync(request.Id);
        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return new Empty();
    }

    public override async Task<ProductListResponse> ListProducts(Empty request, ServerCallContext context)
    {
        var products = await _context.Products.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            Price = (float)p.Price
        }).ToListAsync();

        var response = new ProductListResponse();
        response.Products.AddRange(products);

        return response;
    }
}
