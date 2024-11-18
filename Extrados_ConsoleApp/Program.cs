using Entities;
using DAO;
string connectionString = "Server=localhost;Port=3306;Database=extradosdb;User ID=root;Password=123456;";

GetUser getUsers = new GetUser(connectionString);

Console.WriteLine("Que operacion desea realizar?");
Console.WriteLine("Escriba 1 para GetAll, 2 para GetByID, 3 para DeleteUser, 4 para UpdateUser");
int result = int.Parse(Console.ReadLine());

if (result == 1)
{
    List<User> users = getUsers.GetAllUsers();
    foreach (var user in users)
    {
        Console.WriteLine($"ID: {user.ID}, Nombre: {user.Nombre}, Edad: {user.Edad}");
    }
    return;
}
else if (result == 2)
{
    Console.WriteLine("Escriba el ID del User");
    try
    {
        int userID = int.Parse(Console.ReadLine());
        var user = getUsers.GetUserByID(userID);
        Console.WriteLine($"ID: {user.ID}, Nombre: {user.Nombre}, Edad: {user.Edad}");
        return;
    }
    catch (Exception exc)
    {
        Console.WriteLine("El usuarioID ingresado es invalido");
        Console.WriteLine("Presione *enter* para imprimir un informe detallado del error ");
        Console.ReadKey();
        Console.WriteLine(exc);
    }
}
else if (result == 3)
{
    Console.WriteLine("Escriba el ID del User");
    try
    {
        int userID = int.Parse(Console.ReadLine());
        var userDeleted = getUsers.SoftDeleteUser(userID);
        if (userDeleted)
        {
            Console.WriteLine("El usuario ha sido borrado");
        }
        else
        {
            Console.WriteLine("El usuario no pudo ser borrado");
        }
        return;
    }
    catch (Exception exc)
    {
        Console.WriteLine("El usuarioID ingresado es invalido");
        Console.WriteLine("Presione *enter* para imprimir un informe detallado del error ");
        Console.ReadKey();
        Console.WriteLine(exc);
    }
}
else if (result == 4)
{
    Console.WriteLine("Escriba el ID del usuario");
    try
    {
        int userID = int.Parse(Console.ReadLine());
        var user = getUsers.GetUserByID(userID);

        Console.WriteLine($"Usuario encontrado: ID: {user.ID}, Nombre: {user.Nombre}, Edad: {user.Edad}");

        Console.WriteLine("Escriba el nuevo nombre del usuario");
        string nombre = Console.ReadLine();

        Console.WriteLine("Escriba la nueva edad del usuario");
        int edad = int.Parse(Console.ReadLine());

        var userUpdated = getUsers.UpdateUser(user.ID, nombre, edad);

        Console.WriteLine($"usuario actualizado: ID: {userUpdated.ID}, Nombre: {userUpdated.Nombre}, Edad: {userUpdated.Edad}");

        return;
    }
    catch (Exception exc)
    {
        Console.WriteLine("Ha ocurrido un error");
        Console.WriteLine("Presione *enter* para obtener un informe detallado del problema ");
        Console.ReadKey();
        Console.WriteLine(exc);
    }
}
else
{
    Console.WriteLine("Operacion invalida");
    return;
}
