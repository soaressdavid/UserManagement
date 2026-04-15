namespace UserManagement.API.Utils
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
