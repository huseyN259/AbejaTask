namespace WpfApp1.Models;

public class Department
{
	public int Id { get; set; }
	public string Name { get; set; }
	public List<Table> Tables { get; set; }
}
