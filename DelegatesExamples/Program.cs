namespace DelegateExamples
{
    class Program
    {
        // reference a method that does not return a value
        delegate void LogDel(string text);

        static void Main(string[] args)
        {

            Log log = new Log();

            //LogDel logDel = new LogDel(log.LogTextToScreen);

            // multi-cast delegate to call multiple methods
            LogDel LogTextToScreenDel, LogTextToFileDel;

            LogTextToScreenDel = new LogDel(log.LogTextToScreen);

            LogTextToFileDel = new LogDel(log.LogTextToFile);

            // the two variabels are combined through the + operator
            LogDel multiLogDel = LogTextToFileDel + LogTextToScreenDel;


            Console.WriteLine("input your name");

            var input = Console.ReadLine();

            LogText(multiLogDel, input);

            Console.ReadKey();
        }

        static void LogText(LogDel logDel, string text)
        {
            // wrapper function for the logdel delegate
            logDel(text);
        }
        
    }

    public class Log() {

        // public method can be instanced from delegates
        public void LogTextToScreen(string text)
        {
            Console.WriteLine($"{DateTime.Now}: {text}");
        }

        public void LogTextToFile(string text)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log.txt"), true))
            {
                sw.WriteLine($"{DateTime.Now}: {text}");
            }
        }
    }
}
