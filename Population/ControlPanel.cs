﻿using System;
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

    public partial class ControlPanel : Form
    {
        // Public events to be raised on setting changes.

        public event SolidControlChangeHandler chkSolidChangedEvent;
        public event SolidAllControlChangeHandler chkSolidAllChangedEvent;
        public event RuningStatusChangeHandler runStatusChangeEvent;
        public event EventHandler clearButtonClickEvent;
        public event EventHandler exitButtonClickEvent;

        public ControlPanel()   
        {
            InitializeComponent();
        }

        protected void chkSolid_CheckedChanged(object sender, EventArgs e)
        {
            // Event for change of solid setting.

            SolidControlChangeHandler handler = chkSolidChangedEvent;
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

        private void chkSolidAll_CheckedChanged(object sender, EventArgs e)
        {
            // Event for change of SolidAll property.
            SolidAllControlChangeHandler handler = chkSolidAllChangedEvent;
            if (handler != null)
                handler(chkSolidAll.Checked);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Start process event.
            RuningStatusChangeHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            // Stop process event.
            RuningStatusChangeHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear process event.
            EventHandler handler = clearButtonClickEvent;
            if (handler != null)
                handler(this, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Exit button event.
            EventHandler handler = exitButtonClickEvent;
            if (handler != null)
                handler(this, e);
        }

        private void ControlPanel_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void ControlPanel_Deactivate(object sender, EventArgs e)
        {
             this.Opacity = .15;
        }
    }
}
