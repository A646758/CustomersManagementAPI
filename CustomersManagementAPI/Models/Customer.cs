using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomersManagementAPI.Models
{
    public class Customer
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("firstName")]
        public string Firstname { get; set; }
        [Required]
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
    }
}
