using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClashServer.Entities
{
    [Table("comment")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string UserName { get; set; }

        [ForeignKey(nameof(Clash))]
        public Guid ClashId { get; set; }

        public Clash Clash { get; set; }
    }
}