using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pipeline = new Pipeline()
                .RegisterForRequest(new MyRequestHandler())
                .DecorateRequestsWith(typeof(MyDecorator<,>));

            Console.WriteLine(pipeline.Handle(new MyRequest { Name = "Graeme!"}).Response);
        }

        private class MyDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        {
            private IRequestHandler<TRequest, TResponse> _next;

            public MyDecorator(IRequestHandler<TRequest, TResponse> next) {
                _next = next;
            }
            public TResponse Handle(TRequest request)
            {
                Console.WriteLine("DECORATING...");
                return _next.Handle(request);
            }

            public string Hello() {
                return "FOOFOO!";
            }
        }

        private class MyRequest: IRequest<MyResponse> {
            public string Name {get;set;}
        }
        private class MyResponse {
            public string Response {get;set;}
        }
        private class MyRequestHandler : IRequestHandler<MyRequest, MyResponse>
        {
            public MyResponse Handle(MyRequest request)
            {
                return new MyResponse {
                    Response = "Hello " + request.Name
                };
            }
        }
    }

}
