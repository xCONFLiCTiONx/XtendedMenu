﻿using System;
using System.Windows.Forms;
using XtendedMenu.Properties;

namespace XtendedMenu
{
    public class SendMessage
    {
        public static DialogResult MessageForm(string text, string title = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.Information, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button2, bool IsErrorDialog = true)
        {
            try
            {
                if (IsErrorDialog)
                {
                    // Log this message for debugging
                    if (icon == MessageBoxIcon.Error)
                    {
                        EasyLogger.Error(text);
                    }
                    else if (icon == MessageBoxIcon.Warning)
                    {
                        EasyLogger.Info(text);
                    }
                    else
                    {
                        EasyLogger.Info(text);
                    }
                }

                using (Form form = new Form())
                {
                    form.Icon = (Resources.MAIN_ICON_256);
                    form.Opacity = 0;

                    form.Show();

                    form.WindowState = FormWindowState.Minimized;

                    form.WindowState = FormWindowState.Normal;

                    DialogResult dialogResult = MessageBox.Show(form, text, title, buttons, icon, defaultButton);
                    if (dialogResult == DialogResult.Yes)
                    {
                        form.Close();
                    }
                    return dialogResult;
                }
            }
            catch (Exception ex)
            {
                EasyLogger.Error(ex);

                return DialogResult.None;
            }
        }

        public static void MessageLogging(string text, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            try
            {
                // Log this message for debugging
                if (icon == MessageBoxIcon.Error)
                {
                    EasyLogger.Error(text);
                }
                else if (icon == MessageBoxIcon.Warning)
                {
                    EasyLogger.Info(text);
                }
                else
                {
                    EasyLogger.Info(text);
                }
            }
            catch (Exception ex)
            {
                EasyLogger.Error(ex);
            }
        }
    }
}
