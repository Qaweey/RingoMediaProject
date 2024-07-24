using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Dtos;
using Test.Domain.Entities;

namespace Test.Core.Interface
{
    public  interface IReminderRepository
    {
        List<Reminder> GetListOfReminder();
        Task CreateReminder(ReminderDto reminder);
    }
}
