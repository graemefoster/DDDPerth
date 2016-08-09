namespace Pipline.Testing
{
    public static class MyUseCase
    {
        public class MyRequest: IRequest<MyResponse> {
            public string Name {get;set;}
        }
        public class MyResponse {
            public string Response {get;set;}
        }
        public class MyRequestHandler : IRequestHandler<MyRequest, MyResponse>
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