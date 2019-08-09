using CsvHelper.Configuration;
using System.Collections.Generic;

namespace GuestLogix.Services
{
    public interface ICsvMapper
    {
        IList<T> ParseRecords<T>(string filePath, ClassMap classMap = null);
    }
}
