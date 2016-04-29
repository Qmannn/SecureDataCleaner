﻿using System.Collections.Generic;
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
        public delegate void SaveSecureData(List<SecureData> secureData);
        
        /// <summary>
        /// Метод сохранения полученных secure-данных
        /// </summary>
        public SaveSecureData DataSaver { get; set; }

        public DefaultCleaner(ICleanMode urlCleanMode, ICleanMode requestCleanMode, ICleanMode responseCleanMode)
        {
            _urlCleanMode = urlCleanMode;
            _requestCleanMode = requestCleanMode;
            _responseCleanMode = responseCleanMode;
        }

        public string CleanUrl(string secureUrl)
        {
            CleanResult cleanResult = _urlCleanMode?.CleanString(secureUrl);
            if (cleanResult == null)
            {
                return secureUrl;
            }
            DataSaver?.Invoke(cleanResult.SecureData);
            return cleanResult.CleanString;
        }

        public string CleanReqBody(string secureReqBody)
        {
            CleanResult cleanResult = _requestCleanMode?.CleanString(secureReqBody);
            if (cleanResult == null)
            {
                return secureReqBody;
            }
            DataSaver?.Invoke(cleanResult.SecureData);
            return cleanResult.CleanString;
        }

        public string CleanResBody(string secureResBody)
        {
            CleanResult cleanResult = _responseCleanMode?.CleanString(secureResBody);
            if (cleanResult == null)
            {
                return secureResBody;
            }
            DataSaver?.Invoke(cleanResult.SecureData);
            return cleanResult.CleanString;
        }
    }
}