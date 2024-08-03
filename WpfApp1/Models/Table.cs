using System.Text.Json.Serialization;
using System.Windows.Media;

namespace WpfApp1.Models;

public class Table
{
	public int Id { get; set; }
	public int PastMins { get; set; }
	public int PersonCount { get; set; }
	public int TableStatusId { get; set; }
	public bool IsLocked { get; set; }
	public string Name { get; set; }
	public string UserInsertName { get; set; }
	public string InsertDate { get; set; }
	public decimal RemainingAmount { get; set; }
	[JsonIgnore]
	public SolidColorBrush StatusColor { get; set; }
}
