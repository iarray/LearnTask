using System;
using System.Collections.Generic;

namespace YieldDemo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            foreach (var k in GetSingleDigitNumbers())
            {
                Console.WriteLine(k);   
            }
            
            foreach (var k in GetSingleDigitNumbers2())
            {
                Console.WriteLine(k);   
            }

            Console.Read();
        }
        
        public static IEnumerable<int> GetSingleDigitNumbers()
        {
            yield return 0;
            yield return 1;
            yield return 2;
            yield return 3;
            yield return 4;
            yield return 5;
            yield return 6;
            yield return 7;
            yield return 8;
            yield return 9;
        }
        
        public static IEnumerable<int> GetSingleDigitNumbers2()
        {
            int index = 0;
            while (index++ < 10)
                yield return index;

            yield return 50;

            index = 100;
            while (index++ < 110)
                yield return index;
        }
         
        
        //迭代器方法有一个重要限制：在同一方法中不能同时使用 return 语句和 yield return 语句。 不会编译以下内容
        public static IEnumerable<int> GetSingleDigitNumbers3()
        {
            int index = 0;
            while (index++ < 10)
                yield return index;

            yield return 50;

            // generates a compile time error: 
            //var items = new int[] {100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
            //return items;  
        }
        
        /// <summary>
        ///  上面的方法可以这样改造
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetSingleDigitOddNumbers(bool getCollection)
        {
            if (getCollection == false)
                return new int[] {100, 101, 102, 103, 104, 105, 106, 107, 108, 109 };
            else
                return GetSingleDigitNumbers3();
        }

    }
}