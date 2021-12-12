using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchCase.Forms
{
    public static class FormExtensions
    {
        public static void CloseDialog(this Form form, DialogResult dialogResult = DialogResult.OK)
        {
            form.DialogResult = dialogResult;
            form.Close();
        }
    }
}
