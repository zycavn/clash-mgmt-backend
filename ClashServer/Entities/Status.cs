using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClashServer.Entities
{
    [Table("status")]
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string UserName { get; set; }
        public DateTime Time { get; set; }

        [ForeignKey(nameof(Clash))]
        public Guid ClashId { get; set; }

        public Clash Clash { get; set; }
    }
}