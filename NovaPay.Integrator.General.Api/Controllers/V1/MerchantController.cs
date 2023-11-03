using Common.Libraries.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NovaPay.Integrator.Common.Services.Resources.Requests;
using NovaPay.Integrator.Common.Services.Services.Merchants;
using NovaPay.Integrator.General.Application.Resources.Responses;
using NovaPay.Integrator.General.Application.Services;

namespace NovaPay.Integrator.General.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api")]
    [ApiVersion("1.0")]
    [ApiController]
    public class MerchantController : ControllerBase
    {
        private readonly IMerchantService _merchantService;
        private readonly IApiService _apiService;
        private readonly ILocalService _LocalService;
        private IHttpContextAccessor _httpContextAccessor;
        string _sessionKey;

        public MerchantController(IHttpContextAccessor httpContextAccessor,IMerchantService merchantService, IApiService apiService, ILocalService localService)
        {
            _merchantService = merchantService;
            _apiService = apiService;
            _LocalService = localService;
            _httpContextAccessor = httpContextAccessor;
            _sessionKey = "TOKEN_SESSION";
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateReference([FromBody] ValidationRequest request)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(request.ServiceIdentifier);
            if(merchant == null)
            {
                return NotFound();
            }

            var isLocal = merchant.MerchantConfig?.ValidationConfig?.IsLocal != null ? merchant.MerchantConfig.ValidationConfig.IsLocal : false;
            if (isLocal)
            {
                var result = await _LocalService.ValidateReference(request.Reference, request.ServiceIdentifier);
                return Ok(new Common.Services.Resources.Responses.ValidationResponse
                {
                  Name = result.CustomerName,
                  Success = true,
                  Payload = new
                  {
                      result.CustomerRef,
                      result.Details,
                      result.ServiceUniqueIdentifier,
                      result.CustomerName
                  }
                });

            }
            else
            {
                var token = "";//await RetrieveToken();
                var url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}/validate/{request.Reference}";
                if (merchant.MerchantConfig.ApiConfig.IsCustom)
                {
                    var validationMethod = merchant?.MerchantProcesses?.FirstOrDefault(p => p.IsValidation);
                    url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}{validationMethod.Name}/{request.Reference}";
                    var mappings = merchant.MerchantValidationResponseMappings;
                    var dict = new Dictionary<string, string>();
                    foreach (var mapping in mappings)
                    {
                        dict.Add(mapping.MerchantResponseField, mapping.LocalResponseField);
                    }
                    var result = await _apiService.ValidateReference(token, url, true, dict);
                    return Ok(new Common.Services.Resources.Responses.ValidationResponse
                    {
                        Name = result.Name,
                        Payload = result.Payload,
                        Success = result.Success,
                    });

                }
                else
                {
                    var result = await _apiService.ValidateReference(token, url);
                    return Ok(new Common.Services.Resources.Responses.ValidationResponse
                    {
                        Name = result.Name,
                        Payload = result.Payload,
                        Success = result.Success,
                    });
                }
            }
            
        }
        [HttpPost("postProductTransaction")]
        public async Task<IActionResult> PostProductTransaction([FromBody] ProductTransactionRequest request)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(request.ServiceUniqueId);
            if (merchant == null)
            {
                return NotFound();
            }
            var isLocal = merchant.MerchantConfig?.ValidationConfig?.IsLocal != null ? merchant.MerchantConfig.ValidationConfig.IsLocal : false;
            if (isLocal)
            {
                var result = await _LocalService.PostProductTransaction(request.ServiceUniqueId, request);
                return Ok(new Common.Services.Resources.Responses.TransactionResponse
                {
                    Success = result,
                    Payload = new
                    {
                        Message = result ? "Transaction succesful" : "transaction not successful",
                        TransactionId = request.TransactionRef,
                    }

                });
            }
            else
            {
                var token = await RetrieveToken();
                var url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}/transaction";
                if (merchant.MerchantConfig.ApiConfig.IsCustom)
                {
                    var transactionMethod = merchant?.MerchantProcesses?.FirstOrDefault(p => p.IsPostTransaction);
                    url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}/{transactionMethod}";
                    var mappings = merchant.MerchantPaymentResponseMappings;
                    var dict = new Dictionary<string, string>();
                    foreach (var mapping in mappings)
                    {
                        dict.Add(mapping.MerchantResponseField, mapping.LocalResponseField);
                    }
                    var result = await _apiService.PostProductTransaction(token, url, request, true, dict);
                    return Ok(new Common.Services.Resources.Responses.TransactionResponse
                    {
                        Success = result.Success,

                        Payload = new
                        {
                            Message = result.Success ? "Transaction succesful" : "transaction not successful",
                            TransactionId = request.TransactionRef,
                        }

                    });

                }
                else
                {
                    var result = await _apiService.PostProductTransaction(token, url, request);
                    return Ok(new Common.Services.Resources.Responses.TransactionResponse
                    {
                        Success = result.Success,

                        Payload = new
                        {
                            Message = result.Success ? "Transaction succesful" : "transaction not successful",
                            TransactionId = request.TransactionRef,
                        }

                    });
                }
            }
        }
        [HttpPost("postTransaction")]
        public async Task<IActionResult> PostTransaction([FromBody] TransactionRequest request)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(request.ServiceUniqueId);
            if (merchant == null)
            {
                return NotFound();
            }
            var isLocal = merchant.MerchantConfig?.ValidationConfig?.IsLocal != null ? merchant.MerchantConfig.ValidationConfig.IsLocal : false;
            if (isLocal)
            {
                var result = await _LocalService.PostTransaction(request.ServiceUniqueId,request);
                return Ok(new Common.Services.Resources.Responses.TransactionResponse
                {
                    Success = result,
                    Payload = new
                    {
                        Message = result ? "Transaction succesful" : "transaction not successful",
                        TransactionId = request.TransactionRef,
                    }

                });
            }
            else
            {
                var token = await RetrieveToken();
                var url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}/transaction";
                if (merchant.MerchantConfig.ApiConfig.IsCustom)
                {
                    var transactionMethod = merchant?.MerchantProcesses?.FirstOrDefault(p => p.IsPostTransaction);
                    url = $"{merchant.MerchantConfig.ApiConfig.GeneralApiConfig.BaseUrl}/{transactionMethod}";
                    var mappings = merchant.MerchantPaymentResponseMappings;
                    var dict = new Dictionary<string, string>();
                    foreach (var mapping in mappings)
                    {
                        dict.Add(mapping.MerchantResponseField, mapping.LocalResponseField);
                    }
                    var result = await _apiService.PostTransaction(token, url,request, true, dict);
                    return Ok(new Common.Services.Resources.Responses.TransactionResponse
                    {
                        Success = result.Success,

                        Payload = new
                        {
                            Message = result.Success ? "Transaction succesful" : "transaction not successful",
                            TransactionId = request.TransactionRef,
                        }

                    });

                }
                else
                {
                    var result = await _apiService.PostTransaction(token, url, request);
                    return Ok(new Common.Services.Resources.Responses.TransactionResponse
                    {
                        Success = result.Success,

                        Payload = new
                        {
                            Message = result.Success ? "Transaction succesful" : "transaction not successful",
                            TransactionId = request.TransactionRef,
                        }

                    });
                }
            }
        }
        private async Task<string> RetrieveToken()
        {
           
            var value = HttpContext.Session.GetString(_sessionKey);
            var creationDateString = HttpContext.Session.GetString("CreationTime");

            AuthResponse tokenResponse;
            if (string.IsNullOrEmpty(value))
            {
                var res = await _apiService.GetToken("","","","");
                var token = JsonConvert.SerializeObject(res);
                HttpContext.Session.SetString(_sessionKey, token);
                HttpContext.Session.SetString("CreationTime", DateTime.Now.ToString());
                return res.AccessToken;
            }
            else
            {
                tokenResponse = JsonConvert.DeserializeObject<AuthResponse>(value);
                var createdAt = DateTime.Parse(creationDateString);
                if (createdAt.AddSeconds(tokenResponse.ExpiresIn) > DateTime.Now)
                {
                    return tokenResponse.AccessToken;
                }
                else
                {
                    var res =  await _apiService.GetToken("", "", "", ""); 
                    var token = JsonConvert.SerializeObject(res);
                    HttpContext.Session.SetString(_sessionKey, token);
                    return res.AccessToken;

                }
            }
        }
    }
}
