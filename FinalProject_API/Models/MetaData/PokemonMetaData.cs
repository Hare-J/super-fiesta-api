using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{ 
    public class PokemonMetaData : Auditable
    {
        [Display(Name = "Pokedex Number")]
        [Required(ErrorMessage = "All Pokemon must have a Pokedex Number.")]
        public int Pokedex { get; set; }

        [Display(Name = "Pokemon")]
        [Required(ErrorMessage = "All Pokemon must have a name.")]
        public string Name { get; set; }

        [Display(Name = "HP")]
        [Required(ErrorMessage = "All Pokemon must have a Base HP value.")]
        public int HP { get; set; }

        [Display(Name = "Base Attack")]
        [Required(ErrorMessage = "All Pokemon must have a Base Attack value.")]
        public int Attack { get; set; }

        [Required(ErrorMessage = "Primary Type is a required trait.")]
        public int PrimaryTypeID { get; set; }

        [Display(Name = "Trainer")]
        public int TrainerID { get; set; }

        //public ICollection<PokemonType> Types { get; set; }
    }
}
