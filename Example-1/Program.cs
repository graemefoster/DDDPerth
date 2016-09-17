using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var pipeline = new Pipeline().RegisterForRequest(
                () => new MyRequestHandler());

            Console.WriteLine(
                pipeline.Handle(
                    new MyRequest { Name = "Graeme!"}).Response);
        }

        //TODO - write the request handler!
        public class MyRequestHandler : IRequestHandler<MyRequest, MyResponse>
        {
            public MyResponse Handle(MyRequest request)
            {
                return new MyResponse {
                    Response = "Hello " + request.Name
                };
            }
        }


        public class MyRequest: IRequest<MyResponse> {
            public string Name {get;set;}
        }
        public class MyResponse {
            public string Response {get;set;}
        }












        private class MyRequestHandler2 : IRequestHandler<MyRequest, MyResponse>
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
