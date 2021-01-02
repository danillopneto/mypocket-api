using System.Collections.Generic;

namespace MyPocket.Interfaces.Mapping
{
    public interface IDatabaseMapping
    {
        void DeleteFromDataBase(string path);

        T GetDataFromDataBase<T>(string path);

        IList<T> GetListFromDataBase<T>(string path);

        T SaveIntoDataBase<T>(T model);
    }
}