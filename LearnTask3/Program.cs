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
            Test6();
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
        public async static Task Test4()
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
        
        /// <summary>
        ///  同样的任务也没有终止, 为什么呢?
        /// </summary>
        public static async Task Test5()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            Parallel.For(1, 60, new ParallelOptions(){CancellationToken = cancellationTokenSource.Token} ,async i =>
            {
                await Task.Delay(TimeSpan.FromSeconds(i));
                Console.WriteLine(i + "秒后完成");
            });    
            
            await Task.Delay(TimeSpan.FromSeconds(10));
            Console.WriteLine("正在取消任务");
            cancellationTokenSource.Cancel();
            Console.WriteLine("取消任务完成");
        }

        /// <summary>
        ///  Cancel 方法不会中断任务的运行, 需要自己在任务里面对CancellationToken的状态进行处理来确认是否终止任务
        /// </summary>
        public static async Task Test6()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Parallel.For(1, 6000, new ParallelOptions(){CancellationToken = token} , async i =>
            {
                if (token.IsCancellationRequested)
                {
                    //理论上很难进入到这里
                    Console.WriteLine("任务准备执行,但被及时取消");
                    //token.ThrowIfCancellationRequested();
                }

                await Task.Delay(TimeSpan.FromSeconds(i));
                if (!token.IsCancellationRequested)
                    Console.WriteLine(i + "秒后完成");
                else
                {
                    Console.WriteLine("任务已经在执行, 被强制取消");
                    //token.ThrowIfCancellationRequested();
                }
            });    
            
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine("正在取消任务");
            cancellationTokenSource.Cancel();
            Console.WriteLine("取消任务完成");
        }

    }
}