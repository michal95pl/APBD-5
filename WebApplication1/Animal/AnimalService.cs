namespace WebApplication1.Animal;

public interface IAnimalService
{
    bool DeleteAnimal(int id);
    bool AddAnimal(AnimalDTO dto);
    IEnumerable<Animal> GetAllAnimal(string orderBy);
    bool UpdateAnimal(int id, AnimalDTO dto);
}

public class AnimalService : IAnimalService
{

    private IAnimalRepository _animalRepository;
    
    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public IEnumerable<Animal> GetAllAnimal(string orderBy)
    {
        return _animalRepository.FetchAllAnimals(orderBy);
    }
    
    public bool AddAnimal(AnimalDTO dto)
    {
        return _animalRepository.AddAnimal(dto);
    }
    
    public bool DeleteAnimal(int id)
    {
        return _animalRepository.DeleteAnimal(id);
    }

    public bool UpdateAnimal(int id, AnimalDTO dto)
    {
        return _animalRepository.CreateOrUpdate(id, dto);
    }
}