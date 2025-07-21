using AirlineBookingSystem.Notifications.Application.Commands;
using AirlineBookingSystem.Notifications.Application.Interfaces;
using AirlineBookingSystem.Notifications.Core.Entities;
using MediatR;


namespace AirlineBookingSystem.Notifications.Application.Handlers
{
    public  class SendNotificationHandler:IRequestHandler<SendNotificationCommand>
    {
        private readonly INotificationService _notificationService;

        public SendNotificationHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(), // Generate a new unique identifier for the notification
                Recipient = request.Recipient,
                Message = request.Message,
                Type = request.Type,
                SentAt = DateTime.Now // Set the current date and time as the sent time
            };

            await _notificationService.SendNotificationAsync(notification);
        }
    }
}
