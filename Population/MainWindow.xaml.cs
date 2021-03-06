﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

            try
            {
                // Create new instance of class to manage member operations.
                members = new MemberOps(FieldCanvas);

                // Create primary timer to move objects.
                DispatcherTimer PrimaryClock = new DispatcherTimer();
                PrimaryClock.Tick += new EventHandler(PrimaryClock_Tick);
                PrimaryClock.Interval = new TimeSpan(0, 0, 0, 0, 30);
                PrimaryClock.Start();

                // Create timer to generate new member at specified interval.
                NewMemberClock = new DispatcherTimer();
                NewMemberClock.Tick += new EventHandler(NewMemberClock_Tick);
                NewMemberClock.Interval = new TimeSpan(0, 0, 0, 0, (int)(members.NewMemberInterval * 1000));
                NewMemberClock.Start();
                //FieldCanvas.Children.Add(members.CreateEllipseObject());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }
        }

        private void NewMemberClock_Tick(object sender, EventArgs e)
        {
            try {
                if (members.IsRunning)
                {
                    // Generate a new member and add it to the canvas.
                    FieldCanvas.Children.Add(CreateEllipseObject());
                    memberCount++;
                    // Listen for any changes to the interval from the control panel.
                    NewMemberClock.Interval = new TimeSpan(0, 0, 0, 0, (int)(members.NewMemberInterval * 1000));
                    //if (memberCount > 1)
                    //    NewMemberClock.Stop();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }
        }

        public Ellipse CreateEllipseObject()
        {
            // Creates a new Ellipse object for addition to the field collection.

            // Random values for direction and ellipse color.
            Random directionGen = new Random(System.DateTime.Now.Millisecond);
            Random colorGen = new Random(System.DateTime.Now.Millisecond);
            Ellipse newMember = new Ellipse();
            MemberStats MemberVitals;

            try
            {
                // Create new ellipse and place it in the top left of the canvas.
                newMember.Opacity = 1;
                newMember.Width = 40;
                newMember.Height = 40;
                newMember.Name = "z" + System.DateTime.Now.Ticks.ToString();

                Canvas.SetLeft(newMember, 1d);
                Canvas.SetTop(newMember, 1d);

                // Add MemberStats object to ellipse tag to store directional and health values.
                MemberVitals = new MemberStats(255, directionGen.Next(1, 5), directionGen.Next(1, 5));
                newMember.Tag = MemberVitals;
                newMember.Fill = new RadialGradientBrush(Color.FromRgb(255, 255, 255), members.GenerateMemberColor(MemberVitals.HealthPoints));
                // Add ToolTip to ellipse with health stat.
                // If the Settings panel requires solid members, make it solid.
                if (members.SolidMembers)
                    MemberVitals.Solid = true;
                newMember.ToolTip = "Health: " + MemberVitals.HealthPoints;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }

            return newMember;
        }

        private void ClearMembers()
        {
            // Clear all member objects from canvas children collection.

            try
            {
                for (int idx = FieldCanvas.Children.Count - 1; idx >= 0; idx--)
                {
                    UIElement uiObject = FieldCanvas.Children[idx];

                    Type t = uiObject.GetType();

                    if (t == typeof(Ellipse))
                    {
                        FieldCanvas.Children.RemoveAt(idx);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }
        }


        private void PrimaryClock_Tick(object sender, EventArgs e)
        {

            // When the primary timer fires, iterate through the canvas collection,
            // remove "dead" members and move each ellipse one step.

            try
            {
                if (members.IsRunning)
                {
                    // If Clear Mode is on, clear the collection.

                    if (members.ClearAllMembers)
                    {
                        ClearMembers();
                        memberCount = 0;
                        members.ClearAllMembers = false;
                    }

                    // Remove members with no health points left by iterating backwards through the
                    // collection.

                    for (int idx = FieldCanvas.Children.Count - 1; idx >= 0; idx--)
                    {
                        UIElement uiObject = FieldCanvas.Children[idx];

                        Type t = uiObject.GetType();

                        if (t == typeof(Ellipse))
                        {
                            Ellipse currEllipse = (Ellipse)uiObject;
                            MemberStats stats = members.GetMemberStats(currEllipse);
                            if (stats != null && !stats.Alive)
                            {
                                // Fade out and then remove from collection.
                                currEllipse.Opacity -= .025;
                                if (currEllipse.Opacity < .05)
                                {
                                    FieldCanvas.Children.RemoveAt(idx);
                                    memberCount--;
                                }
                            }

                        }
                    }

                    // Iterate through the collection and move each live member.

                    foreach (UIElement uiObject in FieldCanvas.Children)
                    {
                        Type t = uiObject.GetType();

                        if (t == typeof(Ellipse))
                        {
                            Ellipse currEllipse = (Ellipse)uiObject;
                            if (members.GetMemberStats(currEllipse).Alive)
                            {
                                members.MoveMember(currEllipse);
                            }
                        }
                    }

                    // Update screen stats.

                    this.Title = "BumperCars - Count: " + memberCount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error ...");
            }



        }
    }
}
