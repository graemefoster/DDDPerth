using System;
using System.Reflection;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var pipeline = new Pipeline().RegisterForRequest(new MyRequestHandler());
            Console.WriteLine(pipeline.Handle(new MyRequest { Name = "Graeme!"}).Response);
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

    public interface IRequest<TResponse> {}
    public interface IRequestHandler<TRequest, TResponse> {
        TResponse Handle(TRequest request);
    }
    public class Pipeline
    {
        private Dictionary<Type, object> _handlers = new Dictionary<Type, object>();

        public Pipeline RegisterForRequest<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) {
            _handlers.Add(typeof(TRequest), handler);
            return this;
        }

        public TResponse Handle<TResponse>(IRequest<TResponse> request) 
        {
            var handler = _handlers[request.GetType()];
            var method = handler.GetType().GetMethod("Handle");
            return (TResponse)method.Invoke(handler, new [] { request });
        }
    }
}
