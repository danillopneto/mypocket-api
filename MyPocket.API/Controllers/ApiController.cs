using Microsoft.AspNetCore.Mvc;
using MyPocket.Domain;
using MyPocket.Interfaces.Mapping;
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

        /// <summary>
        /// Delete the item from database.
        /// </summary>
        /// <param name="id">Identifier of the item.</param>
        /// <returns>Result of the deletion.</returns>
        /// <response code="200">Success by getting the products.</response>
        /// <response code="500">Internal error by getting the product.</response>
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

        /// <summary>
        /// Get the list of all items.
        /// </summary>
        /// <returns>The list of the item.</returns>
        /// <response code="200">Success by getting the products.</response>
        /// <response code="500">Internal error by getting the product.</response>
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

        /// <summary>
        /// Get an specific item.
        /// </summary>
        /// <param name="id">Identifier of the item.</param>
        /// <returns>The item with the Identifier informed.</returns>
        /// <response code="200">Success by getting the item.</response>
        /// <response code="500">Internal error by getting the item.</response>
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

        /// <summary>
        /// Insert the item into database.
        /// </summary>
        /// <param name="model">Model to be saved.</param>
        /// <returns>The item that was saved with its Id.</returns>
        /// <response code="200">Success by inserting the item.</response>
        /// <response code="500">Internal error by inserting the item.</response>
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

        /// <summary>
        /// Update a specific item.
        /// </summary>
        /// <param name="id">Identifier of the item.</param>
        /// <param name="model">Model to be saved.</param>
        /// <returns>The item that was saved.</returns>
        /// <response code="200">Success by getting the items.</response>
        /// <response code="500">Internal error by getting the items.</response>
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