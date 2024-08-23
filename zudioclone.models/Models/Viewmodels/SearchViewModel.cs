




using System.Collections.Generic;

namespace zudioclone.models.Models.Viewmodels
{
    public class SearchResultViewModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string BodyText { get; set; }
        public string Excerpt { get; set; }

     
    }

    public class SearchViewModel
    {
        public string Query { get; set; }
        public IEnumerable<SearchResultViewModel> SearchResults { get; set; } = new List<SearchResultViewModel>();
        public bool HasSearched { get; set; }
    }
}


