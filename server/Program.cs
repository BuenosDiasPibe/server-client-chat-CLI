using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace server
{
    class Program
    {
        static async Task Main()
        {
            IPEndPoint puntoEscucha = new IPEndPoint(IPAddress.Any, 5000);

            // El Socket es el componente de comunicación. Se encarga de enviar/recibir datos.
            Socket servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Asociamos el socket a la dirección y puerto definidos.
            servidor.Bind(puntoEscucha);

            // Ponemos el socket a escuchar conexiones. El parámetro (10) es la cantidad máxima en cola.
            servidor.Listen(10);

            Console.WriteLine("Servidor esperando conexiones...");
            while (true)
            {
                // Flujo de un servidor: sirve aceptar conexiones entrantes y atenderla
                Socket cliente = servidor.Accept(); // Aquí se "acepta" a un cliente que quiere conectarse.
                Console.WriteLine("Cliente conectado.");
                _ = Coso(cliente);
            }
        }
        private static List<Socket> connectedClients = new List<Socket>();

        public static async Task Coso(Socket cliente)
        {
            lock (connectedClients)
            {
                connectedClients.Add(cliente);
            }

            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int cantidadBytes = await Task.Factory.FromAsync(
                    cliente.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, null, null),
                    cliente.EndReceive);

                    string mensaje = Encoding.UTF8.GetString(buffer, 0, cantidadBytes);
                    Console.WriteLine(mensaje);

                    byte[] datosRespuesta = Encoding.UTF8.GetBytes(mensaje);

                    // Send to all connected clients
                    List<Socket> clientesToRemove = new List<Socket>();
                    List<Socket> clientsCopy;
                    lock (connectedClients)
                    {
                        clientsCopy = connectedClients.ToList();
                    }

                    foreach (Socket connectedClient in clientsCopy)
                    {
                        try
                        {
                            await Task.Factory.FromAsync(
                                connectedClient.BeginSend(datosRespuesta, 0, datosRespuesta.Length, SocketFlags.None, null, null),
                                connectedClient.EndSend);
                        }
                        catch
                        {
                            clientesToRemove.Add(connectedClient);
                        }
                    }

                    // Remove disconnected clients
                    lock (connectedClients)
                    {
                        foreach (var clientToRemove in clientesToRemove)
                        {
                            connectedClients.Remove(clientToRemove);
                        }
                    }
                }
            }
            catch
            {
                lock (connectedClients)
                {
                    connectedClients.Remove(cliente);
                }
            }
        }
    }
}