using System;
using System.Threading.Tasks;

namespace LearnTask
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //能成功启动
            NewTask().Start();

            //没有执行
            NewTask().Wait();

            //能运行任务
            RunTask();

            StarTask();
            
            Console.ReadKey();
        }

        public static Task StarTask()
        {
            return Task.Factory.StartNew(() => Console.WriteLine("创建并启动一个任务"));
        }
        
        public static Task RunTask()
        {
            return Task.Run(() => Console.WriteLine("运行一个任务"));
        }
        
        public static Task NewTask()
        {
            return new Task(() => Console.WriteLine("新建一个任务"));
        }
    }
}