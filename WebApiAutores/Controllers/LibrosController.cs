using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await context.Libros.Include(libro => libro.Autor).FirstOrDefaultAsync(libro => libro.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> Post(Libro libro)
        {
            var autorExists = await context.Autores.AnyAsync(autor => autor.Id == libro.AutorId);

            if (!autorExists)
            {
                return BadRequest($"No existe el autor de id: {libro.AutorId}");
            }

            context.Add(libro);
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
