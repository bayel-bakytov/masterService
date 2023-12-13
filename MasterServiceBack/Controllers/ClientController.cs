using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MasterServiceBack.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterServiceBack.Models;

namespace MasterServiceBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ClientController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
          if (_context.Clients == null)
          {
              return NotFound();
          }
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("registration")]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
          if (_context.Clients == null)
          {
              return Problem("Entity set 'ApplicationContext.Clients'  is null.");
          }

            client.Password = StringToBase64(client.Password);
            _context.Clients.Add(client); 
            await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                code = 0,
                message = "success",
            });

        }
        
        [HttpPost("auth")]
        public async Task<ActionResult<Client>> AuthClient(AuthDto auth)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'ApplicationContext.Clients'  is null.");
            }

            var client = _context.Clients.FirstOrDefault(x => x.Password == StringToBase64(auth.Password) && x.Login == auth.Login);
            if (client == null)
            {
                return new JsonResult(new
                {
                    code = 1,
                    message = "Неправильный логин или пароль",
                });
            }
            
            return new JsonResult(new
            {
                code = 0,
                message = "Успех",
                data = client
            });
            
        }
        
        public static string StringToBase64(string input)
        {
            // Преобразование строки в байты
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            // Кодирование байтов в строку Base64
            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public static string Base64ToString(string base64String)
        {
            // Декодирование строки Base64 в байты
            byte[] bytes = Convert.FromBase64String(base64String);

            // Преобразование байтов в строку
            string decodedString = Encoding.UTF8.GetString(bytes);

            return decodedString;
        }

        // DELETE: api/Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
