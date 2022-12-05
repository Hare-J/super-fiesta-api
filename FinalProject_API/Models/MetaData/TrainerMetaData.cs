using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{
    public class TrainerMetaData : Auditable
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "All trainers must have a name.")]
        [Display(Name = "Trainer")]
        public string Name { get; set; }

        [Range(0, 80, ErrorMessage = "Trainers can not have less than 0 or more than 80 badges.")]
        public int Badges { get; set; }

        //public ICollection<Pokemon> Pokemons { get; set; }
    }
}
