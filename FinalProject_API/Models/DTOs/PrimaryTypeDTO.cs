using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.DTOs
{
    [ModelMetadataType(typeof(PrimaryTypeMetaData))]
    public class PrimaryTypeDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Byte[] RowVersion { get; set; }

        public ICollection<PokemonDTO> Pokemons { get; set; }
    }
}
