<Query Kind="Program">
  <Reference Relative="..\src\BlazorApp\bin\Debug\netstandard2.0\BlazorApp.dll">C:\Code\BlazorApp\src\BlazorApp\bin\Debug\netstandard2.0\BlazorApp.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>BlazorApp.Models</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var client = new HttpClient { BaseAddress = new Uri("http://localhost:54098/") };
	var newEntity = new DatabaseRecord
	{
		Message = "Some message",
		CreatedDate = DateTime.Now
	};
	
	//(await PostRecordAsync(client, newEntity)).Dump("New Record");
	(await GetAllRecordsAsync(client)).Dump("All Records");
	(await GetRecordAsync(client, 1)).Dump("Specific Record");
	
	//await PostFlashDatabaseAsync(client);
}

public async Task<DatabaseRecord> GetRecordAsync(HttpClient client, int id)
{
	var route = $"api/database/{id}";
	return await GetAsync<DatabaseRecord>(client, route);
}

public async Task<DatabaseRecord[]> GetAllRecordsAsync(HttpClient client)
{
	var route = $"api/database";
	return await GetAsync<DatabaseRecord[]>(client, route);
}

public async Task<DatabaseRecord> PostRecordAsync(HttpClient client, DatabaseRecord record)
{
	var route = $"api/database";
	return await PostAsync<DatabaseRecord>(client, route, record);
}

public async Task PostFlashDatabaseAsync(HttpClient client)
{
	var route = $"api/database/flash";
	await PostJsonAsync(client, route, null);
}

public async Task<string> GetJsonAsync(HttpClient client, string route)
{
	var response = await client.GetAsync(route);	
	response.EnsureSuccessStatusCode();
	
	return await response.Content.ReadAsStringAsync();
}

public async Task<T> GetAsync<T>(HttpClient client, string route)
{
	var content = await GetJsonAsync(client, route);
	return JsonConvert.DeserializeObject<T>(content);
}

public async Task<string> PostJsonAsync(HttpClient client, string route, object entity)
{
	var serialized = JsonConvert.SerializeObject(entity);
	var stringContent = new StringContent(serialized, Encoding.UTF8, "application/json");
	var response = await client.PostAsync(route, stringContent);
	response.EnsureSuccessStatusCode();

	return await response.Content.ReadAsStringAsync();
}

public async Task<T> PostAsync<T>(HttpClient client, string route, object entity)
{
	var content = await PostJsonAsync(client, route, entity);
	return JsonConvert.DeserializeObject<T>(content);
}