using AutoMapper;
using Electrovia.DTOs;
using Electrovia.Errors;
using Electrovia.Helpers;
using ELECTROVIA_AI;
using Electrovia_Core.Entities;
using Electrovia_Core.interfaces;
using Electrovia_Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Electrovia.Controllers
{
    public class ProductsController : BaseApiController
    {
        #region Constructors 
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        private readonly SearchWithImageService _searchWithImageService;

        public ProductsController(IUnitOFWork unitOFWork, IMapper mapper, SearchWithImageService searchWithImageService)
        {
            _mapper = mapper;
            _unitOFWork = unitOFWork;
            _searchWithImageService = searchWithImageService;

        }

        #endregion

        #region GET_ALLProducts
        [HttpGet]
        [CachedAttribute(600)]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] Product_Spec_Parameters product_params)
        {
            var products = await _unitOFWork.Repository<Products>()!.GetAllAsync(new Product_with_Brand_Type_Specification(product_params));

            var data = _mapper.Map<IReadOnlyList<Products>, IReadOnlyList<ProductDTO>>(products);

            var count = await _unitOFWork.Repository<Products>()!.GetCountAsync(new Product_with_Filtration_with_count(product_params));

            return Ok(new Pagination<ProductDTO>(product_params.Pageindex, product_params.Pagesize, count, data));
        }

        [HttpPost("SearchWithImage")]
        public async Task<ActionResult<string>> SearchWithImage(IFormFile imageFile)
        {

            return await _searchWithImageService.SearchWithImageAsync(imageFile);
        }

        #endregion

        #region GET_Product_BY_ID
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIsResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var spec = new Product_with_Brand_Type_Specification(id);
            var product = await _unitOFWork.Repository<Products>()!.GetEntityAsync(spec);
            if (product is null) return NotFound(new APIsResponse(404));
            return Ok(_mapper.Map<Products, ProductDTO>(product));
        }

        #endregion

        #region GET_All Brands
        [HttpGet("brands")] // api/Products/brands
        [CachedAttribute(600)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands = await _unitOFWork.Repository<ProductBrand>()!.GetAllAsync();
            return Ok(brands);
        }
        #endregion

        #region GET_All Types
        [HttpGet("types")] // api/Products/types
        [CachedAttribute(600)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAlltypes()
        {
            var types = await _unitOFWork.Repository<ProductType>()!.GetAllAsync();
            return Ok(types);
        }
        #endregion
    }
}
