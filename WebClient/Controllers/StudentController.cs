using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using WebClient.DTOs;
using WebClient.Service;

namespace WebClient.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentClientService _studentClientService;

        public StudentController(StudentClientService context)
        {
            _studentClientService = context;
        }
        private string? GetToken()
        {
            return HttpContext.Session.GetString("authToken");
        }

        // GET: UserEntities
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Authentication");
            var students = await _studentClientService.GetStudentsAsync();
            return View(students);
        }

        // GET: UserEntities/Details/5
        public async Task<IActionResult> Details(int id)
        {
           
            if (id == null) return NotFound();

            var student = await _studentClientService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET: UserEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDTOs dto)
        {
            if (ModelState.IsValid)
            {
                var created = await _studentClientService.CreateStudentAsync(dto);
                if (created != null)
                {
                    return Json(new
                    {
                        success = true,
                        studentId = created.Id // ✅ Lấy ID tại đây
                    });
                }
            }

            return BadRequest(new { success = false, message = "Invalid data" });
        }

        // GET: UserEntities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null) return NotFound();

            var student = await _studentClientService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            var dto = new UpdateDTOs
            {
                UserName = student.UserName,
                FullName = student.FullName,
                Email = student.Email,
                Password = student.Password,
                Role = student.Role,
            };


      
            return View(dto);
        }

        // POST: UserEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateDTOs dto)
        {
            if (!ModelState.IsValid)
            {
                // Nếu request là AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var errors = ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    return BadRequest(errors);
                }

                ViewBag.StudentId = id;
                return View(dto);
            }

            var success = await _studentClientService.UpdateStudentAsync(id, dto);
            if (success)
                return Json(new { success = true });

            return BadRequest("Cập nhật thất bại.");
        }

        // GET: UserEntities/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null) return NotFound();

            var student = await _studentClientService.GetStudentByIdAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST: UserEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _studentClientService.DeleteStudentAsync(id);
            if (success)
                return RedirectToAction(nameof(Index));

            return NotFound();
        }

    }
}
