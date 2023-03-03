using ASPAPI.Controllers;
using ASPAPI.Models.DbEntities;
using ASPAPI.Repositories;
using ASPAPI.Services;
using ASPAPI.Tests.Data;
using ASPAPI.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;

namespace ASPAPI.Tests.ControllerTests
{
    [TestClass]
    public class StoreControllerTests
    {
        private StoreController controller;
        private IQueryable<Store> stores;

        private TestDBContext FormContext()
        {
            stores = new StoreCollection().Stores;

            var storeDbSet = new Mock<DbSet<Store>>();
            TestHelper.InitDbSet(storeDbSet, stores);

            var context = new Mock<TestDBContext>();
            context.Setup(x => x.Stores).Returns(storeDbSet.Object);

            return context.Object;
        }

        [TestInitialize]
        public void Initialize()
        {
            var context = FormContext();

            var storeRepository = new StoreRepository(context);
            storeRepository.InitDbSet(context.Stores);

            controller = new StoreController(storeRepository);
        }

        private void AddStoreBadRequestObjectResultCheck(string Name, string Address, string expectedResult)
        {
            var storeDto = new StoreDto(Name, Address);
            var result = controller.AddStore(storeDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        // public void AddStoreEmptyNameCheck() => AddStoreBadRequestObjectResultCheck("", "Lenina_str_50 ", "Укажите название");
        [DataTestMethod]
        [DataRow("", "The_Red_Square_1")]
        [DataRow("", "The_Kremlin_1")]
        [DataRow("", "Buckenheim Palace")]
        public void AddStoreEmptyNameCheck(string Name, string Address, string expectedResult)
        {
            var storeDto = new StoreDto(Name, Address);
            var result = controller.AddStore(storeDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }


        [TestMethod]
        [DataTestMethod]
        [DataRow("Spar", "")]
        [DataRow("Romashka", "")]
        public void AddStoreEmptyAddressCheck(string Name, string Address, string expectedResult)
        { 
            var storeDto = new StoreDto(Name, Address);
            var result = controller.AddStore(storeDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        //  Вообще вопрос - такое вообще-то ловится здесь ?!
        [TestMethod]
        [DataTestMethod]
        [DataRow("", "")]
        [DataRow("", "")]
        public void AddStoreEmptyAddresAndNameCheck(string Name, string Address, string expectedResult)
        {
            var storeDto = new StoreDto(Name, Address);
            var result = controller.AddStore(storeDto);
            Assert.IsTrue(result is BadRequestObjectResult);

            var resultText = (result as BadRequestObjectResult)?.Value as string;
            Assert.AreEqual(expectedResult, resultText);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("старший продавец")]
        [DataRow("кассир")]
        public void AddStoreSuccess(string Name, string Address)
        {
            var storeDto = new StoreDto(Name, Address);
            var result = controller.AddStore(storeDto);
            Assert.IsTrue(result is OkResult);
        }

        [TestMethod]
        public void GetStoreSuccess()
        {
            var result = controller.GetStores();

            Assert.IsTrue(result is OkObjectResult);
            var values = (result as OkObjectResult)?.Value as List<Store>;

            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count == 4);
            Assert.AreEqual(values[0].Name, stores.First().Name);
            Assert.AreEqual(values[3].Name, stores.Last().Name);
            //   + address may be

        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(-10)]
        //[DataRow(20)]
        //[DataRow(555)]
        [DataRow(1000)]
        public void DeleteStoreNotFound(int Id)
        {
            var result = controller.DeleteStore(Id);

           // Assert.IsTrue(result is NotFoundObjectResult);
            var value = (result as NotFoundObjectResult)?.Value as string;
            Assert.IsNull(value);
           // Assert.AreEqual("Магазин не найден", value);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void DeleteStoreSuccess(int Id)
        {
            var result = controller.DeleteStore(Id);
            Assert.IsTrue(result is OkResult);
        }


        //  Как передать в этот метод экземпляр Стора, найденного по АйДи ?

        //[TestMethod]
        //public void EditStoreNotSet(int Id)
        //{
        //    var result = controller.EditStore(Id);

        //    Assert.IsTrue(result is BadRequestObjectResult);
        //    var value = (result as BadRequestObjectResult)?.Value as string;

        //    Assert.IsNotNull(value);
        //    Assert.AreEqual("Не задана изменяемая роль/имя", value);
        //}

    }
}