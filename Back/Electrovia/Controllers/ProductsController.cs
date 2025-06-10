using AutoMapper;
using Electrovia.DTOs;
using Electrovia.Errors;
using Electrovia.Helpers;
using Electrovia_Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Electrovia_Core.interfaces;
using Electrovia_Core.Specifications;
using ELECTROVIA_AI;

namespace Electrovia.Controllers
{
    public class ProductsController : BaseApiController
    {
        #region Constructors 
        private readonly IUnitOFWork _unitOFWork;
        private readonly IMapper _mapper;
        private readonly SearchWithImageService _searchWithImageService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IUnitOFWork unitOFWork, IMapper mapper,SearchWithImageService searchWithImageService, ILogger<ProductsController> logger)
        {
            _mapper = mapper;
            _unitOFWork = unitOFWork;
            _searchWithImageService = searchWithImageService;
            _logger = logger;
        }

        #endregion

        #region GET_ALLProducts
        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery] Product_Spec_Parameters product_params)
        {
            var products = await _unitOFWork.Repository<Products>()!.GetAllAsync(new Product_with_Brand_Type_Specification(product_params));

            if (product_params.ImageSearchResult != null)
            {
                var Image_Search_Resault = product_params.ImageSearchResult.Split(',').ToList();

                if (Image_Search_Resault.Count() > 0)
                {
                    products = products.OrderBy(p => Image_Search_Resault.IndexOf(p.Name ?? "")).ToList();
                }
            }

            var data = _mapper.Map<List<Products>, List<ProductDTO>>(products);

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
