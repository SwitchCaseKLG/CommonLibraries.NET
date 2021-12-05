using SwitchCase.Core;

namespace SwitchCase.Forms
{
    public partial class TextProgressBar : UserControl, IProgress
    {
        public TextProgressBar()
        {
            InitializeComponent();
            Reset();
        }

        public void Reset()
        {
            SetProgress(0, string.Empty);
        }

        public void SetProgress(int value, string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(() => SetProgress(value, text));
            }
            else
            {
                progressBar1.Value = value;
                label1.Text = text;
            }
        }

        public void SetProgress(float value, string text)
        {
            SetProgress((int)(100 * value), text);
        }

        public SubProgress GetSubProgress(int max = 100)
        {
            return new SubProgress(this, progressBar1.Value, max);
        }

        public void SetProgress(int value)
        {
            SetProgress(value, label1.Text);
        }

        public void SetProgress(string text)
        {
            SetProgress(progressBar1.Value, text);
        }
    }
}
