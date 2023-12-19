using MasterServiceBack.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MasterServiceBack.Models;
using MasterServiceBack.Notify;
using Microsoft.AspNetCore.SignalR;

namespace MasterServiceBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterSpecController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        private static List<SendData> msg = new List<SendData>();
        private readonly NotificationHub _notificationHub;
        public MasterSpecController(ApplicationContext context, IHubContext<NotificationHub> hubContext, NotificationHub notificationHub)
        {
            _context = context;
            _hubContext = hubContext;
            _notificationHub = notificationHub;
        }

        [HttpPost("getNotify")]
        public async Task<IActionResult> getNotify([FromForm] int idCli)
        {
            
            return new JsonResult(new
            {
                code = 0,
                message = "Успех",
                data = msg.Where(x => x.User == idCli.ToString()).ToList()
            });
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
        [HttpPost("selectMaster")]
        public async Task<IActionResult> PutMasterSpec([FromForm]int idMaster,[FromForm]int idApp)
        {
            var client = _context.Clients.FirstOrDefault(x => x.Id == idMaster);
            var app = _context.Applications.FirstOrDefault(x => x.Id == idApp);
            app.Executor = client.Id;
            app.Status = "В работе";
            _context.Applications.Update(app);
            _context.SaveChangesAsync();
            
           // await _hubContext.Clients.User(.ToString()).SendAsync("ReceiveNotification", "Вас выбрали в качестве исполнителя!");
            _notificationHub.SendNotificationToUser(idMaster.ToString(), "Вас выбрали в качестве исполнителя!");
            msg.Add(new SendData()
            {
                App = idApp.ToString(),
                Message = "Вас выбрали в качестве исполнителя!",
                User = idMaster.ToString()
            });
            return new JsonResult(new
            {
                code = 0,
                message = "Успех"
            });
        }

        [HttpPost("selectMasterApp/{id}")]
        public async Task<IActionResult> SelMasterSpec(int id)
        {
            var client = _context.Applications.Where(x => x.Executor == id);
            return new JsonResult(new
            {
                code = 0,
                message = "Успех",
                data = client
            });
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
