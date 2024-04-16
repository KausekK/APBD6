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
    public IActionResult GetAnimals()
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "";

        command.CommandText = "SELECT * FROM Animal";

        var reader = command.ExecuteReader();
        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                 Name  = reader.GetString(nameOrdinal),
                 Description = reader.GetString(descriptionOrdinal),
                 Category = reader.GetString(categoryOrdinal),
                 Area = reader.GetString(areaOrdinal)
            });
        }
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(Animal animal)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        using SqlCommand command = new SqlCommand();
        command.Connection = connection;

        command.CommandText = "INSERT INTO Animal VALUES (@animalName,'','',''";
        command.Parameters.AddWithValue("@animalName", animal.Name);
        command.ExecuteNonQuery();
        
        return Created("", null);
    }
}