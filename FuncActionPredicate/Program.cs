
using System.Diagnostics;

namespace FuncActionPredicate
{
    class Program
    {
        // possible to create our own delegates for Sum
        delegate TResult Func2<out TResult>();
        delegate TResult Func2<in T1, out TResult>(T1 arg);
        delegate TResult Func2<in T1, in T2, out TResult>(T1 arg, T2 arg2);
        delegate TResult Func2<in T1, in T2, in T3, out TResult>(T1 arg, T2 arg2, T3 arg3);


        static void Main(string[] args)
        {
            //MathClass mathClass = new MathClass();

            // built in Func<> delegate to encapsulate the Sum method
            // param1 : input type
            // param2 : input type
            // param3 : return type
            // Func<int, int, int> calc = mathClass.Sum;

            // can set an anonymous method, not bound by an identifier
            //Func<int, int, int> calc = delegate (int a, int b) { return a + b; };

            // possible to furthur abstract the method
            // lambda expression --anonymous function 
            Func<int, int, int> calc = (a, b) =>  a + b;

            //Func2<int, int, int> calc = (a, b) => a + b;

            int result = calc(3, 2);

            Console.WriteLine(result);

            Console.ReadKey();
        }
    }

    public class MathClass
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}