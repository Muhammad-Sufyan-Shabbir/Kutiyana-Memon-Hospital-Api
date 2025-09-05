namespace Kutiyana_Memon_Hospital_Api.Http
{
    public class PageRequest : IPageRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}