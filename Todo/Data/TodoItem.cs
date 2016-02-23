using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Todo.Data
{
    public class TodoItem
    {
		[PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public bool Completed { get; set; }
        public string TodoContent { get; set; }
    }
}
