namespace UniversityDao.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Idea")]
    public partial class Idea
    {
        public int IdeaID { get; set; }

        [Column(TypeName = "ntext")]
        public string IdeaContent { get; set; }

        [StringLength(500)]
        public string IdeaDescription { get; set; }

        [StringLength(20)]
        public string IdeaEmotion { get; set; }

        public int? IdeaCategory { get; set; }

        public int? IdeaViewCount { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string CreatedBy { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        public DateTime? ClosedDate { get; set; }

        public bool? IdeaStatus { get; set; }
    }
}
