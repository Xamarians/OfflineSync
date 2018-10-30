using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace OfflineSyncDemo.Services
{
    internal class RestAPI
    {
        public enum Methods
        {
            GET, POST, PUT, DELETE
        }

        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            //if (!string.IsNullOrWhiteSpace(AppSecurity.Token))
            //    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppSecurity.Token);
            return client;
        }

        private static RestResult<T> OKResult<T>(string json)
        {
            try
            {
                var responseData = JsonConvert.DeserializeObject<APIResponse<T>>(json);
                return new RestResult<T>
                {
                    Data = responseData.Data,
                    StatusCode = 200,
                    Message = responseData.Messages == null ? "" : (responseData.Messages == null ? "" : responseData.Messages),
                    Success = responseData.Success,
                    Notification = (responseData.Notification == null ? "" : responseData.Notification)
                };
            }
            catch (Exception ex)
            {
                return new RestResult<T>
                {
                    Message = "Parse Json Error: " + ex.Message,
                    StatusCode = 200
                };
            }
        }

        private static RestResult<T> ErrorResult<T>(string message, int statusCode)
        {
            return new RestResult<T>
            {
                Message = message,
                StatusCode = statusCode
            };
        }

        private static StringContent GetStringContent(object postData)
        {
            var requestJson = postData == null ? string.Empty : JsonConvert.SerializeObject(postData);
            return new StringContent(requestJson, Encoding.UTF8, "application/json");
        }

        private static async Task<IRestResult<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return OKResult<T>(responseString);
            }

            else
            {
                if ((response.Content) != null)
                {
                    var str = await response.Content.ReadAsStringAsync();
                    return OKResult<T>(str);
                }

                return ErrorResult<T>(response.ReasonPhrase, (int)response.StatusCode);
            }
        }

        /// <summary>
        /// Send POST request with postdata to uri.
        /// </summary>
        /// <typeparam name="T">Response Data Type</typeparam>
        /// <param name="uri">Request End Point</param>
        /// <param name="postData">Data to be post on server.</param>
        public static async Task<IRestResult<T>> PostAsync<T>(string uri, object postData, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var content = GetStringContent(postData);
                using (var client = CreateHttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                    var response = await client.PostAsync(uri, content, cancellationToken);
                    return await HandleResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                return ErrorResult<T>(ex.Message, 500);
            }
        }

        /// <summary>
        /// Send PUT request with postdata to uri.
        /// </summary>
        /// <typeparam name="T">Response Data Type</typeparam>
        /// <param name="uri">Request End Point</param>
        /// <param name="postData">Data to be post on server.</param>
        public static async Task<IRestResult<T>> PutAsync<T>(string uri, object postData, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var content = GetStringContent(postData);
                using (var client = CreateHttpClient())
                {
                    var response = await client.PutAsync(uri, content, cancellationToken);
                    return await HandleResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                return ErrorResult<T>(ex.Message, 500);
            }
        }

        /// <summary>
        /// Send GET Request with parameters as query string to uri.
        /// </summary>
        /// <typeparam name="T">Response Data Type</typeparam>
        /// <param name="uri">Request End Point</param>
        /// <param name="parameters">Parameters to send as query string</param>
        public static async Task<IRestResult<T>> GetAsync<T>(string uri, Dictionary<string, string> parameters = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                if (parameters != null)
                {
                    var qs = string.Join("&", parameters.Select(x => x.Key + "=" + x.Value));
                    uri = (uri.IndexOf("?") == -1 ? "?" : "&") + qs;
                }
                using (var client = CreateHttpClient())
                {
                    var response = await client.GetAsync(uri, cancellationToken);
                    return await HandleResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                return ErrorResult<T>(ex.Message, 500);
            }
        }

        /// <summary>
        /// Send DELETE request to given uri.
        /// </summary>
        /// <typeparam name="T">Response Data Type</typeparam>
        /// <param name="uri">Request End Point</param>
        public static async Task<IRestResult<T>> DeleteAsync<T>(string uri, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                using (var client = CreateHttpClient())
                {
                    var response = await client.DeleteAsync(uri, cancellationToken);
                    return await HandleResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                return ErrorResult<T>(ex.Message, 500);
            }
        }

    }
}
