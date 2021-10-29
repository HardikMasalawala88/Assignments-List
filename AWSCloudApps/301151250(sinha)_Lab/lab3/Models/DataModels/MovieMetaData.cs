using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace lab3.Models.DataModels
{
    public class MovieMetaData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieMetaId { get; set; }
        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; } 
        public string MovieTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ActorType { get; set; }
        public int Rating { get; set; }

    }
}
