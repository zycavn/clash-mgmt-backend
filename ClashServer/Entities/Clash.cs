using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClashServer.Entities
{
    [Table("clash")]
    public class Clash
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Status { get; set; }
        public string AssignTo { get; set; }
        public string GridLocation { get; set; }
        public string Description { get; set; }
        public string DateFound { get; set; }
        public string ClashPoint { get; set; }
        public string ClashImagePath { get; set; }
        public string Distance { get; set; }

        [Required]
        public int ElementId1 { get; set; }

        public string Layer1 { get; set; }

        public string ItemPath1 { get; set; }

        public string ItemName1 { get; set; }

        public string ItemType1 { get; set; }

        [Required]
        public int ElementId2 { get; set; }

        public string Layer2 { get; set; }

        public string ItemPath2 { get; set; }

        public string ItemName2 { get; set; }

        public string ItemType2 { get; set; }

        [ForeignKey(nameof(ClashGroup))]
        public Guid ClashGroupId { get; set; }

        public ClashGroup ClashGroup { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Status> States { get; set; }
    }
}