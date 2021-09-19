using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrineyConstruction.Models
{
    public class Photo //Dependent Entity
    {
        public int PhotoId { get; set; } 

        [Required] 
        public string Name { get; set; }

        public int CategoryId { get; set; } //Foreign Key

        [Required]
        public Category Category { get; set; } //Reference navigation property
    }

    public class Category //Principle Entity
    {
        public int CategoryId { get; set; } //Principle key

        [Required]
        public string Name { get; set; }
        public List<Photo> Photos { get; set; } //Collection navigation property
    }
}
