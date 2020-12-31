using Firebase.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace mypocket.api.Controllers
{
    public abstract class ApiController<T> : Controller
    {
        #region " CONSTANTS "

        private const string AUTH = "aCmmLJ1LnlLeUtWEEgY8GQ15EVBvTrsq4YWjKhmV";

        private const int DEFAULT_TIME_OUT = 3000;

        private const string URL_BASE = "https://my-pocket-fire.firebaseio.com";

        #endregion " CONSTANTS "

        #region " PROPERTIES "

        protected abstract string PathName { get; }

        #endregion " PROPERTIES "

        #region " APIS "

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(Guid id)
        {
            try
            {
                var result = DeleteFromDataBase(string.Format("/{0}", id));
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return StatusCode((int)HttpStatusCode.OK);
                }

                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        public virtual ActionResult<IEnumerable<T>> Get()
        {
            var result = GetListFromDataBase().Result;
            if (result != null)
            {
                var data = new List<T>();
                foreach (var item in result)
                {
                    data.Add(item.Object);
                }

                return data;
            }

            return new EmptyResult();
        }

        [HttpGet("{id}")]
        public virtual ActionResult<T> Get(Guid id)
        {
            try
            {
                var result = GetDataFromDataBase(id.ToString()).Result;
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion " APIS "

        protected Task DeleteFromDataBase(string path)
        {
            return GetFirebaseClient().Child(path).DeleteAsync();
        }

        protected Task<T> GetDataFromDataBase(string path = "")
        {
            return GetFirebaseClient().Child(GetFullPath(path)).OnceSingleAsync<T>();
        }

        protected Task<IReadOnlyCollection<FirebaseObject<T>>> GetListFromDataBase(string path = "")
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