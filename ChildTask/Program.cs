using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChildTask
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Test1();
            Console.WriteLine("============================================");
            Test1(TaskCreationOptions.AttachedToParent);
            Console.Read();
        }

        public static void Test1(TaskCreationOptions options = TaskCreationOptions.None)
        {
            var t = Task.Factory.StartNew( () => { Console.WriteLine("Running antecedent task {0}...",
                    Task.CurrentId);
                Console.WriteLine("Launching attached child tasks...");
                for (int ctr = 1; ctr <= 5; ctr++)  {
                    int index = ctr;
                    Task.Factory.StartNew( (value) => {
                        Console.WriteLine("   Attached child task #{0} running",
                            value);
                        Thread.Sleep(1000);
                    }, index, options);
                }
                Console.WriteLine("Finished launching attached child tasks...");
            });
            var continuation = t.ContinueWith( (antecedent) => { Console.WriteLine("Executing continuation of Task {0}",
                antecedent.Id);
            });
            continuation.Wait();
        }
    }
}