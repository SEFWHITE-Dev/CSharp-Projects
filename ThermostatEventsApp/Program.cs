

using System.ComponentModel;

namespace ThermostatEventsApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start device");
            Console.ReadKey();

            IDevice device = new Device();

            device.RunDevice();

            Console.ReadKey();
        }
    }


    public class Device : IDevice
    {
        const double WarningLevel = 27;
        const double EmergencyLevel = 75;

        public double WarningTempLevel => WarningLevel;

        public double EmergencyTempLevel => EmergencyLevel;

        private void ShutDownDevice()
        {
            Console.WriteLine("Device shutting down.");
        }

        public void HandleEmergency()
        {
            Console.WriteLine();
            Console.WriteLine("Sending out notifications to emergency services personel.");
            ShutDownDevice();
            Console.WriteLine();
        }

        public void RunDevice()
        {
            Console.WriteLine("Device is running...");
            ICoolingMechanism coolingMechanism = new CoolingMechanism();
            IHeatSensor heatSensor = new HeatSensor(WarningLevel, EmergencyLevel);
            IThermostat thermostat = new Thermostat(this, heatSensor, coolingMechanism);

            thermostat.RunThermostat();
        }
    }


    public class Thermostat: IThermostat
    {
        private ICoolingMechanism _coolingMechanism = null;
        private IHeatSensor _heatSensor = null;
        private IDevice _device = null;

        public Thermostat(IDevice device, IHeatSensor heatSensor, ICoolingMechanism coolingMechanism)
        {
            _device = device;
            _coolingMechanism = coolingMechanism;
            _heatSensor = heatSensor;
        }

        private void WireUpEventsToEventHandlers()
        {
            _heatSensor.TempretureReachesWarningLevelEventHandler += HeatSensor_TempretureReachesWarningLevelEventHandler;
            _heatSensor.TempretureReachesEmergencyLevelEventHandler += HeatSensor_TempretureReachesEmergencyLevelEventHandler;
            _heatSensor.TempretureFallsBelowWarningLevelEventHandler += HeatSensor_TempretureFallsBelowWarningLevelEventHandler;
        }

        private void HeatSensor_TempretureFallsBelowWarningLevelEventHandler(object? sender, TempretureEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine($"Information Alert!! Temp falls below warning level. (Warning level is between {_device.WarningTempLevel} and {_device.EmergencyTempLevel})");
            _coolingMechanism.Off();
            Console.ResetColor();
        }

        private void HeatSensor_TempretureReachesEmergencyLevelEventHandler(object? sender, TempretureEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine($"Emergency Alert!! (Emergency level is {_device.EmergencyTempLevel} and above.)");
            _device.HandleEmergency();
            Console.ResetColor();
        }

        private void HeatSensor_TempretureReachesWarningLevelEventHandler(object? sender, TempretureEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine($"Warning Alert!! (Warning level is between {_device.WarningTempLevel} and {_device.EmergencyTempLevel})");
            _coolingMechanism.On();
            Console.ResetColor();
        }

        public void RunThermostat()
        {
            Console.WriteLine("Thermostat is running...");
            WireUpEventsToEventHandlers();
            _heatSensor.RunHeatSensor();
        }
    }


    public interface IThermostat
    {
        void RunThermostat();
    }


    public interface IDevice
    {
        double WarningTempLevel { get; }
        double EmergencyTempLevel { get; }

        void RunDevice();
        void HandleEmergency();
    }


    public class CoolingMechanism : ICoolingMechanism
    {
        public void Off()
        {
            Console.WriteLine();
            Console.WriteLine("Switching cooling mechanism OFF...");
            Console.WriteLine();
        }

        public void On()
        {
            Console.WriteLine();
            Console.WriteLine("Switching cooling mechanism ON.");
            Console.WriteLine();
        }
    }

    public interface ICoolingMechanism
    {
        void On();
        void Off();
    }

    public class HeatSensor : IHeatSensor
    {

        double _warningLevel = 0;
        double _emergencyLevel = 0;

        bool _hasReachedWarningTemp = false;

        // stores delegates as key-value pair
        protected EventHandlerList _listEventDelegates = new EventHandlerList();

        static readonly object _tempReachesWarningLevelKey = new object();
        static readonly object _tempReachesEmergencyLevelKey = new object();
        static readonly object _tempFallsBelowWarningLevelKey = new object();

        // simulate heat sensor output
        private double[] _tempData = null;


        public HeatSensor(double warningLevel, double emergencyLevel)
        {
            _warningLevel = warningLevel;
            _emergencyLevel = emergencyLevel;

            SeedData();
        }


        private void MonitorTemp()
        {
            foreach (double temp in _tempData)
            {
                Console.ResetColor();
                Console.WriteLine($"DateTime: {DateTime.Now}, Temp: {temp}");

                if (temp >= _emergencyLevel)
                {
                    TempretureEventArgs e = new TempretureEventArgs
                    {
                        Tempreture = temp,
                        CurrentDateTime = DateTime.Now
                    };
                    OnTempReachesEmergencyLevel(e);
                }
                else if (temp >= _warningLevel) 
                {
                    _hasReachedWarningTemp = true;
                    TempretureEventArgs e = new TempretureEventArgs
                    {
                        Tempreture = temp,
                        CurrentDateTime = DateTime.Now
                    };
                    OnTempReachesWarningLevel(e);
                }
                else if (temp < _warningLevel && _hasReachedWarningTemp)
                {
                    _hasReachedWarningTemp = false;
                    TempretureEventArgs e = new TempretureEventArgs
                    {
                        Tempreture = temp,
                        CurrentDateTime = DateTime.Now
                    };
                    OnTempFallsBelowWarningLevel(e);
                }

                // add 1 sec delay
                System.Threading.Thread.Sleep(1000);
            }
        }


        private void SeedData()
        {
            _tempData = new double[] {12,14,55.6,16,172,16.5,16,21,20,22,23,24,25,25.6,26.5,26.7,26.8,25.8,23.4,25.6,67.2,45.1, 12, 22, 44, 23, 78, 34, 32, 23, 44, 12, 21};
        }


        // TempretureEventArgs class is responsible for storing data
        protected void OnTempReachesWarningLevel(TempretureEventArgs e)
        {
            // when assinging the value stored in the _obj to the handler var, must be type casted to the appropriate delegate type(event handler)
            EventHandler<TempretureEventArgs> handler = (EventHandler<TempretureEventArgs>)_listEventDelegates[_tempReachesWarningLevelKey];

            // check if client code has subscribed to the event
            if (handler != null) {
                // this = current obj, HeatSensor type
                // e = contains relevant event info, TempretureEventArgs type
                handler(this, e); 
            }
        }

        protected void OnTempReachesEmergencyLevel(TempretureEventArgs e)
        {            
            EventHandler<TempretureEventArgs> handler = (EventHandler<TempretureEventArgs>)_listEventDelegates[_tempReachesEmergencyLevelKey];

            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnTempFallsBelowWarningLevel(TempretureEventArgs e)
        {
            EventHandler<TempretureEventArgs> handler = (EventHandler<TempretureEventArgs>)_listEventDelegates[_tempFallsBelowWarningLevelKey];

            if (handler != null)
            {
                handler(this, e);
            }
        }



        event EventHandler<TempretureEventArgs> IHeatSensor.TempretureReachesEmergencyLevelEventHandler
        {
            // fires when the client subscribes to the event
            // add the appropriate delegate to the EventListHandler collection
            add
            {
                _listEventDelegates.AddHandler(_tempReachesEmergencyLevelKey, value);
            }
            // fires when the client UNsubscribes to the event
            remove
            {
                _listEventDelegates.RemoveHandler(_tempReachesEmergencyLevelKey, value);
            }
        }

        event EventHandler<TempretureEventArgs> IHeatSensor.TempretureReachesWarningLevelEventHandler
        {
            add
            {
                _listEventDelegates.AddHandler(_tempReachesWarningLevelKey, value);
            }

            remove
            {
                _listEventDelegates.RemoveHandler(_tempReachesWarningLevelKey, value);
            }
        }

        event EventHandler<TempretureEventArgs> IHeatSensor.TempretureFallsBelowWarningLevelEventHandler
        {
            add
            {
                _listEventDelegates.AddHandler(_tempFallsBelowWarningLevelKey, value);
            }

            remove
            {
                _listEventDelegates.RemoveHandler(_tempFallsBelowWarningLevelKey, value);
            }
        }

        public void RunHeatSensor()
        {
            Console.WriteLine("Heat sensor is running...");
            MonitorTemp();
        }
    }

    public interface IHeatSensor
    {
        // TempretureEventArgs used to store event info as a generic parameter to the EventHandler delegate
        event EventHandler<TempretureEventArgs> TempretureReachesEmergencyLevelEventHandler;
        event EventHandler<TempretureEventArgs> TempretureReachesWarningLevelEventHandler;
        event EventHandler<TempretureEventArgs> TempretureFallsBelowWarningLevelEventHandler;

        // method definition for heat sensor class
        void RunHeatSensor();
    }

    // Responsible for storing event data related to the event
    public class TempretureEventArgs:EventArgs
    {
        public double Tempreture { get; set; }
        public DateTime CurrentDateTime { get; set; }
    }
}
