namespace SwitchCase.Core
{
    public class SubProgress : IProgress
    {
        private readonly IProgress parentProgress;
        private readonly int min;
        private readonly int max;

        public SubProgress(IProgress parentProgress, int min, int max)
        {
            this.parentProgress = parentProgress;
            this.min = min;
            this.max = max;
        }

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
