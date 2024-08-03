using Newtonsoft.Json;
using System.Net.Http;
using WpfApp1.Interfaces;
using WpfApp1.Models;

namespace WpfApp1.Services;

public class TableService : ITableService
{
	private readonly HttpClient _httpClient;

	public TableService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<List<Table>> GetTablesForDepartmentAsync(int departmentId)
	{
		var response = await _httpClient.GetStringAsync($"service/dicts/departments?idSection=1&idDepartment={departmentId}");
		var data = JsonConvert.DeserializeObject<DepartmentResponse>(response);
		var department = data.Data.FirstOrDefault(d => d.Id == departmentId);
		return department?.Tables ?? new List<Table>();
	}
}
