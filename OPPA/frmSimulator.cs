using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPPA
{
    public partial class frmSimulator : Form
    {
        private Graphics g; //Form graphics
        private WorldController controller;

        public frmSimulator()
        {
            InitializeComponent();
        }

        private void frmSimulator_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            g = CreateGraphics();
            LoadWorld();
            bgwThread.RunWorkerAsync(); //Initializing the thread
        }

        private void LoadWorld(string map = null)
        {
            List<PointF> checkpoints = new List<PointF>();
            checkpoints.Add(new PointF(711.6544f, 135));
            //checkpoints.Add(new PointF(750, 530));
            //checkpoints.Add(new PointF(170, 530));
            if(map == null)
                controller = new WorldController(OPPA.Properties.Resources.map
                    , new Point(170, 135), checkpoints); //Initializing the main controller
            else
                controller = new WorldController(map, new Point(170, 135), checkpoints);
        }

        private void bgwThread_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!e.Cancel)
                {
                    controller.Update(); //Updating the world
                    g.DrawImage(controller.World, Point.Empty); //Drawing the world
                }
            }
            catch(Exception)
            { }
        }

        private void frmSimulator_FormClosing(object sender, FormClosingEventArgs e)
        {
            bgwThread.CancelAsync();
        }

        private void frmSimulator_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
                controller.Fuzzy = !controller.Fuzzy;
            if (e.KeyCode == Keys.Up)
                controller.AccelerateCar();
            if (e.KeyCode == Keys.Down)
                controller.DecelerateCar();
            if (e.KeyCode == Keys.Left)
                controller.TurnLeft();
            if (e.KeyCode == Keys.Right)
                controller.TurnRight();
            if (e.KeyCode == Keys.M)
                mainMenu.Visible = !mainMenu.Visible;
        }

        private void loadMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdMaps = new OpenFileDialog();
            ofdMaps.Filter = "Image Files |*.bmp;*.jpg;*.png";
            ofdMaps.FilterIndex = 0;
            if (ofdMaps.ShowDialog() == DialogResult.OK)
            {
                LoadWorld(ofdMaps.FileName);
            }
            mainMenu.Visible = false;
        }
    }
}
