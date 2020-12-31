using Firebase.Database;
using mypocket.api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace mypocket.api.Mapping
{
    public abstract class DatabaseMapping<TDto> : IDatabaseMapping where TDto : BaseObject
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
            return GetFirebaseClient().Child(GetFullPath(path)).DeleteAsync();
        }

        public Task<T> GetDataFromDataBase<T>(string path = "")
        {
            return GetFirebaseClient().Child(GetFullPath(path)).OnceSingleAsync<T>();
        }

        public Task<IReadOnlyCollection<FirebaseObject<T>>> GetListFromDataBase<T>(string path = "")
        {
            return GetFirebaseClient().Child(GetFullPath(path)).OnceAsync<T>();
        }

        public Task SaveIntoDataBase<T>(T model)
        {
            var dto = model as TDto;
            return GetFirebaseClient().Child(GetFullPath(dto.Id.ToString())).PutAsync(ConvertToJson(model));
        }

        #region " PRIVATE METHODS "

        private string ConvertToJson<T>(T model)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(model, serializerSettings);
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

        #endregion " PRIVATE METHODS "
    }
}