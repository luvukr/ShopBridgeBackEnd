using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities.Custom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridgeBackend.Services;

namespace BackEndThinkBridge1.Tests.Services
{
    [TestClass]
    public class ProductManagerTest
    {
        List<Product> products;
        private ProuctManager _pManager;

        public ProductManagerTest()
        {
            products = new List<Product>();
            _pManager = new ProuctManager();
        }

        //[TestMethod]
        //public async Task RunAllTestAsync()
        //{
        //    await TestAdd();
        //    await TestUpdate();
        //    await TestGet();
        //    await TestGetAll();
        //    await TestDelete();

        //}
        #region Success Cases
        [TestMethod]
        public async Task TestAdd()
        {
            //ProuctManager _pManager1 = new ProuctManager();
            Product p = new Product
            {
                ImageAddress = "C:\\Img1.jpg",
                Price = 10.00M,
                ProductName = "P4"

            };
            var product=await _pManager.AddProductAsync(p);
            products.Add(product);
            Assert.IsNotNull(product.ProductId);
        }
        [TestMethod]
        public async Task TestUpdate()
        {
            var products1 = await _pManager.GetAllProducts(); 
            products1[0].Price = 12.00M;
            var p = await _pManager.UpdateProduct(products1[0].ProductId,products1[0]);
            Assert.AreEqual<decimal>(p.Price, 12.00M);
        }

        [TestMethod]
        public async Task TestGet()
        {
            var products1 = await _pManager.GetAllProducts();

            var product = await _pManager.GetProduct(products1[0].ProductId);
            Assert.IsNotNull(product);
        }
        [TestMethod]
        public async Task TestGetAll()
        {

            var product = await _pManager.GetAllProducts();
            Assert.IsNotNull(product);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            var products1 = await _pManager.GetAllProducts();

            var product = await _pManager.DeleteProduct(products1[0].ProductId);
            Assert.IsNotNull(product);
        }
        #endregion
        #region Failed Cases

        [TestMethod]
        public async Task TestAddFail()
        {
            bool status = false;
            //ProuctManager _pManager1 = new ProuctManager();
            Product p = new Product
            {
                ImageAddress = "C:\\Img123.jpg",
                Price = 10.00M,

            };
            try
            {
                var product = await _pManager.AddProductAsync(p);

            }
            catch (Exception)
            {

                status = true;
            }
            Assert.IsTrue(status);
            
        }
        [TestMethod]
        public async Task TestUpdateFail()
        {
            bool status = false;

            var products1 = await _pManager.GetAllProducts();
            try
            {

                products1[0].Price = 12.00M;
                products1[0].ProductId = Guid.NewGuid();
                var p = await _pManager.UpdateProduct(products1[0].ProductId, products1[0]);
            }
            catch (Exception)
            {
                status = true;

            }
            Assert.IsTrue(status);
        }

        [TestMethod]
        public async Task TestGetFail()
        {

            var products1 = await _pManager.GetAllProducts();
            Product product=new Product();
            try
            {
                product = await _pManager.GetProduct(Guid.NewGuid());

            }
            catch (Exception)
            {

            }
            Assert.IsNull(product);
        }
       

        [TestMethod]
        public async Task TestDeleteFail()
        {
            bool status = false;

            Product product = new Product();

            try
            {
                 product = await _pManager.DeleteProduct(Guid.NewGuid());
            }
            catch (Exception)
            {
                status = true;


            }
            Assert.IsTrue(status);
        }
        #endregion
    }
}