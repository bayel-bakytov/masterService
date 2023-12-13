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
    public class MasterSpecController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MasterSpecController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/MasterSpec
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MasterSpec>>> GetMasterSpecs()
        {
          if (_context.MasterSpecs == null)
          {
              return NotFound();
          }
            return await _context.MasterSpecs.ToListAsync();
        }

        // GET: api/MasterSpec/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MasterSpec>> GetMasterSpec(int id)
        {
            if (_context.MasterSpecs == null)
            {
                return NotFound();
            }

            var masterSpec = _context.MasterSpecs.Where(x => x.AppId == id).ToList();
            foreach (var master in masterSpec)
            {
               master.Master = _context.Clients.FirstOrDefault(x => x.Id == master.MasterId);
            }

            return new JsonResult(new
            {
                code = 0,
                message = "Успех",
                data = masterSpec
            });

        }

        // PUT: api/MasterSpec/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMasterSpec(int id, MasterSpec masterSpec)
        {
            if (id != masterSpec.Id)
            {
                return BadRequest();
            }

            _context.Entry(masterSpec).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MasterSpecExists(id))
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

        // POST: api/MasterSpec
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MasterSpec>> PostMasterSpec(MasterSpec masterSpec)
        {
          if (_context.MasterSpecs == null)
          {
              return Problem("Entity set 'ApplicationContext.MasterSpecs'  is null.");
          }

          var check = _context.MasterSpecs.FirstOrDefault(x =>
              x.MasterId == masterSpec.MasterId && x.AppId == masterSpec.AppId && x.Sum == masterSpec.Sum);
          if (check != null)
          {
              return new JsonResult(new
              {
                    code = 1,
                    message = "Вы уже отправляли заявку!"
              });
          }
            _context.MasterSpecs.Add(masterSpec);
            await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                code = 0,
                message = "Успех!"
            });

            return CreatedAtAction("GetMasterSpec", new { id = masterSpec.Id }, masterSpec);
        }

        // DELETE: api/MasterSpec/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterSpec(int id)
        {
            if (_context.MasterSpecs == null)
            {
                return NotFound();
            }
            var masterSpec = await _context.MasterSpecs.FindAsync(id);
            if (masterSpec == null)
            {
                return NotFound();
            }

            _context.MasterSpecs.Remove(masterSpec);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MasterSpecExists(int id)
        {
            return (_context.MasterSpecs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}