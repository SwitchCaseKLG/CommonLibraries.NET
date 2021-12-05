namespace SwitchCase.Core
{
    public class Environment
    {
        public static T LoadVariable<T>(string name, T defaultValue = default) where T : struct
        {
            string strValue = System.Environment.GetEnvironmentVariable(name) ?? string.Empty;
            T value;

            if (string.IsNullOrEmpty(strValue))
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
            return value;
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
