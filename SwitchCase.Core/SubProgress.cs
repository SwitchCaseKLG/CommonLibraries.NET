namespace SwitchCase.Core
{
    public class SubProgress(IProgress parentProgress, int min, int max) : IProgress
    {
        public void Reset()
        {
            parentProgress.SetProgress(min, string.Empty);
        }

        public void SetProgress(int value, string text)
        {
            parentProgress.SetProgress((int)(value * (float)(max - min) / 100f + min), text);
        }

        public void SetProgress(float value, string text)
        {
            parentProgress.SetProgress((int)(value * 100f * (float)(max - min) / 100f + min), text);
        }

        public void SetProgress(int value)
        {
            parentProgress.SetProgress((int)(value * (float)(max - min) / 100f + min));
        }

        public void SetProgress(string text)
        {
            parentProgress.SetProgress(text);
        }
    }
}
