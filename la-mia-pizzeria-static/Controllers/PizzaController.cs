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
                List<Pizza> pizzas = context.Pizzas.Include("Category").Include("Ingredients").ToList();
                return View("Index", pizzas);
            }
        }

        public IActionResult Detail(int id)
        {
            using (PizzaContext context = new PizzaContext())
            {
                Pizza pizzaDetail = context.Pizzas.Where(pizza => pizza.PizzaID == id).Include(pizza => pizza.Category).FirstOrDefault();

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
            PizzaContext context = new PizzaContext();
            PizzasCategories pizzasCategories = new PizzasCategories();

            pizzasCategories.Categories = new PizzaContext().Categories.ToList();
            pizzasCategories.Ingredients = context.Ingredients.ToList();

            return View(pizzasCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (PizzasCategories formData)
        {
            PizzaContext context = new PizzaContext();

            if (!ModelState.IsValid)
            {
                formData.Categories = context.Categories.ToList();
                formData.Ingredients = context.Ingredients.ToList();
                return View("Create", formData);
            }
            //Pizza pizza = new Pizza();
            //pizza.Name = formData.Pizza.Name;
            //pizza.Description = formData.Pizza.Description;
            //pizza.Photo = formData.Pizza.Photo;
            //pizza.Price = formData.Pizza.Price;
            //pizza.CategoryId = formData.Pizza.CategoryId;

            List<Ingredient> selected = new List<Ingredient>();

            foreach(int ingredientId in formData.SelectedIngredient)
            {
                Ingredient ingredient = context.Ingredients.Where(ingredient => ingredient.Id == ingredientId).FirstOrDefault();

                selected.Add(ingredient);
            }
            formData.Pizza.Ingredients = selected;

            context.Pizzas.Add(formData.Pizza);

            context.SaveChanges();

            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult Update (int id)
        {
            using (PizzaContext context = new PizzaContext()) 
            { 
                Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaID == id).FirstOrDefault();
                
                if (pizza == null)
                {
                    return NotFound("Pizza non trovata");
                }

                PizzasCategories pizzasCategories = new PizzasCategories();

                pizzasCategories.Pizza = pizza;
                pizzasCategories.Categories = context.Categories.ToList();
                pizzasCategories.Ingredients = context.Ingredients.ToList();

                return View(pizzasCategories);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzasCategories formData)
        {
            using (PizzaContext context = new PizzaContext()) 
            {
                if (!ModelState.IsValid)
                {
                    formData.Categories = context.Categories.ToList();
                    formData.Ingredients = context.Ingredients.ToList();
                    return View("Update", formData);
                }

                Pizza pizza = context.Pizzas.Where(pizza => pizza.PizzaID == id).Include("Ingredients").FirstOrDefault();

                if (pizza != null)
                {
                    pizza.Name = formData.Pizza.Name;
                    pizza.Description = formData.Pizza.Description;
                    pizza.Photo = formData.Pizza.Photo;
                    pizza.Price = formData.Pizza.Price;
                    pizza.CategoryId = formData.Pizza.CategoryId;
                    pizza.Ingredients = context.Ingredients.Where(ingredient => formData.SelectedIngredient.Contains(ingredient.Id)).ToList<Ingredient>();

                    context.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Pizza non trovata");
                }
            }
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
