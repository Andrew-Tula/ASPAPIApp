using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Abstract.Repositories;
using System;

namespace ASPAPI.Controllers {
    public record ProductDto(int id, string name, decimal price): ProductBaseDto(name, price);
    public record ProductBaseDto(string name, decimal price);

    [Route("[controller]/[action]")]
    [ApiController]

  
    public class ProductController : ControllerBase
    {
        private IGenericRepository<Product> productRepository;
        public ProductController(IGenericRepository<Product> productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProduct() => Ok(productRepository.GetAll());
       
        [HttpPost]
        public IActionResult AddProduct(ProductBaseDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название продукта");
            if(data.price <= 0)
                return BadRequest("Заполните цену продукта");

            var product = new Product{ 
                Name = data.name,
                Price = data.price 
            };

            productRepository.Add(product);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var productToDelete = productRepository.GetById(id);
            if (productToDelete == null)
                return NotFound("Такого подукта не существует");

           productRepository.Remove(productToDelete);
           return Ok();
        }

        [HttpPut]
        public IActionResult EditProduct(ProductDto data)
        {
            if (data.price <= 0)
                return BadRequest("Не задана новая цена");

            var productToUpdate = productRepository.GetById(data.id);
            if (productToUpdate == null)
                return NotFound("Такого продукта не существует");

            productToUpdate.Price = data.price;
            productRepository.Update(productToUpdate);
            return Ok();
        }
    }
}