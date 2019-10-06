using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumiSoft.Net;
using LumiSoft.Net.POP3;
using LumiSoft.Net.MIME;
namespace sendandreceive
{
    class pop3client
    {
        public string sender1;
        public string senderaddress;
        public string senderdate;
        public string subject;
        public string content;
        public MIME_Entity[] attachment;
        public string filename;
        public int num;
        public pop3client(string send,string add, string date,string sub,string con,MIME_Entity[] en, int number)
        {
            this.sender1 = send;
            this.senderaddress = add;
            this.senderdate = date;
            this.subject = sub;
            this.content = con;
            this.attachment = en;
          
            this.num = number;
        }
        public void getfilename(string name)
        {
            filename = name;
        }

    }
}
