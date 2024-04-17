using System.Data;
using Lab6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Lab6.Controllers;
[ApiController]
[Route("/api/animals")]
public class AnimalsControllers : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsControllers(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet]
    public IEnumerable<Animal> GetAnimals([FromQuery] string orderBy = "name")
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        switch (orderBy)
        {
            case "description":
                command.CommandText = "SELECT * FROM Animal ORDER BY Description ASC";
                break;
            case "category":
                command.CommandText = "SELECT * FROM Animal ORDER BY Category ASC";
                break;
            case "area":
                command.CommandText = "SELECT * FROM Animal ORDER BY Area ASC";
                break;
            default:
                command.CommandText = "SELECT * FROM Animal ORDER BY Name ASC";
                break;
        }
        var reader = command.ExecuteReader();
        List<Animal> animals = new List<Animal>();
        
        while (reader.Read())
        {
           var animal = new Animal
            {
                IdAnimal = (int) reader["IdAnimal"],
                 Name  = reader["Name"].ToString(),
                 Description =  reader["Description"].ToString(),
                 Category =  reader["Category"].ToString(),
                 Area =  reader["Area"].ToString(),
            };
            animals.Add(animal);
        }
        connection.Close();
        return animals;
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = "INSERT INTO Animal (Name, Description, Category, Area) " +
                              "SELECT @animalName, @description, @category, @area";
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.Parameters.AddWithValue("@description", animal.Description);
        command.Parameters.AddWithValue("@category", animal.Category);
        command.Parameters.AddWithValue("@area", animal.Area);

        command.ExecuteNonQuery();
        connection.Close();

        return Created("", null);
    }
    
    [HttpDelete("{idAnimal}")]
    public IActionResult DeleteAnimal(int idAnimal)
    {
        using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        con.Open();
    
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "DELETE FROM Animal WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
    
        var affectedCount = cmd.ExecuteNonQuery();
        con.Close();
        if (affectedCount > 0)
        {
            return NoContent(); 
        }
        return NotFound();
    }

    [HttpPut ("{idAnimal}")]
    public IActionResult  UpdateAnimal(Animal animal, int idAnimal)
    {
        using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
        con.Open();
        
        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "UPDATE Animal SET Name = @Name, Description = @Description, " +
                          "Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal";
        cmd.Parameters.AddWithValue("@IdAnimal", idAnimal);
        cmd.Parameters.AddWithValue("@Name", animal.Name);
        cmd.Parameters.AddWithValue("@Description", animal.Description);
        cmd.Parameters.AddWithValue("@Category", animal.Category);
        cmd.Parameters.AddWithValue("@Area", animal.Area);
        
        var affectedCount = cmd.ExecuteNonQuery();
        con.Close();
        if (affectedCount > 0)
        {
            return Ok();
        }

        return NotFound();
    }
}