using Electrovia_Core.Entities;
using Electrovia_Repository;

namespace Electrovia_Core.Specifications
{
    public class Product_with_Filtration_with_count : BaseSpecification<Products>
    {
        public Product_with_Filtration_with_count(Product_Spec_Parameters product_params) :
             base(P =>
             (string.IsNullOrEmpty(product_params.Search!) || P.Name!.ToLower()!.Contains(product_params.Search!)) &&
             (!product_params.brandid.HasValue            || P.ProductBrandId == product_params.brandid.Value) &&
             (!product_params.typeid.HasValue             || P.ProductTypeId == product_params.typeid.Value))
        {
        }
    }
}
