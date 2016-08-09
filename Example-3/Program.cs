using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Pipline.Testing
{
    public class PipelineTests
    {
        private readonly TestHarness _harness;
        private Pipeline _pipeline;

        public PipelineTests() {
            _pipeline = new Pipeline().RegisterForRequest(new MyUseCase.MyRequestHandler());
            _harness = new TestHarness(_pipeline);
        }

        [Fact]
        public void ReturnsHelloInResponseToName()
        {
            var response = _pipeline.Handle(new MyUseCase.MyRequest { Name = "Graeme" });
            response.Response.ShouldBe("Hello Graeme");
        }

        [Fact]
        public void TestWithMoreComplicatedSetup()
        {
            _harness.Given(
                    MyUseCaseScenarios.NewMyRequest("Graeme")
                    .And(MyUseCaseScenarios.NewMyRequest("Alice")) 
                    .And(MyUseCaseScenarios.NewMyRequest("Olivee")));

        }

        public static void Main(string[] args) {}
    }

    public static class MyUseCaseScenarios
    {
        public static RequestScenario<MyUseCase.MyResponse> NewMyRequest(string name) {
            return RequestScenario<MyUseCase.MyResponse>.FromRequest(new MyUseCase.MyRequest { Name = name });
        }
    }

    public interface IScenario {}

    public class RequestScenario<TResponse> : IScenario {
        public static  RequestScenario<TResponse> FromRequest(IRequest<TResponse> request) {
            return new RequestScenario<TResponse>() { Request = request };
        } 
        public IRequest<TResponse> Request { get; private set;}
        public TResponse ExecuteIn(Pipeline pipeline) {
            return pipeline.Handle(Request);
        }
    }

    public class TestHarness {

        private Pipeline _pipeline;

        public TestHarness(Pipeline pipeline) {
            _pipeline = pipeline;
        }

        public TResponse Given<TResponse>(RequestScenario<TResponse> scenario) {
            Console.WriteLine("Given");
            return Dispatch(scenario);
        }

        public void Given(IEnumerable<IScenario> scenarios) {
            Console.WriteLine("Given");
            foreach(var scenario in scenarios) {
                Dispatch((dynamic)scenario);
            }
        }

        public TResponse Dispatch<TResponse>(RequestScenario<TResponse> requestScenario) {
            Console.WriteLine("   Request {0} is executed", requestScenario.Request.GetType().Name);
            return requestScenario.ExecuteIn(_pipeline);
        }
    }

}
