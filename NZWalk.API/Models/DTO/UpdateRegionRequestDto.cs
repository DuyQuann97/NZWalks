using System.ComponentModel.DataAnnotations;

namespace NZWalk.API.Models.DTO
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be minimum  of 3 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be minimum  of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be minimum  of 3 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
