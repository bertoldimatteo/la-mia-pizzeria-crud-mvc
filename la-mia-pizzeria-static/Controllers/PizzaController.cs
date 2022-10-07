using la_mia_pizzeria_crud_mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_crud_mvc.Controllers
{
    public class PizzaController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext context = new PizzaContext())
            {
                List<Pizza> pizzas = context.Pizzas.Include("Category").ToList();
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
            PizzasCategories pizzasCategories = new PizzasCategories();

            pizzasCategories.Categories = new PizzaContext().Categories.ToList();

            return View(pizzasCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (PizzasCategories formData)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", formData);
            }

            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizza = new Pizza();
                pizza.Name = formData.Pizza.Name;
                pizza.Description = formData.Pizza.Description;
                pizza.Photo = formData.Pizza.Photo;
                pizza.Price = formData.Pizza.Price;
                pizza.CategoryId = formData.Pizza.CategoryId;

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

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            PizzaContext context = new PizzaContext();

            Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaID == id).FirstOrDefault();

            if(pizza == null)
            {
                return NotFound("Pizza non trovata");
            }else
            {
                context.Pizzas.Remove(pizza);
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }

}
