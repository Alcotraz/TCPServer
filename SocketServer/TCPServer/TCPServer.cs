using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SocketServer
{
    class TCPServer
    {
        private int port, clientsCount;

        private TcpListener server = null;
        private IPAddress lAddress;

        public TCPServer(IPAddress ipAddress, int portNumber)
        {
            server = new TcpListener(lAddress = ipAddress, port = portNumber);
        }

      //   [JsonProperty(PropertyName = "firstname")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public IPAddress IpAddress
        {
            get { return lAddress; }
            set { lAddress = value; }
        }
        
        public TcpListener Listener
        {
            get { return server; }
            set { server = value; }
        }

        public int ClientsCount
        {
            get { return clientsCount; }
        }



        public void Update()
        {
           
            
            while (true)
            {
                // При появлении клиента добавляем в очередь потоков его обработку.
                ThreadPool.QueueUserWorkItem(ObrabotkaZaprosa,  server.AcceptTcpClient());




            }

        }



        static void ObrabotkaZaprosa(object client_obj)
        {
            // Буфер для принимаемых данных.
            Byte[] bytes = new Byte[256];
            String data = null;

            //Можно раскомментировать Thread.Sleep(1000); 
            //Запустить несколько клиентов
            //и наглядно увидеть как они обрабатываются в очереди. 
            Thread.Sleep(1000);

            TcpClient client = client_obj as TcpClient;

            data = null;

            // Получаем информацию от клиента
            NetworkStream stream = client.GetStream();

            int i;

            // Принимаем данные от клиента в цикле пока не дойдём до конца.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Преобразуем данные в ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                // Преобразуем строку к верхнему регистру.
                data = data.ToUpper();

                // Преобразуем полученную строку в массив Байт.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Отправляем данные обратно клиенту (ответ).
                stream.Write(msg, 0, msg.Length);

            }

            // Закрываем соединение.
            client.Close();


        }



    }
}
