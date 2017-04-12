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
        public enum EdgeContact { Left, Top, Right, Bottom, None};

        // Class requires a reference to the canvas for current height and width.
        private Canvas workingCanvas;


        public MemberOps(Canvas CurrentCanvas)
        {
            // Primary constructor
            workingCanvas = CurrentCanvas;
            ControlPanel settingsPanel = new ControlPanel();
            settingsPanel.Show();
        }

        public Ellipse CreateEllipseObject()
        {
            // Creates a new Ellipse object for addition to the field collection.

            // Random values for direction and ellipse color.
            Random directionGen = new Random(System.DateTime.Now.Millisecond);
            Random colorGen = new Random(System.DateTime.Now.Millisecond);
            string colorHex = colorGen.Next(1048576, 16777215).ToString("X");

            // Create new ellipse and place it in the top left of the canvas.
            Ellipse newMember = new Ellipse();
            newMember.Opacity = 1;
            newMember.Width = 50;
            newMember.Height = 50;
            Canvas.SetLeft(newMember, 1d);
            Canvas.SetTop(newMember, 1d);
            newMember.Fill = new RadialGradientBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"), 
                (Color)ColorConverter.ConvertFromString("#" + colorHex));

            // Add MemberStats object to ellipse tag to store directional and health values.
            MemberStats MemberVitals = new MemberStats(50, directionGen.Next(1,5), directionGen.Next(1, 5));                        
            newMember.Tag = MemberVitals;

            // Add ToolTip to ellipse with health stat.
            newMember.ToolTip = "Health: " + MemberVitals.HealthPoints.ToString();

            return newMember;
        }

        public void MoveMember(Ellipse MemberObject)
        {
            // Get vital stats dictionary
            Type t = MemberObject.Tag.GetType();
            MemberStats memberVitals;
            double leftMargin, topMargin;
            EdgeContact contactEdge;

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
                if (contactEdge != EdgeContact.None)
                {
                    MemberBounce(MemberObject, contactEdge);
                }
            }
        }

        private void MemberBounce(Ellipse memberObject, EdgeContact ContactEdge)
        {
            MemberStats memberVitals = (MemberStats)memberObject.Tag;

            // Determine which edge of the object impacted and change its direction as needed.
            switch(ContactEdge)
            {
                case EdgeContact.Left:
                    memberVitals.XDirect = Math.Abs(memberVitals.XDirect);
                    break;
                case EdgeContact.Right:
                    memberVitals.XDirect = Math.Abs(memberVitals.XDirect) * (-1);
                    break;
                case EdgeContact.Top:
                    memberVitals.YDirect = Math.Abs(memberVitals.YDirect);
                    break;
                case EdgeContact.Bottom:
                    memberVitals.YDirect = Math.Abs(memberVitals.YDirect) * (-1);
                    break;
            }
        }

        public EdgeContact EdgeDetect(Ellipse MemberObject)
        {
            // Get the margin values for the object.
            Thickness memberBorders = MemberObject.Margin;
            double memberLeft = memberBorders.Left;
            double memberTop = memberBorders.Top;
            double memberRight = (memberBorders.Left + MemberObject.Width);
            double memberBottom = (memberBorders.Top + MemberObject.Height);
            MemberStats MemberVitals = (MemberStats)MemberObject.Tag;

            EdgeContact firstContact = EdgeContact.None;

            // Left side contact
            if (memberBorders.Left <= 0)
            { 
                firstContact = EdgeContact.Left;
                MemberObject.Margin = new Thickness(1, memberTop, 0, 0);             
            }

            // Top contact
            if (firstContact == EdgeContact.None && memberBorders.Top <= 0)
            {
                firstContact = EdgeContact.Top;
                MemberObject.Margin = new Thickness(memberLeft, 1, 0, 0);
            }

            // Right side contact
            if (firstContact == EdgeContact.None && (memberRight >= workingCanvas.ActualWidth))
            { 
                firstContact = EdgeContact.Right;
                MemberObject.Margin = new Thickness((workingCanvas.ActualWidth - MemberObject.Width - 1), memberTop, 0, 0);
            }

            // Bottom contact
            if (firstContact == EdgeContact.None && (memberBottom >= workingCanvas.ActualHeight))
            {
                firstContact = EdgeContact.Bottom;
                MemberObject.Margin = new Thickness(memberLeft, (workingCanvas.ActualHeight - MemberObject.Height - 1), 0, 0);
            }

            return firstContact;
        }
    }
}
