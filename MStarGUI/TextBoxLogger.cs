using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MStarGUI
{
    class TextBoxLogger : IMessageLogger
    {
        readonly TextBox LoggerTextBox;

        public TextBoxLogger (TextBox loggerTextBox)
        {
            LoggerTextBox = loggerTextBox;
        }

        public void logMessage (string message, bool crlf = true)
        {
            if (LoggerTextBox.Lines.Length != 0 && crlf)
                LoggerTextBox.AppendText( "\r\n" );
            LoggerTextBox.AppendText( message );
        }
    }
}
