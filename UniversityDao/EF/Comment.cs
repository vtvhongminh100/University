
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace UniversityDao.EF
{
    [Table("Comment")]
    public partial class Comment
    {
        [Key]
        public int CommentID { get; set; }

        public int IdeaID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Emotion { get; set; }

        public int? Category { get; set; }

        public int? ViewCount { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(250)]
        public string CreatedBy { get; set; }

        [StringLength(250)]
        public string ModifiedBy { get; set; }

        public DateTime? ClosedDate { get; set; }

        public bool? Status { get; set; }
    }
}
