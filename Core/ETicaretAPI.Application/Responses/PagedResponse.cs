namespace ETicaretAPI.Application.Responses
{
    public class PagedResponse<T>
    {
        public int TotalCount { get; set; }
        public List<T> Data { get; set; }

        public PagedResponse(List<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }
    }
}
