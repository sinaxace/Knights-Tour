using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KnightsTour
{
    public static partial class DialogInput
    {
        // Windows Forms Custom Pop-Up Reference: https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
        public static string promptDialog(string message, string title)
        {

            Form dialog = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label mes = new Label() { Left = 30, Top = 20, Text = message };
            Button ok = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            TextBox box = new TextBox() { Left = 150, Top = 20, Width = 100 };

            dialog.Controls.Add(mes);
            dialog.Controls.Add(box);
            dialog.Controls.Add(ok);
            dialog.AcceptButton = ok;

            ok.Click += (sender, e) => { dialog.Close(); };
 
            return dialog.ShowDialog() == DialogResult.OK ? box.Text : "";
        }
    }
}
