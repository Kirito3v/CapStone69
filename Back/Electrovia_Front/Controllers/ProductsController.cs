using Electrovia.DTOs;
using Electrovia_Core.Specifications;
using Electrovia_Front.Controllers;
using Electrovia_Front.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;


namespace GraduationProj.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient _httpClient;

        public Pagination<ProductDTO>? pagination { get; set; } = new();

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;

        }



        [HttpPost("ImageSearch")]
        public async Task<ActionResult> ImageSearch([FromForm]IFormFile imageFile)
        {


            if (imageFile == null || imageFile.Length == 0)
                return RedirectToAction();

            using var multipartContent = new MultipartFormDataContent();

            using var fileStreamContent = new StreamContent(imageFile.OpenReadStream());
            fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);

    
            multipartContent.Add(
                content: fileStreamContent,
                name: "imageFile",     
                fileName: $"{imageFile.FileName}"
            );



            var baseUrl = "https://localhost:7228/api/Products/SearchWithImage"; // api url


            var response = await _httpClient.PostAsync(
                baseUrl,
                multipartContent
            );

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                return Json(json);
            }

            ModelState.AddModelError("", $"Upload failed: {response.StatusCode}");

            return Json(null);
        }

        public async Task<IActionResult> Index([FromQuery] Product_Spec_Parameters specParams)
        {

            //query to send to api
            var query = HttpUtility.ParseQueryString(string.Empty);

            if (specParams.brandid.HasValue)
                query["brandid"] = specParams.brandid.Value.ToString();
            if (specParams.typeid.HasValue)
                query["typeid"] = specParams.typeid.Value.ToString();
            if (!string.IsNullOrWhiteSpace(specParams.sort))
                query["sort"] = specParams.sort;
            if (!string.IsNullOrWhiteSpace(specParams.Search))
                query["Search"] = specParams.Search;
            if (!string.IsNullOrWhiteSpace(specParams.ImageSearchResult))
                query["ImageSearchResult"] = specParams.ImageSearchResult;

            query["Pageindex"] = specParams.Pageindex.ToString();
            query["Pagesize"] = specParams.Pagesize.ToString();

            var baseUrl = "https://localhost:7228/api/Products"; // api url

            var uriBuilder = new UriBuilder(baseUrl)
            {
                Query = query.ToString()
            };

            var response = await _httpClient.GetAsync(uriBuilder.Uri); // send request

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                pagination = JsonSerializer.Deserialize<Pagination<ProductDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (pagination != null)
                    return View(pagination);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}
