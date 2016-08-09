using System;
using System.Collections.Generic;
using System.Reflection;

namespace Pipline.Testing
{
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
            var currentHandler = _handlers[request.GetType()];
            var method = currentHandler.GetType().GetMethod("Handle");
            return (TResponse)method.Invoke(currentHandler, new [] { request });

        }
    }
    
}