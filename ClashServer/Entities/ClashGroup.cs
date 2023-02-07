using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClashServer.Entities
{
    [Table("clashgroup")]
    public class ClashGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string ClashCode { get; set; }

        public string Tolerance { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
        public ICollection<Clash> Clashes { get; set; }
    }
}