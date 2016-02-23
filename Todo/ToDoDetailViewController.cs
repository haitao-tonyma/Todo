using System;
using UIKit;
using Todo.Data;
using System.Collections.Generic;
using System.Linq;

namespace Todo
{
	partial class ToDoDetailViewController : UIViewController
	{

		TodoItem selectedTodo;
		public TodoItem SelectedTodo {
			get {
				return selectedTodo;
			}
			set {
				if (selectedTodo != value) {
					selectedTodo = value;
					OnUpdateDetails();
				}
			}
		}

		void OnUpdateDetails()
		{
			if (!IsViewLoaded)
				return;

			if (SelectedTodo != null) {

				this.ToDo.Text = SelectedTodo.TodoContent;

			} else {
				this.ToDo.Text = string.Empty;
			}
		}

		public ToDoDetailViewController (IntPtr handle) : base (handle)
		{
		}

		async void SaveTodo(object sender, EventArgs e)
		{
			if (SelectedTodo != null) {

				isBusyIndicator.StartAnimating();

				try {
					string value = ToDo.Text;
					SelectedTodo.TodoContent = string.IsNullOrWhiteSpace(value) ? "New Todo" : value;
					SelectedTodo.Completed = false;

					DataStore db = new DataStore();
					await db.Update(SelectedTodo);
				}
				finally {
					isBusyIndicator.StopAnimating();
				}
			}

			NavigationController.PopViewController(true);
		}

		public override void ViewDidLoad()
		{
			saveButton.TouchUpInside += SaveTodo;

			OnUpdateDetails();
		}
	}
}
