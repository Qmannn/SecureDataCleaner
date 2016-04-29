using SecureDataCleaner.Interfaces;

namespace SecureDataCleaner
{
    public class HttpResultCleaner
    {
        private readonly ICleaner _cleaner;
        private readonly IHttpResult _httpResult;
        public string Url
        {
            get { return _httpResult.Url; }
            set { _httpResult.Url = value; }
        }

        public string RequestBody
        {
            get { return _httpResult.RequestBody; }
            set { _httpResult.RequestBody = value; }
        }

        public string ResponseBody
        {
            get { return _httpResult.ResponseBody; }
            set { _httpResult.ResponseBody = value; }
        }

        public IHttpResult CleanResult
        {
            get { return _httpResult; }
        }

        public HttpResultCleaner(IHttpResult secureResult, ICleaner cleaner)
        {
            _cleaner = cleaner;
            _httpResult = secureResult;
        }
        
    }
}