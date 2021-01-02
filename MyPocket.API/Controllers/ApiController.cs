using Microsoft.AspNetCore.Mvc;
using MyPocket.API.Mapping;
using MyPocket.Domain;
using System;
using System.Collections.Generic;
using System.Net;

namespace MyPocket.API.Controllers
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
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public virtual ActionResult Delete(Guid id)
        {
            try
            {
                var result = Mapping.DeleteFromDataBase(id.ToString());
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return Ok();
                }

                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
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

                    return Ok(data);
                }

                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public virtual ActionResult<T> Get(Guid id)
        {
            try
            {
                var result = Mapping.GetDataFromDataBase<T>(id.ToString()).Result;
                if (result == null)
                {
                    return NotFound(new { message = "The item was not found." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public virtual ActionResult Insert([FromBody] T model)
        {
            try
            {
                model.Id = Guid.NewGuid();
                var result = Mapping.SaveIntoDataBase(model);
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return Ok(model);
                }

                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public virtual ActionResult Update(Guid id, [FromBody] T model)
        {
            try
            {
                var result = Mapping.SaveIntoDataBase(model);
                result.Wait(DEFAULT_TIME_OUT);
                if (result.IsCompleted)
                {
                    return Ok(model);
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