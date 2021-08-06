using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.codingChallenge;
using Sabio.Models.Requests;
using Sabio.Models.Requests.CodingChallenge;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers.codingChallenge
{
    [Route("api/courses")]
    [ApiController]
    public class CourseApiController : BaseApiController
    {
        private ICourseService _service = null;

        public CourseApiController(ICourseService service, ILogger<CourseApiController> logger) : base(logger)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemsResponse<Course>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Course course = _service.Get(id);

                if (course == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User record not found");
                }
                else
                {
                    // iCode = 200 (by default)                 
                    response = new ItemResponse<Course> { Item = course };
                }
            }
            catch (ArgumentException argEx)
            {
                iCode = 500;
                response = new ErrorResponse($"ArgumentException Error: {argEx.Message}");
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CourseAddRequest model)
        {

            ObjectResult result = null;

            try
            {
                int id = _service.Add(model);

                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                result = Created201(response);

            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse($"Generic Error: {ex.Message}");
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Course>>> GetPage(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Course> page = _service.GetPage(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Record not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Course>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(CourseUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            
            try
            {
                _service.Update(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {

                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
    }
}
