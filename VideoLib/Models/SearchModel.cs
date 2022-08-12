using VideoLib.Data;

namespace VideoLib.Models
{
    public class SearchModel
    {
        public string? SearchTerm { get; set; }

        public List<VideoAsset> SearchResults { get; set; }

        public string Message { get; set; }

        public bool Searched { get; set; }
    }
}