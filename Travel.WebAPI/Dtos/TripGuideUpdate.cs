using System.ComponentModel.DataAnnotations;

namespace Travel.API.Dtos
{
    public class TripGuideUpdate
    {

        [Range(1, int.MaxValue, ErrorMessage = "Trip ID must be a positive integer.")]
        public int Id { get; set; }

        public List<int> GuideIds { get; set; } = new();
    }
}
