using System;
using System.Resources;
using System.Runtime.ConstrainedExecution;
using SecureCleanerDemo.HttpResultCleaners;
using SecureDataCleaner.Interfaces;

namespace SecureCleanerDemo
{
    class Program
    {
        /// <summary>
        /// Простая демонстрация работы библиотеки
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            BookingcomHttpResultCleaner bookingcomHttpResultCleaner = new BookingcomHttpResultCleaner();
            var bookingcomHttpResult = new BookingcomHttpResult
            {
                Url = "http://test.com?user=max&pass=123456",
                RequestBody = "<auth><user>max</user><pass>123456</pass></auth>",
                ResponseBody = "<auth user='max' pass='123456'>"
            };

            Console.WriteLine("BookingcomHttpResult");
            Console.WriteLine("FROM");
            PrintHttpResult(bookingcomHttpResult);
            bookingcomHttpResult = bookingcomHttpResultCleaner.Clean(bookingcomHttpResult);
            Console.WriteLine("TO");
            PrintHttpResult(bookingcomHttpResult);

            OstrovokHttpResultCleaner ostrovokHttpResultCleaner = new OstrovokHttpResultCleaner();
            var ostrovokHttpResult = new OstrovokHttpResult
            {
                Url = "http://test.com/users/max/info",
                RequestBody = "{user:'max',pass:'123456'}",
                ResponseBody = "{user:{value:'max'},pass:{value:'123456'}}"
            };

            Console.WriteLine("OstrovokHttpResult");
            Console.WriteLine("FROM");
            PrintHttpResult(ostrovokHttpResult);
            ostrovokHttpResult = ostrovokHttpResultCleaner.Clean(ostrovokHttpResult);
            Console.WriteLine("TO");
            PrintHttpResult(ostrovokHttpResult);

            AgodaHttpResultCleaner agodaHttpResultCleaner = new AgodaHttpResultCleaner();
            var agodaHttpResult = new AgodaHttpResult
            {
                Url = "http://test.com?user=max&pass=123456",
                RequestBody = @"
<auth>
    <user>max</user>
    <pass>123456</pass>
</auth>",
                ResponseBody = "<auth user='max' pass='123456'>"
            };

            Console.WriteLine("AgodaHttpResult");
            Console.WriteLine("FROM");
            PrintHttpResult(agodaHttpResult);
            agodaHttpResult = agodaHttpResultCleaner.Clean(agodaHttpResult);
            Console.WriteLine("TO");
            PrintHttpResult(agodaHttpResult);

            ExpediaHttpResultCleaner expediaHttpResultCleaner = new ExpediaHttpResultCleaner();

            var expediaHttpResult = new ExpediaHttpResult
            {
                Url = "http://test.com/users/max/info",
                RequestBody = @"
{
       user : 'max',
       pass : '123456'
}
",
                ResponseBody = @"
{
       user : {
             value : 'max'
       },
       pass : {
             value : '123456'
       }
}
"
            };  

            Console.WriteLine("AgodaHttpResult");
            Console.WriteLine("FROM");
            PrintHttpResult(expediaHttpResult);
            expediaHttpResult = expediaHttpResultCleaner.Clean(expediaHttpResult);
            Console.WriteLine("TO");
            PrintHttpResult(expediaHttpResult);

            
        }

        static void PrintHttpResult(IHttpResult result)
        {
            Console.WriteLine(result.Url);
            Console.WriteLine(result.RequestBody);
            Console.WriteLine(result.ResponseBody);
            Console.WriteLine();
        }
    }
}
