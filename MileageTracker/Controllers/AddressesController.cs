using MileageTracker.Extensions;
using MileageTracker.Infrastructure.Logging;
using MileageTracker.Interfaces;
using MileageTracker.Models;
using System;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MileageTracker.Controllers {
    [Authorize]
    public class AddressesController: ApiController {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService) {
            _addressService = addressService;
        }

        /// <summary>
        /// Receive a paginated list of addresses created by the user.
        /// 
        /// EXAMPLE CALL: /api/addresses?pagenumber=1&amp;pagesize=15
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Maximum number of items to return per page</param>
        [HttpGet]
        [Route("api/addresses")]
        public HttpResponseMessage GetAddressesPaginated([FromUri] int pageNumber = 1, [FromUri] int pageSize = 15) {
            try {
                var addresses = _addressService.GetAddressesPaginated(pageNumber, pageSize);
                return Request.CreateResponse(HttpStatusCode.OK, addresses);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Receive an address with a given ID.
        /// </summary>
        /// <param name="id">ID of address</param>
        [HttpGet]
        [Route("api/addresses/{id}")]
        public HttpResponseMessage Get(int id) {
            try {
                var address = _addressService.GetAddressById(id);
                if (address == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, address);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Create a new address
        /// </summary>
        /// <param name="address">Address to create</param>
        [HttpPost]
        [Route("api/addresses")]
        public HttpResponseMessage AddAddress([FromBody]Address address) {
            try {
                if (address == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                address = _addressService.AddAddress(address);
                return Request.CreateResponse(HttpStatusCode.Created, address);
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, ex.GetErrorResult());
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing address
        /// </summary>
        /// <param name="id">ID of address to update</param>
        /// <param name="address">Address data to update</param>
        [HttpPut]
        [Route("api/addresses/{id}")]
        public HttpResponseMessage UpdateAddress(int id, [FromBody]Address address) {
            try {
                if (address == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                address.Id = id;
                address = _addressService.UpdateAddress(address);
                return Request.CreateResponse(HttpStatusCode.OK, address);
            } catch (ObjectNotFoundException) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, ex.GetErrorResult());
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Delete an address
        /// </summary>
        /// <param name="id">ID of address</param>
        [HttpDelete]
        [Route("api/addresses/{id}")]
        public HttpResponseMessage DeleteAddress(int id) {
            try {
                _addressService.DeleteAddress(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (ObjectNotFoundException) {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, ex.GetErrorResult());
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}