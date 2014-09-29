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
    public class TripsController : ApiController {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService) {
            _tripService = tripService;
        }

        /// <summary>
        /// Receive a paginated list of trips created by the user.
        /// 
        /// EXAMPLE CALL: /api/trips?pagenumber=1&amp;pagesize=15
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Maximum number of items to return per page</param>
        [HttpGet]
        [Route("api/trips")]
        public HttpResponseMessage GetTripsPaginated([FromUri] int pageNumber = 1, [FromUri] int pageSize = 15) {
            try {
                var trips = _tripService.GetTripsPaginated(pageNumber, pageSize);
                return Request.CreateResponse(HttpStatusCode.OK, trips);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Reveice a template of a trip for creating a new trip with the current date, origin address prefilled with the last destination address 
        /// and the car prefilled with the users last used car.
        /// </summary>
        [HttpGet]
        [Route("api/trips/template")]
        public HttpResponseMessage GetNewTripTemplate() {
            try {
                var trip = _tripService.GetNewTripTemplate();
                return Request.CreateResponse(HttpStatusCode.OK, trip);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Receive a trip with a given ID
        /// </summary>
        /// <param name="id">ID of trip</param>
        [HttpGet]
        [Route("api/trips/{id}")]
        public HttpResponseMessage Get(int id) {
            try {
                var trip = _tripService.GetTripById(id);
                if (trip == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Not found."});
                }
                return Request.CreateResponse(HttpStatusCode.OK, trip);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Create a new trip
        /// </summary>
        /// <param name="trip">Trip to create</param>
        [HttpPost]
        [Route("api/trips")]
        public HttpResponseMessage AddTrip([FromBody]Trip trip) {
            try {
                if (trip == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                trip = _tripService.AddTrip(trip);
                return Request.CreateResponse(HttpStatusCode.Created, trip);
            } catch (DbEntityValidationException ex) {
                throw new HttpResponseException(Request.CreateResponse((HttpStatusCode)422, new { message = ex.GetErrorResult() }));
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update an existing trip
        /// </summary>
        /// <param name="id">ID of trip to update</param>
        /// <param name="trip">Trip data to update</param>
        [HttpPut]
        [Route("api/trips/{id}")]
        public HttpResponseMessage UpdateTrip(int id, [FromBody]Trip trip) {
            try {
                if (trip == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                trip.Id = id;
                trip = _tripService.UpdateTrip(trip);
                return Request.CreateResponse(HttpStatusCode.OK, trip);
            } catch (ObjectNotFoundException) {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Not found." });
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, new { message = ex.GetErrorResult() });
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Delete a trip
        /// </summary>
        /// <param name="id">ID of trip</param>
        [HttpDelete]
        [Route("api/trips/{id}")]
        public HttpResponseMessage DeleteTrip(int id) {
            try {
                _tripService.DeleteTrip(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            } catch (ObjectNotFoundException) {
                return Request.CreateResponse(HttpStatusCode.NotFound, new { message = "Not found." });
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, new { message = ex.GetErrorResult() });
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
