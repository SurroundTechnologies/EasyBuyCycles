using A4DN.Core.BOS.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace BOS.OrderManagement.ExternalAPIs
{
	public static class IPAPIClient
	{
		#region Configuration

		private const string ClientName = nameof(IPAPIClient);
		private const string APIBaseURL = "http://ip-api.com/json/<IPADDRESS>";

		#endregion

		#region Low-level methods

		private static async Task<string> SendAsync(HttpRequestMessage request)
		{
			using (var handler = new HttpClientHandler())
			{
				// Disable old insecure protocols and enable new ones (including newer ones we don't know about here)
				handler.SslProtocols = (handler.SslProtocols & SslProtocols.Tls & SslProtocols.Ssl3 & SslProtocols.Tls11) | SslProtocols.Tls12 | SslProtocols.Tls13;

				handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, errors) =>
				{
					if (errors == SslPolicyErrors.None)
						return true;

					AB_LoggerFactory.am_GetLogger().am_WriteLog(ClientName, AB_LogLevel.Error, "SendAsync",
						$"SSL Certificate Failure {errors}\n{certificate}");

					return false;
				};

				ServicePointManager.SecurityProtocol = (ServicePointManager.SecurityProtocol & SecurityProtocolType.Tls & SecurityProtocolType.Ssl3 & SecurityProtocolType.Tls11) | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

				ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
				{
					if (errors == SslPolicyErrors.None)
						return true;

					AB_LoggerFactory.am_GetLogger().am_WriteLog(ClientName, AB_LogLevel.Error, "SendAsync",
						$"SSL Certificate Failure {errors}\n{certificate}");

					return false;
				};

				using (var client = new HttpClient(handler))
				{
					try
					{
						var response = client.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();

						if (!response.IsSuccessStatusCode)
							throw new ApplicationException(response.ReasonPhrase);

						var jsonBody = await response.Content.ReadAsStringAsync();
						return jsonBody;
					}
					catch (HttpRequestException reqEx)
					{
						AB_LoggerFactory.am_GetLogger().am_WriteLog(ClientName, AB_LogLevel.Error, "SendAsync",
							$"HTTP Exception for: {request.Method} {request.RequestUri}\n{string.Join("\n", reqEx.am_GatherDetailedExceptionMessages())}");

						throw;
					}
					catch (Exception ex)
					{
						AB_LoggerFactory.am_GetLogger().am_WriteLog(ClientName, AB_LogLevel.Error, "SendAsync",
							$"Failed {request.Method} {request.RequestUri}\n{string.Join("\n", ex.am_GatherDetailedExceptionMessages())}");

						throw;
					}
				}
			}
		}

		private static async Task<Tresult> GetResult<Tresult>(string method, Uri uri, Func<string, Tresult> getResult, [CallerMemberName] string callerMember = null)
		{
			using (var request = new HttpRequestMessage(new HttpMethod(method), uri))
			{
				var jsonBody = await SendAsync(request);

				try
				{
					var result = getResult(jsonBody);
					return result;
				}
				catch (Exception e)
				{
					AB_LoggerFactory.am_GetLogger().am_WriteLog(ClientName, AB_LogLevel.Error, callerMember,
						$"Json Parse Error: {e.am_FlattenedMessage()}" +
						$"\n{method} {uri}" +
						$"\n{jsonBody}");

					throw;
				}
			}
		}

		#endregion

		#region BOS-level Endpoints

		public static async Task<string> GetZipCode(string ipAddress)
		{
			var uri = new UriBuilder(APIBaseURL.Replace("<IPADDRESS>", ipAddress));

			var apiResponse = await GetResult<string>("GET", uri.Uri, (jsonBody) => GetZipCodeFromJSON(jsonBody));

			return apiResponse.ToString();
		}

		#endregion

		#region Helper Methods

		private static string GetZipCodeFromJSON(string jsonString)
		{
			JObject json = JObject.Parse(jsonString);
			string postalCode = json["zip"]?.ToString();

			if (!string.IsNullOrEmpty(postalCode))
				return postalCode;
			else return string.Empty;
		}

		public static string GetPublicIp()
		{
			HttpClient client = new HttpClient();
			var ipTask = client.GetStringAsync("https://api64.ipify.org?format=jso");
			var ipAddress = ipTask.GetAwaiter().GetResult();

			return ipAddress;
		}

		#endregion
	}
}
