using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using DBLayer;
using DBLayer.Repos;
using Entities.Custom;
using Entities.DBEntities;
using Microsoft.Ajax.Utilities;
using ShopBridgeBackend.Helpers;
using ShopBridgeBackend.Services;

namespace BackEndThinkBridge1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ProductsController : ApiController
    {
        private const string imageDirectory= "C:\\UserImages\\";
        private const string thumbnilDirectory = "C:\\UserThumbnils\\";

        private ProuctManager _productManager;

        public ProductsController()
        {
            _productManager = new ProuctManager();
            InitUserImageDirectory();
            InitUserThumbnilDirectory();

        }
        //Var db = new List<Product>();
        // GET: api/Products
        public async Task<IEnumerable<Product>> GetProducts(int? pageNumber=null,int? pageSize=null )
        {
            var products= await _productManager.GetAllProducts(pageNumber,pageSize);
            foreach(var p in products)
            {
                if (!string.IsNullOrEmpty(p.ImageAddress))
                {
                    try
                    {
                        p.Base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(p.ImageAddress));
                        p.Base64Thumbnil = Convert.ToBase64String(System.IO.File.ReadAllBytes(p.ImageAddress.Replace("UserImages", "UserThumbnils")));

                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return products;

        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            //Product product = await db.Products.FindAsync(id);
            Product product;
            try
            {
                product = await _productManager.GetProduct(id);
                if(product!=null &&  !string.IsNullOrEmpty(product.ImageAddress))
                {
                    try
                    {
                        product.Base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(product.ImageAddress));
                        product.Base64Thumbnil = Convert.ToBase64String(System.IO.File.ReadAllBytes(product.ImageAddress.Replace("UserImages", "UserThumbnils")));


                    }
                    catch (Exception )
                    {

                    }
                }
            }
            catch (SqlException e)
            {

                return BadRequest(e.GetBaseException().Message);
            }
            catch (CustomException.NotFoundException<Product> e)
            {

                return NotFound();
            }
            catch (Exception e)
            {

                throw new CustomException.GeneralErrorMessage(e.GetBaseException().Message);
             }

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct([FromUri] Guid id, [FromBody] Product product)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            //db.Entry(product).State = EntityState.Modified;

            try
            {
                // await db.SaveChangesAsync();
                await _productManager.UpdateProduct(id, product);
            }
            catch (SqlException e)
            {

                return BadRequest(e.GetBaseException().Message);
            }
            catch (CustomException.NotFoundException<Product> e)
            {

                return NotFound();
            }
            catch (Exception e)
            {

                throw new CustomException.GeneralErrorMessage(e.GetBaseException().Message);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //POST: api/Products
       [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }


            Product product = new Product
            {
                Price = Convert.ToDecimal(HttpContext.Current.Request.Form["Price"]),
                ProductName = (string)HttpContext.Current.Request.Form["ProductName"]
            };

            if (string.IsNullOrEmpty(product.ProductName))
                return BadRequest("Product Name cannot be blank.");
            if (product.Price<0)
                return BadRequest("Product price cannot be less than 0.");
            // db.Products.Add(product);
            try
            {
                var image = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                string filePath = null;
                string thumbnilPath = null;

                product.ImageAddress = filePath;
                if (image != null && image.ContentLength>0)
                {
                    var imageValidityResult = CheckImageValidity(image);
                    if (imageValidityResult.Item1)
                    {
                        filePath = imageDirectory+image.FileName;
                        thumbnilPath = thumbnilDirectory + image.FileName;

                        product.ImageAddress = filePath;

                    }
                    else
                    {
                        return BadRequest(imageValidityResult.Item2);
                    }

                }

                product=await _productManager.AddProductAsync(product);
                if (filePath != null)
                {
                    image.SaveAs(filePath);
                    GetThumbnil(image).Save(thumbnilPath);
                    
                }


                // await db.SaveChangesAsync();
            }
            catch (SqlException e)
            {

                return BadRequest(e.GetBaseException().Message);
            }
            catch (CustomException.NotFoundException<Product> e)
            {

                return NotFound();
            }
            catch (Exception e)
            {

                throw new CustomException.GeneralErrorMessage(e.GetBaseException().Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        

        //[ResponseType(typeof(void))]
        //public IHttpActionResult PostImage([FromBody] HttpPostedFile file)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // db.Products.Add(product);
        //    try
        //    {
        //        var image = file;
        //        string filePath = null;
        //        if (image != null)
        //        {
        //            var imageValidityResult = CheckImageValidity(image);
        //            if (imageValidityResult.Item1)
        //            {

        //            }
        //            else
        //            {
        //                return BadRequest(imageValidityResult.Item2);
        //            }

        //        }

        //        image.SaveAs(filePath);


        //        // await db.SaveChangesAsync();
        //    }
        //    catch (CustomException.GeneralErrorMessage)
        //    {

        //        throw;
        //    }
        //    catch (CustomException.NotFoundException<Product>)
        //    {
        //        return NotFound();
        //    }

        //    return Ok();
        //}


        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct([FromUri] Guid id)
        {
            Product product;
            //if (product == null)
            //{
            //    return NotFound();
            //}

            // db.Products.Remove(product);
            // await db.SaveChangesAsync();
            try
            {
                product = await _productManager.DeleteProduct(id);
            }
            catch (SqlException e)
            {

                return BadRequest(e.GetBaseException().Message);
            }
            catch (CustomException.NotFoundException<Product> e)
            {

                return NotFound();
            }
            catch (Exception e)
            {

                throw new CustomException.GeneralErrorMessage(e.GetBaseException().Message);
            }
            return Ok(product);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ProductExists(Guid id)
        //{
        //    return db.Products.Count(e => e.ProductId == id) > 0;
        //}

        private Tuple<bool, string> CheckImageValidity(HttpPostedFile image)
        {
            Tuple<bool, string> validity;
            if (image != null && image.ContentLength > 0)
            {

                int MaxContentLength = 1024 * 1024 * 2;

                IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                var ext = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var extension = ext.ToLower();
                if (!AllowedFileExtensions.Contains(extension))
                {

                    var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                    validity = new Tuple<bool, string>(false, message);
                }
                else if (image.ContentLength > MaxContentLength)
                {

                    var message = string.Format("Please Upload a file upto 1 mb.");

                    validity = new Tuple<bool, string>(false, message);
                }
                else
                {




                    validity = new Tuple<bool, string>(true, "Valid image.");


                }
            }
            else
            {
                var message = string.Format("Image file is null.");

                validity = new Tuple<bool, string>(false, message);
            }
            return validity;


        }


        private Image GetThumbnil(HttpPostedFile image)
        {
            System.Drawing.Image img = System.Drawing.Image.FromStream(image.InputStream);
            Image thumb = img.GetThumbnailImage(120, 120, () => false, IntPtr.Zero);
            return thumb;

        }
        private void InitUserImageDirectory()
        {
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }
        }
        private void InitUserThumbnilDirectory()
        {
            if (!Directory.Exists(thumbnilDirectory))
            {
                Directory.CreateDirectory(thumbnilDirectory);
            }
        }

    }
}