using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{
    [ModelMetadataType(typeof(PokemonTypeMetaData))]
    public class PokemonType : Auditable
    {
        public PokemonType()
        {
            PokemonPokemonTypes = new HashSet<PokemonPokemonType>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<PokemonPokemonType> PokemonPokemonTypes { get; set; }
    }
}
