using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Utils;

public static class UIUtils
{
	private static TextBlock _selectedTextBlock;

	public static void SetPlaceholder(TextBox textBox, string placeholderText)
	{
		if (string.IsNullOrEmpty(textBox.Text))
		{
			textBox.Text = placeholderText;
			textBox.Foreground = Brushes.Gray;
		}
	}

	public static void UpdateDateTime(TextBlock nameTextBlock, TextBlock dateTimeTextBlock, string userName)
	{
		nameTextBlock.Text = userName;
		dateTimeTextBlock.Text = $"{DateTime.Now:dddd, MMMM d} {DateTime.Now:HH:mm}";
	}

	public static SolidColorBrush GetStatusColor(int statusId)
	{
		return statusId switch
		{
			1 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#82bebe")),
			2 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ed863d")),
			3 => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#287bac")),
			_ => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#fefffe")) 
		};
	}

	public static void DisplayDepartments(
		Panel initialDepartmentsPanel,
		List<Department> departmentsToShow,
		Action<int> loadTablesCallback)
	{
		initialDepartmentsPanel.Children.Clear();

		foreach (var department in departmentsToShow)
		{
			var textBlock = new TextBlock
			{
				Text = department.Name,
				Margin = new Thickness(5),
				Tag = department,
				Foreground = Brushes.Black,
				Background = Brushes.Transparent,
				FontSize = 14,
				Padding = new Thickness(10),
				Cursor = Cursors.Hand
			};

			textBlock.MouseEnter += (s, e) =>
			{
				if (textBlock != _selectedTextBlock)
					textBlock.Foreground = Brushes.Aqua;
			};

			textBlock.MouseLeave += (s, e) =>
			{
				if (textBlock != _selectedTextBlock)
					textBlock.Foreground = Brushes.Black;
			};

			textBlock.MouseLeftButtonDown += (s, e) =>
			{
				if (_selectedTextBlock != null && _selectedTextBlock != textBlock)
					_selectedTextBlock.Foreground = Brushes.Black;

				textBlock.Foreground = Brushes.Aqua;
				_selectedTextBlock = textBlock;

				if (textBlock.Tag is Department selectedDepartment)
					loadTablesCallback(selectedDepartment.Id);
			};

			initialDepartmentsPanel.Children.Add(textBlock);
		}
	}

	public static async Task SearchTables(
		TextBox textBox,
		List<Department> allDepartments,
		TableService tableService,
		ItemsControl tablesListBox,
		Action<int> loadTablesCallback)
	{
		var searchTerm = textBox.Text.ToLower();
		if (string.IsNullOrWhiteSpace(searchTerm))
		{
			ResetTablesView(allDepartments, tablesListBox);
			return;
		}

		var filteredTables = new List<Table>();
		foreach (var department in allDepartments)
		{
			if (department.Tables == null || !department.Tables.Any())
				department.Tables = await tableService.GetTablesForDepartmentAsync(department.Id);
			
			var matchedTables = department.Tables?.Where(t => t.Name.ToLower().Contains(searchTerm)).ToList();
			if (matchedTables != null && matchedTables.Any())
				filteredTables.AddRange(matchedTables);
		}

		foreach (var table in filteredTables)
		{
			table.StatusColor = GetStatusColor(table.TableStatusId);
		}

		tablesListBox.ItemsSource = filteredTables;
		if (!filteredTables.Any())
			ShowError("Error", "No Tables found!");
	}

	public static void ShowError(string title, string message)
	{
		MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
	}

	private static void ResetTablesView(List<Department> allDepartments, ItemsControl tablesListBox)
	{
		var allTables = allDepartments.SelectMany(d => d.Tables).ToList();
		tablesListBox.ItemsSource = allTables;
	}
}