using Entities.Custom;
using Entities.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DBLayer.Repos
{
    public class ItemRepo: Repository<Item>
    {
        public ItemRepo(InventoryContextScope contextScope) : base(contextScope)
        {
        }
    }
}
