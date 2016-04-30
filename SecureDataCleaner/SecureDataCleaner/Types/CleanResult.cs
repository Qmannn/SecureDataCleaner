using System.Collections.Generic;

namespace SecureDataCleaner.Types
{
    public class CleanResult
    {
        public string CleanString { get; set; }

        public List<SecureData> SecureData { get; private set; }


        public CleanResult()
        {
            SecureData = new List<SecureData>();
        }
    }
}