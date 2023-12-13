using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterServiceBack.Models;

namespace MasterServiceBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IWebHostEnvironment _environment;

        public ApplicationController(ApplicationContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/Application
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
          if (_context.Applications == null)
          {
              return NotFound();
          }
          return new JsonResult(new
          {
              code = 0,
              message = "success",
              data = await _context.Applications.ToListAsync()
          });
        }

        // GET: api/Application/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
          if (_context.Applications == null)
          {
              return NotFound();
          }
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            return application;
        }

        // PUT: api/Application/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Application
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<Application>> PostApplication(Application application)
        {
          if (_context.Applications == null)
          {
              return Problem("Entity set 'ApplicationContext.Applications'  is null.");
          }
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }*/
        [HttpPost("getAppClient/{id}")]
        public IActionResult GetAppClient(int id)
        {
            var clientApp = _context.Applications.Where(x => x.ClientId == id);
            return new JsonResult(new
            {
                code = 0,
                message = "Успех",
                data = clientApp
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] Application applicationDto)
        {
            if (applicationDto == null)
            {
                return BadRequest("Invalid request");
            }

            try
            {
                string uniqueFileName = $"{Guid.NewGuid().ToString()}.png"; // или другое расширение в зависимости от формата фото
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "img");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                byte[] imageBytes = Convert.FromBase64String(applicationDto.LinkImg);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await stream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }

                var application = new Application
                {
                    Title = applicationDto.Title,
                    Description = applicationDto.Description,
                    DateStart = applicationDto.DateStart,
                    DateEnd = applicationDto.DateEnd,
                    LinkImg = "https://localhost:7074/img/"+uniqueFileName,
                    CategoryId = applicationDto.CategoryId,
                    ClientId = applicationDto.ClientId
                };

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                return new JsonResult(new
                {
                    code = 0,
                    message = "success",
                    data = application
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    code = 1,
                    message = "fail",
                });
            }
        }
    

        // DELETE: api/Application/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            if (_context.Applications == null)
            {
                return NotFound();
            }
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationExists(int id)
        {
            return (_context.Applications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
