using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MSHC.WPF
{
	/// <summary>
	/// Statische Hilfsmethoden für WPF DataGrid
	/// 
	/// http://blogs.msdn.com/b/vinsibal/archive/2008/11/05/wpf-datagrid-new-item-template-sample.aspx
	/// </summary>
	public static class DataGridHelper
	{
		public static DataGridCell GetCell(DataGrid dataGrid, int row, int column)
		{
			DataGridRow rowContainer = GetRow(dataGrid, row);
			if (rowContainer != null)
			{
				DataGridCellsPresenter presenter = rowContainer.GetChildOfType<DataGridCellsPresenter>();

				// try to get the cell but it may possibly be virtualized
				DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
				if (cell == null)
				{
					// now try to bring into view and retreive the cell
					dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);

					cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
				}

				return cell;
			}

			return null;
		}

		public static int GetColumnCount(DataGrid dataGrid)
		{
			return dataGrid.Columns.Count;
		}

		private static DataGridRow GetRow(DataGrid dataGrid, int index)
		{
			if (index < 0 || index >= dataGrid.Items.Count) return null;

			DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
			if (row == null)
			{

				dataGrid.ScrollIntoView(dataGrid.Items[index]);
				dataGrid.UpdateLayout();

				row = (DataGridRow) dataGrid.ItemContainerGenerator.ContainerFromIndex(index);
			}

			return row;
		}

		private static TContainer GetContainerFromIndex<TContainer>(ItemsControl itemsControl, int index) where TContainer : DependencyObject
		{
			return (TContainer) itemsControl.ItemContainerGenerator.ContainerFromIndex(index);
		}

		public static bool IsEditing(DataGrid dataGrid)
		{
			return GetEditingRow(dataGrid) != null;
		}

		private static DataGridRow GetEditingRow(DataGrid dataGrid)
		{
			var sIndex = dataGrid.SelectedIndex;
			if (sIndex >= 0)
			{
				var selected = GetContainerFromIndex<DataGridRow>(dataGrid, sIndex);
				if (selected.IsEditing) return selected;
			}

			for (int i = 0; i < dataGrid.Items.Count; i++)
			{
				if (i == sIndex) continue;
				var item = GetContainerFromIndex<DataGridRow>(dataGrid, i);
				if (item.IsEditing) return item;
			}

			return null;
		}
	}
}
