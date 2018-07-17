using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LearnTask3
{
    internal class Program
    {
        static volatile int i = 0;
        
        public static void Main(string[] args)
        {
            Test4();
            Console.Read();
        }

        /// <summary>
        /// 声明了volatile, 但还是全部都5秒后完成
        /// 因为创建了Task并不会立即运行, 到task真正开始运行的时候, i已经累加完了
        /// </summary>
        public static void Test()
        {
            for (i = 0;  i< 5; i++)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(i));
                    Console.WriteLine(i + "秒后完成");
                });
            }
        }

        /// <summary>
        /// 按预期的结果进行, 可以正常1,2,3...秒完成
        /// </summary>
        public static void Test2()
        {
            Parallel.For(1, 6, async i =>
            {
                await Task.Delay(TimeSpan.FromSeconds(i));
                Console.WriteLine(i + "秒后完成");
            });
        }
        
        /// <summary>
        /// 加上延时之后,不会出现全部任务等待10秒的情况, 而是变得以无法预料的间隔运行
        /// </summary>
        public static void Test3()
        {
            for (i = 0;  i< 10; i++)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(i));
                    Console.WriteLine(i + "秒后完成");
                });
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 任务没有终止. 没有按预期的,运行10秒后终止所有任务
        /// </summary>
        public async static void Test4()
        {
            List<Task> tasks = new List<Task>(100);
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            for (int i = 0; i < 100; i++)
            {
                int a = i;
                tasks.Add(Task.Run(async ()=>
                { 
                    await Task.Delay(TimeSpan.FromSeconds(a));
                    Console.WriteLine($"{a}秒后完成任务.");
                }, cancellationTokenSource.Token));
            }

            await Task.Delay(TimeSpan.FromSeconds(10));
            cancellationTokenSource.Cancel();
        }
    }
}