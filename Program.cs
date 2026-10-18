using Blog.Models;
using Blog.Repositories;
using Microsoft.Data.SqlClient;

var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString =
    $"Server=127.0.0.1,1433;Database=Blog;User ID=sa;Password={password};TrustServerCertificate=True";

using var connection = new SqlConnection(connectionString);
var repository = new Repository<User>(connection);

// Deixe só UMA linha sem // e rode com: dotnet run
ReadUsers(repository);
//CreateUser(repository);
//ReadUser(repository);
//UpdateUser(repository);
//ReadWithRoles(connection);
//DeleteUser(repository);

static void CreateUser(Repository<User> repository)
{
    var user = new User
    {
        Bio = "Estudante de .NET",
        Email = "maria@teste.com",
        Image = "https://exemplo.com/maria.jpg",
        Name = "Maria Teste",
        Slug = "maria-teste",
        PasswordHash = Guid.NewGuid().ToString()
    };

    repository.Create(user);
    Console.WriteLine("Usuário criado");
}

static void ReadUsers(Repository<User> repository)
{
    var users = repository.Read();
    foreach (var item in users)
        Console.WriteLine($"{item.Id} - {item.Email}");
}

static void ReadUser(Repository<User> repository)
{
    var user = repository.Read(3);
    Console.WriteLine(user?.Email);
}

static void UpdateUser(Repository<User> repository)
{
    var user = repository.Read(3);
    user.Name = "Maria Silva";
    repository.Update(user);

    Console.WriteLine(user.Name);
}

static void DeleteUser(Repository<User> repository)
{
    var user = repository.Read(3);
    repository.Delete(user);
    Console.WriteLine("Usuário excluído");
}

static void ReadWithRoles(SqlConnection connection)
{
    var repository = new UserRepository(connection);
    var users = repository.ReadWithRole();

    foreach (var user in users)
    {
        Console.WriteLine(user.Email);
        foreach (var role in user.Roles) Console.WriteLine($" - {role.Slug}");
    }
}