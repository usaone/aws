using System;
using System.Linq;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Email_Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = 0;

            var sqs = new AmazonSQSClient(RegionEndpoint.USEast1);

            var queueUrl = sqs.GetQueueUrlAsync("EmailQueue").Result.QueueUrl;

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = queueUrl
            };

            var receiveMessageResponse = sqs.ReceiveMessageAsync(receiveMessageRequest).Result;

            foreach (var message in receiveMessageResponse.Messages)
            {
                counter++;
                Console.WriteLine($"Message Count: {counter}");
                Console.WriteLine("Message \n");
                Console.WriteLine($" MessageId: {message.MessageId} \n");
                Console.WriteLine($"  ReceiptHandle: {message.ReceiptHandle} \n");
                Console.WriteLine($"  MD5Body {message.MD5OfBody} \n");
                Console.WriteLine($"  Body: {message.Body} \n");

                foreach (var attribute in message.Attributes)
                {
                    Console.WriteLine("Attribute \n");
                    Console.WriteLine($"   Name: {attribute.Key}");
                    Console.WriteLine($"   Name: {attribute.Value}");
                }

                var messageReceiptHandle = receiveMessageResponse.Messages.FirstOrDefault()?.ReceiptHandle;

                var deleteRequest = new DeleteMessageRequest
                {
                    QueueUrl = queueUrl,
                    ReceiptHandle = messageReceiptHandle
                };

                sqs.DeleteMessageAsync(deleteRequest);

                Console.ReadKey();
            }

        }
    }
}
