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
    public partial class ControlPanel : Form
    {
        public event EventHandler chkSolidChangedEvent;
        public event EventHandler chkSolidAllChangedEvent;
        public event EventHandler tbOpacityValueChangedEvent;
        public event EventHandler chkOpacityAllChangedEvent;
        public event EventHandler runStatusChangeEvent;
        public event EventHandler clearButtonClickEvent;
        public event EventHandler exitButtonClickEvent;

        private bool vchkSolidVal;
        private bool vchkSolidAllVal;
        private int vtbOpacityVal;
        private bool vchkOpacityAllVal;
        private bool vRunningVal;

        public bool RunStatus
        {
            get { return vRunningVal; }
        }

        public int MemberOpacityValue
        {
            get { return vtbOpacityVal; }
        }

        public bool ApplyOpacityToAll
        {
            get { return vchkOpacityAllVal; }
        }

        public bool MembersSolid
        {
            get { return vchkSolidVal; }
        }

        public bool AllMembersSolid
        {
            get { return vchkSolidAllVal; }
        }

        public ControlPanel()
        {
            InitializeComponent();
        }

        protected void chkSolid_CheckedChanged(object sender, EventArgs e)
        {
            vchkSolidVal = this.chkSolid.Checked; // Make value available outside.
            EventHandler handler = chkSolidChangedEvent;
            if (handler != null)
                handler(this, e);

            this.chkSolidAll.Enabled = this.chkSolid.Checked;
        }

        private void chkSolidAll_CheckedChanged(object sender, EventArgs e)
        {
            vchkSolidAllVal = this.chkSolidAll.Checked; // Make value availble.
            EventHandler handler = chkSolidAllChangedEvent;
            if (handler != null)
                handler(this, e);
        }

        private void tbOpacity_ValueChanged(object sender, EventArgs e)
        {
            vtbOpacityVal = this.tbOpacity.Value;
            EventHandler handler = tbOpacityValueChangedEvent;
            if (handler != null)
                handler(this, e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            vRunningVal = true;
            EventHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(this, e);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            vRunningVal = false;
            EventHandler handler = runStatusChangeEvent;
            if (handler != null)
                handler(this, e);
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

        private void chkOpaqueAll_CheckedChanged(object sender, EventArgs e)
        {
            vchkOpacityAllVal = chkOpaqueAll.Checked;
            EventHandler handler = chkOpacityAllChangedEvent;
            if (handler != null)
                handler(this, e);
        }
    }
}
