using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mypocket.api.Mapping
{
    public interface IDatabaseMapping
    {
        Task DeleteFromDataBase(string path);

        Task<T> GetDataFromDataBase<T>(string path);

        Task<IReadOnlyCollection<FirebaseObject<T>>> GetListFromDataBase<T>(string path);

        Task SaveIntoDataBase<T>(T model);
    }
}