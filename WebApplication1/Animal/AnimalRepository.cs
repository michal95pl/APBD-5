using System.Data.SqlClient;

namespace WebApplication1.Animal;

public interface IAnimalRepository
{
    IEnumerable<Animal> FetchAllAnimals(string orderBy);
    bool AddAnimal(AnimalDTO dto);
    bool DeleteAnimal(int id);
    bool CreateOrUpdate(int id, AnimalDTO dto);
}

public class AnimalRepository : IAnimalRepository
{
    private IConfiguration _configuration;
    
    public AnimalRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private bool CheckOrderBy(string orderBy)
    {
        switch (orderBy)
        {
            case "idAnimal": return true;
            case "Name": return true;
            case "Description": return true;
            case "Category": return true;
            case "Area": return true;
            
            default: return false;
        }
    }
    
    public IEnumerable<Animal> FetchAllAnimals(string orderBy)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("SELECT IdAnimal, Name, Description, Category, Area FROM Animal ORDER BY " +
            (CheckOrderBy(orderBy)? orderBy : "name"), connection);
        
        using var reader = command.ExecuteReader();
        
        var animals = new List<Animal>();
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = (int)reader["IdAnimal"],
                Name = (string)reader["Name"],
                Description = reader["Description"] is DBNull? null : (string)reader["Description"],
                Category = (string)reader["Category"],
                Area = (string)reader["Area"]
            });
        }

        return animals;
    }

    public bool AddAnimal(AnimalDTO dto)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("INSERT INTO Animal (Name, Description, Category, Area) VALUES " +
                                           "(@name, @description, @category, @area)", connection);
        
        command.Parameters.AddWithValue("@name", dto.Name);
        command.Parameters.AddWithValue("@description", dto.Description is null? DBNull.Value : dto.Description);
        command.Parameters.AddWithValue("@category", dto.Category);
        command.Parameters.AddWithValue("@area", dto.Area);
        
        var effects = command.ExecuteNonQuery();
        
        return effects > 0;
    }

    public bool DeleteAnimal(int id)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @idAnimal", connection);
        
        command.Parameters.AddWithValue("@idAnimal", id);
        
        var effects = command.ExecuteNonQuery();
        
        return effects > 0;
    }

    public bool CreateOrUpdate(int id, AnimalDTO dto)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        connection.Open();
        
        using var command = new SqlCommand("IF EXISTS(SELECT 1 FROM Animal WHERE IdAnimal = @idAnimal)" +
                                           " UPDATE Animal SET Name = @name, Description = @description, CATEGORY = @category, AREA = @area WHERE IdAnimal = @idAnimal;" +
                                           " ELSE" +
                                           " INSERT INTO Animal (Name, Description, CATEGORY, AREA) VALUES (@name, @description, @category, @area);", 
            connection);
        
        command.Parameters.AddWithValue("@idAnimal", id);
        command.Parameters.AddWithValue("@name", dto.Name);
        command.Parameters.AddWithValue("@description", dto.Description is null? DBNull.Value : dto.Description);
        command.Parameters.AddWithValue("@category", dto.Category);
        command.Parameters.AddWithValue("@area", dto.Area);
        
        var effects = command.ExecuteNonQuery();
        
        return effects > 0;
    }
}