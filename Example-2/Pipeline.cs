using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApplication{
    public class Pipeline
    {
        private Dictionary<Type, object> _handlers = new Dictionary<Type, object>();
        private List<Type> _decorators = new List<Type>();

        public Pipeline RegisterForRequest<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) {
            _handlers.Add(typeof(TRequest), handler);
            return this;
        }

        public Pipeline DecorateRequestsWith(Type decorator) {
            _decorators.Add(decorator);
            return this;
        }

        public TResponse Handle<TResponse>(IRequest<TResponse> request) 
        {
            //Get the handler by looking it up against the request type.
            var currentHandler = _handlers[request.GetType()];

            //Run through the decorators. 
            //Create an instance of a decorator, passing the 'next' in the chain to its constructor.
            foreach(var decorator in _decorators) {
                currentHandler = Activator.CreateInstance(
                    decorator.MakeGenericType(request.GetType(), typeof(TResponse)),
                currentHandler);

            }

            //Reflect the Handle method and invoke it with the request.
            var method = currentHandler.GetType().GetMethod("Handle");
            return (TResponse)method.Invoke(currentHandler, new [] { request });

        }
    }
    
    public interface IRequest<TResponse> {}
    public interface IRequestHandler<TRequest, TResponse> {
        TResponse Handle(TRequest request);
    }

}