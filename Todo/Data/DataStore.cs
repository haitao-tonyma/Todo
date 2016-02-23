using System;
using System.Collections.Generic;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace Todo.Data
{
	public class DataStore
	{
		const string DbFilename = "todo.db3";
		SQLiteAsyncConnection db;

		async Task Initialize()
		{
			string filename = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
				"..", "Library", DbFilename);
			db = new SQLiteAsyncConnection(filename);
			await db.CreateTableAsync<TodoItem>();
		}

		public async Task<IList<TodoItem>> LoadTodos()
		{
			if (db == null) {
				await Initialize();
			}

			var items = await db.Table<TodoItem>().ToListAsync();
			if (!items.Any()) {
				items.AddRange(InitToDos());
				await db.InsertAllAsync(items);
			}

			return items;
		}

		public async Task Update(TodoItem expense)
		{
			if (db == null) {
				await Initialize();
			}

			if (expense.Id == 0) {
				await db.InsertAsync(expense);
			} else {
				await db.UpdateAsync(expense);
			}
		}

		public async Task Update(IEnumerable<TodoItem> input)
		{
			if (db == null) {
				await Initialize();
			}

			var expenses = input.ToList();
			await db.RunInTransactionAsync(conn => {
				conn.UpdateAll(expenses.Where(e => e.Id > 0));
				conn.InsertAll(expenses.Where(e => e.Id == 0));
			});
		}

		public async Task Delete(TodoItem todo)
		{
			if (db == null) {
				await Initialize();
			}
			await db.DeleteAsync(todo);
		}

		public async Task<IList<TodoItem>> Reset()
		{
			if (db == null) {
				await Initialize();
			}

			await db.DropTableAsync<TodoItem>();
			await db.CreateTableAsync<TodoItem>();

			var items = InitToDos().ToList();
			await db.InsertAllAsync(items);

			return items;
		}

		private IEnumerable<TodoItem> InitToDos()
		{
			yield return (new TodoItem {
				Completed = false,
				TodoContent = "Buy Milk"
			});
			yield return (new TodoItem {
				Completed = false,
				TodoContent = "Check mail"
			});
			yield return (new TodoItem {
				Completed = false,
				TodoContent = "Go to GYM"
			});
			yield return (new TodoItem {
				Completed = false,
				TodoContent = "Watch YouTube"
			});
			yield return (new TodoItem {
				Completed = false,
				TodoContent = "Other thing ..."
			});
		}

	}
}

