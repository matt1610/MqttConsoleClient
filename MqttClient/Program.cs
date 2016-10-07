using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttClientApp
{
    class Program
    {
        public static string clientName { get; set; }
        static void Main(string[] args)
        {
            // create client instance 
            MqttClient client = new MqttClient(IPAddress.Parse("127.0.0.1"));
            //MqttClient client = new MqttClient(IPAddress.Parse("10.1.21.178"));

            Console.WriteLine("Please enter your name and press enter...");
            clientName = Console.ReadLine();

            // register to message received 
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
            Console.WriteLine(clientId);

            // subscribe to the topic "/home/temperature" with QoS 2 
            client.Subscribe(new string[] { "/home/Mobile :: Mobile Services :: Forms Service" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            start:

            Console.WriteLine("Type your message and press enter to send it...");
            string msg = Console.ReadLine();

            client.Publish("/home/temperature", Encoding.ASCII.GetBytes(msg));
            Console.WriteLine("{0} Published a Message!", clientName);

            goto start;
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(clientName + " Received a message.");
            Console.WriteLine(System.Text.Encoding.Default.GetString(e.Message));
        }

    }
}
