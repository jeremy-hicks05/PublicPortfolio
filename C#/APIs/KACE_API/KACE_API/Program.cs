using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json.Serialization;
using System.Runtime.InteropServices.JavaScript;
using System.Net.Http;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace KACE_API
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            CookieContainer cookies = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler()
            {
                UseCookies = true
            };
            handler.CookieContainer = cookies;
            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri("http://192.168.122.33/");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-dell-api-version", "13");

            var content = new StringContent("{ \"userName\": \"*\", \"password\": \"*\", \"organizationName\": \"Default\"}");

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpRequestMessage message = new HttpRequestMessage()
            {
                Content = content,
                Method = HttpMethod.Post,
                RequestUri = new Uri("http://192.168.122.33/ams/shared/api/security/login")
            };

            var response = client.Send(message);

            //var reply = HttpRequest();

            //var response = await client.PostAsync(message.RequestUri.ToString(), message.Content);

            await Console.Out.WriteLineAsync(response.ToString());
            await Console.Out.WriteLineAsync(response.Content.ToString());

            foreach (var c in cookies.GetCookies(new Uri("http://192.168.122.33/ams/shared/api/security/login")))
            {
                await Console.Out.WriteLineAsync(c.ToString());
            }

            var responseString = await response.Content.ReadAsStringAsync();
            await Console.Out.WriteLineAsync(responseString);
            Console.ReadLine();
        }
    }
}
