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
            DispatcherTimer NewMemberClock = new DispatcherTimer();
            NewMemberClock.Tick += new EventHandler(NewMemberClock_Tick);
            NewMemberClock.Interval = new TimeSpan(0, 0, 0, 3);
            NewMemberClock.Start();
            //FieldCanvas.Children.Add(members.CreateEllipseObject());

        }

        private void NewMemberClock_Tick(object sender, EventArgs e)
        {
            // Generate a new member and add it to the canvas.
            FieldCanvas.Children.Add(members.CreateEllipseObject());
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
