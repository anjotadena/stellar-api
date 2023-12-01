using Core.Entities;

namespace Core.Specifications;
public class ProductsSpecification : BaseSpecification<Product>
{
    public ProductsSpecification(string sort, int? brandId, int? typeId)
        : base(x => (!brandId.HasValue || x.ProductBrandId == brandId.Value) && (!typeId.HasValue || x.ProductTypeId == typeId.Value))
    {
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);
        AddOrderBy(x => x.Name);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;   
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }

    public ProductsSpecification(int id) : base(x => x.Id == id)
    {
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);
    }
}