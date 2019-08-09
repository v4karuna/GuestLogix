namespace GuestLogix.Model
{
    public class SearchResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public SearchResult()
        {
            Success = false;
            Message = string.Empty;
            Data = default;
        }
    }
}
