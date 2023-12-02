using Core.Entities;

namespace Core.Specifications
{
    public class ProductFilterCountSpecification : BaseSpecification<Product>
    {
        public ProductFilterCountSpecification(ProductSpecParams productSpecParams)
            : base(x => (!productSpecParams.BrandId.HasValue || x.ProductBrandId == productSpecParams.BrandId) && (!productSpecParams.TypeId.HasValue || x.ProductTypeId == productSpecParams.TypeId))
        {
        }
    }
}