using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda.DynamoDBEvents;
using Amazon.SQS;
using Amazon.SQS.Model;
using Compartilhador.Enum;
using Compartilhador.Model;
using Newtonsoft.Json;



namespace Compartilhador
{
    public static class AmazonUtil
    {
        public static async Task SalvarAsync(this Pedido pedido)
        {

            var client = new AmazonDynamoDBClient(RegionEndpoint.SAEast1);
            var context = new DynamoDBContext(client);
            await context.SaveAsync(pedido);
        }
        public static T ToObject<T>(this Dictionary<string, AttributeValue> dictionary)
        {
            var client = new AmazonDynamoDBClient(RegionEndpoint.SAEast1);
            var context = new DynamoDBContext(client);

            var doc = Document.FromAttributeMap(dictionary);
            return context.FromDocument<T>(doc);
        }
        public static async Task EnviarParaFila(EnumFilasSQS fila, Pedido pedido)
        {
            var json = JsonConvert.SerializeObject(pedido);
            var client = new AmazonSQSClient(RegionEndpoint.SAEast1);

            var queueUrl = Environment.GetEnvironmentVariable("SQS_QUEUE_URL");
            var request = new SendMessageRequest
            {
                QueueUrl = $"{queueUrl}{fila}",
                MessageBody = json
            };

            await client.SendMessageAsync(request);
        }


        // unica forma que encontrei de processar o dado vindo do DynamoDBEvent
        public static Dictionary<string, AttributeValue> ToModelAttributeMap(this Dictionary<string, DynamoDBEvent.AttributeValue> source)
        {
            return source.ToDictionary(
                kvp => kvp.Key,
                kvp => new AttributeValue
                {
                    S = kvp.Value.S,
                    N = kvp.Value.N,
                    BOOL = kvp.Value.BOOL,
                    M = kvp.Value.M?.ToModelAttributeMap(),
                    L = kvp.Value.L?.Select(item => new AttributeValue
                    {
                        S = item.S,
                        N = item.N,
                        BOOL = item.BOOL,
                        M = item.M?.ToModelAttributeMap(),
                        L = item.L?.Select(sub => new AttributeValue
                        {
                            S = sub.S,
                            N = sub.N,
                            BOOL = sub.BOOL,
                            M = sub.M?.ToModelAttributeMap(),
                            L = sub.L?.Select(x => new AttributeValue { S = x.S, N = x.N, BOOL = x.BOOL }).ToList()
                        }).ToList()
                    }).ToList()
                }
            );
        }
    }
}