using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using KeyAuth;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BraveBypass
{
    public partial class bypass : Form
    {
        private const string MutexIdentifier = "BRAVE";
        private readonly string[] reversingToolProcessNames = { "OllyDbg", "ida_32bit", "ida_64bit", "cheatengine-x86_64-SSE4-AVX2.exe", "Wireshark" };
        private static Mutex singleInstanceMutex;

        public static api KeyAuthApp = new api(
        name: "BGMI BYPASS",
        ownerid: "5J0eFrdWq9", 
        secret: "00caa6078fd967bea73eb81c3cc2bae408b800956e8d23e3b0b2921bc9aa754b",
        version: "1.0"
    );

        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        private bypass1 Main = new bypass1();

        public bypass()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            singleInstanceMutex = new Mutex(true, MutexIdentifier, out bool isFirstInstance);

            if (!isFirstInstance)
            {
                MessageBox.Show("Another instance of the application is already running.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (DetectReversingTools())
            {
                MessageBox.Show("Reversing tools detected. Madarchod ! Baap Hu tera Randi ka Baccha", "Security Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(0);
            }
        }

        private bool DetectReversingTools()
        {
            foreach (string processName in reversingToolProcessNames)
            {
                if (IsProcessRunning(processName))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsProcessRunning(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Langsung login tanpa verifikasi key
            this.Hide();
            Main.Show();
        }

        private DateTime UnixTimeToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }

        private void bypass_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();
            this.MouseDown += FormMouseDown;
            this.MouseMove += FormMouseMove;
            this.MouseUp += FormMouseUp;
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y);
            }
        }

        private void FormMouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() => Process.Start("https://t.me/"));
        }

        private void status_Click(object sender, EventArgs e)
        {
        }
    }
}
