using System;
using System.Threading.Tasks;

namespace RetryTask
{
    /// <summary>
    /// 异步重试执行
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Retry<string>(() => { throw new Exception("error"); }, 5, i => Task.Delay(TimeSpan.FromSeconds(i)));
            Console.Read();
        }
        
        /// <summary>
        /// 重试执行任务
        /// </summary>
        /// <param name="doAction"></param>
        /// <param name="tryTimes"></param>
        /// <param name="retryWhen"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> Retry<T>(Func<Task<T>> doAction, int tryTimes, Func<int, Task> retryWhen)
        {
            for (int i = 0; i < tryTimes; i++)
            {
                try
                {
                    //不考虑上下文回调
                    return await doAction.Invoke().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                }
                
                //不考虑上下文回调
                await retryWhen(i + 1).ConfigureAwait(false);
                Console.WriteLine($"进行第{i + 1}次尝试.");
            }

            Console.WriteLine($"{tryTimes}次尝试后失败.");
            return default(T);
        }
    }
}