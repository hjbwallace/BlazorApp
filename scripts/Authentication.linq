<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <DisableMyExtensions>true</DisableMyExtensions>
</Query>

public static readonly string BaseAddress = "http://localhost:54098";

async Task Main()
{
	using (var client = await GetClientAsync())
	using (var authenticatedClient = await GetClientAsync(username: "admin", password: "SOME_PASSWORD"))
	using (var tokenClient = await GetClientAsync(token: "TOKEN"))
	{
		(await GetAllDatabaseRecords(client)).Dump("Standard Forecasts");
		(await GetAllDatabaseRecords(authenticatedClient)).Dump("Authenticated Forecasts");
		(await GetAllDatabaseRecords(tokenClient)).Dump("Token Forecasts");
	}
}

public async Task<HttpClient> GetClientAsync()
{
	return await Task.FromResult(new HttpClient());
}

public async Task<HttpClient> GetClientAsync(string username, string password)
{
	var client = new HttpClient();
	
	var loginResult = await LoginAsync(client, username, password);
	loginResult.Dump();
	
	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);
	
	return client;
}

public async Task<HttpClient> GetClientAsync(string token)
{
	var client = new HttpClient();
	client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
	return await Task.FromResult(client);
}

public async Task<string> GetAllDatabaseRecords(HttpClient client)
{
	return await Http.GetJsonAsync(client, $"{BaseAddress}/api/database");
}

public async Task<LoginResult> LoginAsync(HttpClient client, string username, string password)
{
	var model = new 
	{
		Username = username,
		Password = password
	};
	
	return await Http.PostAsync<LoginResult>(client, $"{BaseAddress}/api/user/login", model);
}

public class LoginResult
{
	public bool Successful { get; set; }
	public string Error { get; set; }
	public string Token { get; set; }
}

public static class Http
{
	public async static Task<T> GetAsync<T>(this HttpClient client, string route)
	{
		var content = await client.GetJsonAsync(route);
		return JsonConvert.DeserializeObject<T>(content);
	}

	public async static Task<T> GetAsync<T>(string route)
	{
		using (var client = GenerateClient())
			return await client.GetAsync<T>(route);
	}

	public async static Task<string> GetJsonAsync(this HttpClient client, string route)
	{
		var response = await client.GetAsync(route);
		var content = await response.ValidateResponseAsync();
		return content;
	}

	public async static Task<string> GetJsonAsync(string route)
	{
		using (var client = GenerateClient())
			return await client.GetJsonAsync(route);
	}

	public async static Task<string> PostJsonAsync(this HttpClient client, string route, object content)
	{
		var requestContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
		var response = await client.PostAsync(route, requestContent);
		var responseContent = await response.ValidateResponseAsync();
		return responseContent;
	}

	public async static Task<T> PostAsync<T>(this HttpClient client, string route, object content)
	{
		var responseContent = await client.PostJsonAsync(route, content);
		return JsonConvert.DeserializeObject<T>(responseContent);
	}

	public async static Task<string> PostJsonAsync(string route, object content)
	{
		using (var client = GenerateClient())
			return await client.PostJsonAsync(route, content);
	}

	public async static Task<T> PostAsync<T>(string route, object content)
	{
		using (var client = GenerateClient())
			return await client.PostAsync<T>(route, content);
	}

	private async static Task<string> ValidateResponseAsync(this HttpResponseMessage response)
	{
		var content = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
		{
			response.StatusCode.Dump("Status Code");
			content.Dump("Response Content");
			return null;
		}

		return content;
	}

	private static HttpClient GenerateClient() => new HttpClient();
}