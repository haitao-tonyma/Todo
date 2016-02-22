using Foundation;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using UIKit;

namespace Todo
{
	partial class ToDoTableViewController : UITableViewController
	{
        const string CELL_ID = "id";
        List<TodoItem> data;
        public ToDoTableViewController (IntPtr handle) : base (handle)
		{
            data = TodoFactory.GetToDoList();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            TableView.ContentInset = new UIEdgeInsets(this.TopLayoutGuide.Length, 0, 0, 0);
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(CELL_ID);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CELL_ID);

                cell.TextLabel.TextColor = UIColor.FromRGB(59, 102, 136);
                cell.TextLabel.Font = UIFont.SystemFontOfSize(20, UIFontWeight.Bold);

                cell.DetailTextLabel.TextColor = UIColor.FromRGB(0, 142, 255);
                cell.DetailTextLabel.Font = UIFont.ItalicSystemFontOfSize(12);
            }

            var todo = data[indexPath.Row];

            cell.TextLabel.Text = todo.TodoContent;

            return cell;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return data.Count;
        }

    }
}
