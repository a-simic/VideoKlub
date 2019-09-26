using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoKlub2019.Models
{
    [Table("Iznajmljivanje")]
    public partial class Iznajmljivanje
    {
        public int IznajmljivanjeId { get; set; }
        public int FilmId { get; set; }
        [Required]
        [StringLength(450)]
        public string ClanId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DatumIznajmljivanja { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DatumVracanja { get; set; }

        [ForeignKey("FilmId")]
        [InverseProperty("Iznajmljivanja")]
        public virtual Film Film { get; set; }
    }
}