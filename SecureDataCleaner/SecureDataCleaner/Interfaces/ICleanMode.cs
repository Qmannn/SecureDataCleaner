using SecureDataCleaner.Types;

namespace SecureDataCleaner.Interfaces
{
    public interface ICleanMode
    {
        /// <summary>
        /// Обработка строки в соответствии с реализуемым режимом
        /// </summary>
        /// <param name="secureString">Строка для очистки</param>
        /// <returns>Результат: очищенная строка и полученные secure-значения</returns>
        CleanResult CleanString(string secureString);
    }
}