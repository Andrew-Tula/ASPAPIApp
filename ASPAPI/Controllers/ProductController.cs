using System;
using Microsoft.AspNetCore.Mvc;
using ASPAPI.Models.DbEntities;
using ASPAPI.Services;

namespace ASPAPI.Controllers
{
   // public record ProductDto(int id);
 //   public record ProductExtendedDto(string name, decimal price) : ProductDto(id);
    public record ProductDto(int id, string name, decimal price);
    [Route("[controller]/[action]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private TestDBContext dbContext;
        public ProductController(TestDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(dbContext.Products?.ToList());
        }

        [HttpPost]
        //public IActionResult AddProduct(string name, decimal price?)
        public IActionResult AddProduct(ProductDto data)
        {
            if (string.IsNullOrWhiteSpace(data.name))
                return BadRequest("Заполните название продукта");
            if(data.price == null || data.price == ' ')
                return BadRequest("Заполните цену продукта");

            var product = new Product
            { Name = data.name, Price = data.price };

            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var ProductToDelete = dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (ProductToDelete == null)
                return NotFound("Такого подукта не существует");

            dbContext.Products.Remove(ProductToDelete);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult EditProduct(ProductDto data)
        {
            if (data.price == null || data.price == ' ')
                return BadRequest("Не задана новая цена");

            var ProductToUpdate = dbContext.Products.FirstOrDefault(p => p.Id == data.id);
            if (ProductToUpdate == null)
                return NotFound("Такого продукта не существует");

            ProductToUpdate.Price = data.price;
            dbContext.Products.Update(ProductToUpdate);
            dbContext.SaveChanges();
            return Ok();
        }





    }



}