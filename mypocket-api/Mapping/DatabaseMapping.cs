using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mypocket.api.Mapping
{
    public abstract class DatabaseMapping
    {
        #region " CONSTANTS "

        private const string AUTH = "aCmmLJ1LnlLeUtWEEgY8GQ15EVBvTrsq4YWjKhmV";

        private const string URL_BASE = "https://my-pocket-fire.firebaseio.com";

        #endregion " CONSTANTS "

        #region " PROPERTIES "

        protected abstract string PathName { get; }

        #endregion " PROPERTIES "

        public Task DeleteFromDataBase(string path)
        {
            return GetFirebaseClient().Child(path).DeleteAsync();
        }

        public Task<T> GetDataFromDataBase<T>(string path = "")
        {
            return GetFirebaseClient().Child(GetFullPath(path)).OnceSingleAsync<T>();
        }

        public Task<IReadOnlyCollection<FirebaseObject<T>>> GetListFromDataBase<T>(string path = "")
        {
            return GetFirebaseClient().Child(GetFullPath(path)).OnceAsync<T>();
        }

        private FirebaseClient GetFirebaseClient()
        {
            var options = new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(AUTH) };
            var firebaseClient = new FirebaseClient(URL_BASE, options);
            return firebaseClient;
        }

        private string GetFullPath(string path)
        {
            var fullPath = string.IsNullOrWhiteSpace(path) ? PathName : string.Format("{0}/{1}", PathName, path);
            return fullPath;
        }
    }
}