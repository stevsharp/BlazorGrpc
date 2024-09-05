using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlazorGrpc.Model;


public class Product
{
    public int Id { get; set; }

    [Required]
    public virtual ProductName Name { get; set; }

    [Required]
    public virtual Price Price { get; set; }

    [JsonConstructor]
    private Product(){}

    public static class ProductFactory
    {
        public static Product CreateProduct()
        {
            return new Product(0, string.Empty,0m);
        }

        public static Product CreateProduct(string productName, decimal price)
        {
            // Add any validation or creation logic here
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be empty", nameof(productName));
            }

            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(price));
            }

            return new Product(0,productName, price);
        }

        public static Product Update(int id , string productName, decimal price)
        {
            // Add any validation or creation logic here
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Product name cannot be empty", nameof(productName));
            }

            if (price < 0)
            {
                throw new ArgumentException("Price cannot be negative", nameof(price));
            }

            return new Product(id, productName, price);
        }
    }

    internal Product(int id , string productName, decimal price)
    {
        Id = id;

        Name = new ProductName { Value = productName };

        Price = new Price { Value = price };
    }
}

public record ProductName
{
    public string Value { get; set; }
}


public record Price
{
    public decimal Value { get; set; }
}