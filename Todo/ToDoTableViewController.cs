using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Todo.Data;

namespace Todo
{
	partial class ToDoTableViewController : UITableViewController
	{
		const string CellIdentifier = "TodoCell";
        List<TodoItem> data;
        public ToDoTableViewController (IntPtr handle) : base (handle)
		{
        }

        public async override void ViewDidLoad()
        {
			base.ViewDidLoad();

			this.NavigationItem.RightBarButtonItem = this.EditButtonItem;

			UIBarButtonItem addButton = new UIBarButtonItem(UIBarButtonSystemItem.Add, (s,e) => OnAddTodo());
			this.NavigationItem.LeftBarButtonItem = addButton;

			UIRefreshControl refreshControl = new UIRefreshControl();
			refreshControl.ValueChanged += async (sender, e) => {
				data = (await new DataStore().Reset()).ToList();
				BeginInvokeOnMainThread(() => {
					TableView.ReloadData();
					refreshControl.EndRefreshing();
				});
			};
			this.RefreshControl = refreshControl;

			data = new List<TodoItem>();


			DefinesPresentationContext = true;

			DataStore db = new DataStore();
			data.AddRange(await db.LoadTodos());
			TableView.ReloadData();
        }

		public override void ViewWillAppear(bool animated)
		{
			if (newTodo != null) {
				if (newTodo.Id != 0) {
					data.Add(newTodo);
				}
				newTodo = null;
			}

			TableView.ReloadData();
		}

		UITableViewRowAction[ ] editActions;

		public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (editActions == null) {
				editActions = new[] {
					UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Complete", OnComplete),
					UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, "Incomplete", OnComplete),
					UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive,"Delete", OnDelete),
				};
				editActions[0].BackgroundColor = UIColor.Blue;
			}

			TodoItem todo = data[indexPath.Row];

			var rowActions = new UITableViewRowAction[2];
			rowActions[0] = (todo.Completed)?editActions[1]:editActions[0];
			rowActions[1] = editActions[2];
			return rowActions;
		}

		async void OnComplete(UITableViewRowAction rowAction, NSIndexPath indexPath)
		{
			TodoItem todo = data[indexPath.Row];
			todo.Completed = !todo.Completed;
			TableView.ReloadRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
			await new DataStore().Update(todo);
		}

		async void OnDelete(UITableViewRowAction rowAction, NSIndexPath indexPath)
		{
			await DeleteTodo(indexPath.Row, indexPath);
		}

		async Task DeleteTodo(int row, NSIndexPath indexPath)
		{
			var todo = data[row];
			data.RemoveAt(row);
			TableView.DeleteRows(new[] { indexPath }, UITableViewRowAnimation.Automatic);
			await new DataStore().Delete(todo);
		}

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

			var cell = tableView.DequeueReusableCell(CellIdentifier, indexPath);

			TodoItem todo = data[indexPath.Row];

			cell.TextLabel.Text = todo.TodoContent;

			if (todo.Completed) {
				cell.TextLabel.TextColor = UIColor.Gray;
			}
			else {
				cell.TextLabel.TextColor = UIColor.Black;
			}

			return cell;
        }

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return data.Count;
		}

		TodoItem newTodo;

		void OnAddTodo()
		{
			newTodo = new TodoItem();
			PerformSegue("showDetail", this);
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "showDetail") {
				var detailViewController = segue.DestinationViewController as ToDoDetailViewController;
				if (detailViewController != null) {
					
					var selectedTodo = newTodo;
					if (selectedTodo == null)
					{
						selectedTodo = data[TableView.IndexPathForSelectedRow.Row];
					}
					detailViewController.SelectedTodo = selectedTodo;
				}
			}
		}
	}
}
