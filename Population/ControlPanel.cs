using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Population
{
    // Delegates for data settings.
    public delegate void SolidControlChangeHandler(bool MembersSolid);
    public delegate void SolidAllControlChangeHandler(bool AllMembersSolid);
    public delegate void RuningStatusChangeHandler(bool Running);
    public delegate void NewMemberIntervalHander(double Seconds);

    public partial class ControlPanel : Form
    {
        // Public events to be raised on setting changes.

        public event SolidControlChangeHandler chkSolidChangedEvent;
        public event SolidAllControlChangeHandler chkSolidAllChangedEvent;
        public event RuningStatusChangeHandler runStatusChangeEvent;
        public event EventHandler clearButtonClickEvent;
        public event EventHandler exitButtonClickEvent;
        public event NewMemberIntervalHander secondsChangeEvent;

        public ControlPanel()   
        {
            InitializeComponent();
        }

        private void nudSeconds_ValueChanged(object sender, EventArgs e)
        {


            try
            {
                NewMemberIntervalHander handler = secondsChangeEvent;
                if (handler != null)
                    handler((double)nudSeconds.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }

        }

        protected void chkSolid_CheckedChanged(object sender, EventArgs e)
        {
            // Event for change of solid setting.

            SolidControlChangeHandler handler = chkSolidChangedEvent;
            try
            {
                if (handler != null)
                    handler(chkSolid.Checked);

                // If Solid is unchecked, uncheck the SolidAll setting.

                if (!this.chkSolid.Checked)
                {
                    this.chkSolidAll.Checked = false;
                }

                // Disable SolidAll if Solid is unchecked.

                this.chkSolidAll.Enabled = this.chkSolid.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error ...");
            }

        }

        private void chkSolidAll_CheckedChanged(object sender, EventArgs e)
        {
            // Event for change of SolidAll property.
            try
            {
                SolidAllControlChangeHandler handler = chkSolidAllChangedEvent;
                if (handler != null)
                    handler(chkSolidAll.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Start process event.
                RuningStatusChangeHandler handler = runStatusChangeEvent;
                if (handler != null)
                    handler(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                // Stop process event.
                RuningStatusChangeHandler handler = runStatusChangeEvent;
                if (handler != null)
                    handler(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear process event.
                EventHandler handler = clearButtonClickEvent;
                if (handler != null)
                    handler(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                // Exit button event.
                EventHandler handler = exitButtonClickEvent;
                if (handler != null)
                    handler(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }
        }


        private void ControlPanel_Deactivate(object sender, EventArgs e)
        {
            this.Opacity = .20;
        }

        private void ControlPanel_MouseEnter(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

    }
}
