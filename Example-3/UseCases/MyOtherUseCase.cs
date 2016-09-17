
using Pipeline.Testing.Decorators;

namespace Pipeline.Testing.UseCases
{
    public static class MyOtherUseCase
    {
        [RequiresPermissionAttribute(Permission = "MyOtherPermission")]
        public class MyOtherRequest: IRequest<MyOtherResponse> {
            public string Name {get;set;}
        }
        public class MyOtherResponse {
            public string Response {get;set;}
        }
        public class MyRequestHandler : IRequestHandler<MyOtherRequest, MyOtherResponse>
        {
            public MyOtherResponse Handle(MyOtherRequest request)
            {
                return new MyOtherResponse {
                    Response = "Boo " + request.Name
                };
            }
        }
    }
}