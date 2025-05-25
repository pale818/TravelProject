namespace Travel.API.Dtos
{
    public class TripGuideUpdate
    {

        public int Id { get; set; }
        public List<int> GuideIds { get; set; } = new();
    }
}
