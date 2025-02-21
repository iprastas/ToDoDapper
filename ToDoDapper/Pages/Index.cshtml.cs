using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Data;

namespace ToDoDapper.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<Todo> TodoList { get; set; } = [];

        readonly string? connectionString;
        readonly IConfiguration? configuration;
        public IndexModel(IConfiguration? _configuration)
        {
            configuration = _configuration;
            connectionString = configuration == null ? string.Empty : configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(connectionString);

        public List<Todo> GetTodos() //получить все задачи
        {
            using var connection = CreateConnection(); 
            var list = connection.Query<Todo>("SELECT * FROM todos").AsList<Todo>();
            return list;
        }
        public void OnGet()
        {
            TodoList = GetTodos();
        }

        public IActionResult OnGetAdd(string title)
        {
            using var connection = CreateConnection();
            var taskId = connection.ExecuteScalar<int>("insert into todos (title, is_completed) values (@Title, @is_completed) returning Id", new { Title = title, is_completed = false });
            return RedirectToPage();
        }

        public void OnGetUpdate(int id, bool isCompleted) //обновить статус задачи
        {
            using var connection = CreateConnection();
            string sql = "update todos set is_completed = @is_completed where id = @Id";
            connection.Execute(sql, new { Id = id, is_completed = isCompleted });
            //return RedirectToPage();
        }

        public void OnGetDelete(int id) // удалить задачу 
        {
            using var connection = CreateConnection();
            string sql = "delete from todos where id = @Id";
            connection.Execute(sql, new { Id = id });
        }
    }
}
