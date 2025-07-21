using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace AirlineBookingSystem.Payments.Application.Commands
{
    public record  RefundPaymentCommand(Guid BookingId) : IRequest;


}
