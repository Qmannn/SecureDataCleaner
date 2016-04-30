using SecureDataCleaner.Interfaces;

namespace SecureDataCleaner
{
    public class HttpResultCleaner
    {
        private readonly ICleaner _cleaner;

        public IHttpResult Clean(IHttpResult secureHttpResult)
        {
            var cleanResult = secureHttpResult.Copy();
            cleanResult.Url = _cleaner.CleanUrl(secureHttpResult.Url);
            cleanResult.RequestBody = _cleaner.CleanReqBody(secureHttpResult.RequestBody);
            cleanResult.ResponseBody = _cleaner.CleanResBody(secureHttpResult.ResponseBody);
            return cleanResult;
        }

        public HttpResultCleaner(ICleaner cleaner)
        {
            _cleaner = cleaner;
        }
        
    }
}