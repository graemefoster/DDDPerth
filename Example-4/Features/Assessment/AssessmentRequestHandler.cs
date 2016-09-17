using System.Collections.Generic;

namespace DDDPerth.Features.Assessment
{
    public class AssessmentRequestHandler : MediatR.IRequestHandler<AssessmentRequest, AssessmentResponse>
    {
        public AssessmentResponse Handle(AssessmentRequest message)
        {
            var theRabbit = new RabbitMQ.Client.ConnectionFactory();
            theRabbit.Uri = "amqp://graeme:whatdowethink@:5672/172.17.0.2";
            using (var theRabbitConnection = theRabbit.CreateConnection())
            {
                using (var model = theRabbitConnection.CreateModel())
                {
                    model.ExchangeDeclare("GraemesExchange", RabbitMQ.Client.ExchangeType.Direct, true, false, new Dictionary<string, object>());
                    var queueOk = model.QueueDeclare("GraemesQueue", true, false, false, new Dictionary<string, object>());
                    model.QueueBind("GraemesQueue", "GraemesExchange", "FooFoo", new Dictionary<string, object>());
                    model.BasicPublish("GraemesExchange", "FooFoo", false, null, System.Text.Encoding.UTF8.GetBytes("HELLO WORLD!!!"));
                }
            }

            return new AssessmentResponse {
                Greeting = "Hello " + message.Name
            };
        }
    }}