using System;
using System.Collections.Generic;
using System.Text;

namespace Todo
{
    public class TodoItem
    {
        public int ID { get; set; }
        public bool Completed { get; set; }
        public string TodoContent { get; set; }
    }

    public static class TodoFactory
    {
        public static List<TodoItem> GetToDoList()
        {
            return new List<TodoItem>{
                new TodoItem() {ID=1, Completed=false,TodoContent="Buy milk from Coles" },
                new TodoItem() {ID=2, Completed=false,TodoContent="Deploy PortalData and PortalTNSP" },
                new TodoItem() {ID=3, Completed=false,TodoContent="Test Conduit" },
                new TodoItem() {ID=4, Completed=false,TodoContent="Text Ollie" },
                new TodoItem() {ID=5, Completed=false,TodoContent="Watch YouTube" },
            };
        }
    }
}
