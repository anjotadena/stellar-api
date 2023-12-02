using Core.Entities;

namespace Core.Specifications;
public class ProductsSpecification : BaseSpecification<Product>
{
    public ProductsSpecification(ProductSpecParams productSpecParams)
        : base(x => (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId) && (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId))
    {
        AddInclude(x => x.ProductBrand);
        AddInclude(x => x.ProductType);
        AddOrderBy(x => x.Name);
        ApplyPaging(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);

        if (!string.IsNullOrEmpty(productSpecParams.Sort))
        {
            switch (productSpecParams.Sort)
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