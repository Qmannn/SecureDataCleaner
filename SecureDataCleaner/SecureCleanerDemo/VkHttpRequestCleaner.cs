using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;

namespace SecureCleanerDemo
{
    public class VkHttpResult : IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }

    public class VkHttpRequestCleaner : HttpResultCleaner
    {
        /// <summary>
        /// Шаблон обработки HttpResult's
        /// </summary>
        private static readonly ICleaner VkCleaner = new DefaultCleaner(
            null,
            null,
            null);

        public VkHttpRequestCleaner()
            : base(VkCleaner)
        {
            
        }

        public VkHttpResult Clean(VkHttpResult vkResult)
        {
            return (VkHttpResult) base.Clean(vkResult);
        }
    }
}