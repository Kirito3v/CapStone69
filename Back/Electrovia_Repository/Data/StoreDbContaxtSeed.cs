using System.Text.Json;
using Electrovia_Core.Entities;
using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia_Repository.Data
{
    public static class StoreDbContaxtSeed
    {
        public static async Task SeedAsync(StoreDbContext context)
        {
            if (!context.ProductBrand.Any())
            {
                var brandsData = File.ReadAllText("../Electrovia_Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands is not null && brands.Count > 0)
                {
                    foreach (var brand in brands)
                        await context.Set<ProductBrand>().AddAsync(brand);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.ProductType.Any())
            {
                var typesData = File.ReadAllText("../Electrovia_Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types is not null && types.Count > 0)
                {
                    foreach (var type in types)
                        await context.Set<ProductType>().AddAsync(type);
                    await context.SaveChangesAsync();
                }
            }
            
            if (!context.Products.Any())
            {
                var produtsData = File.ReadAllText("../Electrovia_Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Products>>(produtsData);
                if (products is not null && products.Count > 0)
                {
                    foreach (var product in products)
                        await context.Set<Products>().AddAsync(product);
                    await context.SaveChangesAsync();
                }
            }

            if (!context.DeliveryMethods.Any())
            {
                var deliverymethod = File.ReadAllText("../Electrovia_Repository/Data/DataSeed/delivery.json");
                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverymethod);
                if (methods is not null && methods.Count > 0)
                {
                    foreach (var method in methods)
                        await context.Set<DeliveryMethod>().AddAsync(method);
                    await context.SaveChangesAsync();
                }
            }
            
        }
    }
}
