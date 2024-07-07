using System;
using System.Windows.Forms;

namespace CD_NameChanger
{
    class DetailScrollDialog
    {
        public static DialogResult show(string title, string message, string details)
        {
            //ダイアログの種類
            var dialogTypeName = "System.Windows.Forms.PropertyGridInternal.GridErrorDlg";
            var dialogType = typeof(Form).Assembly.GetType(dialogTypeName);

            //ダイアログインスタンスを生成
            var dialog = (Form)Activator.CreateInstance(dialogType, new PropertyGrid());

            //ダイアログのプロパティを設定
            dialog.Text = title;
            dialogType.GetProperty("Message").SetValue(dialog, message, null);
            dialogType.GetProperty("Details").SetValue(dialog, details, null);

            dialog.AutoSize = true;
            //ダイアログの表示
            var result = dialog.ShowDialog();
            return result;
        }
    }
}
