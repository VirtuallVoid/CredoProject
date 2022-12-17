using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectMvc.Classes;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMvc.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly IOptions<Settings> _Setting;

        public ApplicationController(IOptions<Settings> setting, ILogger<ApplicationController> logger = null)
        {
            _logger = logger;
            _Setting = setting;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<List<LoanApplications>> GetUserApplications(string userId, string token)
        {
            var jsonObject = JsonConvert.SerializeObject(new { UserId = userId });
            var jsonResult = string.Empty;
            try
            {

                if (token != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/user-loans", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                    .Result.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Response<List<LoanApplications>>>(jsonResult);
                        if (result.Data != null)
                        {
                            result.Data.ForEach(x => { x.LoanPeriodStr = x.LoanPeriod.ToString("dd.MM.yyyy"); });
                        }
                        return result.Data == null ? null : result.Data;
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "GetUserApplications");
                throw new Exception(ex.Message);
            }
        }

        public async Task<LoanApplications> GetUserApplicationById(string userId, string loanId, string token)
        {
            var jsonObject = JsonConvert.SerializeObject(new { UserId = userId, LoanId = loanId });
            var jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/user-loan-by-id", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<LoanApplications>>(jsonResult);
                    if (result.Data != null)
                    {
                        result.Data.LoanPeriodStr = result.Data.LoanPeriod.ToString("yyy-MM-dd");
                    }
                    return result.Data == null ? null : result.Data;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "GetUserApplications");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> AddNewApplication(LoanApplications loan, string token)
        {
            var jsonObject = JsonConvert.SerializeObject(loan);
            var jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/insert-loan", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                    return result.Data == null ? null : result.Data;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "AddNewApplication");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> EditApplication(LoanApplications loan, string token)
        {
            var jsonObject = JsonConvert.SerializeObject(loan);
            var jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/update-loan", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<int>>(jsonResult);
                    return result.Data == 0 ? null : result.Data.ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "AddNewApplication");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> DeleteApplication(string userId, string loanId, string token)
        {
            var jsonObject = JsonConvert.SerializeObject(new { UserId = userId, Id = loanId });
            var jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/delete-loan", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<int>>(jsonResult);
                    return result.Data == 0 ? null : result.Data.ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "AddNewApplication");
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<StatusTypes>> getStatuses()
        {
            using (var client = new HttpClient())
            {
                var jsonResult = await client.GetAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/status-types").Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<List<StatusTypes>>>(jsonResult);
                return result.Data == null ? null : result.Data;
            }
        }

        public async Task<List<LoanTypes>> getLoanTypes()
        {
            using (var client = new HttpClient())
            {
                var jsonResult = await client.GetAsync($"{_Setting.Value.BaseUrl}api/LoanApplications/loan-types").Result.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Response<List<LoanTypes>>>(jsonResult);
                return result.Data == null ? null : result.Data;
            }
        }

        public void LoggingHelper(Exception ex, string obj, string jsonResult, string methodName)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(new CompactJsonFormatter(), _Setting.Value.WebRoot.Replace(@"\\", @"\"))
                .CreateLogger();
            var log = new { LogDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), Exception = ex, Object = JsonConvert.SerializeObject(obj), JsonResult = jsonResult, MethodName = methodName };
            Log.Warning("Log  {@Log}", log);
            Log.CloseAndFlush();
        }
    }
}
