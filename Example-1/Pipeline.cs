using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApplication
{
    public interface IRequest<TResponse> {}
    public interface IRequestHandler<TRequest, TResponse> {
        TResponse Handle(TRequest request);
    }

    public class Pipeline
    {
        private Dictionary<Type, Func<object>> _handlers = new Dictionary<Type, Func<object>>();

        public Pipeline RegisterForRequest<TRequest, TResponse>(Func<IRequestHandler<TRequest, TResponse>> handler) {
            _handlers.Add(typeof(TRequest), 
                () => (object)handler());
            return this;
        }

        public TResponse Handle<TResponse>(IRequest<TResponse> request) 
        {
            var handler = _handlers[request.GetType()]();
            var method = handler.GetType().GetMethod("Handle");
            return (TResponse)method.Invoke(handler, new [] { request });
        }
    }
}