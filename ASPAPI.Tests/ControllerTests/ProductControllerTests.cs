using ASPAPI.Controllers;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Data;

namespace ASPAPI.Tests.ControllerTests {
    [TestClass]
    public class ProductControllerTests
    {
        private ProductController controller;
        private IQueryable<Product> products;

        private TestDBContext FormContext()
        {
            products = new ProductCollection().Products;

            var productDbSet = new Mock<DbSet<Product>>();
            TestHelper.InitDbSet(productDbSet, products);

            var storeProducts = new StoreProductCollection().StoreProducts;
            var storeProductDbSet = new Mock<DbSet<StoreProduct>>();
            TestHelper.InitDbSet(storeProductDbSet, storeProducts);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Products).Returns(productDbSet.Object);
            context.Setup(x => x.StoreProducts).Returns(storeProductDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var context = FormContext();

            var productRepository = new ProductRepository(context);
            productRepository.InitDbSet(context.Products);

            controller = new ProductController(productRepository);
        }

        private void AddProductBadRequestObjectResultCheck(string name, decimal price, string expectedResult)
        {
            var productBaseDto = new ProductBaseDto(name, price);
            var result = controller.AddProduct(productBaseDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        public void AddProductEmptyNameCheck() => AddProductBadRequestObjectResultCheck("", 10, "Заполните название продукта");

        [TestMethod]
        public void AddProductEmptyPriceCheck() => AddProductBadRequestObjectResultCheck("Chupa", 0, "Заполните цену продукта");

        [DataTestMethod]
        [DataRow("Bently", 100.25)]
        [DataRow("Mazeratti", 200.35)]
        [TestMethod]
        public void AddProductSucces(string name, double price)
        {
            var productBaseDto = new ProductBaseDto(name, (decimal)price);
            var result = controller.AddProduct(productBaseDto);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetProductSuccess()
        {
            var result = controller.GetProduct();

            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<Product>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
            Assert.AreEqual(values[0].Name, products.First().Name);
            Assert.AreEqual(values[3].Name, products.Last().Name);
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(20)]
        [DataRow(555)]
        [DataRow(1000)]
        public void DeleteProductNotFound(int id)
        {
            var result = controller.DeleteProduct(id);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Такого подукта не существует", value);
        }

        [DataTestMethod]
       // [DataRow(1)]
        [DataRow(2)]
        public void DeleteProductSuccess(int id)
        {
            var result = controller.DeleteProduct(id);
            Assert.IsTrue(result is OkResult);
        }


        [DataTestMethod]
        [DataRow(-1, "Milk", 15)]
        [DataRow(0, "Milky", 25)]
        public void EditProductNotSet(int id, string name, double price) {
            var productDto = new ProductDto(id, name, (decimal)price);

            var result = controller.EditProduct(productDto);

            Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;

            Assert.IsNotNull(value);
            Assert.AreEqual("Такого продукта не существует", value);
        }

        [DataTestMethod]
        [DataRow(1, "Milk", 0)]
        //[DataRow(55, "test")]
        //[DataRow(666, "second")]
        public void EditProductWrongPrice(int id, string name, double price)
        {
            var productDto = new ProductDto(id, name, (decimal)price);
            var result = controller.EditProduct(productDto);

            Assert.IsTrue(result is BadRequestObjectResult);
            var value = (result as BadRequestObjectResult)?.Value as string;
            Assert.IsNotNull(value);
            Assert.AreEqual("Не задана новая цена", value);
        }

        [DataTestMethod]
        [DataRow(1, "Chiken", 25)]
      //[DataRow(2, "changed 2")]
        public void EditProductSuccess(int id, string name, double price)
        {
            var productDto = new ProductDto(id, name, (decimal)price);
            var result = controller.EditProduct(productDto);

            Assert.IsTrue(result is OkResult);
        }
    }
}