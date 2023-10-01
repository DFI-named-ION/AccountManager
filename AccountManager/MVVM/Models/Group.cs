namespace AccountManager.MVVM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Group")]
    public partial class Group
    {
        public int? UserID { get; set; }

        public int ID { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(150)]
        public string ImagePath { get; set; }

        public virtual User User { get; set; }
    }
}
