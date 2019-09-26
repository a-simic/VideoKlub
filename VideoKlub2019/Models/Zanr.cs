using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoKlub2019.Models
{
    [Table("Zanr")]
    public partial class Zanr
    {
        public Zanr()
        {
            Filmovi = new HashSet<Film>();
        }

        public int ZanrId { get; set; }
        [Required]
        [StringLength(50)]
        public string NazivZanra { get; set; }

        [InverseProperty("Zanr")]
        public virtual ICollection<Film> Filmovi { get; set; }
    }
}