using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.DTOs
{
    [ModelMetadataType(typeof(PokemonPokemonTypeMetaData))]
    public class PokemonPokemonTypeDTO
    {
        public int ID { get; set; }

        public int PokemonID { get; set; }

        public PokemonDTO Pokemon { get; set; }

        public int PokemonTypeID { get; set; }

        public PokemonTypeDTO PokemonType { get; set; }

        public Byte[] RowVersion { get; set; }
    }
}
