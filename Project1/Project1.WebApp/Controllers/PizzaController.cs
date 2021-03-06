﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Library;
using Project1.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project1.WebApp.Controllers
{
    public class PizzaController : Controller
    {
        public IProject1DbRepository Repo { get; }
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(IProject1DbRepository repo, ILogger<PizzaController> logger)
        {
            Repo = repo;
            _logger = logger;
        }

        // GET: Pizza
        public ActionResult Index()
        {
            try
            {
                IEnumerable<Pizza> pizzas = Repo.GetAllPizzas();

                var viewModels = pizzas.Select(m => new PizzaViewModel(m));
                return View(viewModels);
            }
            catch (ArgumentNullException ex)
            {
                // should log that, and redirect to error page
                _logger.LogTrace(ex, "DB Pizzas was empty.");
                return RedirectToAction("Error", "Home");
            }
            catch (InvalidOperationException ex)
            {
                // should log that, and redirect to error page
                _logger.LogTrace(ex, "Invalid state operation.");
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Pizza/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var pizza = Repo.GetPizzaById(id);

                var viewModels = new PizzaViewModel(pizza);
                return View(viewModels);
            }
            catch (ArgumentNullException ex)
            {
                // should log that, and redirect to error page
                _logger.LogTrace(ex, $"DB Pizza {id} was not found.");
                return RedirectToAction("Error", "Home");
            }
            catch (InvalidOperationException ex)
            {
                // should log that, and redirect to error page
                _logger.LogTrace(ex, "Invalid state operation.");
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Pizza/Create
        public ActionResult Create()
        {
            var viewModel = new PizzaViewModel
            {
                Ingredients = Repo.GetAllIngredients().Select(i => new IngredientViewModel(i)).ToList()
                //pass all ingredients for choosing in pizza creation
            };

            return View(viewModel);
        }

        // POST: Pizza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PizzaViewModel collection)
        {
            try
            {
                var pizza = new Pizza(collection.Name, collection.Price)
                {
                    Id = collection.Id,
                };


                Repo.Add(pizza);

                for (var i = 0; i < collection.Ingredients.Count; i++)
                {
                    if (collection.Ingredients[i].Checked)
                    {
                        var pizzaingredient = new PizzaIngredient(collection.IngredientsAmount[i].Quantity)
                        {
                            Ingredient = new Ingredient(collection.Ingredients[i].Name)
                            { Id = collection.Ingredients[i].Id },
                            Pizza = pizza
                        };

                        Repo.Add(pizzaingredient);
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                _logger.LogTrace(e, "Create pizza error.");
                return View(collection);
            }
        }

        //// GET: Pizza/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Pizza/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Pizza/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Pizza/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}