using AirlineBookingSystem.Payments.Application.Commands;
using AirlineBookingSystem.Payments.Core.Entities;
using AirlineBookingSystem.Payments.Core.Repositories;
using MassTransit;
using MediatR;


namespace AirlineBookingSystem.Payments.Application.Handlers
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, Guid>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProcessPaymentHandler(IPaymentRepository paymentRepository, IPublishEndpoint publishEndpoint)
        {
            _paymentRepository = paymentRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                BookingId = request.BookingId,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow
            };

             await _paymentRepository.ProcessPaymentAsync(payment);
             return payment.Id;
        }
    }
}
