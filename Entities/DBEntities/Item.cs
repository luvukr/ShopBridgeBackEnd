namespace Entities.DBEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Item")]
    public partial class Item
    {
        public Item()
        {
            ItemId = Guid.NewGuid();
        }
        public Guid ItemId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }

        public string ImageAddress { get; set; }

        public bool IsDeleted { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime AddedOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastUpdatedOn { get; set; }



    }
}
