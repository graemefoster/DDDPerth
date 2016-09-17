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
                .DecorateRequestsWith(typeof(MyLogDecorator<,>));

            Console.WriteLine(pipeline.Handle(new MyRequest { Name = "Graeme!"}).Response);

        }

        private class MyLogDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        {
            private IRequestHandler<TRequest, TResponse> _next;

            public TResponse Handle(TRequest request)
            {
                Console.WriteLine("Handling " + request.GetType().Name);
                return _next.Handle(request);
            }

            public MyLogDecorator(IRequestHandler<TRequest, TResponse> next) {
                _next = next;
            }        
            }











        private class MyDecorator2<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        {
            private IRequestHandler<TRequest, TResponse> _next;

            public MyDecorator2(IRequestHandler<TRequest, TResponse> next) {
                _next = next;
            }
            public TResponse Handle(TRequest request)
            {
                Console.WriteLine("DECORATING...");
                return _next.Handle(request);
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
