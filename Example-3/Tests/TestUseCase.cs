using Shouldly;
using Xunit;
using Pipeline.Testing.UseCases;

namespace Pipeline.Testing.Tests
{
    public class TestUseCase
    {
        private Pipeline _pipeline;

        public TestUseCase() {
            _pipeline = new Pipeline().RegisterForRequest(new MyUseCase.MyRequestHandler());
        }

        [Fact]
        public void ReturnsHelloInResponseToHelloRequest()
        {
            var response = _pipeline.Handle(new MyUseCase.MyRequest { Name = "Graeme" });
            response.Response.ShouldBe("Hello Graeme");
        }

        [Fact]
        public void ReturnsGoAwayInResponseToHelloRequestForPaul()
         {
             var response = _pipeline.Handle(new MyUseCase.MyRequest { Name = "Paul" });
             response.Response.ShouldBe("Go away Paul");
        }
    }
}
