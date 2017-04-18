using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Population
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MemberOps members;
        int memberCount = 0;
        DispatcherTimer NewMemberClock;
        public MainWindow()
        {
            InitializeComponent();

            // Create new instance of class to manage member operations.
            members = new MemberOps(FieldCanvas);

            // Create primary timer to move objects.
            DispatcherTimer PrimaryClock = new DispatcherTimer();
            PrimaryClock.Tick += new EventHandler(PrimaryClock_Tick);
            PrimaryClock.Interval = new TimeSpan(0, 0, 0, 0, 25);
            PrimaryClock.Start();

            // Create timer to generate new member every few seconds.
            NewMemberClock = new DispatcherTimer();
            NewMemberClock.Tick += new EventHandler(NewMemberClock_Tick);
            NewMemberClock.Interval = new TimeSpan(0, 0, 0, 3);
            NewMemberClock.Start();
            //FieldCanvas.Children.Add(members.CreateEllipseObject());

        }

        private void NewMemberClock_Tick(object sender, EventArgs e)
        {
            // Generate a new member and add it to the canvas.
            FieldCanvas.Children.Add(CreateEllipseObject());
            memberCount++;
            //if (memberCount > 1)
            //    NewMemberClock.Stop();
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
            newMember.Width = 40;
            newMember.Height = 40;
            Canvas.SetLeft(newMember, 1d);
            Canvas.SetTop(newMember, 1d);
            newMember.Fill = new RadialGradientBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"),
                (Color)ColorConverter.ConvertFromString("#" + colorHex));

            // Add MemberStats object to ellipse tag to store directional and health values.
            MemberStats MemberVitals = new MemberStats(50, directionGen.Next(1, 5), directionGen.Next(1, 5));
            newMember.Tag = MemberVitals;

            // Add ToolTip to ellipse with health stat.
            newMember.ToolTip = "Health: " + MemberVitals.HealthPoints.ToString();

            // If the Settings panel requires solid members, make it solid.
            if (members.SolidMembers)
                MemberVitals.Solid = true;

            return newMember;
        }

        private void PrimaryClock_Tick(object sender, EventArgs e)
        {

            // When the primary timer fires, iterate through the canvas collection
            // and move each ellipse one step.

            foreach (UIElement uiObject in FieldCanvas.Children)
            {
                Type t = uiObject.GetType();

                if(t == typeof(Ellipse))
                {
                    Ellipse currEllipse = (Ellipse)uiObject;
                    members.MoveMember(currEllipse);

                }
            }
        }
    }
}
