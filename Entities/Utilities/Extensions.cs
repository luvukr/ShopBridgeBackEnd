using Entities.Custom;
using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Entities.Utilities
{
    public static class Extensions
    {
        public static Product ToProduct(this Item item)
        {
            return new Product
            {
                ImageAddress = item.ImageAddress,
                Price = item.Price,
                ProductId = item.ItemId,
                ProductName = item.ItemName
            };
        }
        
        public static Item ToItem(this Product product)
        {
             return new Item
                {
                    ImageAddress = product.ImageAddress,
                    Price = product.Price,
                    ItemName = product.ProductName


                };
            
            
        }

    }
}