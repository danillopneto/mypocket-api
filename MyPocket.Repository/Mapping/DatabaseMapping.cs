using Firebase.Database;
using MyPocket.Domain;
using MyPocket.Interfaces.Mapping;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyPocket.Repository.Mapping
{
    public abstract class DatabaseMapping<TDto> : IDatabaseMapping where TDto : BaseObject
    {
        #region " CONSTANTS "

        private const string AUTH = "aCmmLJ1LnlLeUtWEEgY8GQ15EVBvTrsq4YWjKhmV";

        private const int DEFAULT_TIME_OUT = 3000;

        private const string URL_BASE = "https://my-pocket-fire.firebaseio.com";

        #endregion " CONSTANTS "

        #region " PROPERTIES "

        protected abstract string PathName { get; }

        #endregion " PROPERTIES "

        public void DeleteFromDataBase(string path)
        {
            var result = GetFirebaseClient().Child(GetFullPath(path)).DeleteAsync();
            result.Wait(DEFAULT_TIME_OUT);
            if (!result.IsCompleted)
            {
                throw new TimeoutException();
            }
        }

        public T GetDataFromDataBase<T>(string path = "")
        {
            var result = GetFirebaseClient().Child(GetFullPath(path)).OnceSingleAsync<T>();
            result.Wait(DEFAULT_TIME_OUT);
            if (!result.IsCompleted)
            {
                throw new TimeoutException();
            }

            return result.Result;
        }

        public IList<T> GetListFromDataBase<T>(string path = "")
        {
            var result = GetFirebaseClient().Child(GetFullPath(path)).OnceAsync<T>();
            result.Wait(DEFAULT_TIME_OUT);
            if (result.IsCompleted)
            {
                if (result != null)
                {
                    var data = new List<T>();
                    foreach (var item in result.Result)
                    {
                        data.Add(item.Object);
                    }

                    return data;
                }
            }
            else
            {
                throw new TimeoutException();
            }

            return null;
        }

        public T SaveIntoDataBase<T>(T model)
        {
            var dto = model as TDto;
            if (!dto.Id.HasValue)
            {
                dto.Id = Guid.NewGuid();
            }

            var result = GetFirebaseClient().Child(GetFullPath(dto.Id.ToString())).PutAsync(ConvertToJson(model));
            result.Wait(DEFAULT_TIME_OUT);
            return result.IsCompleted
                    ? model
                    : throw new TimeoutException();
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