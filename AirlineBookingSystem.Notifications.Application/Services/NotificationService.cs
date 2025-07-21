using AirlineBookingSystem.Notifications.Application.Interfaces;
using AirlineBookingSystem.Notifications.Core.Entities;


namespace AirlineBookingSystem.Notifications.Application.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotificationAsync(Notification notification)
        {
            // Simulate sending a notification (e.g., email, SMS, etc.)
            Console.WriteLine($"Notification send to {notification.Recipient}: {notification.Message}");
        }
    }
}