using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;

namespace Population
{
    class MemberOps
    {
        // Enumeration for canvas edges.
        public enum ObjectContact { Left, Top, Right, Bottom, None};

        // Class requires a reference to the canvas for current height and width.
        private Canvas workingCanvas;
        ControlPanel settingsPanel;

        // Values returned from settings panel.
        private bool vSolidMembers;
        private bool vSolidAllMembers;
        private int vOpacity;
        private bool vOpacityAll;
        private bool vRunning;

        // Collision count
        private int collisions;

        // Properties for settings panel values.
        public bool SolidMembers
        {
            get { return vSolidMembers; }
        }

        public bool AllMembersSolid
        {
            get { return vSolidMembers; }
        }

        public int MemberOpacity
        {
            get { return vOpacity; }
        }

        public bool ApplyOpacityToAll
        {
            get { return vOpacityAll; }
        }

        public bool IsRunning
        {
            get { return vRunning; }
        }

        public MemberOps(Canvas CurrentCanvas)
        {
            // Primary constructor
            workingCanvas = CurrentCanvas;
            settingsPanel = new ControlPanel();
            SettingsEventWiring();
            settingsPanel.Show();
        }

        private void SettingsEventWiring()
        {
            // Link each event from the control panel to an event handler.
            settingsPanel.chkSolidChangedEvent += SettingsPanel_chkSolidChangedEvent;
            settingsPanel.chkSolidAllChangedEvent += SettingsPanel_chkSolidAllChangedEvent;
            settingsPanel.tbOpacityValueChangedEvent += SettingsPanel_tbOpacityValueChangedEvent;
            settingsPanel.chkOpacityAllChangedEvent += SettingsPanel_chkOpacityAllChangedEvent;
            settingsPanel.runStatusChangeEvent += SettingsPanel_runStatusChangeEvent;
            settingsPanel.clearButtonClickEvent += SettingsPanel_clearButtonClickEvent;
            settingsPanel.exitButtonClickEvent += SettingsPanel_exitButtonClickEvent;
        }

        private void SettingsPanel_chkOpacityAllChangedEvent(bool ApplyOpacityToAll)
        {
            this.vOpacityAll = ApplyOpacityToAll;
        }

        private void SettingsPanel_exitButtonClickEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SettingsPanel_clearButtonClickEvent(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SettingsPanel_runStatusChangeEvent(bool RunStatus)
        {
            this.vRunning = RunStatus;
        }

        private void SettingsPanel_tbOpacityValueChangedEvent(int MemberOpacityValue)
        {
            this.vOpacity = MemberOpacityValue;
        }

        private void SettingsPanel_chkSolidAllChangedEvent(bool AllMembersSolid)
        {
            this.vSolidAllMembers = AllMembersSolid;
        }

        private void SettingsPanel_chkSolidChangedEvent(bool MembersSolid)
        {
            this.vSolidMembers = MembersSolid;
        }

        public void MoveMember(Shape MemberObject)
        {
            // Get vital stats dictionary
            Type t = MemberObject.Tag.GetType();
            MemberStats memberVitals;
            double leftMargin, topMargin;
            ObjectContact contactEdge;

            // Verify type of object tag.
            if (t == typeof(MemberStats)){
                memberVitals = (MemberStats)MemberObject.Tag;
                // Determine next position by adding information from stats object to current position.
                leftMargin = MemberObject.Margin.Left + memberVitals.XDirect;
                topMargin = MemberObject.Margin.Top + memberVitals.YDirect;
                MemberObject.Margin = new Thickness(leftMargin, topMargin, 0, 0);
                // Determine if the object has come into contact with one of the edges.
                // If it has, change direction.
                contactEdge = EdgeDetect(MemberObject);
                if (contactEdge != ObjectContact.None)
                {
                    MemberBounce(MemberObject, contactEdge);
                }
                ScanForCollisions(MemberObject);
            }
        }

        public void ScanForCollisions(Shape MovingShape)
        {
            foreach (UIElement uiObject in workingCanvas.Children)
            {
                if(uiObject != MovingShape)
                { 
                    Type t = uiObject.GetType();
                    ObjectContact collideDetect = ObjectContact.None;
                
                    if (t.UnderlyingSystemType.BaseType == typeof(Shape))
                    {
                        Shape StaticShape = (Shape)uiObject;
                        collideDetect = CollisionDetect(MovingShape, StaticShape);

                        if (collideDetect != ObjectContact.None)
                        {
                            MemberBounce(MovingShape, collideDetect);
                        }                    
                    }
                }

            }
        }

        public ObjectContact CollisionDetect(Shape MovingShape, Shape StaticShape)
        {
            bool collision;

            Rect MovingShapeRect = new Rect(MovingShape.Margin.Left, MovingShape.Margin.Top, MovingShape.ActualWidth, MovingShape.ActualHeight);
            Rect StaticShapeRect = new Rect(StaticShape.Margin.Left, StaticShape.Margin.Top, StaticShape.ActualWidth, StaticShape.ActualHeight);
            ObjectContact firstContact = ObjectContact.None;
            
            collision = (MovingShapeRect.IntersectsWith(StaticShapeRect));

            if (collision)
            {

                // Left side contact
                if (MovingShapeRect.Left <= StaticShapeRect.Right)
                {
                    firstContact = ObjectContact.Left;
                    MovingShape.Margin = new Thickness(MovingShape.Margin.Left + 1, MovingShape.Margin.Top, 0, 0);
                }

                // Top contact
                if (firstContact == ObjectContact.None && MovingShapeRect.Top <= StaticShapeRect.Bottom)
                {
                    firstContact = ObjectContact.Top;
                    MovingShape.Margin = new Thickness(MovingShape.Margin.Left, MovingShape.Margin.Top + 1, 0, 0);
                }

                // Right side contact
                if (firstContact == ObjectContact.None && (MovingShapeRect.Right >= StaticShapeRect.Left))
                {
                    firstContact = ObjectContact.Right;
                    MovingShape.Margin = new Thickness((StaticShapeRect.Left - MovingShape.Width - 1), MovingShape.Margin.Top, 0, 0);
                }

                // Bottom contact
                if (firstContact == ObjectContact.None && (MovingShapeRect.Bottom >= StaticShapeRect.Top))
                {
                    firstContact = ObjectContact.Bottom;
                    MovingShape.Margin = new Thickness(MovingShape.Margin.Left, (StaticShape.Height - MovingShape.Height - 1), 0, 0);
                }
            }

            return firstContact;
        }


        public ObjectContact EdgeDetect(Shape MemberObject)
        {
            // Get the margin values for the object.
            Thickness memberBorders = MemberObject.Margin;
            double memberLeft = memberBorders.Left;
            double memberTop = memberBorders.Top;
            double memberRight = (memberBorders.Left + MemberObject.Width);
            double memberBottom = (memberBorders.Top + MemberObject.Height);
            MemberStats MemberVitals = (MemberStats)MemberObject.Tag;

            ObjectContact firstContact = ObjectContact.None;

            // Left side contact
            if (memberBorders.Left <= 0)
            { 
                firstContact = ObjectContact.Left;
                MemberObject.Margin = new Thickness(1, memberTop, 0, 0);             
            }

            // Top contact
            if (firstContact == ObjectContact.None && memberBorders.Top <= 0)
            {
                firstContact = ObjectContact.Top;
                MemberObject.Margin = new Thickness(memberLeft, 1, 0, 0);
            }

            // Right side contact
            if (firstContact == ObjectContact.None && (memberRight >= workingCanvas.ActualWidth))
            { 
                firstContact = ObjectContact.Right;
                MemberObject.Margin = new Thickness((workingCanvas.ActualWidth - MemberObject.Width - 1), memberTop, 0, 0);
            }

            // Bottom contact
            if (firstContact == ObjectContact.None && (memberBottom >= workingCanvas.ActualHeight))
            {
                firstContact = ObjectContact.Bottom;
                MemberObject.Margin = new Thickness(memberLeft, (workingCanvas.ActualHeight - MemberObject.Height - 1), 0, 0);
            }

            return firstContact;
        }

        private void MemberBounce(Shape memberObject, ObjectContact ContactEdge)
        {
            MemberStats memberVitals = (MemberStats)memberObject.Tag;

            // Determine which edge of the object impacted and change its direction as needed.
            switch (ContactEdge)
            {
                case ObjectContact.Left:
                    memberVitals.XDirect = Math.Abs(memberVitals.XDirect);
                    break;
                case ObjectContact.Right:
                    memberVitals.XDirect = Math.Abs(memberVitals.XDirect) * (-1);
                    break;
                case ObjectContact.Top:
                    memberVitals.YDirect = Math.Abs(memberVitals.YDirect);
                    break;
                case ObjectContact.Bottom:
                    memberVitals.YDirect = Math.Abs(memberVitals.YDirect) * (-1);
                    break;
            }
        }
    }
}
