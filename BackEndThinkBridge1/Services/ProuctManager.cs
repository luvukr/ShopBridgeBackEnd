using DBLayer;
using DBLayer.Repos;
using Entities.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entities.Utilities;
using Entities.DBEntities;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using ShopBridgeBackend.Helpers;
using Log;

namespace ShopBridgeBackend.Services
{
    public class ProuctManager
    {
        public async Task<Product> AddProductAsync(Product product)
        {
            Item item = null;
            try
            {
                item = product.ToItem();
                item.AddedOn = DateTime.UtcNow;
                using (InventoryContextScope contextScope = new InventoryContextScope())
                {
                    ItemRepo itemRepo = new ItemRepo(contextScope);
                    item = itemRepo.Add(item);
                    await contextScope.Complete();
                }
            }
            catch(SqlException e)

            { 
                Logger.Error(e.Message, e);
                throw e;
            }
            catch (Exception e)
            {

                Logger.Error(e.Message, e);

                throw e;
            }
            return item?.ToProduct();
        }


        public async Task<Product> UpdateProduct(Guid id,Product product)
        {
            Item item = null;
            try
            {
                using (InventoryContextScope contextScope = new InventoryContextScope())
                {
                    ItemRepo itemRepo = new ItemRepo(contextScope);
                    item = await itemRepo.masterEntities.FirstOrDefaultAsync(x => x.ItemId == id && !x.IsDeleted);
                    if (item == null)
                        throw new CustomException.NotFoundException<Product>();
                    item.ImageAddress = string.IsNullOrEmpty(product.ImageAddress)? item.ImageAddress: product.ImageAddress;
                    item.ItemName = product.ProductName;
                    item.Price = product.Price;
                    item.LastUpdatedOn = DateTime.UtcNow;
                    await contextScope.Complete();    
                }
            }
            catch (CustomException.NotFoundException<Product>)
            {

                throw;
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message, e);

                throw e;
            }
            catch (Exception e)
            {

                Logger.Error(e.Message, e);

                throw e;
            }
            return item.ToProduct();
        }
        public async Task<Product> DeleteProduct(Guid id)
        {
            Item item = null;


            try
            {
                using (InventoryContextScope contextScope = new InventoryContextScope())
                {
                    ItemRepo itemRepo = new ItemRepo(contextScope);
                    
                        item = await itemRepo.masterEntities.FirstOrDefaultAsync(x => x.ItemId == id && !x.IsDeleted);
                        if (item == null)
                            throw new CustomException.NotFoundException<Product>();
                        item.IsDeleted = true;
                         await contextScope.Complete();

                }
            }
            catch (CustomException.NotFoundException<Product> e)
            {
                Logger.Error(e.Message, e);

                throw e;
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message, e);

                throw e;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);

                throw e;
            }
            return item.ToProduct();
        }
        public async Task<Product> GetProduct(Guid id)
        {
            Item item = null;
            try
            {
                using (InventoryContextScope contextScope = new InventoryContextScope())
                {
                    ItemRepo itemRepo = new ItemRepo(contextScope);
                    item = await itemRepo.masterEntities.FirstOrDefaultAsync(x => x.ItemId == id && !x.IsDeleted);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item?.ToProduct();
        }

        public async Task<List<Product>> GetAllProducts(int? pageNumber=null,int? pageSize=null)
        {
            List<Item> items = null;
            try
            {
                using (InventoryContextScope contextScope = new InventoryContextScope())
                {
                    ItemRepo itemRepo = new ItemRepo(contextScope);
                    items = await itemRepo.masterEntities.Where(x=>!x.IsDeleted).OrderByDescending(x => x.AddedOn).Skip((pageNumber??0) * (pageSize??10)).Take(pageSize??10).ToListAsync();

                }
            }
            catch (SqlException e)
            {
                Logger.Error(e.Message, e);

                throw e;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, e);
                throw e;
            }
            return items?.Select(x=>x.ToProduct()).ToList();
        }
    }
}