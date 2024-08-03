using System.Net.Http;
using System.Net.Http.Headers;

namespace WpfApp1.Utils;

public static class HttpClientUtils
{
	public static HttpClient CreateHttpClient(string baseAddress)
	{
		var httpClient = new HttpClient
		{
			BaseAddress = new Uri(baseAddress)
		};
		httpClient.DefaultRequestHeaders.Accept.Clear();
		httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		httpClient.DefaultRequestHeaders.Add("DEVICEID", "4");
		httpClient.DefaultRequestHeaders.Add("USERID", "1");
		httpClient.DefaultRequestHeaders.Add("PROGRAMTYPE", "DESKTOP");
		httpClient.DefaultRequestHeaders.Add("LOCALE", "en");
		httpClient.DefaultRequestHeaders.Add("IND", "1");
		httpClient.DefaultRequestHeaders.Add("MACADDR", "7C:8A:E1:BC:2D:67");
		httpClient.DefaultRequestHeaders.Add("WPID", "0");
		httpClient.DefaultRequestHeaders.Add("Access-Control-Allow-Origin", "*");

		return httpClient;
	}
}
