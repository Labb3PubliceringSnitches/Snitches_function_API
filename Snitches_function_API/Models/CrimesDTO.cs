using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snitches_function_API.Models
{
    public class CrimesDTO
    {
        public int Id { get; set; }
        public string Datetime { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public Location Location { get; set; }
    }
}
