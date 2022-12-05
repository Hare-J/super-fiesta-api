using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{
    [ModelMetadataType(typeof(PrimaryTypeMetaData))]
    public class PrimaryType : Auditable
    {
        public PrimaryType()
        {
            this.Pokemons = new HashSet<Pokemon>();
            //this.Moves = new HashSet<Move>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Pokemon> Pokemons { get; set; }

        //public ICollection<Move> Moves { get; set; }
    }
}
