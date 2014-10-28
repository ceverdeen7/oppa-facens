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
            controller = new WorldController(); //Initializing the main controller
            bgwThread.RunWorkerAsync(); //Initializing the thread
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
            if (e.KeyData == Keys.F)
                controller.Fuzzy = !controller.Fuzzy;
            if (e.KeyData == Keys.Up)
                controller.AccelerateCar();
            if (e.KeyData == Keys.Down)
                controller.DecelerateCar();
            if (e.KeyData == Keys.Left)
                controller.TurnLeft();
            if (e.KeyData == Keys.Right)
                controller.TurnRight();

        }
    }
}
