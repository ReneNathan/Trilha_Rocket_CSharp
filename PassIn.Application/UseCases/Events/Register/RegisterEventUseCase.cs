using PassIn.Communication.Requests;
using PassIn.Exceptions;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public void execute(RequestEventJson request)
        {
            validate(request);
        }

        private void validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0) {

                throw new PassInException("The maximum attendes is invalid.");
            }

            if(string.IsNullOrWhiteSpace(request.Title))
            {
                throw new PassInException("The title is invalid.");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new PassInException("The details informed are invalid.");
            }
        }
    }
}
