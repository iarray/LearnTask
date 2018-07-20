using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NeedOnlyOneComplete
{
    /**
     * 只需要多个任务中的一个完成即刻返回, 通过CancellationToken取消
     */
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(
                await NeedOnlyOne(
                    ctx => DownLoadHtmlAsync(5, ctx),
                    ctx => DownLoadHtmlAsync(3, ctx),
                    ctx => DownLoadHtmlAsync(2, ctx),
                    ctx => DownLoadHtmlAsync(4, ctx))
            );

            Console.Read();
        }

        public static async Task<string> DownLoadHtmlAsync(int delaySeconds, CancellationToken token)
        {
            if (delaySeconds<0)
            {
                throw new Exception("fuck !!");
            }
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            token.ThrowIfCancellationRequested();
            return $"<html><body>我最快, 我只用了 {delaySeconds} 秒! </body></html>";
        }

        /// <summary>
        /// 多个任务只需其中一个完成就返回
        /// </summary>
        /// <param name="tastProviders"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<T> NeedOnlyOne<T>(params Func<CancellationToken, Task<T>>[] tastProviders)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            //注意IEnumerable是延迟执行的, 这里ToArray是为了立即执行返回, 避免WhenAny的时候可能会出现死锁的现象
            var tasks = tastProviders.Select(t => t.Invoke(tokenSource.Token)).ToArray();
            var completed = await Task.WhenAny(tasks);
            tokenSource.Cancel();
            
            foreach (var t in tasks)
            {
                //处理执行异常的任务, 打印错误信息
                t.ContinueWith(c => Console.WriteLine(c.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
            
            return await completed;
        }
    }
}