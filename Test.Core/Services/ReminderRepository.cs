using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dtos;
using Test.Core.Interface;
using Test.Domain.Entities;
using Test.Infrastructure;

namespace Test.Core.Services
{
    public  class ReminderRepository : IReminderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReminderRepository> _logger;

        public ReminderRepository(ApplicationDbContext context, ILogger<ReminderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async  Task CreateReminder(ReminderDto reminder)
        {
            try
            {
                var data = new Reminder
                {
                    ReminderDateTime = reminder.ReminderDateTime,
                    Title = reminder.Title,
                };
                _context.Add(data);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"information is successfully saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

              
            }
           
        }

        public List<Reminder> GetListOfReminder()
        {
            try
            {
                var reminders = _context.Reminders.ToList();
                _logger.LogInformation("Retrieved list of reminders");
                return reminders;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return null;
                
            }

        }
    }
}
