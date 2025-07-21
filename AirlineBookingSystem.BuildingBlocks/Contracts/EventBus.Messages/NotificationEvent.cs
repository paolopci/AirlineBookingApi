namespace AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages
{
    public record NotificationEvent(string Recipient, string Message, string Type);
}
