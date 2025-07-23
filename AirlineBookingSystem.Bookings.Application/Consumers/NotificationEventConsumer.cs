using AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages;
using MassTransit;

namespace AirlineBookingSystem.Bookings.Application.Consumers
{
    public class NotificationEventConsumer : IConsumer<NotificationEvent>
    {
        

        public async Task Consume(ConsumeContext<NotificationEvent> context)
        {
            var notificationEvent= context.Message;

            // per ora non invio messaggi di notifica
            Console.WriteLine($"REceived Notification Evnt: Recipient={notificationEvent.Recipient}, "+
                              $"Message={notificationEvent.Message}, Type={notificationEvent.Type}");

            await Task.CompletedTask; // Simulate async operation, if needed
        }
    }
}
