using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Entities.Custom;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridgeBackend;
using BackEndThinkBridge1.Controllers;

namespace BackEndThinkBridge1.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public async Task Get()
        {
            // Arrange
            ProductsController controller = new ProductsController();

            // Act
            IEnumerable<Product> result = await controller.GetProducts();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count()>0);
        }

        [TestMethod]
        public async Task GetById()
        {
            // Arrange
            ProductsController controller = new ProductsController();

            // Act
            IHttpActionResult result = await controller.GetProduct(new Guid("7F80C7E9-FC41-4FB2-87FB-099081182FBC"));
           
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetByIdFail()
        {
            // Arrange
            ProductsController controller = new ProductsController();

            // Act
            IHttpActionResult result = await controller.GetProduct(new Guid("7D886F16-F1E0-436F-81D7-5CA3B2F165C8"));

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public async Task GetByIdNullCase()
        {
            // Arrange
            ProductsController controller = new ProductsController();

            // Act
            IHttpActionResult result = await controller.GetProduct(new Guid("9D886F16-F1E0-436F-81D7-5CA3B2F165C8"));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Put()
        {
            // Arrange
            ProductsController controller = new ProductsController();
            Product p = new Product { ProductId = new Guid("7F80C7E9-FC41-4FB2-87FB-099081182FBC"), Price = 34.00M, ProductName = "TestProduct" };

           
               IHttpActionResult res= await controller.PutProduct(new Guid("7F80C7E9-FC41-4FB2-87FB-099081182FBC"),p );
            Assert.IsNotNull(res);



            // Assert
        }

        [TestMethod]
        public async Task PutFail()
        {
            bool status = true ;
            try
            {
                ProductsController controller = new ProductsController();
                Product p = new Product { ProductId = new Guid("B3CD2D25-765F-4191-AF1D-B2CA38A41660"), Price = 34.00M };


                IHttpActionResult res = await controller.PutProduct(new Guid("B3CD2D25-765F-4191-AF1D-B2CA3890A41660"), p);

            }
            catch (Exception)
            {

                status=false;
            }
            Assert.IsFalse(status);



            // Assert
        }

        [TestMethod]
        public async Task Delete()
        {
            bool status = true;
            // Arrange
            ProductsController controller = new ProductsController();
            try
            {
                await controller.DeleteProduct(new Guid("7F80C7E9-FC41-4FB2-87FB-099081182FBC"));

            }
            catch (Exception)
            {

                status = false;
            }
            try
            {
                await controller.DeleteProduct(new Guid("C25FB87C-025D-4E9F-BBE2-D7D013F790dsDE"));

            }
            catch (Exception)
            {
                status = true;
            }
            Assert.IsTrue(status);

        }
    }
}
