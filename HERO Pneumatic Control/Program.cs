/**
 * Make the PCM go clicky
 */
using System;
using System.Threading;
using Microsoft.SPOT;
using System.Text;

using CTRE;
using CTRE.Phoenix;
using CTRE.Phoenix.Controller;
using CTRE.Phoenix.MotorControl;
using CTRE.Phoenix.MotorControl.CAN;

namespace HERO_Pneumatic_Control
{
    /** Simple stub to start our project */
    public class Program
    {
        static RobotApplication _robotApp = new RobotApplication();
        public static void Main()
        {
            while (true)
            {
                _robotApp.Run();
            }
        }
    }
    /**
     * The custom robot application.
     */
    public class RobotApplication
    {
        /** make a talon with PCM device ID 0 */
        PneumaticControlModule _PCM = new PneumaticControlModule(0);

        /** Use a USB gamepad plugged into the HERO */
        GameController _gamepad = new GameController(UsbHostDevice.GetInstance());

        /** hold the current button values from gamepad*/
        bool[] _btns = new bool[10];

        /** hold the last button values from gamepad, this makes detecting on-press events trivial */
        bool[] _btnsLast = new bool[10];

        public void Run()
        {
            /* loop forever */
            while (true)
            {
                Loop10Ms();

                //if (_gamepad.GetButton(kEnableButton)) // check if bottom left shoulder buttom is held down.
                if (_gamepad.GetConnectionStatus() == CTRE.Phoenix.UsbDeviceConnection.Connected) // check if gamepad is plugged in OR....
                {
                    /* then enable motor outputs*/
                    //Debug.Print("Watchdog fed");
                    Watchdog.Feed();

                    //Start the Compressor if the Pressure switch is triggered, GetPressureSwitchValue returns true if pressure if low
                    if (_PCM.GetPressureSwitchValue())
                    {
                        _PCM.StartCompressor();
                        Debug.Print("Compressor Current: " + _PCM.GetCompressorCurrent());
                    }
                    else
                    {
                        _PCM.StopCompressor();
                        Debug.Print("Compressor Current: " + _PCM.GetCompressorCurrent());
                    }
                }

                /* 10ms loop */
                Thread.Sleep(10);
            }
        }

        void Loop10Ms()
        {
            /* get all the buttons */
            FillBtns(ref _btns);

            //Not sure what button 0 on the Logitech F310 is
            if (_btns[0])
            {
                Debug.Print("0");
            }
            else if(!_btns[0])
            {
                //Button Zero is no longer pressed
            }

            //X Button on the F310
            if (_btns[1])
            {
                Debug.Print("1");
                _PCM.SetSolenoidOutput(0, true);
            }
            else if (!_btns[1])
            {
                _PCM.SetSolenoidOutput(0, false);
            }

            //A Button on the F310
            if (_btns[2])
            {
                Debug.Print("2");
                _PCM.SetSolenoidOutput(1, true);
            }
            else if (!_btns[2])
            {
                _PCM.SetSolenoidOutput(1, false);
            }

            //B Button on the F310
            if (_btns[3])
            {
                Debug.Print("3");
                _PCM.SetSolenoidOutput(2, true);
            }
            else if (!_btns[3])
            {
                _PCM.SetSolenoidOutput(2, false);
            }

            //Y Button on the F310
            if (_btns[4])
            {
                Debug.Print("4");
                _PCM.SetSolenoidOutput(3, true);
            }
            else if (!_btns[4])
            {
                _PCM.SetSolenoidOutput(3, false);
            }

            //LB Button on the F310
            if (_btns[5])
            {
                Debug.Print("5");
                _PCM.SetSolenoidOutput(4, true);
            }
            else if (!_btns[5])
            {
                _PCM.SetSolenoidOutput(4, false);
            }

            //RB Button on the F310
            if (_btns[6])
            {
                Debug.Print("6");
                _PCM.SetSolenoidOutput(5, true);
            }
            else if (!_btns[6])
            {
                _PCM.SetSolenoidOutput(5, false);
            }

            //LT Button on the F310
            if (_btns[7])
            {
                Debug.Print("7");
                _PCM.SetSolenoidOutput(6, true);
            }
            else if (!_btns[7])
            {
                _PCM.SetSolenoidOutput(6, false);
            }

            //RT Button on the F310
            if (_btns[8])
            {
                Debug.Print("8");
                _PCM.SetSolenoidOutput(7, true);
            }
            else if (!_btns[8])
            {
                _PCM.SetSolenoidOutput(7, false);
            }

            //Back Button on the F310
            if (_btns[9])
            {
                Debug.Print("9");
            }
            else if (!_btns[9])
            {
                //Debug.Print("9");
            }
          
            /* copy btns => btnsLast */
            System.Array.Copy(_btns, _btnsLast, _btns.Length);
        }

        /* throw all the gamepad buttons into an array */
        void FillBtns(ref bool[] btns)
        {
            for (uint i = 1; i < btns.Length; ++i)
                btns[i] = _gamepad.GetButton(i);
        }
    }
}