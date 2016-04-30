using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;

namespace SecureCleanerDemo.HttpResultCleaners
{
    public class BookingcomHttpResult : IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
    }
    public class BookingcomHttpResultCleaner : HttpResultCleaner
    {
        /// <summary>
        /// Шаблон обработки HttpResult's
        /// </summary>
        private static readonly ICleaner BookingcomCleaner = new DefaultCleaner(
            new NoSpaces("http://test.com?user={templValue:user}&pass={templValue:pass}"),
            new SomeSeparators("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
            new SomeSeparators("<auth user='{templValue:user}' pass='{templValue:pass}'>"));

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        public BookingcomHttpResultCleaner()
            : base(BookingcomCleaner)
        {

        }


    }
}