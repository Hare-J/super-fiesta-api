using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Models.MetaData
{
    public class PrimaryTypeMetaData : Auditable
    {
        [Required(ErrorMessage = "Type name is required.")]
        public string Name { get; set; }
    }
}
