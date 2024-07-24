using Microsoft.AspNetCore.Mvc;
using Serilog;
using Test.Domain.Entities;
using Test.Infrastructure;

namespace Test.Web.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RemindersController> _logger;

        public RemindersController(ApplicationDbContext context,ILogger<RemindersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var reminders = _context.Reminders.ToList();
               _logger.LogInformation("Retrieved list of reminders");
                return View(reminders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest();
                throw;
            }
       
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ReminderDateTime")] Reminder reminder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(reminder);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created a new reminder with title {Title}", reminder.Title);
                    return RedirectToAction(nameof(Index));
                }
                return View(reminder);

            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.Message}");
                return BadRequest();
            }
         
        }
    }
}
