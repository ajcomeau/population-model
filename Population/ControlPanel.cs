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
    public delegate void SolidControlChangeHandler(bool MembersSolid);
    public delegate void SolidAllControlChangeHandler(bool AllMembersSolid);
    public delegate void OpacityChangeHandler(int Opacity);
    public delegate void OpacityAllHandler(bool ApplyOpacityToAll);
    public delegate void RuningStatusChangeHandler(bool Running);

    public partial class ControlPanel : Form
    {
        public event SolidControlChangeHandler chkSolidChangedEvent;
        public event SolidAllControlChangeHandler chkSolidAllChangedEvent;
        public event OpacityChangeHandler tbOpacityValueChangedEvent;
        public event OpacityAllHandler chkOpacityAllChangedEvent;
        public event RuningStatusChangeHandler runStatusChangeEvent;
        public event EventHandler clearButtonClickEvent;
        public event EventHandler exitButtonClickEvent;

        public ControlPanel()   
        {
            InitializeComponent();
        }

        protected void chkSolid_CheckedChanged(object sender, EventArgs e)
        {
            SolidControlChangeHandler handler = chkSolidChangedEvent;
            if (handler != null)
                handler(chkSolid.Checked);

            if(!this.chkSolid.Checked)
            {
                this.chkSolidAll.Checked = false;
            }

            this.chkSolidAll.Enabled = this.chkSolid.Checked;
        }

        private void chkSolidAll_CheckedChanged(object sender, EventArgs e)
        {
            SolidAllControlChangeHandler handler = chkSolidAllChangedEvent;
            if (handler != null)
                handler(chkSolidAll.Checked);
        }

        private void tbOpacity_ValueChanged(object sender, EventArgs e)
        {
            OpacityChangeHandler handler = tbOpacityValueChangedEvent;
            if (handler != null)
                handler(tbOpacity.Value);
        }

        private void chkOpaqueAll_CheckedChanged(object sender, EventArgs e)
        {
            OpacityAllHandler handler = chkOpacityAllChangedEvent;
            if (handler != null)
                handler(chkOpaqueAll.Checked);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            RuningStatusChangeHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            RuningStatusChangeHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            EventHandler handler = clearButtonClickEvent;
            if (handler != null)
                handler(this, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            EventHandler handler = exitButtonClickEvent;
            if (handler != null)
                handler(this, e);
        }


    }
}
