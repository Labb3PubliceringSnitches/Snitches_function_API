using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snitching_function_API.Models
{
    public class SnitchesStory
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.Now;
        public string SnitchStory { get; set; }
    }

    public class CreateSnitch
    {
        public string SnitchStory { get; set; }
    }
}
