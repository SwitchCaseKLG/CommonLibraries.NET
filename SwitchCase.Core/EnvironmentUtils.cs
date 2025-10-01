using System.Diagnostics;

namespace SwitchCase.Core
{
    public class EnvironmentUtils
    {
        public static T LoadEnvVar<T>(string name, T defaultValue = default!)
        {
            string? strValue = Environment.GetEnvironmentVariable(name);
            T value;

            if (string.IsNullOrWhiteSpace(strValue))
            {
                value = defaultValue;
            }
            else
            {
                try
                {
                    value = (T)Convert.ChangeType(strValue, typeof(T));
                }
                catch (Exception)
                {
                    value = defaultValue;
                }
            }
            //Log.Info($"{name} = {value}");
            return value;
        }

        public static Stopwatch StartTimer()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            return sw;
        }

        public static string TraceTime(Stopwatch sw, string name, bool restart = true)
        {
            string message = name + ": " + sw.ElapsedMilliseconds + "ms";
            if (restart)
            {
                sw.Restart();
            }
            return message;
        }

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
