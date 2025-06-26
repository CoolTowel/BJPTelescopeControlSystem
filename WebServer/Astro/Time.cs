using CosineKitty;

namespace WebServer.Astro
{
    public class Time
    {
        public static AstroTime GetCurrentAstroTime()
        {
            // Get the current time and convert it to AstroTime
            AstroTime time = new(DateTime.Now);
            Console.WriteLine($"Current AstroTime: {time}");
            return time;
        }
    }
}
