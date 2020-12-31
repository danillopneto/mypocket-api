using Microsoft.AspNetCore.Mvc;
using mypocket.api.Mapping;
using System;
using System.Collections.Generic;
using System.Net;

namespace mypocket.api.Controllers
{
    public abstract class ApiController<T> : Controller
    {
        private const int DEFAULT_TIME_OUT = 3000;

        private IDatabaseMapping DataBase { get; }

        public ApiController(IDatabaseMapping dataBase)
        {
            this.DataBase = dataBase;
        }

        #region " APIS "

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(Guid id)
        {
            try
            {
                var result = DataBase.DeleteFromDataBase(string.Format("/{0}", id));
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
            try
            {
                var result = DataBase.GetListFromDataBase<T>(string.Empty).Result;
                if (result != null)
                {
                    var data = new List<T>();
                    foreach (var item in result)
                    {
                        data.Add(item.Object);
                    }

                    return data;
                }

                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{id}")]
        public virtual ActionResult<T> Get(Guid id)
        {
            try
            {
                var result = DataBase.GetDataFromDataBase<T>(id.ToString()).Result;
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion " APIS "
    }
}