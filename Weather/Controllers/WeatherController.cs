using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Models;

namespace Weather.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await GetWeatherAsJsonAsync("data/2.5/group?id=1580578,1581129,1581297,1581188,1587923&units=metric&appid=91b7466cc755db1a94caf6d86a9c788a");
            return new ObjectResult(response);
        }

        private void InitHeaderRequest(HttpClient Client)
        {
            try
            {
                Client.BaseAddress = new Uri("http://api.openweathermap.org");
                Client.DefaultRequestHeaders.Accept.Clear();
            }
            catch (Exception ex) { }
        }

        private async Task<ResponseModels> GetWeatherAsJsonAsync(string ApiURL)
        {
            var response = new ResponseModels();
            HttpClient client = new HttpClient();
            InitHeaderRequest(client);
            try
            {
                HttpResponseMessage httpResponse = await client.GetAsync(ApiURL);
                var statusCode = (int)httpResponse.StatusCode;
                if (statusCode == (int)HttpStatusCode.Unauthorized || statusCode == (int)HttpStatusCode.Forbidden)
                {
                    response.StatusCode = statusCode;
                    response.Message = "Tài khoản của bạn không có quyền truy cập tài nguyên";
                }
                else
                {
                    var result = await httpResponse.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(result))
                    {
                        var data  = JsonConvert.DeserializeObject<WeatherResponseModels>(result);
                        if(data != null && data.List != null && data.List.Any())
                        {
                            var dynamicObject = new List<Object>();
                            foreach(var item in data.List)
                            {
                                dynamicObject.Add(new 
                                {
                                    CityId = item.Id,
                                    CityName = item.Name,
                                    WeatherMain = item.Weather != null && item.Weather.Any() ? item.Weather.Select(z => z.Main).FirstOrDefault() : null,
                                    WeatherDescription = item.Weather != null && item.Weather.Any() ? item.Weather.Select(z => z.Description).FirstOrDefault() : null,
                                    WeatherIcon = item.Weather != null && item.Weather.Any() ? string.Format("{0}{1}{2}", "http://openweathermap.org/img/wn/", item.Weather.Select(z => z.Icon).FirstOrDefault(), "@2x.png")  : null,
                                    MainTemp = item.Main != null ? item.Main.Temp : null,
                                    MainHumidity = item.Main != null ? item.Main.Humidity : null,
                                });
                            }
                            response.Data = dynamicObject;
                        }
                    }
                    response.StatusCode = statusCode;
                    response.Message = "Current weather information of cities";
                }
            }
            catch (Exception ex) { }
            finally
            {
                client.CancelPendingRequests();
            }
            return response;
        }
    }
}
