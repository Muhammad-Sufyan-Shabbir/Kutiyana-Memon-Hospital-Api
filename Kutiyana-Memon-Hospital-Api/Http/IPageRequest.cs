namespace Kutiyana_Memon_Hospital_Api.Http
{
    public interface IPageRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}