using System.Collections.Generic;

namespace MyPocket.Interfaces
{
    public interface IBaseRepository
    {
        void DeleteFromDataBase(string path);

        T GetDataFromDataBase<T>(string path);

        IList<T> GetListFromDataBase<T>(string path);

        T SaveIntoDataBase<T>(T model);
    }
}