using Electrovia_Core.Entities;
using Electrovia_Repository;

namespace Electrovia_Core.Specifications
{
    public class Product_with_Brand_Type_Specification : BaseSpecification<Products>
    {
        // this Constructor get all products without where 
        public Product_with_Brand_Type_Specification(Product_Spec_Parameters product_params) :
        base(P =>
            (string.IsNullOrEmpty(product_params.Search!) || P.Name!.ToLower()!.Contains(product_params.Search!)) &&
            (!product_params.brandid.HasValue            || P.ProductBrandId == product_params.brandid.Value) &&
            (!product_params.typeid.HasValue             || P.ProductTypeId == product_params.typeid.Value))
        {
            include.Add(p => p.ProductBrand!);
            include.Add(p => p.ProductType!);
            if (!string.IsNullOrEmpty(product_params.sort))
            {
                switch (product_params.sort)
                {
                    case "PriceAsc": 
                        Add_OrederBy(P => P.Price);
                        break;

                    case "PriceDesc":
                        Add_OrederByDesc(P => P.Price);
                        break;

                    default:
                        Add_OrederBy(P => P.Name!);
                        break;
                }

            }

            Applypagination(product_params.Pagesize * (product_params.Pageindex-1) , product_params.Pagesize );
        }

        // this Constructor get specific product by id 
        public Product_with_Brand_Type_Specification(int id) : base(p => p.Id == id)
        {
            include.Add(p => p.ProductBrand!);
            include.Add(p => p.ProductType!);
        }
    }
}
