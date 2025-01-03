﻿using eCom.Web.Models;
using eCom.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using static eCom.Web.Utility.SD;

namespace eCom.Web.Service
{
	public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly ITokenProvider _tokenProvider;
		public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
		{
			_httpClientFactory = httpClientFactory;
			_tokenProvider = tokenProvider;
		}

		public async Task<ResponseDTO?> SendAsync(RequestDTO requestDTO, bool withBear = true)
		{
			try
			{


				HttpClient client = _httpClientFactory.CreateClient("eComAPI");
				HttpRequestMessage message = new();
				if(requestDTO.ContentType == ContentType.MultipartFormData)
				{
					message.Headers.Add("Accept", "*/*");
				}
				else
				{
					message.Headers.Add("Accept", "application/json");
				}
				

				if(withBear)
				{
					var token = _tokenProvider.GetToken();
					message.Headers.Add("Authorization", $"Bearer {token}");
				}



				message.RequestUri = new Uri(requestDTO.Url);

				if(requestDTO.ContentType == ContentType.MultipartFormData)
				{
                    var content = new MultipartFormDataContent();

                    foreach (var prop in requestDTO.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDTO.Data);
                        if (value is FormFile)
                        {
                            var file = (FormFile)value;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value == null ? "" : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
				else
				{
					if (requestDTO.Data != null)
					{
						message.Content = new StringContent(JsonConvert.SerializeObject(requestDTO.Data), Encoding.UTF8, "application/json");
					}

				}


                HttpResponseMessage? apiResponse = null;

				switch (requestDTO.ApiType)
				{
					case ApiType.POST:
						message.Method = HttpMethod.Post;
						break;
					case ApiType.PUT:
						message.Method = HttpMethod.Put;
						break;
					case ApiType.DELETE:
						message.Method = HttpMethod.Delete;
						break;
					default:
						message.Method = HttpMethod.Get;
						break;
				}

				apiResponse = await client.SendAsync(message);

				switch(apiResponse.StatusCode) {
					case HttpStatusCode.NotFound:
						return new() { IsSuccess = false, Message = "Not Found" };
					case HttpStatusCode.Forbidden:
						return new() { IsSuccess = false, Message = "Access Denied" };
					case HttpStatusCode.Unauthorized:
						return new() { IsSuccess = false, Message = "Unauthorized" };
					case HttpStatusCode.InternalServerError:
						return new() { IsSuccess = false, Message = "Internal Server Error" };
					default:
						var apiContent = await apiResponse.Content.ReadAsStringAsync();
						var apiResponseDTO = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
                        
                        return apiResponseDTO;
				}
			}
			catch (Exception ex)
			{
				var DTO = new ResponseDTO
				{
					Message = ex.Message,
					IsSuccess = false
				};
				return DTO;
			}


		}
	}
}
