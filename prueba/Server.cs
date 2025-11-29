using System.Net;
using System.Net.Sockets;
using System.Text;

namespace  networking_final;
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
