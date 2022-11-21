using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(autor => autor.Libros).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")] // api.autores/id
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            var autorExists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!autorExists)
            {
                return NotFound();
            }

            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }

            context.Update(autor);
            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")] // api.autores/id
        public async Task<ActionResult> Delete(int id)
        {
            var autorExists = await context.Autores.AnyAsync(autor => autor.Id == id);

            if (!autorExists)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();

            return Ok();
        }

    }
}
