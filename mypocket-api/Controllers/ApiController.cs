using Microsoft.AspNetCore.Mvc;
using mypocket.api.Mapping;
using mypocket.api.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace mypocket.api.Controllers
{
    public abstract class ApiController<T> : Controller where T : BaseObject
    {
        private const int DEFAULT_TIME_OUT = 3000;

        private IDatabaseMapping Mapping { get; }

        public ApiController(IDatabaseMapping mapping)
        {
            this.Mapping = mapping;
        }

        #region " APIS "

        [HttpDelete("{id}")]
        public virtual ActionResult Delete(Guid id)
        {
            try
            {
                var result = Mapping.DeleteFromDataBase(id.ToString());
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
                var result = Mapping.GetListFromDataBase<T>(string.Empty).Result;
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
                var result = Mapping.GetDataFromDataBase<T>(id.ToString()).Result;
                return result;
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public virtual ActionResult Insert([FromBody] T model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                var result = Mapping.SaveIntoDataBase(model);
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return Json(model);
                }

                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        public virtual ActionResult Update(Guid id, [FromBody] T model)
        {
            try
            {
                var result = Mapping.SaveIntoDataBase(model);
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return Json(model);
                }

                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        #endregion " APIS "
    }
}