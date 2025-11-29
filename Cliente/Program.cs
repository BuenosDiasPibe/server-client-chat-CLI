using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientePrueba
{
    class Program
    {
        static void Main()
        {
            // Flujo de un cliente: conectarse a un servidor conocido y enviar/recibir mensajes.
            // El cliente necesita conocer la IP y el puerto del servidor.
            IPEndPoint puntoServidor = new IPEndPoint(IPAddress.Loopback, 5000);

            // El socket del cliente. También usa TCP.
            Socket cliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Conexión hacia el servidor.
            cliente.Connect(puntoServidor);
            Console.WriteLine("Conectado al servidor.");
            Console.Write("escriba su nombre: ");
            string a = Console.ReadLine() ?? "Anonymous";
            Cliente cliente2 = new(a);

            // Buffer: espacio temporal para almacenar los mensajes que llegan.
            byte[] buffer = new byte[1024];

            while (true)
            {
                string mensaje = cliente2.nombre + ": " + cliente2.WriteMessage();

                // Codificamos el texto en bytes para enviarlo por el socket.
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                cliente.Send(datos); // Flujo de salida del cliente.
                _ = cliente2.ListenForServerResponses(cliente);
            }
        }
    }
    public class Cliente(string nombre)
    {
        public string nombre = nombre;
        public string WriteMessage()
        {
            Console.Write("escribe un mensaje: ");
            string mensaje = Console.ReadLine() ?? "";
            return mensaje;
        }
        public async Task ListenForServerResponses(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                try
                {
                    int bytesReceived = await Task.Run(() => clientSocket.Receive(buffer));
                    if (bytesReceived > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                        Console.WriteLine("\n"+message);
                    }
                }
                catch (SocketException)
                {
                    Console.WriteLine("Disconnected from server");
                    break;
                }
            }
        }
    }
}