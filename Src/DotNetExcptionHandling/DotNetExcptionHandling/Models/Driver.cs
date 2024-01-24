using System.ComponentModel.DataAnnotations;

namespace DotNetExcptionHandling.Models
{
    public class Driver
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]

        public int DriverNumber { get; set; }
    }
}
