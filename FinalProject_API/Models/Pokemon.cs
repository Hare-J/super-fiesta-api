using FinalProject_API.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{ 
    [ModelMetadataType(typeof(PokemonMetaData))]
    public class Pokemon : Auditable
    {
        /*public Pokemon()
        {
            this.Moves = new HashSet<Move>();
        }*/

        public int ID { get; set; }

        public int Pokedex { get; set; }

        public string Name { get; set; }

        public int HP { get; set; }

        public int Attack { get; set; }

        public int PrimaryTypeID { get; set; }

        public PrimaryType PrimaryType { get; set; }

        public int TrainerID { get; set; }

        public Trainer Trainer { get; set; }

        //public ICollection<Move> Moves { get; set; }
    }
}
