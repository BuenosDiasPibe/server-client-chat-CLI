using System.Net.Sockets;

namespace networking_final;
public class Program
{
    static Random r = new();
    public static void Main(string[] args)
    {
        int port = r.Next(1000,5000);
        if(!args.Any())
            throw new ArgumentNullException("Use 'dotnet run <client/server> to make it work");
        switch(args[0].ToLower())
        {
            case "server":
                Server server = new(new UdpClient(port), port);
                server.Run();
                break;
            case "client":
                if(args.Length < 2)
                    throw new ArgumentNullException("add the port of the server");

                UdpClient clien = new(port);

                if(!Int32.TryParse(args[1], out int result))
                {
                    throw new ArgumentException($"{args[1]} cant be passed as a number");
                }

                Client client = new(clien, result);
                client.Run();
                break;
            default:
                throw new ArgumentException("use client or server for the operator");
        }
    }
}
