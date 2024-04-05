using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entidades;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisteredJson execute(RequestEventJson request)
        {
            validate(request);

            var dbContext = new PassInDbContext();

            var entity = new Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ","-")
            };

            dbContext.Events.Add(entity);
            dbContext.SaveChanges();


            return new ResponseRegisteredJson
            {
                Id = entity.Id
            };

        }


        private void validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0) {

                throw new ErrorOnValidationException("The maximum attendes is invalid.");
            }

            if(string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ErrorOnValidationException("The title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("The details informed are invalid.");
            }
        }
    }
}
