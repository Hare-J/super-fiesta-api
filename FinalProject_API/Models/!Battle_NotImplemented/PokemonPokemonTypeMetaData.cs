using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{
    public class PokemonPokemonTypeMetaData
    {
        [Display(Name = "Pokemon")]
        public int PokemonID { get; set; }

        [Display(Name = "Type")]
        public int PokemonTypeID { get; set; }
    }
}
