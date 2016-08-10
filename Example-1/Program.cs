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
