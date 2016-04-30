using SecureDataCleaner.Interfaces;

namespace SecureDataCleaner
{
    public class HttpResultCleaner
    {
        private readonly ICleaner _cleaner;

        protected IHttpResult Clean(IHttpResult secureHttpResult)
        {
            secureHttpResult.Url = _cleaner.CleanUrl(secureHttpResult.Url);
            secureHttpResult.RequestBody = _cleaner.CleanReqBody(secureHttpResult.RequestBody);
            secureHttpResult.ResponseBody = _cleaner.CleanResBody(secureHttpResult.ResponseBody);
            return secureHttpResult;
        }

        public HttpResultCleaner(ICleaner cleaner)
        {
            _cleaner = cleaner;
        }
        
    }
}