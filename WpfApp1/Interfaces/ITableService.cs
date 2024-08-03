using WpfApp1.Models;

namespace WpfApp1.Interfaces
{
	public interface ITableService
	{
		Task<List<Table>> GetTablesForDepartmentAsync(int departmentId);
	}
}
