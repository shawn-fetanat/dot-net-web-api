using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces.CodingChallenge;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers.codingChallenge
{
    [Route("api/students")]
    [ApiController]
    public class StudentApiController : BaseApiController
    {
        private IStudentService _service = null;

        public StudentApiController(IStudentService service, ILogger<StudentApiController> logger) : base(logger)
        {
            _service = service;
        }

        // api/friends/
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
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
