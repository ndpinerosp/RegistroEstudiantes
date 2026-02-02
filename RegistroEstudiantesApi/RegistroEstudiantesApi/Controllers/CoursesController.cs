using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Services.Interfaces;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseResponseDto>>> GetCourses()
    {
        var courses = await _courseService.GetAllCoursesAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseResponseDto>> GetCourse(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);

        if (course == null)
            return NotFound(new { message = "Materia no encontrada" });

        return Ok(course);
    }

    [HttpGet("{id}/classmates")]
    public async Task<ActionResult<IEnumerable<string>>> GetClassmates(int id)
    {
        var classmates = await _courseService.GetClassmatesAsync(id);
        return Ok(classmates);
    }
}