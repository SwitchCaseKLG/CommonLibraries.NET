namespace SwitchCase.Forms
{
    public class ControlState
    {
        private Control control;
        private string text;
        private const string DefaultText = "working...";

        public ControlState(Control control, bool disable = false)
        {
            this.control = control;
            text = control.Text;

            if (disable)
            {
                Disable();
            }
        }

        public ControlState(Control control, string alternateText)
        {
            this.control = control;
            text = control.Text;

            Disable(alternateText);
        }

        public void Disable(string alternateText = DefaultText)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => Disable(alternateText)));
            }
            else
            {
                control.Enabled = false;
                control.Text = alternateText;
            }
        }

        public void Enable()
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new Action(() => Enable()));
            }
            else
            {
                control.Enabled = true;
                control.Text = text;
            }
        }

        public static Task RunTask(Control control, Action action, string alternateText = DefaultText)
        {
            ControlState cs = new ControlState(control);
            cs.Disable(alternateText);
            Task task = new Task(action);
            task.ContinueWith((t) => cs.Enable());
            task.Start();
            return task;
        }
    }
}
