
//namespace Covarience_Contravarience_Delegates
//{
//    class Covarience
//    {
//        delegate Car CarFactoryDel(int id, string name);

//        static void Main(string[] args)
//        {

//            // delegate is assigned the method ReturnICECar
//            // Covariance allows this assignment because ICECar inherits from Car, making it a valid return type
//            CarFactoryDel carFactoryDel = CarFactory.ReturnICECar;

//            Car iceCar = carFactoryDel(1, "Audi R8");

//            Console.WriteLine($"Object type: {iceCar.GetType()}");
//            Console.WriteLine($"Car Details: {iceCar.GetCarDetails()}");


//            // the same delegate is assigned the method ReturnEVCar
//            // the same delegate can be reassigned at runtime
//            // Covariance allows this despite being of different return types, because they derive from the same type
//            carFactoryDel = CarFactory.ReturnEVCar;

//            Car evCar = carFactoryDel(2, "Honda Prius");

//            Console.WriteLine();

//            Console.WriteLine($"Object type: {evCar.GetType()}");
//            Console.WriteLine($"Car Details: {evCar.GetCarDetails()}");

//            Console.ReadKey();

//        }
//    }


//    public static class CarFactory
//    {
//        public static ICECar ReturnICECar(int id, string name)
//        {
//            return new ICECar { Id = id, Name = name };
//        }

//        public static EVCar ReturnEVCar(int id, string name)
//        {
//            return new EVCar { Id = id, Name = name };
//        }
//    }



//    public abstract class Car
//    {
//        public int Id { get; set; }
//        public string? Name { get; set; }


//        public virtual string GetCarDetails()
//        {
//            return $"{Id} - {Name}";
//        }
//    }

//    public class ICECar : Car
//    {
//        public override string GetCarDetails()
//        {
//            return $"{base.GetCarDetails()} - Internal Combustion Engine";
//        }
//    }

//    public class EVCar : Car
//    {
//        public override string GetCarDetails()
//        {
//            return $"{base.GetCarDetails()} - Electric";
//        }
//    }
//}