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
        long start_interval, end_interval, interval;
        float distance;
        //int i = 0;
        //int interval[10];
        bool first;

        private GT.Timer timer = new GT.Timer(3000);
                
        public static GT.SocketInterfaces.DigitalOutput trigger;
        public static GT.SocketInterfaces.DigitalInput echo;
     
        

        void ProgramStarted()
        {
            trigger = breakout.CreateDigitalOutput(GT.Socket.Pin.Four, false); //trigger down
            echo = breakout.CreateDigitalInput(GT.Socket.Pin.Five, GT.SocketInterfaces.GlitchFilterMode.Off, GT.SocketInterfaces.ResistorMode.Disabled);        //echo
            trigger.Write(false);
            first = false;
            
            GT.Timer timer = new GT.Timer(3000);
            timer.Tick += timer_Tick;
            timer.Start();
            
            Debug.Print("Program Started");
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

            
        

        void timer_Tick(GT.Timer timer) //function called at each occurrency of the timer (10 s)
        {
            Debug.Print("timer occurred");
            first = true;
            trigger.Write(true);
            Thread.Sleep(500);
            trigger.Write(false);
            Debug.Print("timer occurred 2");
            //echo.Interrupt += echo_Interrupt; //to activate interrupt
            while (echo.Read() == false) ; // attendo che vada a 1
            start_interval = Microsoft.SPOT.Hardware.Utility.GetMachineTime().Ticks;
            while (echo.Read() == true) ; // attendo che torni a 0
            end_interval = Microsoft.SPOT.Hardware.Utility.GetMachineTime().Ticks;
            Debug.Print("start interval: " + start_interval);
            Debug.Print("end interval time: " + end_interval);
            interval = end_interval - start_interval;   // in ms
            Debug.Print("interval: " + interval);


            
            
        }


    }
}
