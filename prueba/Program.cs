using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
          throw new ArgumentException($"{args[1]} cant be passed as a number");
        Client client = new(clien, result);
        client.Run();
        break;
      default:
        throw new ArgumentException("use client or server for the operator");
    }
  }
}
public class Server(UdpClient server, int port)
{
  public UdpClient server = server;
  public List<IPEndPoint> players = new();
  private int port = port;
  public List<string> strings = new();
  public void Run()
  {
    Console.WriteLine($"Server started at port {port}");
    while(true)
    {
      IPEndPoint client = new(IPAddress.Any, 0);
      byte[] data = server.Receive(ref client);
      string command = Encoding.UTF8.GetString(data);
      Console.WriteLine(command);

      if(!players.Contains(client))
      {
        players.Add(client);
        Console.WriteLine("added new Clinet");
      }

      StringBuilder messagess = new();
      int commander = 0;
      if(command.Contains('1'))
      {
        string a = command.Remove(0, 1);
        strings.Add(a);
        messagess.Append("added obejct");
        commander = 1;
      }else if(command.Contains('2'))
      {
        for(int i = 0; i < strings.Count; i++)
        {
          messagess.Append($"{i}: {strings[i]}\n");
        }
        commander = 2;
      }
      else{
        messagess.Append(command);
      }

      byte[] message = Encoding.UTF8.GetBytes(messagess.ToString());
      foreach(var p in players)
      {
        switch(commander)
        {
          case 1:
            if(p != client) continue;
            server.Send(message, message.Length, p);
            break;
          case 2:
            if(p != client) continue;
            server.Send(message, message.Length, p);
            break;
          default:
          server.Send(message, message.Length, p);
            break;
        }
      }
    }
  }
}
public class Client(UdpClient client, int port)
{
  UdpClient client = client;
  private int port = port;
  public void Run()
  {
    IPEndPoint server = new(IPAddress.Any, port);
    IPEndPoint remote = new(IPAddress.Any, 0);
    Console.WriteLine($"Conected to port {port}");
    Thread receptor = new Thread(() =>
    {
        while (true)
        {
            byte[] data = client.Receive(ref remote);
            string texto = Encoding.UTF8.GetString(data);
            Console.WriteLine(texto);
        }
    });
    receptor.Start();
    Console.WriteLine("1. Add a new object to the list\n2. show the list");
    while(true)
    {
      string commnad = Console.ReadLine() ?? "h";
      if(commnad.Contains('1'))
      {
        Console.Write("write what you want to add: ");
        string s = Console.ReadLine() ?? string.Empty;
        if(!s.Any())
        {
          Console.WriteLine("sopenco");
        }
        commnad = "1/" + s;
      }
      byte[] data = Encoding.UTF8.GetBytes(commnad);
      client.Send(data, data.Length, server);
    }
  }
}
