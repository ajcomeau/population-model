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
        public enum ObjectContact { Left, TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, None };

        // Program constants
        public const int MAX_JUMP = 5;              // Maximum distance to travel in one step.
        public const int BOUNCE_DIST = 3;           // Amount of recoil when hitting something.
        public const int COLLISION_MEMBER = -15;    // Health affect for hitting another member.
        public const int COLLISION_WALL = 50;       // Health affect for touching wall.
        public const int HEALTH_PER_STEP = 0;       // Health affect for each step.
        public const int BALL_SIZE = 40;            // Ball radius

        // Class requires a reference to the canvas for current height and width.
        private Canvas workingCanvas;
        ControlPanel settingsPanel;

        // Values returned from settings panel.
        private bool vSolidMembers;
        private bool vSolidAllMembers;
        private bool vRunning;
        private bool vClearAll;

        // Properties for settings panel values.
        public bool SolidMembers
        {
            get { return vSolidMembers; }
        }

        public bool AllMembersSolid
        {
            get { return vSolidAllMembers; }
        }

        public bool IsRunning
        {
            get { return vRunning; }
        }

        public bool ClearAllMembers
        {
            get { return vClearAll; }
            set { vClearAll = value; }
        }

        public MemberOps(Canvas CurrentCanvas)
        {
            // Primary constructor
            workingCanvas = CurrentCanvas;
            settingsPanel = new ControlPanel();
            SettingsEventWiring();
            settingsPanel.Show();
            settingsPanel.TopMost = true;
            this.vRunning = true;
        }

        private void SettingsEventWiring()
        {
            // Link each event from the control panel to an event handler.
            settingsPanel.chkSolidChangedEvent += SettingsPanel_chkSolidChangedEvent;
            settingsPanel.chkSolidAllChangedEvent += SettingsPanel_chkSolidAllChangedEvent;
            settingsPanel.runStatusChangeEvent += SettingsPanel_runStatusChangeEvent;
            settingsPanel.clearButtonClickEvent += SettingsPanel_clearButtonClickEvent;
            settingsPanel.exitButtonClickEvent += SettingsPanel_exitButtonClickEvent;
        }

        private void SettingsPanel_exitButtonClickEvent(object sender, EventArgs e)
        {
            // Shutdown the program.
            Application.Current.Shutdown();
        }

        private void SettingsPanel_clearButtonClickEvent(object sender, EventArgs e)
        {
            // Turn on clear mode.
            ClearAllMembers = true;
        }

        private void SettingsPanel_runStatusChangeEvent(bool RunStatus)
        {
            this.vRunning = RunStatus;
        }

        private void SettingsPanel_chkSolidAllChangedEvent(bool AllMembersSolid)
        {
            this.vSolidAllMembers = AllMembersSolid;
        }

        private void SettingsPanel_chkSolidChangedEvent(bool MembersSolid)
        {
            this.vSolidMembers = MembersSolid;
        }

        public MemberStats GetMemberStats(Shape MemberObject)
        {
            MemberStats returnStats = null;
            Type t = null;

            if (MemberObject.Tag != null)
            {
                 t = MemberObject.Tag.GetType();
            }

            if (t != null && t == typeof(MemberStats))
            {
                returnStats = (MemberStats)MemberObject.Tag;
            }

            return returnStats;
        }



        public void MoveMember(Shape MemberObject)
        {
            // Get vital stats dictionary
            MemberStats memberStats = GetMemberStats(MemberObject);
            double leftMargin, topMargin;
            ObjectContact contactEdge;

            // Verify presence of member stats.
            if (memberStats != null){

                // Update color based on health.
                MemberObject.Fill = new RadialGradientBrush(Color.FromRgb(255, 255, 255), 
                    GenerateMemberColor(memberStats.HealthPoints));

                // Update solid setting.
                if (!memberStats.Solid && AllMembersSolid)
                {
                    memberStats.Solid = true;
                }

                // Determine next position by adding information from stats object to current position.
                leftMargin = MemberObject.Margin.Left + memberStats.XDirect;
                topMargin = MemberObject.Margin.Top + memberStats.YDirect;
                MemberObject.Margin = new Thickness(leftMargin, topMargin, 0, 0);

                // Scan for collisions with another object.
                ScanForCollisions(MemberObject);

                // Check for contact with edge and bounce.
                contactEdge = EdgeDetect(MemberObject);
                if (contactEdge != ObjectContact.None)
                {
                    memberStats.HealthPoints += COLLISION_WALL;
                    MemberBounce(MemberObject, contactEdge);
                }

                // Add any health per step value.
                memberStats.HealthPoints += HEALTH_PER_STEP;

                // Update ToolTip
                MemberObject.ToolTip = "Health: " + memberStats.HealthPoints;
            }
        }

        public void ScanForCollisions(Shape MovingShape)
        {

            ObjectContact collideDetect = ObjectContact.None;
            MemberStats memberVitals = GetMemberStats(MovingShape);

            foreach (UIElement uiObject in workingCanvas.Children)
            {
                // Iterate through collection of canvas objects to detect any collisions.
                if(uiObject != MovingShape)
                { 
                    Type t = uiObject.GetType();

                    // Look for shapes ...
                    if (t.UnderlyingSystemType.BaseType == typeof(Shape))
                    {
                        // Is this shape colliding with it?
                        Shape StaticShape = (Shape)uiObject;
                        collideDetect = CollisionDetect(MovingShape, StaticShape);

                        // If there is a collsion, bounce ...
                        if (collideDetect != ObjectContact.None)
                        {
                            memberVitals.HealthPoints += COLLISION_MEMBER;
                            MemberBounce(MovingShape, StaticShape, collideDetect);
                        }
                    }
                }
            }
        }

        public bool ShapeIsSolid(Shape MemberShape)
        {
            MemberStats ms = GetMemberStats(MemberShape);
            bool returnValue;

            // Check the settings of the MemberStats tag to 
            // find out if this shape is solid.

            if (ms != null)
                returnValue = ms.Solid;
            else
                returnValue = false;

            return returnValue;
        }

        public ObjectContact CollisionDetect(Shape MovingShape, Shape StaticShape)
        {
            bool collision;

            // Create rectangle from each object.
            Rect MovingShapeRect = new Rect(MovingShape.Margin.Left, MovingShape.Margin.Top, MovingShape.Width, MovingShape.Height);
            Rect StaticShapeRect = new Rect(StaticShape.Margin.Left, StaticShape.Margin.Top, StaticShape.Width, StaticShape.Height);
            ObjectContact contactPoint = ObjectContact.None;
            
            // Look for intersection between rectangles.
            collision = (MovingShapeRect.IntersectsWith(StaticShapeRect));

            // If there is an intersection and both shapes are supposed to be solid,
            // determine where the impact was.
            if (collision && ShapeIsSolid(MovingShape) && ShapeIsSolid(StaticShape))
            {
                if (StaticShapeRect.Contains(MovingShapeRect.TopLeft) || MovingShapeRect.Contains(StaticShapeRect.BottomRight))
                {
                    contactPoint = ObjectContact.TopLeft;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (StaticShapeRect.Contains(MovingShapeRect.TopRight) || MovingShapeRect.Contains(StaticShapeRect.BottomLeft)))
                {
                    contactPoint = ObjectContact.TopRight;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (StaticShapeRect.Contains(MovingShapeRect.BottomRight) || MovingShapeRect.Contains(StaticShapeRect.TopLeft)))
                {
                    contactPoint = ObjectContact.BottomRight;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (StaticShapeRect.Contains(MovingShapeRect.BottomLeft) || MovingShapeRect.Contains(StaticShapeRect.TopRight)))
                {
                    contactPoint = ObjectContact.BottomLeft;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (MovingShapeRect.Top <= StaticShapeRect.Bottom && MovingShapeRect.Top > StaticShapeRect.Top))
                {
                    contactPoint = ObjectContact.Top;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (MovingShapeRect.Bottom >= StaticShapeRect.Top && MovingShapeRect.Bottom < StaticShapeRect.Bottom))
                {
                    contactPoint = ObjectContact.Bottom;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (MovingShapeRect.Left <= StaticShapeRect.Right && MovingShapeRect.Left > StaticShapeRect.Left))
                {
                    contactPoint = ObjectContact.Left;
                }

                if ((contactPoint == ObjectContact.None) &&
                    (MovingShapeRect.Right >= StaticShapeRect.Left && MovingShapeRect.Right > StaticShapeRect.Right))
                {
                    contactPoint = ObjectContact.Right;
                }
            }

            // Return the impact point.
            return contactPoint;
        }


        public ObjectContact EdgeDetect(Shape MemberObject)
        {
            // Get the margin values for the object.
            Thickness memberBorders = MemberObject.Margin;
            double memberLeft = memberBorders.Left;
            double memberTop = memberBorders.Top;
            double memberRight = (memberBorders.Left + MemberObject.Width);
            double memberBottom = (memberBorders.Top + MemberObject.Height);

            ObjectContact firstContact = ObjectContact.None;

            // Test each side for contacts.

            // Left side contact
            if (memberBorders.Left <= 0)
            { firstContact = ObjectContact.Left; }

            // Top contact
            if (firstContact == ObjectContact.None && memberBorders.Top <= 0)
            { firstContact = ObjectContact.Top; }

            // Right side contact
            if (firstContact == ObjectContact.None && (memberRight >= workingCanvas.ActualWidth))
            { firstContact = ObjectContact.Right; }
            
            // Bottom contact
            if (firstContact == ObjectContact.None && (memberBottom >= workingCanvas.ActualHeight))
            { firstContact = ObjectContact.Bottom; }

            return firstContact;
        }

        private void MemberBounce(Shape memberObject, ObjectContact ContactEdge)
        {
            MemberStats memberVitals = GetMemberStats(memberObject);

            // Determine which edge of the object impacted and change its direction as needed.
            if (memberVitals != null)
            {
                switch (ContactEdge)
                {
                    case ObjectContact.Left:
                        memberVitals.XDirect = Math.Abs(memberVitals.XDirect);
                        memberObject.Margin = new Thickness(1, memberObject.Margin.Top, 0, 0);
                        break;
                    case ObjectContact.Right:
                        memberVitals.XDirect = Math.Abs(memberVitals.XDirect) * (-1);
                        memberObject.Margin = new Thickness((workingCanvas.ActualWidth - memberObject.Width - 1), memberObject.Margin.Top, 0, 0);
                        break;
                    case ObjectContact.Top:
                        memberVitals.YDirect = Math.Abs(memberVitals.YDirect);
                        memberObject.Margin = new Thickness(memberObject.Margin.Left, 1, 0, 0);
                        break;
                    case ObjectContact.Bottom:
                        memberVitals.YDirect = Math.Abs(memberVitals.YDirect) * (-1);
                        memberObject.Margin = new Thickness(memberObject.Margin.Left, (workingCanvas.ActualHeight - memberObject.Height - 1), 0, 0);
                        break;
                }
            }
        }

        private void MemberBounce(Shape movingObject, Shape staticObject, ObjectContact ContactEdge)
        {
            // Bounce on impact between two objects.
            MemberStats movingVitals = GetMemberStats(movingObject);
            MemberStats staticVitals = GetMemberStats(staticObject);
            Random randomAngle = new Random(System.DateTime.Now.Millisecond);

            // Determine which edge of the object impacted and change its direction or speed as needed.

            if (movingVitals != null && staticVitals != null)
            {
                switch (ContactEdge)
                {
                    case ObjectContact.TopLeft:
                        movingVitals.XDirect = Math.Abs(movingVitals.XDirect);
                        movingObject.Margin = new Thickness(movingObject.Margin.Left + BOUNCE_DIST, movingObject.Margin.Top + BOUNCE_DIST, 0, 0);
                        break;
                    case ObjectContact.TopRight:
                        movingVitals.XDirect = randomAngle.Next(1, MAX_JUMP) * -1;
                        movingObject.Margin = new Thickness(movingObject.Margin.Left - BOUNCE_DIST, movingObject.Margin.Top + BOUNCE_DIST, 0, 0);
                        break;
                    case ObjectContact.BottomLeft:
                        movingVitals.XDirect = Math.Abs(movingVitals.XDirect);
                        movingObject.Margin = new Thickness(movingObject.Margin.Left + BOUNCE_DIST, movingObject.Margin.Top - BOUNCE_DIST, 0, 0);
                        break;
                    case ObjectContact.BottomRight:
                        movingVitals.XDirect = randomAngle.Next(1, MAX_JUMP) * -1;
                        movingObject.Margin = new Thickness(movingObject.Margin.Left - BOUNCE_DIST, movingObject.Margin.Top - BOUNCE_DIST, 0, 0);
                        break;
                    case ObjectContact.Top:
                        movingVitals.YDirect = Math.Abs(movingVitals.YDirect);
                        break;
                    case ObjectContact.Bottom:
                        movingVitals.YDirect = randomAngle.Next(1, MAX_JUMP) * -1;
                        break;
                    case ObjectContact.Left:
                        movingVitals.XDirect = Math.Abs(movingVitals.XDirect);
                        break;
                    case ObjectContact.Right:
                        movingVitals.XDirect = randomAngle.Next(1, MAX_JUMP) * -1;
                        break;
                }
            }

        }

        public Color GenerateMemberColor(int HealthPoints)
        {
            Color returnValue;
            int redComponent = 0;
            int blueComponent = 0;
            int greenComponent = 0;
            int remaining = HealthPoints;

            // THERE'S PROBABLY A SHORTER WAY TO DO THIS INVOLVING A WHILE LOOP OR SOMETHING 
            // Subtract 255 at a time from the health points. Apply the points
            // first to the Blue, then Green, then Red.
            // This will make the color fade from White to Yellow to Red to Black.

            // Keep the number within certain limits.
            // If it's too bright or too dark, it doesn't look good.
            if (remaining > 700)
                remaining = 700;

            if (remaining < 225)
                remaining = 225;

            if (remaining > 510)
            {
                blueComponent = (remaining - 510);
                remaining -= blueComponent;
            }

            if(remaining > 255)
            {
                greenComponent = (remaining - 255);
                remaining -= greenComponent;
            }

            if(remaining > 0)
            {
                redComponent = remaining;
                remaining -= redComponent;
            }

            returnValue = Color.FromRgb((byte)redComponent, (byte)greenComponent, (byte)blueComponent);

            return returnValue;
        }
    }
}
