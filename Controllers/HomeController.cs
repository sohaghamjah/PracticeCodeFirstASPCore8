using CodeFirstASPCore8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CodeFirstASPCore8.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentDBContext studentDB;

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(StudentDBContext studentDB)
        {
            this.studentDB = studentDB;
        }

        public IActionResult Index()
        {
            var stdData = studentDB.Students.ToList();
            return View(stdData);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student std)
        {
            if (ModelState.IsValid) { 
                await studentDB.Students.AddAsync(std);
                await studentDB.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student data has been saved successfully!";
                return RedirectToAction("Index", "Home");
            }
            return View(std);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var stdData = await studentDB.Students.FirstOrDefaultAsync (x => x.Id == Id);
            if (stdData == null) { 
                return NotFound();
            }
            return View(stdData);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var stdData = await studentDB.Students.FindAsync(Id);
            if (stdData == null)
            {
                return NotFound();
            }
            return View(stdData);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? Id, Student stdData)
        {
            if (ModelState.IsValid) { 
                studentDB.Students.Update(stdData);
                await studentDB.SaveChangesAsync();
                TempData["SuccessMessage"] = "Student data has been updated successfully!";
                return RedirectToAction("Index", "Home");
            }
            return View(stdData);
        }

        public async Task<ActionResult> Delete(int? Id) {
            var stdData = await studentDB.Students.FirstOrDefaultAsync(x => x.Id == Id);
            return View(stdData);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var stdData = await studentDB.Students.FirstOrDefaultAsync(x => x.Id == Id);
            if (stdData == null)
            {
                return NotFound();
            }

            studentDB.Students.Remove(stdData);
            await studentDB.SaveChangesAsync();

            TempData["SuccessMessage"] = "Student data has been deleted successfully!";
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Privacy()
        {
            return View(); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
