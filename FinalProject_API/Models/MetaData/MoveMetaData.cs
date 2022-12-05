using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{ 
    public class MoveMetaData : Auditable
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "All moves must have a name.")]
        public string Name { get; set; }

        public int TypeID { get; set; }

        public int PokemonID { get; set; }
    }
}
