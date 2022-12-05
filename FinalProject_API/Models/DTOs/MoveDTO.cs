using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.DTOs
{
    [ModelMetadataType(typeof(MoveMetaData))]
    public class MoveDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Byte[] RowVersion { get; set; }

        public int TypeID { get; set; }

        public PrimaryTypeDTO PrimaryType { get; set; }

        public int PokemonID { get; set; }
        public PokemonDTO Pokemon { get; set; }
    }
}
