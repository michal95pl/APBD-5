using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Animal;

[Route("/api/animal")]
public class AnimalController : ControllerBase
{

    private IAnimalService _animalService;
    
    public AnimalController(IAnimalService animalService)
    {
        _animalService = animalService;
    }
    
    [HttpGet("")]
    public IActionResult GetAllAnimals([FromQuery] string orderBy)
    {
        var data = _animalService.GetAllAnimal(orderBy);
        return Ok(data);
    }
    
    [HttpPost("")]
    public IActionResult CreateAnimal([FromBody] AnimalDTO dto)
    {
        if (_animalService.AddAnimal(dto))
            return Created();

        return Conflict();
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateAnimal(int id, AnimalDTO dto)
    {
        if (_animalService.UpdateAnimal(id, dto))
            return Created();

        return Conflict();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteAnimal([FromRoute] int id)
    {
        if (_animalService.DeleteAnimal(id))
            return NoContent();
        
        return Conflict();
    }
}