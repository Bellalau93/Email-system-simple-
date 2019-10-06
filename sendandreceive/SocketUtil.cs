using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace sendandreceive
{
    class SocketUtil
    {
        public static Socket sSocket;
        public static Socket smtpSocket;
        public static NetworkStream sNetstream;
        public static NetworkStream sNetstream1;
        public static String sUserId;
        public static String sPasswrd;
        public static String ssmtpServer;
        public static String spopServer;
        private static int ssmtpPort;
        private static int spopPort;
        private static String[] sCommandCode = new String[] { "APOP", "QUIT", "NOOP", "STAT", "LIST", "RETR", "TOP", "DELTE", "RSET", "UIDL" };
        private static StreamReader Streamr;
        private static StreamWriter Streamw;
    }
}
