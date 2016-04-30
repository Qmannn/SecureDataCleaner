using System.IO;
using SecureDataCleaner;
using SecureDataCleaner.CleanModes;
using SecureDataCleaner.Interfaces;
using SecureDataCleaner.Types;

namespace SecureCleanerDemo.HttpResultCleaners
{
    public class BookingcomHttpResult : IHttpResult
    {
        public string Url { get; set; }
        public string RequestBody { get; set; }
        public string ResponseBody { get; set; }
        public IHttpResult Copy()
        {
            return new BookingcomHttpResult()
            {
                Url = Url,
                RequestBody = RequestBody,
                ResponseBody = ResponseBody
            };
        }
    }

    public class BookingcomHttpResultCleaner : HttpResultCleaner
    {
        private static readonly object SyncFileAccessObject = new object();
        public static string ResultFileName { get; set; }

        /// <summary>
        /// Шаблон обработки полей BookingcomHttpResult's
        /// </summary>
        private static readonly ICleaner BookingcomCleaner = new DefaultCleaner(
            new NoSpaces("http://test.com?user={templValue:user}&pass={templValue:pass}"),
            new SomeSeparators("<auth><user>{templValue:user}</user><pass>{templValue:pass}</pass></auth>"),
            new SomeSeparators("<auth user='{templValue:user}' pass='{templValue:pass}'>"))
        {
            DataSaver = SecureDataSaver
        };

        /// <summary>
        /// Инициализация объекта
        /// </summary>
        public BookingcomHttpResultCleaner()
            : base(BookingcomCleaner)
        {
            ResultFileName = "BookingcomResultFile.txt"; // on default file name
        }

        /// <summary>
        /// Очистка объекта от secure-данных
        /// </summary>
        /// <param name="bookingcomResult">Очищаемый обхект</param>
        /// <returns>Очищенный объект</returns>
        public BookingcomHttpResult Clean(BookingcomHttpResult bookingcomResult)
        {
            return (BookingcomHttpResult)base.Clean(bookingcomResult);
        }

        private static void SecureDataSaver(CleanResult result)
        {
            lock (SyncFileAccessObject)
            {
                using (StreamWriter sw = File.AppendText(ResultFileName))
                {
                    sw.WriteLine(result.CleanString);
                    foreach (var secureData in result.SecureData)
                    {
                        sw.WriteLine("{0} : {1}", secureData.Key, secureData.Value);
                    }
                    sw.WriteLine("---");
                }
            }
        }
    }
}