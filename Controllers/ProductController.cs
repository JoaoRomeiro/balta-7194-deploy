using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// Endpoint => URL
// http://localhost:5000
// https://localhost:5001
[Route("v1/products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
    {
        var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();
        return (products);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
    {
        var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return BadRequest(new { message = "Registro não encontrado" });

        return Ok(product);
    }

    [HttpGet]
    [Route("categories/{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> GetByCategoryId(int id, [FromServices] DataContext context)
    {
        var product = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.Category.Id == id).ToListAsync();

        if (product == null)
            return BadRequest(new { message = "Registro não encontrado" });

        return Ok(product);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Product>> Post([FromBody] Product model, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();

            return Ok(model);
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possível criar o registro" });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<Product>> Put(int id, [FromBody] Product model, [FromServices] DataContext context)
    {
        if (!model.Id.Equals(id))
            return NotFound(new { message = "O Id do Query Params deve ser igual ao enviado no JSON" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Entry<Product>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Não foi possível editar o registro, pois ele pode estar sendo utilizado por outro processo" });
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possível editar o registro" });
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "manager")]
    public async Task<ActionResult<Product>> Delete(int id, [FromServices] DataContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (product == null)
            return BadRequest(new { message = "Registro não encontrado" });

        try
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Ok(new { message = "Registro removido com sucesso" });
        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Não foi possível remover o registro" });
        }
    }
}
