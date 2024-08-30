using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;
using Sistema.Repositories.Interfaces;

namespace Sistema.Controllers
{
    [Route("usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApplicationUser>>> BuscarTodosUsuarios()
        {
            List<ApplicationUser> usuarios = await _userManager.Users.ToListAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> BuscarUsuarioPorId(string id)
        {
            ApplicationUser usuarios = await _userManager.FindByIdAsync(id);

            return Ok(usuarios);
        }
    }
}
