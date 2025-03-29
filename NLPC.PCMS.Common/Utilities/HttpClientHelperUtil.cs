using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NLPC.PCMS.Common.DTOs;
using NLPC.PCMS.Common.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;

namespace NLPC.PCMS.Common.Utilities
{
    public class HttpClientHelperUtil
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientHelperUtil(IHttpClientFactory httpClientFactory, IOptions<AppSettingsDto> appConfigSettings)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetJSON<T>(string path, IDictionary<string, string> headers)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                using var httpResponse = await httpClient.GetAsync(path);
                return await ProcessHttpResponse<T>(httpResponse, path);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> GetJSON<T>(string path, IDictionary<string, string>? queryParams, IDictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var queryString = string.Empty;
                if (queryParams != null)
                {
                    var values = HttpUtility.ParseQueryString(string.Empty);
                    foreach (var queryParam in queryParams)
                    {
                        values[queryParam.Key] = queryParam.Value;
                    }
                    queryString = values.ToString();
                }

                var fullUrl = path + (string.IsNullOrEmpty(queryString) ? string.Empty : "?" + queryString);
                using var httpResponse = await httpClient.GetAsync(fullUrl);
                return await ProcessHttpResponse<T>(httpResponse, path);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> PostJSON<T>(string path, object payload, IDictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                using var httpResponse = await httpClient.PostAsJsonAsync(path, payload);
                return await ProcessHttpResponse<T>(httpResponse, path, payload);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> PostMultipartFormData<T>(string path, IDictionary<string, object> payload, IDictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var multipartContent = new MultipartFormDataContent();
                foreach (var keyValuePair in payload)
                {
                    if (keyValuePair.Value != null)
                    {
                        if (keyValuePair.Value is IFormFile file)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                            {
                                ContentType = new MediaTypeHeaderValue(file.ContentType)
                            }
                            };
                            multipartContent.Add(fileContent, keyValuePair.Key, file.FileName);
                        }
                        else
                        {
                            multipartContent.Add(new StringContent(keyValuePair.Value.ToString()!), keyValuePair.Key);
                        }
                    }
                }

                using var httpResponse = await httpClient.PostAsync(path, multipartContent);
                return await ProcessHttpResponse<T>(httpResponse, path, payload);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> PutJSON<T>(string path, object? payload = null, IDictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                using var httpResponse = await httpClient.PutAsJsonAsync(path, payload);
                return await ProcessHttpResponse<T>(httpResponse, path, payload);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> PutMultipartFormData<T>(string path, IDictionary<string, object> payload, IDictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var multipartContent = new MultipartFormDataContent();
                foreach (var keyValuePair in payload)
                {
                    if (keyValuePair.Value != null)
                    {
                        if (keyValuePair.Value is IFormFile file)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream())
                            {
                                Headers =
                            {
                                ContentType = new MediaTypeHeaderValue(file.ContentType)
                            }
                            };
                            multipartContent.Add(fileContent, keyValuePair.Key, file.FileName);
                        }
                        else
                        {
                            multipartContent.Add(new StringContent(keyValuePair.Value.ToString()!), keyValuePair.Key);
                        }
                    }
                }

                using var httpResponse = await httpClient.PutAsync(path, multipartContent);
                return await ProcessHttpResponse<T>(httpResponse, path, payload);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        public async Task<T> Delete<T>(string path, IDictionary<string, string>? queryParams, IDictionary<string, string>? headers = null)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ApiException("missing url detected");

            try
            {
                var httpClient = _httpClientFactory.CreateClient("Myhttp");
                httpClient.DefaultRequestHeaders.Clear();

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                var queryString = string.Empty;
                if (queryParams != null)
                {
                    var values = HttpUtility.ParseQueryString(string.Empty);
                    foreach (var queryParam in queryParams)
                    {
                        values[queryParam.Key] = queryParam.Value;
                    }
                    queryString = values.ToString();
                }

                var fullUrl = path + (string.IsNullOrEmpty(queryString) ? string.Empty : "?" + queryString);
                using var httpResponse = await httpClient.DeleteAsync(fullUrl);
                return await ProcessHttpResponse<T>(httpResponse, path);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.StackTrace!);
            }
        }

        private async Task<T> ProcessHttpResponse<T>(HttpResponseMessage httpResponse, string path, object? payload = null)
        {
            var result = await httpResponse.Content.ReadFromJsonAsync<T>() ?? default(T)!;
            return result;
        }
    }

}
