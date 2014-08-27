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
    public class CarsController: ApiController {
        private readonly ICarService _carService;

        public CarsController(ICarService carService) {
            _carService = carService;
        }

        /// <summary>
        /// Receive a paginated list of cars created by the user.
        /// 
        /// EXAMPLE CALL: /api/cars?pagenumber=1&amp;pagesize=15
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Maximum number of items to return per page</param>
        [HttpGet]
        [Route("api/cars")]
        public HttpResponseMessage GetCarsPaginated([FromUri] int pageNumber = 1, [FromUri] int pageSize = 15) {
            try {
                var cars = _carService.GetCarsPaginated(pageNumber, pageSize);
                return Request.CreateResponse(HttpStatusCode.OK, cars);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Receive a car with a given ID
        /// </summary>
        /// <param name="id">ID of car</param>
        [HttpGet]
        [Route("api/cars/{id}")]
        public HttpResponseMessage Get(int id) {
            try {
                var car = _carService.GetCarById(id);
                if (car == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, car);
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Create a new car
        /// </summary>
        /// <param name="car">Car to create</param>
        [HttpPost]
        [Route("api/cars")]
        public HttpResponseMessage AddCar([FromBody]Car car) {
            try {
                if (car == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                car = _carService.AddCar(car);
                return Request.CreateResponse(HttpStatusCode.Created, car);
            } catch (DbEntityValidationException ex) {
                return Request.CreateResponse((HttpStatusCode)422, ex.GetErrorResult());
            } catch (Exception ex) {
                LoggingFactory.GetLogger().LogError(ex.Message, ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// Update an existing car
        /// </summary>
        /// <param name="id">ID of car to update</param>
        /// <param name="car">Car data to update</param>
        [HttpPut]
        [Route("api/cars/{id}")]
        public HttpResponseMessage UpdateCar(int id, [FromBody]Car car) {
            try {
                if (car == null) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                car.Id = id;
                car = _carService.UpdateCar(car);
                return Request.CreateResponse(HttpStatusCode.OK, car);
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
        /// Delete a car
        /// </summary>
        /// <param name="id">ID of car</param>
        [HttpDelete]
        [Route("api/cars/{id}")]
        public HttpResponseMessage DeleteCar(int id) {
            try {
                _carService.DeleteCar(id);
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