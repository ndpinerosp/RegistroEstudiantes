using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroEstudiantes.Server.Data.Repositories;
using RegistroEstudiantes.Server.DTOs;
using RegistroEstudiantes.Server.Entities;
using RegistroEstudiantes.Server.Services.Interfaces;
using System.Security.Claims;

namespace RegistroEstudiantes.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpGet("my-dashboard")]
        public async Task<ActionResult<MyDashboardDto>> GetMyDashboard()
        {
            var dashboard = await _enrollmentService.GetStudentDashboardAsync(GetUserId());
            return Ok(dashboard);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> Enroll([FromBody] EnrollmentRequestDto request)
        {
            try
            {
                await _enrollmentService.EnrollStudentAsync(GetUserId(), request.CourseId);
                return Ok(new { message = "Inscripción exitosa" });
            }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpDelete("unenroll/{courseId}")]
        public async Task<IActionResult> Unenroll(int courseId)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(GetUserId(), courseId);
            if (!result) return NotFound("No se encontró la inscripción.");
            return Ok(new { message = "Materia dada de baja exitosamente." });
        }

        [AllowAnonymous]
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<StudentProgressDto>>> GetPublicData()
        {
            return Ok(await _enrollmentService.GetPublicDataAsync());
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
    }
}
