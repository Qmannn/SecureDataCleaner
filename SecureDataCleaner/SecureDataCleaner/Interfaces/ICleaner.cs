namespace SecureDataCleaner.Interfaces
{
    /// <summary>
    /// Логика очистки для IHttpresult
    /// При необходимости secure-данные сохранять в имплементации
    /// </summary>
    public interface ICleaner
    {
        /// <summary>
        /// Логика очистки HttpResult.Url
        /// </summary>
        /// <param name="secureUrl">Url с secure-даннфми</param>
        /// <returns>url с затертыми secure-даннфми</returns>
        string CleanUrl(string secureUrl);

        /// <summary>
        /// Логика очистки HttpResult.RequestBody 
        /// </summary>
        /// <param name="secureReqBody">ReqBody с secure-даннфми</param>
        /// <returns>ReqBody с затертыми secure-даннфми</returns>
        string CleanReqBody(string secureReqBody);

        /// <summary>
        /// Логика очистки HttpResult.ResponseBody 
        /// </summary>
        /// <param name="secureResBody">ResBody с secure-даннфми</param>
        /// <returns>ResBody с затертыми secure-даннфми</returns>
        string CleanResBody(string secureResBody);
    }
}