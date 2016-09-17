
using Pipeline.Testing.Decorators;

namespace Pipeline.Testing.UseCases
{
    public static class MyUseCase
    {
        [RequiresPermissionAttribute(Permission = "MyRequestPermission")]
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
                if (request.Name == "Paul") {
                     return new MyResponse { Response = "Go away " + request.Name };
                }
                return new MyResponse {
                    Response = "Hello " + request.Name
                };
            }
        }
    }
}