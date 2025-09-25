using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerPrueba2
{
    class Program
    {
        static void Main()
        {
            // Un "endpoint" es la dirección y puerto donde el servidor estará escuchando.
            // IPAddress.Any significa que acepta conexiones desde cualquier IP local.
            IPEndPoint puntoEscucha = new IPEndPoint(IPAddress.Any, 5000);

            // El Socket es el componente de comunicación. Se encarga de enviar/recibir datos.
            Socket servidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Asociamos el socket a la dirección y puerto definidos.
            servidor.Bind(puntoEscucha);

            // Ponemos el socket a escuchar conexiones. El parámetro (10) es la cantidad máxima en cola.
            servidor.Listen(10);

            Console.WriteLine("Servidor esperando conexiones...");

            // Flujo de un servidor: sirve aceptar conexiones entrantes y atenderlas.
            Socket cliente = servidor.Accept(); // Aquí se "acepta" a un cliente que quiere conectarse.
            Console.WriteLine("Cliente conectado.");

            // El buffer es un espacio de memoria temporal para almacenar los datos recibidos.
            byte[] buffer = new byte[1024];

            // Ciclo infinito para recibir mensajes continuamente.
            while (true)
            {
                int cantidadBytes = cliente.Receive(buffer); // Recibe datos del cliente en el buffer.
                string mensaje = Encoding.UTF8.GetString(buffer, 0, cantidadBytes); // Decodificamos a texto.

                Console.WriteLine(mensaje);

                // Enviar respuesta al cliente (flujo de salida del servidor).
                string respuesta = mensaje;
                
                // Codificamos el texto en bytes para enviarlo por el socket.
                byte[] datosRespuesta = Encoding.UTF8.GetBytes(respuesta);
                cliente.Send(datosRespuesta); // Se envían los datos de vuelta.
            }
        }
    }
}