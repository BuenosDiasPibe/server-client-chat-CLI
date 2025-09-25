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

            // Buffer: espacio temporal para almacenar los mensajes que llegan.
            byte[] buffer = new byte[1024];

            while (true)
            {
                // El usuario escribe un mensaje por consola.
                Console.Write("Escribir mensaje: ");
                string mensaje = Console.ReadLine();

                // Codificamos el texto en bytes para enviarlo por el socket.
                byte[] datos = Encoding.UTF8.GetBytes(mensaje);
                cliente.Send(datos); // Flujo de salida del cliente.

                // Recibimos la respuesta del servidor.
                int cantidadBytes = cliente.Receive(buffer);
                // Decodificamos el texto en bytes para transformarlo en string.
                string respuesta = Encoding.UTF8.GetString(buffer, 0, cantidadBytes); // Flujo de entrada del cliente.

                Console.WriteLine(respuesta);
            }
        }
    }
}