using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BrineyConstruction.Models
{
    public class Project
    {
        public int ProjectId { get; set; } //Principle key

        [Required]
        public string Name { get; set; }
        public List<Image> Images { get; set; } //Collection navigation property

    }

    public class Image //Dependent Entity
    {
        public int ImageId { get; set; }

        [Required]
        public string Name { get; set; }

        public int ProjectId { get; set; } //Foreign Key

        public Project Project { get; set; } //Reference navigation property
    }
}
