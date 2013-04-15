using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SocketServer
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;

        TCPServer server;
        Thread th;

        Order order = new Order();

        public MainForm()
        {
            // Определим нужное максимальное количество потоков
            int MaxThreadsCount = Environment.ProcessorCount * 4;

            // Установим максимальное количество рабочих потоков
            ThreadPool.SetMaxThreads(MaxThreadsCount, MaxThreadsCount);

            // Установим минимальное количество рабочих потоков
            ThreadPool.SetMinThreads(2, 2);


            // Устанавливаем порт для TcpListener = 9595.
            Int32 port = 9595;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");


            server = new TCPServer(localAddr, port);

            th = new Thread(new ThreadStart(Slu));


            



            
           


           

            
            



            InitializeComponent();

            

        }

        private void Slu()
        {
            
            server.Update();
            
        }


































        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Окно " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
















        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Connect();
        }









        // Параметры сервера
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;

            Label lPort = new Label();
            lPort.Parent = childForm;

            Label lAddress = new Label();
            lAddress.Parent = childForm;

            lPort.Text = "Порт: " + server.Port.ToString();
            lAddress.Dock = lPort.Dock = DockStyle.Top;

            lAddress.Text = "ip: " + server.IpAddress.ToString();

            childForm.Text = "Параметры сервера";
            childForm.Show();
        }



        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Connect();

            // !!!!!
          //  order.Time = 666;
          //  string output = JsonConvert.SerializeObject(order);
        }



        private void tCloseConnection_Click(object sender, EventArgs e)
        {
            this.Disconect();
        }





       
        // Редактор нового заказа
        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Width = this.Width / 2;
            childForm.Height = this.Height / 2;

            Label lName = new Label();
            lName.Parent = childForm;
            lName.Text = "Имя";


            TextBox tBoxName = new TextBox();
            tBoxName.Parent = childForm;
            tBoxName.Text = "Введите имя";
            tBoxName.TextAlign = HorizontalAlignment.Center;
            tBoxName.Width = 200;
            tBoxName.Location = new Point(10, 25);

            Label lSecondName = new Label();
            lSecondName.Parent = childForm;
            lSecondName.Text = "Фамилия";
            lSecondName.Location = new Point(0, 50);

            TextBox tBoxSecondName = new TextBox();
            tBoxSecondName.Parent = childForm;
            tBoxSecondName.Text = "Введите фамилию";
            tBoxSecondName.TextAlign = HorizontalAlignment.Center;
            tBoxSecondName.Width = 200;
            tBoxSecondName.Location = new Point(10, 75);



            childForm.Text = "Новый заказ";
            childForm.Show();

        }
















        private void Connect()
        {
            if (!server.Listener.Server.Connected)
            {
                server.Listener.Start();
                if (th.ThreadState == ThreadState.Unstarted) th.Start();
                else if (th.ThreadState == ThreadState.Suspended) th.Resume();

                th.IsBackground = true;
                sLabel.Text = "Состояние сервера: запущен";
                tConnect.Enabled = false;
                tStartConnection.Enabled = false;
                tCloseConnection.Enabled = true;
            }
        }

        private void Disconect()
        {
            if (!server.Listener.Server.Connected)
            {
                th.Suspend();
                server.Listener.Stop();
                sLabel.Text = "Состояние сервера: отключен";
                tConnect.Enabled = true;
                tStartConnection.Enabled = true;
                tCloseConnection.Enabled = false;
            }
        }
    }
}
