using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyPadelDesktopApp.Models.Responses;

namespace MyPadelDesktopApp.Services.HttpClientServices
{
    public class HttpClientService : IHttpClientService
    {
        #region Services

        public string baseUrl = "http://ufficio.pointer.re.it:7070/api/";

        #endregion

        public async Task<GeneralResponse> PostAsync(string url, object data, bool isToken, bool IsPost = true)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler();

                clientHandler.SslProtocols = System.Security.Authentication.SslProtocols.Tls13;
                clientHandler.AllowAutoRedirect = true;
                clientHandler.UseCookies = true;
                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                    Timeout = TimeSpan.FromMinutes(2)
                };

                string NewUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken && Preferences.Default.Get("Token", string.Empty) != null && !string.IsNullOrEmpty(Preferences.Default.Get("Token", string.Empty)))
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Default.Get("Token", string.Empty));

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                StringContent stringContent = new StringContent("");
                var header_data = JsonSerializer.Serialize(data);
                if (header_data != null)
                    stringContent = new StringContent(header_data, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse;
                if (IsPost)
                    httpResponse = header_data != null ? await client.PostAsync(baseUrl + url, stringContent) : await client.PostAsync(baseUrl + url, null);
                else
                    httpResponse = header_data != null ? await client.PatchAsync(baseUrl + url, stringContent) : await client.PatchAsync(baseUrl + url, null);
                var responseCon = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseCon);
                return response != null ? response : new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }
        public async Task<GeneralResponse> PutAsync(string url, object data, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = Preferences.Default.Get("Token", string.Empty);
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                StringContent stringContent = new StringContent("");
                if (data != null)
                {
                    var jsonData = JsonSerializer.Serialize(data);
                    stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage httpResponse = await client.PutAsync(fullUrl, stringContent);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);
                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }

        public async Task<GeneralResponse> DeleteAsync(string url, object data, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = Preferences.Default.Get("Token", string.Empty);
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                // Convert payload to JSON string
                StringContent stringContent = new StringContent(string.Empty);
                if (data != null)
                {
                    string jsonData = JsonSerializer.Serialize(data);
                    stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(fullUrl),
                    Content = stringContent
                };

                HttpResponseMessage httpResponse = await client.SendAsync(request);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);
                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
            }
            return new GeneralResponse();
        }
        public async Task<GeneralResponse> GetAsync(string url, bool isToken)
        {
            try
            {
                HttpClientHandler clientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls13,
                    AllowAutoRedirect = true,
                    UseCookies = true
                };

                HttpClient client = new HttpClient(clientHandler)
                {
                    DefaultRequestVersion = HttpVersion.Version30,
                    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower,
                    Timeout = TimeSpan.FromSeconds(60)
                };

                string fullUrl = baseUrl + url;

                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();

                if (isToken)
                {
                    string token = Preferences.Default.Get("Token", string.Empty);
                    if (!string.IsNullOrEmpty(token))
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }

                client.DefaultRequestHeaders.ConnectionClose = false;
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage httpResponse = await client.GetAsync(fullUrl);
                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GeneralResponse>(responseContent);

                return response ?? new GeneralResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAsync: {ex}");
            }

            return new GeneralResponse();
        }

    }
}