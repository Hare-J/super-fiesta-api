using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{
    public class PokemonPokemonType
    {
        public int ID { get; set; }

        public int PokemonID { get; set; }

        public Pokemon Pokemon { get; set; }

        public int PokemonTypeID { get; set; }

        public PokemonType PokemonType { get; set; }
    }
}
