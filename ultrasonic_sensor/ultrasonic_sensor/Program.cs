using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace ultrasonic_sensor
{
    public partial class Program
    {
        int start_interval, end_interval, interval;
        double distance = 0;
        //int i = 0;
        //int interval[10];
        bool first;

        private GT.Timer timer = new GT.Timer(10000);
        private GT.SocketInterfaces.InterruptInput echo;
        
        public static GT.SocketInterfaces.DigitalOutput trigger;
        //private System.TimeSpan timer2 = new System.TimeSpan();
        private TimeSpan timer2 = GT.Timer.GetMachineTime();
        

        void ProgramStarted()
        {

            trigger = breakout.CreateDigitalOutput(GT.Socket.Pin.Four, false); //trigger down
            echo = breakout.CreateInterruptInput(GT.Socket.Pin.Five, GT.SocketInterfaces.GlitchFilterMode.Off, GT.SocketInterfaces.ResistorMode.Disabled, GT.SocketInterfaces.InterruptMode.RisingAndFallingEdge);        //echo

            trigger.Write(false);
            first = false;
            
            GT.Timer timer = new GT.Timer(10000);
            timer.Tick += timer_Tick;
            timer.Start();

            Debug.Print("Program Started");
        }


        void echo_Interrupt(GT.SocketInterfaces.InterruptInput sender, bool value)
        {
            Debug.Print("interrupt occurred");
            if (first == true)
            {
                start_interval = timer2.Milliseconds;
                Debug.Print("time1: "+ start_interval);

                first = false;
            }
            else
            {
                end_interval = timer2.Milliseconds;
                Debug.Print("time2: " + end_interval);
                interval = end_interval - start_interval;   // in ms
                distance = 0.034 * (interval / 2);           // in cm       (or also 0,01715*interval)
                if (interval > 38000)   //out of range
                {
                    interval = -1;  //remove this if with the average mode
                }

                Debug.Print("Distance: " + distance + " cm");
            }

            // UPGRADE CODE WITH AVERAGE
            //interval[i] = end_interval - start_interval;
            //i++;
            //if(if==10)
            //{ 
            //  for(int j=0; J<10; j++)
            //      {
            //          mean_interval=mean_interval+interval[i];
            //}          
        }

        void timer_Tick(GT.Timer timer) //function called at each occurrency of the timer (10 s)
        {
            Debug.Print("timer occurred");
            first = true;
            trigger.Write(true);
            Thread.Sleep(500);
            trigger.Write(false);
            Debug.Print("timer occurred 2");
            echo.Interrupt += echo_Interrupt;
         
        }

        //comment by simosilva
        //comment by mattispring

    }
}
