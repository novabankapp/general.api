using Common.Libraries.Services.Services;
using Flurl.Http;
using Newtonsoft.Json;
using NovaPay.Integrator.Common.Data.DTOs;
using NovaPay.Integrator.Common.Data.Entities;
using NovaPay.Integrator.Common.Services.Resources.Requests;
using NovaPay.Integrator.Common.Services.Resources.Responses;
using NovaPay.Integrator.Common.Services.Services.Merchants;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaPay.Integrator.General.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly IApiRequestService _requestService;
        private readonly IService<RequestLog, RequestLogDto> _logService;

        public ApiService(IApiRequestService requestService,
            IService<RequestLog, RequestLogDto> logService)
        {
            _requestService = requestService;
            _logService = logService;
        }
        public async Task<TransactionResponse> PostTransaction(string token, string url, TransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            var log = new RequestLog();
            try
            {
                if (isCustom && fields != null)
                {
                    var res = await _requestService.PostAsync<ExpandoObject>(url,
                     new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                         }, request, async (req, resp, code) =>
                         {
                             await Task.Run(() =>
                             {
                                 log.RawRequest = req;
                                 log.RawResponse = resp;
                                 log.RequestUrl = url;
                                 log.ResponseStatusCode = code;
                                 log.ResponseTime = DateTime.Now;
                             });

                         });
                    var r = await Adapters.Converters.ConvertTransactionResponse(JsonConvert.SerializeObject(res), fields);
                    await _logService.CreateAsync(log);
                    return r;

                }
                var response = await _requestService.PostAsync<TransactionResponse>(url,
                       new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                           }, request, async (req, resp, code) =>
                           {
                               await Task.Run(() =>
                               {
                                   log.RawRequest = req;
                                   log.RawResponse = resp;
                                   log.RequestUrl = url;
                                   log.ResponseStatusCode = code;
                                   log.ResponseTime = DateTime.Now;
                               });

                           });
                await _logService.CreateAsync(log);
                return response.result;
            }
            catch (FlurlHttpException ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = JsonConvert.SerializeObject(url);
                log.ResponseStatusCode = ex.StatusCode;
                switch (ex.StatusCode)
                {
                    case 409:
                        log.Message = "a dublicate receipt record was sent";
                        break;
                    default:
                        log.Message = "Something went wrong";
                        break;
                }
                await _logService.CreateAsync(log);
                throw ex;
            }
            catch (Exception ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = Newtonsoft.Json.JsonConvert.SerializeObject(url);
                log.Message = "Something went wrong";
                await _logService.CreateAsync(log);
                throw ex;
            }

        }
        public async Task<ValidationResponse> ValidateReference(string token, string url, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            var log = new RequestLog();
            try
            {
                if (isCustom && fields != null)
                {
                    var res = await _requestService.GetAsync<ExpandoObject>(url,
                     new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                         }, async (req, resp, code) =>
                         {
                             await Task.Run(() =>
                             {
                                 log.RawRequest = req;
                                 log.RawResponse = resp;
                                 log.RequestUrl = url;
                                 log.ResponseStatusCode = code;
                                 log.ResponseTime = DateTime.Now;
                             });

                         });
                    var d = JsonConvert.SerializeObject(res.result);

                    var r = await Adapters.Converters.ConvertValidationResponse(JsonConvert.SerializeObject(res.result), fields);
                    r.Success = r.Name != null && r.Name != string.Empty;
                    await _logService.CreateAsync(log);
                    return r;

                }
                var response = await _requestService.GetAsync<ValidationResponse>(url,
                       new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                           }, async (req, resp, code) =>
                       {
                           await Task.Run(() =>
                           {
                               log.RawRequest = req;
                               log.RawResponse = resp;
                               log.RequestUrl = url;
                               log.ResponseStatusCode = code;
                               log.ResponseTime = DateTime.Now;
                           });

                       });
                await _logService.CreateAsync(log);
                return response.result;
            }
            catch (FlurlHttpException ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = JsonConvert.SerializeObject(url);
                log.ResponseStatusCode = ex.StatusCode;
                switch (ex.StatusCode)
                {
                    case 409:
                        log.Message = "a dublicate receipt record was sent";
                        break;
                    default:
                        log.Message = "Something went wrong";
                        break;
                }
                await _logService.CreateAsync(log);
                throw ex;
            }
            catch (Exception ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = Newtonsoft.Json.JsonConvert.SerializeObject(url);
                log.Message = "Something went wrong";
                await _logService.CreateAsync(log);
                throw ex;
            }

        }

        public async Task<Resources.Responses.AuthResponse> GetToken(string url, string grantType, string clientId, string clientSecret)
        {

            var log = new RequestLog();
            try
            {
                var response = await _requestService.PostUrlEncodeAsync<Resources.Responses.AuthResponse>(url,
                      new Dictionary<string, string> {
                       { "Content-Type", "application/x-www-form-urlencoded" },

                       },
                      new Dictionary<string, string> {
                       { "grant_type", grantType },
                       { "client_id", clientId },
                       { "client_secret", clientSecret },
                       },
                      async (req, resp, code) =>
                      {
                          await Task.Run(() =>
                          {
                              log.RawRequest = req;
                              log.RawResponse = resp;
                              log.RequestUrl = url;
                              log.ResponseTime = DateTime.Now;
                          });

                      }
                    );
                if (response.statusCode == 200)
                {

                    log.Message = "The request was successful";
                }
                else
                {

                    log.Message = "Something went wrong while processing your request";

                }
                await _logService.CreateAsync(log);
                return response.result;
            }
            catch (FlurlHttpException ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = "";
                switch (ex.StatusCode)
                {

                    default:
                        log.Message = "Something went wrong";
                        break;
                }
                await _logService.CreateAsync(log);
                throw ex;
            }
            catch (Exception ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = "";
                log.Message = "Something went wrong";
                await _logService.CreateAsync(log);
                throw ex;

            }
        }

        public async Task<TransactionResponse> PostProductTransaction(string token, string url, ProductTransactionRequest request, bool isCustom = false, Dictionary<string, string>? fields = null)
        {
            var log = new RequestLog();
            try
            {
                if (isCustom && fields != null)
                {
                    var res = await _requestService.PostAsync<ExpandoObject>(url,
                     new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                         }, request, async (req, resp, code) =>
                         {
                             await Task.Run(() =>
                             {
                                 log.RawRequest = req;
                                 log.RawResponse = resp;
                                 log.RequestUrl = url;
                                 log.ResponseStatusCode = code;
                                 log.ResponseTime = DateTime.Now;
                             });

                         });
                    var r = await Adapters.Converters.ConvertTransactionResponse(JsonConvert.SerializeObject(res), fields);
                    await _logService.CreateAsync(log);
                    return r;

                }
                var response = await _requestService.PostAsync<TransactionResponse>(url,
                       new Dictionary<string, string> {
                           { "Content-Type", "application/json" },
                           { "Accept", "application/json" },
                           {"Authorization", $"Bearer {token}" }
                           }, request, async (req, resp, code) =>
                           {
                               await Task.Run(() =>
                               {
                                   log.RawRequest = req;
                                   log.RawResponse = resp;
                                   log.RequestUrl = url;
                                   log.ResponseStatusCode = code;
                                   log.ResponseTime = DateTime.Now;
                               });

                           });
                await _logService.CreateAsync(log);
                return response.result;
            }
            catch (FlurlHttpException ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = JsonConvert.SerializeObject(url);
                log.ResponseStatusCode = ex.StatusCode;
                switch (ex.StatusCode)
                {
                    case 409:
                        log.Message = "a dublicate receipt record was sent";
                        break;
                    default:
                        log.Message = "Something went wrong";
                        break;
                }
                await _logService.CreateAsync(log);
                throw ex;
            }
            catch (Exception ex)
            {
                log.RawResponse = ex.Message;
                log.RequestUrl = url;
                log.ResponseTime = DateTime.Now;
                log.RawRequest = Newtonsoft.Json.JsonConvert.SerializeObject(url);
                log.Message = "Something went wrong";
                await _logService.CreateAsync(log);
                throw ex;
            }
        }
    }
}
