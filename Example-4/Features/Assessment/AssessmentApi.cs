using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DDDPerth.Features.Assessment
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/greeting/{Name}")]
        public Response Get(Request request) {
            return _mediator.Send(request);
        }
    }

    public class RequestValidator : IValidate<Request>
    {
        public IEnumerable<ValidationFault> Validate(Request request)
        {
            if (request.Name == "Graeme") yield return new ValidationFault("Not you again!");
        }
    }
    public class Request: MediatR.IRequest<Response> {
        public string Name {get;set;}
    }

    public class Response {
        public string Greeting {get;set;}
    }

    public class RequestHandler : MediatR.IRequestHandler<Request, Response>
    {
        public Response Handle(Request message)
        {
            return new Response {
                Greeting = "Hello " + message.Name
            };
        }
    }
}