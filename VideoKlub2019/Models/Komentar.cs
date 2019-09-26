using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoKlub2019.Models
{
    [Table("Komentar")]
    public partial class Komentar
    {
        public int KomentarId { get; set; }
        [StringLength(450)]
        public string ClanId { get; set; }
        [Required]
        [StringLength(30)]
        public string Korisnik { get; set; }
        [Required]
        [StringLength(30)]
        public string Poruka { get; set; }
        public int? FilmId { get; set; }

        [ForeignKey("FilmId")]
        [InverseProperty("Komentari")]
        public virtual Film Film { get; set; }
    }
}