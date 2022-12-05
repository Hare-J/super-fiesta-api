using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{
    public class PokemonTypeMetaData : Auditable
    {
        [Required(ErrorMessage = "All Pokemon Types must have a name.")]
        [Display(Name = "Type")]
        public string Name { get; set; }
    }
}
