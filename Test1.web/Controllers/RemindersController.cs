using Microsoft.AspNetCore.Mvc;
using Serilog;
using Test.Core.Dtos;
using Test.Core.Interface;
using Test.Domain.Entities;
using Test.Infrastructure;

namespace Test.Web.Controllers
{
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IReminderRepository _reminder;
        private readonly ILogger<RemindersController> _logger;

        public RemindersController(IReminderRepository reminder,ILogger<RemindersController> logger)
        {
            _reminder = reminder;
           
            _logger = logger;
        }

        public  IActionResult Index()
        {
            try
            {
                var reminders =   _reminder.GetListOfReminder() ;
               _logger.LogInformation("Retrieved list of reminders");
                return View(reminders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return BadRequest();
                
            }
       
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,ReminderDateTime")] ReminderDto reminder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = new Reminder
                    {
                        ReminderDateTime = reminder.ReminderDateTime,
                        Title = reminder.Title,
                    };
                    _context.Add(data);
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
