namespace SecureDataCleaner.Interfaces
{
    public interface IHttpResult
    {
        string Url { get; set; }
        string RequestBody { get; set; }
        string ResponseBody { get; set; }

        /// <summary>
        /// Генератор копий
        /// </summary>
        /// <param name="httpResult">Копируемый объект</param>
        /// <returns>Копия</returns>
        IHttpResult Copy();
    }
}