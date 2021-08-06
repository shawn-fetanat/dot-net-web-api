using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        private IUsersService _service = null;
        private IAuthenticationService<int> _authService = null;
        
        public UserApiController(IUsersService service
            , ILogger<UserApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        // api/users/
        [HttpGet]
        public ActionResult<ItemsResponse<User>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<User> list = _service.GetTop();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User record not found");
                }
                else
                {
                    // iCode = 200;
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            catch (Exception ex)
            {

                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        // api/users/{id:int}
        [HttpGet("{id:int}")]
        public ActionResult<ItemsResponse<User>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                User user = _service.Get(id);

                if (user == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("User record not found");
                }
                else
                {
                    // iCode = 200 (by default)                 
                    response = new ItemResponse<User> { Item = user };
                }
            }
            catch (SqlException sqlEx)
            {
                iCode = 500;
                response = new ErrorResponse($"SqlException Error: {sqlEx.Message}");
                base.Logger.LogError(sqlEx.ToString());
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

        // api/users/
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null; // do not declare instance

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

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(UserAddRequest model)
        {
            //The default response code in this instance is 201. 
            //BUT we do not need it because of what we do below in the try block
            //int iCode = 201;

            //we need this instead of the BaseResponse
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                //if this operation errors, it would generate an exception and jump to the catch
                int id = _service.Add(model, userId);

                //if this operation errors, it would generate an exception and jump to the catch
                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                //This sets the status code for us but also set Url that points back to 
                // the Get By Id endpoint. Setting a Url in the Response (for a 201 Response code) is a common practice
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

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(UserUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null; // do not declare instance

            // id of new user
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

        [HttpGet("search")]
        public ActionResult<ItemsResponse<User>> Search(string q)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<User> list = _service.Search(q);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Query does not exist in database.");
                }
                else
                {
                    // iCode = 200;
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            catch (Exception ex)
            {

                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<User>>> GetPage(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;//do not declare an instance.

            try
            {
                Paged<User> page = _service.GetPage(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Record not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<User>> { Item = page };
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

    }
}
