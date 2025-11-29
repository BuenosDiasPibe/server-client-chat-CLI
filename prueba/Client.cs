using System.Net;
using System.Net.Sockets;
using System.Text;

namespace networking_final;
public class Client(UdpClient client, int port)
{
    UdpClient client = client;
    private int port = port;
    public void Run()
    {
        IPEndPoint server = new(IPAddress.Any, port);
        IPEndPoint remote = new(IPAddress.Any, 0);
        Console.WriteLine($"Conected to port {port}");

        Thread receptor = receptorTask(remote);
        receptor.Start();

        Console.WriteLine("1. Add a new object to the list\n2. show the list");
        while(true)
        {
            string commnad = Console.ReadLine() ?? "";
            if(commnad.Contains('1'))
            {
                Console.Write("write what you want to add: ");
                string s = Console.ReadLine() ?? string.Empty;
                if(!s.Any())
                {
                    Console.WriteLine("sopenco");
                    continue;
                }
                commnad = "1/" + s;
            }
            byte[] data = Encoding.UTF8.GetBytes(commnad);
            client.Send(data, data.Length, server);
        }
    }
    private Thread receptorTask(IPEndPoint remote)
    {
        return new Thread(() => {
            while (true)
            {
                byte[] data = client.Receive(ref remote);
                string texto = Encoding.UTF8.GetString(data);
                Console.WriteLine(texto);
            }
        });
    }
}
