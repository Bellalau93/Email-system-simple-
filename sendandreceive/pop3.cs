using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Net.Mail;

namespace sendandreceive
{
    class pop3
    {
        public string POPserver;
        public string user;
        public string password;
        public pop3(){}
        public pop3(string server,string _user,string _password)
        {
            POPserver = server;
            user = _user;
            password = _password;
        }
        public void Connect()
        {
            TcpClient receiver = new TcpClient(POPserver, 110);
            Byte[] outbytes;
            string input;
            NetworkStream nwStream = null;
            try
            {
                nwStream = receiver.GetStream();
                StreamReader sr = new StreamReader(nwStream);
                Console.WriteLine(sr.ReadLine());
                input = "USER" + user + "/r/n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                nwStream.Write(outbytes, 0, outbytes.Length);
                Console.WriteLine(sr.ReadLine());
              
                input = "PASS" + password + "/r/n";
                outbytes = System.Text.Encoding.ASCII.GetBytes(input.ToCharArray());
                nwStream.Write(outbytes, 0, outbytes.Length);
                Console.WriteLine(sr.ReadLine());
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("连接失败");
            }
        }

    }
}
