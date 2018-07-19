using System.Net.Http;
using System.Threading.Tasks;

namespace LearnTask4
{
    internal class Program
    {
        public static void Main(string[] args)
        {
        }
        
        public Task<string> GetHtmlAsync()
        {
            // Execution is synchronous here
            var client = new HttpClient();

            return client.GetStringAsync("http://www.dotnetfoundation.org");
        }
    }
}