using Newtonsoft.Json;
using System.Net.Http;
using WpfApp1.Interfaces;
using WpfApp1.Models;

namespace WpfApp1.Services;

public class DepartmentService : IDepartmentService
{
	private readonly HttpClient _httpClient;
	public DepartmentService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}
	public async Task<List<Department>> GetDepartmentsAsync(int sectionId, int departmentId = 0)
	{
		var response = await _httpClient.GetStringAsync($"service/dicts/departments?idSection={sectionId}&idDepartment={departmentId}");
		var data = JsonConvert.DeserializeObject<DepartmentResponse>(response);
		return data.Data;
	}
}
