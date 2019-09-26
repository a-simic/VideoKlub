using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VideoKlub2019.Models
{
    [Table("Film")]
    public partial class Film
    {
        public Film()
        {
            Iznajmljivanja = new HashSet<Iznajmljivanje>();
            Komentari = new HashSet<Komentar>();
        }

        public int FilmId { get; set; }
        [Required]
        [StringLength(100)]
        public string Naziv { get; set; }
        public int ZanrId { get; set; }
        [StringLength(100)]
        public string Reziser { get; set; }
        public int Godina { get; set; }
        public byte[] Slika { get; set; }
        [StringLength(20)]
        public string SlikaTip { get; set; }
        public int CenaPoDanu { get; set; }

        [ForeignKey("ZanrId")]
        [InverseProperty("Filmovi")]
        public virtual Zanr Zanr { get; set; }
        [InverseProperty("Film")]
        public virtual ICollection<Iznajmljivanje> Iznajmljivanja { get; set; }
        [InverseProperty("Film")]
        public virtual ICollection<Komentar> Komentari { get; set; }
    }
}