namespace SwitchCase.Core
{
    public interface IProgress
    {
        void Reset();
        void SetProgress(int value);
        void SetProgress(string text);
        void SetProgress(int value, string text);
        void SetProgress(float value, string text);
    }
}
