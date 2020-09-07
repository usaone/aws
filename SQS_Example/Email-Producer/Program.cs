using System;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace Email_Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("******************************************");
            Console.WriteLine("Amazon SQS");
            Console.WriteLine("******************************************");

            IAmazonSQS sqs = new AmazonSQSClient(RegionEndpoint.USEast1);

            Console.WriteLine("Create a queue called EmailQueue.\n");

            var sqsRequest = new CreateQueueRequest
            {
                QueueName = "EmailQueue"
            };

            var createQueueResponse = sqs.CreateQueueAsync(sqsRequest).Result;

            var myQueueUrl = createQueueResponse.QueueUrl;

            var listQueuesRequest = new ListQueuesRequest();
            var listQueuesResponse = sqs.ListQueuesAsync(listQueuesRequest);

            Console.WriteLine("List of Amazon SQS queues.\n");

            foreach (var queueUrl in listQueuesResponse.Result.QueueUrls)
            {
                Console.WriteLine($"QueueUrl: {queueUrl}");
            }

            Console.WriteLine("Sending a message to our EmailQueue.\n");
            var sqsMessageRequest = new SendMessageRequest
            {
                QueueUrl = myQueueUrl,
                MessageBody = "Email message abc " + DateTime.Now
            };

            sqs.SendMessageAsync(sqsMessageRequest);

            Console.WriteLine("Finished sending message to our SQS queue.\n");

            Console.ReadLine();
        }
    }
}
