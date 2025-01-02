using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyDockerProject.Models;
using System.Diagnostics;

namespace MyDockerProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var users = _context.Users.ToList(); 
            return View(users); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel createViewModel)
        {
            var user = new User
            {
                UserName = createViewModel.UserName,
                Email = createViewModel.Email,
                DateOfBirth = createViewModel.DateOfBirth,
                FirstName = createViewModel.FirstName,
                LastName = createViewModel.LastName,
            };

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            UpdateViewModel updateViewModel = new UpdateViewModel();
            var user = await _context.Users.FindAsync(id);

            updateViewModel.Id = user.Id;
            updateViewModel.UserName = user.UserName;
            updateViewModel.Email = user.Email;
            updateViewModel.FirstName = user.FirstName;
            updateViewModel.LastName = user.LastName;
            updateViewModel.DateOfBirth = user.DateOfBirth;

            return View(updateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateViewModel updateViewModel)
        {
            var user = new User
            {
                Id = updateViewModel.Id,
                UserName = updateViewModel.UserName,
                Email = updateViewModel.Email,
                FirstName = updateViewModel.FirstName,
                LastName = updateViewModel.LastName,
                DateOfBirth = updateViewModel.DateOfBirth,

            };

            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
