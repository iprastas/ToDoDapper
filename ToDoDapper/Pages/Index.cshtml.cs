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
            return connection.Query<Todo>("SELECT * FROM todos").AsList<Todo>();
        }
        public void OnGet()
        {
            TodoList = GetTodos();
        }

        public IActionResult OnGetAdd(string title)
        {
            using var connection = CreateConnection();
            var taskId = connection.ExecuteScalar<int>("insert into todos (title) values (@Title) returning Id", new { Title = title });
            return RedirectToPage();
        }
        public void OnPostUpdate(int id, bool isCompleted) //обновить статус задачи
        {
            using var connection = CreateConnection();
            string sql = "update todos set is_completed = @IsCompleted where id = @Id";
            connection.Execute(sql, new { Id = id, IsCompleted = isCompleted });
        }
        public void OnPostDelete(int id) // удалить задачу 
        {
            using var connection = CreateConnection();
            string sql = "delete from todos where id = @Id";
            connection.Execute(sql, new { Id = id });
        }
    }
}
