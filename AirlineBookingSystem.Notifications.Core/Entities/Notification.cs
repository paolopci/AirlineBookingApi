﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Notifications.Core.Entities
{
    public class Notification
    {
         public Guid Id { get; set; }
         public string Recipient { get; set; }
         public string Message { get; set; }
         public string Type { get; set; } // Email, Sms , etc.
          public DateTime SentAt { get; set; }


    }
}
