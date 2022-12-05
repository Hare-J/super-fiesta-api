using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models
{
    public class Move : Auditable
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int TypeID { get; set; }

        public PrimaryType PrimaryType { get; set; }
        
        public int PokemonID { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
