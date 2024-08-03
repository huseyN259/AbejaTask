using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WpfApp1.Models;
using WpfApp1.Services;
using WpfApp1.Utils;

namespace WpfApp1;

public partial class MainWindow : Window
{
	private const string PlaceholderText = "Masa ara...";
	private const int InitialDepartmentCount = 3;

	private readonly DispatcherTimer _timer;
	private readonly HttpClient _httpClient;
	private readonly DepartmentService _departmentService;
	private readonly TableService _tableService;
	private List<Department> _allDepartments = new List<Department>();
	private bool _isShowMore = false;

	public MainWindow()
	{
		InitializeComponent();
		UIUtils.SetPlaceholder(TextBox1, PlaceholderText);
		_httpClient = HttpClientUtils.CreateHttpClient("http://185.202.223.38:16720/");
		_departmentService = new DepartmentService(_httpClient);
		_tableService = new TableService(_httpClient);
		_timer = TimerUtils.CreateTimer(UpdateDateTime, TimeSpan.FromSeconds(1));
		_timer.Start();
		LoadDepartments();
	}

	private void TextBox1_GotFocus(object sender, RoutedEventArgs e)
	{
		SetTextBoxPlaceholder(TextBox1, true);
	}

	private void TextBox1_LostFocus(object sender, RoutedEventArgs e)
	{
		SetTextBoxPlaceholder(TextBox1, false);
	}

	private void SetTextBoxPlaceholder(TextBox textBox, bool isFocused)
	{
		if (isFocused && textBox.Text == PlaceholderText)
		{
			textBox.Text = string.Empty;
			textBox.Foreground = Brushes.Black;
		}
		else if (!isFocused && string.IsNullOrWhiteSpace(textBox.Text))
		{
			textBox.Text = PlaceholderText;
			textBox.Foreground = Brushes.Gray;
		}
	}

	private void UpdateDateTime()
	{
		UIUtils.UpdateDateTime(NameTextBlock, DateTimeTextBlock, "Huseyn");
	}

	private async void LoadDepartments()
	{
		try
		{
			_allDepartments = await _departmentService.GetDepartmentsAsync(1);
			DisplayDepartments();
		}
		catch (Exception ex)
		{
			UIUtils.ShowError("Error loading departments", ex.Message);
		}
	}

	private void DisplayDepartments()
	{
		var initialDepartments = _allDepartments.Take(InitialDepartmentCount).ToList();
		UIUtils.DisplayDepartments(InitialDepartmentsPanel, initialDepartments, LoadTablesForDepartment);

		DepartmentsListBox.ItemsSource = _allDepartments.Skip(InitialDepartmentCount).ToList();
		UpdateShowMoreButton();
	}

	private void UpdateShowMoreButton()
	{
		ShowMoreButton.Content = _isShowMore ? "<" : ">";
		DepartmentsPopup.IsOpen = _isShowMore;
	}
	private void ShowMoreButton_LostFocus(object sender, RoutedEventArgs e)
	{
		_isShowMore = false;
		ShowMoreButton.Content = _isShowMore ? "<" : ">";
	}

	private async void LoadTablesForDepartment(int departmentId)
	{
		try
		{
			var tables = await _tableService.GetTablesForDepartmentAsync(departmentId);
			foreach (var table in tables)
				table.StatusColor = UIUtils.GetStatusColor(table.TableStatusId);
			TablesListBox.ItemsSource = tables;
		}
		catch (Exception ex)
		{
			UIUtils.ShowError("Error loading tables", ex.Message);
		}
	}

	private void ShowMoreButton_Click(object sender, RoutedEventArgs e)
	{
		_isShowMore = !_isShowMore;
		UpdateShowMoreButton();
	}

	private void DepartmentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (DepartmentsListBox.SelectedItem is Department selectedDepartment)
		{
			LoadTablesForDepartment(selectedDepartment.Id);
			DepartmentsPopup.IsOpen = false;
		}
	}

	private async void SearchButton_Click(object sender, RoutedEventArgs e)
	{
		await UIUtils.SearchTables(TextBox1, _allDepartments, _tableService, TablesListBox, LoadTablesForDepartment);
	}

	private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
	{
		if (sender is TextBlock textBlock)
			textBlock.Foreground = Brushes.Aqua;
	}

	private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
	{
		if (sender is TextBlock textBlock)
			textBlock.Foreground = Brushes.Black;
	}
}
