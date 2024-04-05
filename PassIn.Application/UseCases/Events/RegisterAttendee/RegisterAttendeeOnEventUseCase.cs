using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
        private readonly PassInDbContext _dbContext;
        public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        }
        public ResponseRegisteredJson execute(Guid EventId, RequestRegisterEventJson request)
        {
            validate(EventId, request);

            var entity = new Attendee
            {
                Name = request.Name,
                Email = request.Email,
                Event_Id = EventId,
                Created_At = DateTime.UtcNow
            };

            _dbContext.attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson()
            {
                Id = entity.id
            };
        }

        private void validate(Guid EventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(EventId);

            if (eventEntity is null) {
                throw new NotFoundException("An event with this id does not exist");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("The name is invalid");
            }
            /*
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ErrorOnValidationException("The Email is invalid");
            }
            */
            if (IsEmailValid(request.Email) == false)
            {
                throw new ErrorOnValidationException("The email is invalid");
            }

            var attendeeIsAlreadyRegistered = _dbContext.attendees.Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == EventId);

            if (attendeeIsAlreadyRegistered) {
                throw new ErrorOnValidationException("You can not register twice ont the same event");
            }

            int attendessForThisEvent = _dbContext.attendees.Count(attendee => attendee.Event_Id == EventId);

            if(attendessForThisEvent >= eventEntity.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("There is no room for this event");
            }
        }


        private bool IsEmailValid(string email)
        {
            try
            {
                new MailAddress(email);

                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
