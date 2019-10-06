using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.IO;
using LumiSoft.Net.POP3.Client;
using LumiSoft.Net.Mail;
using LumiSoft.Net.MIME;

namespace sendandreceive
{
    public partial class client : Form
    {
        
        private MailMessage msg;
        private Attachment atm;
        public TcpClient server;
        static public NetworkStream nwStream;
        public StreamReader sr;
        public String data;
        public string POPserver;
        public string user;
        public string password;
        public byte[] szdata;
        public String CRLF = "\r\n";
        private List<pop3client> mailreceive = new List<pop3client>(100);
        public int mailnumber;
       
        public client()
        {
            InitializeComponent();
            
        }
         
        
        private String getStatus()
        {
            String ret = sr.ReadLine();
            //liststatus.Items.Add(ret);
            //listView2.Items.Count = listView2.Items.Count - 1;
            return ret;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            button10.PerformClick();
                
            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
      
        private void button7_Click(object sender, EventArgs e)
        {
            msg = new MailMessage();
            string at = comboBox1.Text;
            comboBox1.Text ="";
            SocketUtil.sUserId = textBox9.Text;
            SocketUtil.sPasswrd = textBox6.Text;
            

            try
            {
                msg.To.Add(textBox1.Text);
                msg.From = new MailAddress(SocketUtil.sUserId);
                msg.Subject = textBox2.Text;
                msg.SubjectEncoding = Encoding.Default;
                msg.Body = textBox3.Text;
                msg.BodyEncoding = Encoding.Default;
               // if (comboBox1.Text!="")
                //{
                    msg.Attachments.Add(new Attachment(at));
               /// }

                SmtpClient smtpclient = new SmtpClient();
                smtpclient.Host = textBox7.Text;
                smtpclient.Port = 25;
                smtpclient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpclient.Credentials = new System.Net.NetworkCredential(SocketUtil.sUserId, SocketUtil.sPasswrd);
                smtpclient.Send(msg);

                MessageBox.Show("发送成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "D:\\";
            openFileDialog1.Filter = "all files(*.*)|*.*";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            comboBox1.Items.Add(openFileDialog1.FileName.Trim());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text=="")
            {
                MessageBox.Show("没有可删除文件");
            }
            else
            {
                comboBox1.Items.Remove(comboBox1.Text.Trim());
            }


        }


        private void button10_Click(object sender, EventArgs e)
        {


            POPserver = textBox8.Text;
            user = textBox9.Text;
            password = textBox6.Text;
            
          TcpClient receiver = new TcpClient();
            try
            {
                receiver.Connect(POPserver, 110);
                liststatus.Items.Add("连接成功");
            }
            catch
            {
                MessageBox.Show("无法连接到服务器");
            }
            
            nwStream = receiver.GetStream();
            if (nwStream == null)
            {
                throw new Exception("无法取得回复");
            }
            string returnMsg = ReadStream(ref nwStream);
            checkerror(returnMsg);
            liststatus.Items.Add("连接应答" + returnMsg + "\r\n");
            
            try
            {
               WriteStream(ref nwStream, "USER " + user);
                returnMsg = ReadStream(ref nwStream);
                checkerror(returnMsg);
                liststatus.Items.Add("POP3server:" + returnMsg + "\r\n");
                WriteStream(ref nwStream, "PASS " + password);
                returnMsg = ReadStream(ref nwStream);
                checkerror(returnMsg);
                liststatus.Items.Add("POP3server:" + returnMsg + "\r\n");
                WriteStream(ref nwStream, "STAT");
                returnMsg = ReadStream(ref nwStream);
                checkerror(returnMsg);
                liststatus.Items.Add("POP3server:" + returnMsg + "\r\n");
                string[] totalstat = returnMsg.Split(new char[] {' '});
                mailnumber=Int32.Parse(totalstat[1]);
                textBox10.Text = "共" + totalstat[1] + "封邮件";
                //接收邮件
                POP3_Client pop = new POP3_Client();
                pop.Connect(POPserver, 995, true);
                liststatus.Items.Add("连接成功");
                pop.Login(user, password);
                POP3_ClientMessageCollection messages = pop.Messages;


                for(int i = 0; i < mailnumber; i++)
                {
                    POP3_ClientMessage message = messages[i];
                  if(message != null)
                    {
                        Byte[] messageBytes = message.MessageToByte();
                        Mail_Message mimemessage = Mail_Message.ParseFromByte(messageBytes);
                        pop3client cli = new pop3client(mimemessage.From[0].DisplayName, mimemessage.From[0].Address, mimemessage.Date.ToLongDateString(),
                          mimemessage.Subject, mimemessage.BodyText, mimemessage.GetAttachments(true, true),i+1);
                        mailreceive.Add(cli);
                       // mailreceive[i].sender1= mimemessage.From[0].DisplayName;
                        //mailreceive[i].senderaddress = mimemessage.From[0].Address;
                        //mailreceive[i].senderdate = mimemessage.Date.ToLongDateString();
                       // mailreceive[i].subject = mimemessage.Subject;
                        //mailreceive[i].content = mimemessage.BodyText;
                        //mailreceive[i].attachment = mimemessage.GetAttachments(true, true);
                        //mailreceive[i].num = i + 1;
                            listView1.Items.Add(new ListViewItem(new string[] { (i+1).ToString(), mailreceive[i].sender1, mailreceive[i].subject, mailreceive[i].senderdate, "无" }));
                            foreach (MIME_Entity entity in mailreceive[i].attachment)
                            {
                                string filename = entity.ContentDisposition.Param_FileName;
                            cli.getfilename(filename);
                            listView1.Items[i].SubItems.Add(" ");
                            listView1.Items[i].SubItems[4].Text = filename;
                             
                            }
                    }
                }
               

            }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine("登陆失败");
                }
          
   
           
            
        }
      private string ReadStream(ref NetworkStream netstream)
        {
            StringBuilder strB = new StringBuilder();
            StreamReader strr = new StreamReader(netstream);
            String str = strr.ReadLine();
            
           while (str == null || str.Length == 0)
            {
                str = strr.ReadLine();
            }
            strB.Append(str);
            if(strr.Peek()!=-1)//判断是否读取结束
            {
                while((str=strr.ReadLine())!=null)
                {
                    strB.Append(str);
                }
            }
            return strB.ToString();
        }
        private void WriteStream(ref NetworkStream netstream,string command)
        {
            string sendstring = command + "\r\n";
            Byte[] outbytes = Encoding.ASCII.GetBytes(sendstring.ToCharArray());
            netstream.Write(outbytes, 0, outbytes.Length);
        }
        private void checkerror(string strmessage)
        {
            if (strmessage.IndexOf("+OK") == -1)
                throw new Exception("收到来自POP3服务器错误信息：" + strmessage);
        }
      

        
        private void button9_Click(object sender, EventArgs e)
        {
            Cursor cr = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            //logout
            TcpClient receiver = new TcpClient(POPserver, 110);
            NetworkStream nwStream = null;
            nwStream = receiver.GetStream();
            data = "QUIT" + CRLF;
            szdata = System.Text.Encoding.ASCII.GetBytes(data.ToCharArray());
            nwStream.Write(szdata, 0, szdata.Length);
            nwStream.Close();
            liststatus.Items.Add("断开连接");
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            listView1.Clear();
            textBox9.Clear();
            textBox6.Clear();

            Cursor.Current = cr;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (listView1.SelectedItems.Count > 0)
            {
                index = this.listView1.SelectedItems[0].Index;
                string aa = listView1.Items[index].SubItems[0].Text;
            }

            for(int i = 0; i < mailnumber; i++)
            {
                if (index+1== mailreceive[i].num)
                {
                    textBox13.Text = mailreceive[i].senderaddress;
                    textBox12.Text = mailreceive[i].subject;
                    textBox11.Text = mailreceive[i].content;
                }
            }

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (listView1.SelectedItems.Count > 0)
            {
                index = this.listView1.SelectedItems[0].Index;
                string aa = listView1.Items[index].SubItems[0].Text;
            }
            for (int i = 0; i < mailnumber; i++)
            {
                if (index+1 == mailreceive[i].num)
                {
                    foreach (MIME_Entity entity in mailreceive[i].attachment)
                    {
                        DirectoryInfo dir = new DirectoryInfo(@"D:\");
                        if (!dir.Exists) dir.Create();
                        string path = Path.Combine(dir.FullName, mailreceive[i].filename);
                        MIME_b_SinglepartBase byteObj = (MIME_b_SinglepartBase)entity.Body;
                        Stream decoded = byteObj.GetDataStream();
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            LumiSoft.Net.Net_Utils.StreamCopy(decoded, fs, 4000);

                        }
                        liststatus.Items.Add("附件已被下载");
            

                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (listView1.SelectedItems.Count > 0)
            {
                index = this.listView1.SelectedItems[0].Index;
                string aa = listView1.Items[index].SubItems[0].Text;
            }
                    try
                    {
                   WriteStream(ref nwStream, "DELE " + index+1);
                    string  returnMsg = ReadStream(ref nwStream);
                    checkerror(returnMsg);
                    liststatus.Items.Add("删除成功");
                
                for(int i = index; i < listView1.Items.Count; i++)
                {
                    int num = Int32.Parse(listView1.Items[i].SubItems[0].Text);
                    num--;
                    listView1.Items[i].SubItems[0].Text = num.ToString();
                   // MessageBox.Show(listView1.Items[index].SubItems[0].Text);
                }
               listView1.Items[index].Remove();
                textBox10.Text = "共" + (listView1.Items.Count).ToString() + "封邮件";
                }

            catch
            {
                liststatus.Items.Add("删除失败");
            }
                }
    }
}
