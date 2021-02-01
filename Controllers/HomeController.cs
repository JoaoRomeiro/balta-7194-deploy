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
[Route("v1")]
public class HomeController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<dynamic>>> Get([FromServices] DataContext context)
    {
        var employee = new User { Id = 1, UserName = "Robin", Password = "123", Role = "employee" };
        var manager = new User { Id = 2, UserName = "Batman", Password = "123", Role = "manager" };
        var category = new Category { Id = 1, Title = "Informática" };
        var product = new Product { Id = 1, Category = category, Title = "Mouse", Price = 10.99M, Description = "Mouse Óptico" };

        context.Users.Add(employee);
        context.Users.Add(manager);
        context.Categories.Add(category);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return Ok(new { message = "Dados configurados" });
    }
}
