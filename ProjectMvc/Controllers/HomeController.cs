using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectMvc.Classes;
using ProjectMvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Formatting.Compact;

namespace ProjectMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOptions<Settings> _Setting;

        public HomeController(IOptions<Settings> setting, ILogger<HomeController> logger = null)
        {
            _logger = logger;
            _Setting = setting;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<string> LoginCustomer(string userName, string password)
        {
            var jsonObject = JsonConvert.SerializeObject(new { UserName = userName, PassWord = password });
            string jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/User/login-customer", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<string>>(jsonResult);
                    return result.Data == null ? result.Code.ToString() : result.Data;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "LoginCustomer");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetUserId(string userName, string password)
        {
            var jsonObject = JsonConvert.SerializeObject(new { UserName = userName, PassWord = password });
            string jsonResult = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/User/user-info", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<UserInfo>>(jsonResult);
                    return result.Data == null ? result.Message : result.Data.Id.ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "GetUserId");
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> UserRegister(UserInfo userInfo)
        {
            var jsonObject = JsonConvert.SerializeObject(userInfo);
            string jsonResult = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    jsonResult = await client.PostAsync($"{_Setting.Value.BaseUrl}api/User/register-user", new StringContent(jsonObject, Encoding.UTF8, "application/json"))
                                .Result.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<Response<int?>>(jsonResult);
                    return result.Data == null ? result.Error : result.Data.ToString();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper(ex, jsonObject, jsonResult, "UserRegister");
                throw new Exception(ex.Message);
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
