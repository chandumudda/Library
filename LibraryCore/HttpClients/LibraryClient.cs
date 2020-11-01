
using LibraryCore.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LibraryCore.HttpClients
{
    public class LibraryClient : ILibraryClient
    {
        private readonly HttpClient _httpClient;
        private const string _serviceUrl = "http://bookapi/connect/token";
        public LibraryClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Models.HttpResponse> ExternalGetApiResultAsync(string webServiceUrl, Dictionary<string, string> headers = null)
        {
            try
            {
                if (headers != null && headers.Any())
                {
                    foreach (var (key, value) in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(key);
                        _httpClient.DefaultRequestHeaders.Add(key, value);
                    }
                }
                var response = await _httpClient.GetAsync(webServiceUrl);
                return new Models.HttpResponse() { StatusCode = (int)response.StatusCode, StatusMessage = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return new Models.HttpResponse()
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<Models.HttpResponse> ExternalPatchApiResultAsync(string webServiceUrl, string jsonInput, Dictionary<string, string> headers = null)
        {
            try
            {
                var content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                if (headers != null && headers.Any())
                {
                    foreach (var (key, value) in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(key);
                        _httpClient.DefaultRequestHeaders.Add(key, value);
                    }
                }
                var response = await _httpClient.PatchAsync(webServiceUrl, content);
                return new Models.HttpResponse() { StatusCode = (int)response.StatusCode, StatusMessage = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return new Models.HttpResponse()
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<Models.HttpResponse> ExternalPostApiResultAsync(string webServiceUrl, string jsonInput, Dictionary<string, string> headers = null)
        {
            try
            {
                var content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                if (headers != null && headers.Any())
                {
                    foreach (var (key, value) in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(key);
                        _httpClient.DefaultRequestHeaders.Add(key, value);
                    }
                }
                var response = await _httpClient.PostAsync(webServiceUrl, content);
                return new Models.HttpResponse() { StatusCode = (int)response.StatusCode, StatusMessage = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return new Models.HttpResponse()
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable,
                    StatusMessage = ex.Message
                };
            }
        }

        public async Task<Models.HttpResponse> ExternalDeleteApiResultAsync(string webServiceUrl, Dictionary<string, string> headers = null)
        {
            try
            {
                if (headers != null && headers.Any())
                {
                    foreach (var (key, value) in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Remove(key);
                        _httpClient.DefaultRequestHeaders.Add(key, value);
                    }
                }

                var response = await _httpClient.DeleteAsync(webServiceUrl);
                return new Models.HttpResponse() { StatusCode = (int)response.StatusCode, StatusMessage = await response.Content.ReadAsStringAsync() };
            }
            catch (Exception ex)
            {
                return new Models.HttpResponse()
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable,
                    StatusMessage = ex.Message
                };
            }
        }

        public string GenerateAccessToken(User user)
        {
            var client = new RestClient(_serviceUrl) { Timeout = -1 };
            var restRequest = new RestRequest(Method.POST);
            restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restRequest.AddParameter("grant_type", "password");
            restRequest.AddParameter("username", user.ClaimName);
            restRequest.AddParameter("password", user.Password);
            var response = client.Execute(restRequest);
            var token = JsonConvert.DeserializeObject<Token>(response.Content);

            return token.token_type + " " + token.access_token;
        }
    }
}
