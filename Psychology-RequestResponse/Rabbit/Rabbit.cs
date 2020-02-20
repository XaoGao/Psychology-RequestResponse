using System;
using System.Text;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Psychology_RequestResponse.Domain;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Psychology_RequestResponse.Rabbit
{
    public class Rabbit
    {
        public void Received()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new System.Uri(Settings.Uri);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: Settings.KeyResponse,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    var result = JsonConvert.DeserializeObject<InterdepartResponse>(message);
                    Update(result);
                };
                channel.BasicConsume(queue: Settings.KeyResponse,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Нажмите любую клавишу для вызода из программы");
                Console.ReadLine();
            }
        }
        private void Update(InterdepartResponse interdepart)
        {
            try
            {
                string Sql = $"update InterdepartRequests set InterdepartStatusId = {interdepart.InterdepartStatusId}, Request = {interdepart.Request}, Response = {DateTime.Now} where Id = {interdepart.Id}"; 
                MySqlConnection connection = new MySqlConnection("Server=localhost; Database=psychologyDB; Uid=psyappuser; Password=password");
                MySqlCommand command = new MySqlCommand(Sql, connection);
                connection.Open();
        
                MySqlDataReader reader = command.ExecuteReader();

                connection.Close();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}