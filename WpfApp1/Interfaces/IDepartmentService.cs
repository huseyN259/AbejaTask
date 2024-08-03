using WpfApp1.Models;

namespace WpfApp1.Interfaces;

public interface IDepartmentService
{
	Task<List<Department>> GetDepartmentsAsync(int sectionId, int departmentId = 0);
}
