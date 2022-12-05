using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{
    [ModelMetadataType(typeof(TrainerMetaData))]
    public class Trainer : Auditable
    {
        public Trainer()
        {
            this.Pokemons = new HashSet<Pokemon>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public int Badges { get; set; }

        public ICollection<Pokemon> Pokemons { get; set; }
    }
}
