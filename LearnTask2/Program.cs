using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnTask2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Test();
            //Test2();
            //Test3();
            //Test4();

            Console.Read();
        }
        
        /// <summary>
        /// 打印100个 100秒后完成任务(和预期结果不同)
        /// </summary>
        public static void Test()
        {
            List<Task> tasks = new List<Task>(100);
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=>
                {
                    //这里取到的i总是100
                    Task.Delay(TimeSpan.FromSeconds(i));
                    Console.WriteLine($"{i}秒后完成任务.");
                }));
            }
        }
        
        /// <summary>
        /// 没有打印任何内容
        /// </summary>
        public static void Test2()
        {
            List<Task> tasks = new List<Task>(100);
            for (int i = 0; i < 100; i++)
            {
                int a = i;
                var t = new Task(() =>
                { 
                    Task.Delay(TimeSpan.FromSeconds(a));
                    Console.WriteLine($"{a}秒后完成任务.");
                });
                
                //漏了这句就不运行
                //t.Start();
                tasks.Add(t);
            }

            Task.WaitAll(tasks.ToArray());
        }
        
        
        /// <summary>
        /// 没有按期望的一样每个任务比上一个任务多一秒
        /// </summary>
        public static void Test3()
        {
            List<Task> tasks = new List<Task>(100);
            for (int i = 0; i < 100; i++)
            {
                int a = i;
                tasks.Add(Task.Run(()=>
                { 
                    Task.Delay(TimeSpan.FromSeconds(a));
                    Console.WriteLine($"{a}秒后完成任务.");
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
        
        /// <summary>
        /// 期望的结果, 每个任务比上一个任务晚一秒运行
        /// </summary>
        public static void Test4()
        {
            List<Task> tasks = new List<Task>(100);
            for (int i = 0; i < 100; i++)
            {
                int a = i;
                tasks.Add(Task.Run(async ()=>
                { 
                    await Task.Delay(TimeSpan.FromSeconds(a));
                    Console.WriteLine($"{a}秒后完成任务.");
                }));
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}