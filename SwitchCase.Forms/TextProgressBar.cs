namespace SwitchCase.Forms
{
    public partial class TextProgressBar : UserControl
    {
        public TextProgressBar()
        {
            InitializeComponent();

            Reset();
        }

        public void Reset()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => Reset()));
            }
            else
            {
                progressBar1.Value = 0;
                label1.Text = "";
            }
        }

        public void SetProgress(double value)
        {
            SetProgress(value, value.ToString("0%"));
        }

        public void SetProgress(double value, string text)
        {
            SetProgress((int)(100 * value), text);
        }

        public void SetProgress(int value, string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetProgress(value, text)));
            }
            else
            {
                progressBar1.Value = value;
                label1.Text = text;
            }
        }
    }
}
