namespace SecureDataCleaner.Interfaces
{
    public interface IHttpResult
    {
        string Url { get; set; }
        string RequestBody { get; set; }
        string ResponseBody { get; set; }
    }
}