using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext context = new PizzaContext())
            {
                List<Pizza> pizzas = context.Pizzas.ToList<Pizza>();
                return View("Index", pizzas);
            }
        }

        public IActionResult Detail(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaDetail = context.Pizzas.Where(pizza => pizza.PizzaID == id).FirstOrDefault();

                if ( pizzaDetail == null)
                {
                    return NotFound("La pizza con id non è stata trovata");
                } else
                {
                    return View("Detail", pizzaDetail);
                }
                
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (Pizza formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }

            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza = new Pizza();
                pizza.Name = formData.Name;
                pizza.Description = formData.Description;
                pizza.Photo = formData.Photo;
                pizza.Price = formData.Price;

                context.Pizzas.Add(pizza);

                context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Update (int id)
        {
            PizzaContext context = new PizzaContext();

            Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaID == id).FirstOrDefault();
            
            if (pizza == null)
            {
                return NotFound("Pizza non trovata");
            }

            return View(pizza);
        }

        [HttpPost]
        public IActionResult Update(int id, Pizza formData)
        {
            PizzaContext context = new PizzaContext();

            Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaID == id).FirstOrDefault();

            if(pizza != null)
            {
                pizza.Name = formData.Name;
                pizza.Description = formData.Description;
                pizza.Photo = formData.Photo;
                pizza.Price = formData.Price;
            }else
            {
                return NotFound("Pizza non trovata");
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
