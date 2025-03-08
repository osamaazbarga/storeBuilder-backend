using superecommere.Models.Products;

namespace superecommere.Repositories.Specification
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;
        
        private List<string> _brands = [];
        private List<string> _types = [];
        private int _pageSize = 6;


        public int PageSize
        {
            get=>_pageSize; 
            set => _pageSize = (value>MaxPageSize)?MaxPageSize:value;
        }
        public List<string> Brands {
            get
            {

                return _brands;
            }
            set{
                _brands = value.SelectMany(x=>x.Split(',',
                    StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
        }

        public List<string> Types
        {
            get => _types;
            set
            {
                //_types = value;
                _types = value.SelectMany(x => x.Split(',',
                    StringSplitOptions.RemoveEmptyEntries)).ToList();
            }
        }

        public string? Sort { get; set; }
        
        private string? _search;
        public string Search
        {
            get => _search ?? "";
            set => _search = value.ToLower();
        }

    }
}
