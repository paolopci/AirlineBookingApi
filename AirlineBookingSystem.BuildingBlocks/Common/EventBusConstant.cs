 namespace AirlineBookingSystem.BuildingBlocks.Common
{
    public class EventBusConstant
    {
        // Define constants for event bus topics and queues.. sono le code di rabbitMq che userò
        public const string FlightBookedQueue = "flight-booked-queue";
        public const string PaymentProcessedQueue = "payment-processed-queue";
        public const string NotificationSentQueue = "notification-sent-queue";

    }
}
