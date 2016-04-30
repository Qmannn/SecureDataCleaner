using System.Collections.Generic;
using SecureDataCleaner.Interfaces;
using SecureDataCleaner.Types;

namespace SecureDataCleaner
{
    public class DefaultCleaner : ICleaner
    {
        private readonly ICleanMode _urlCleanMode;
        private readonly ICleanMode _requestCleanMode;
        private readonly ICleanMode _responseCleanMode;

        /// <summary>
        /// Делегат сохранения полученный secure-данных
        /// </summary>
        public delegate void SaveSecureData(CleanResult secureData);
        
        /// <summary>
        /// Метод сохранения полученных secure-данных
        /// </summary>
        public SaveSecureData DataSaver { get; set; }

        /// <summary>
        /// Инициализация объекта-cleaner'a. 
        /// null для параметра -  не очищать соответствующую строку
        /// </summary>
        /// <param name="urlCleanMode">Режим чистки URL</param>
        /// <param name="requestCleanMode">Режим чистки request</param>
        /// <param name="responseCleanMode">Режим чистки response</param>
        public DefaultCleaner(ICleanMode urlCleanMode, ICleanMode requestCleanMode, ICleanMode responseCleanMode)
        {
            _urlCleanMode = urlCleanMode;
            _requestCleanMode = requestCleanMode;
            _responseCleanMode = responseCleanMode;
        }

        public string CleanUrl(string secureUrl)
        {
            CleanResult cleanResult = _urlCleanMode == null ? null : _urlCleanMode.CleanString(secureUrl);
            if (cleanResult == null)
            {
                return secureUrl;
            }
            if (DataSaver != null)
            {
                DataSaver.Invoke(cleanResult);
            }
            return cleanResult.CleanString;
        }

        public string CleanReqBody(string secureReqBody)
        {
            CleanResult cleanResult = _requestCleanMode == null ? null : _requestCleanMode.CleanString(secureReqBody);
            if (cleanResult == null)
            {
                return secureReqBody;
            }
            if (DataSaver != null)
            {
                DataSaver.Invoke(cleanResult);
            }
            return cleanResult.CleanString;
        }

        public string CleanResBody(string secureResBody)
        {
            CleanResult cleanResult = _responseCleanMode == null ? null : _responseCleanMode.CleanString(secureResBody);
            if (cleanResult == null)
            {
                return secureResBody;
            }
            if (DataSaver != null)
            {
                DataSaver.Invoke(cleanResult);
            }
            return cleanResult.CleanString;
        }
    }
}