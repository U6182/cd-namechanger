using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Security;
using Microsoft.VisualBasic.FileIO;
using Shell32;
using JR.Utils.GUI.Forms;

namespace CD_NameChanger
{

    /// <summary>
    /// CDファイルから名前変更を行うツール
    /// @file CDNameChanger.cs
    /// @date 2022/08/28
    /// @author 木村 憂哉
    /// </summary>
    public partial class CDNameChanger : Form
    {

        /// <summary>フォルダフラグ</summary>
        private Boolean bFolder = false;
        /// <summary>選択したパスリスト</summary>
        private IEnumerable<string> selectPathList = null;
        /// <summary>ファイルパスリスト</summary>
        private List<List<string>> filePathLists = new List<List<string>>();
        /// <summary>エラーパスリスト</summary>
        private List<string> errorPathList = new List<string>();
        /// <summary>想定外の変更リスト</summary>
        private List<string> unexpectedPathList = new List<string>();
        /// <summary>変更後のファイル名</summary>
        private string editFileNames = "";

        /// <summary>
        /// コンストラクタ
        /// コンポーネントの初期化を行う
        /// </summary>
        public CDNameChanger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 変更ボタンの処理を行う
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">ボタンクリックのイベントの種類</param>
        private void change_Click(object sender, EventArgs e)
        {

            //パスが選択されていない場合処理を終了する
            if (this.selectPathList == null)
            {
                MessageBox.Show("対象のファイルまたはフォルダを選択してください。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //ファイルパスリストの追加
            addFilePathLists();

            //変更予定のファイル名の取得
            string fileNames = "";
            int fileCount = 0;
            foreach (List<string> filePathList in this.filePathLists)
            {
                foreach (string filePath in filePathList)
                {
                    //ファイル名を取得
                    fileNames += Path.GetFileName(filePath) + "\n";
                    fileCount++;
                }
            }

            //変更の確認
            DialogResult result = FlexibleMessageBox.Show("下記の" + fileCount + "ファイルを変更します\n" + fileNames, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            int editCount = 0;
            //ファイルのID3情報を取得
            foreach (List<string> filePathList in this.filePathLists)
            {
                foreach (string filePath in filePathList)
                {
                    //ファイル名の変更
                    if (editFileName(filePath))
                    {
                        editCount++;
                    }
                }
            }

            FlexibleMessageBox.Show(editCount + "件変更しました\n【変更内容】\n" + editFileNames, "変更結果", MessageBoxButtons.OK);

            if(this.unexpectedPathList.Count > 0)
            {
                string unexpectedPaths = concatPathList(this.unexpectedPathList);
                FlexibleMessageBox.Show(this.unexpectedPathList.Count + "件の想定外の変更がありました\n【想定外の変更内容】\n" + unexpectedPaths, "想定外の変更結果", MessageBoxButtons.OK);
            }

            if (this.errorPathList.Count > 0)
            {
                string errorPaths = concatPathList(this.errorPathList);
                FlexibleMessageBox.Show(this.errorPathList.Count + "件失敗しました\n【失敗内容】\n" + errorPaths, "失敗結果", MessageBoxButtons.OK);
            }

            this.editFileNames = "";
            this.selectPathList = null;
            this.pathTextBox.Clear();
            this.unexpectedPathList.Clear();
            this.errorPathList.Clear();
        }

        /// <summary>
        /// フォルダを開くボタンの処理を行う
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">ボタンクリックのイベントの種類</param>
        private void folderOpenBtn_Click(object sender, EventArgs e)
        {

            //フォルダダイアログの生成
            using (var folderDialog = new CommonOpenFileDialog()
            {

                InitialDirectory = @"D:\"
                ,
                IsFolderPicker = true
                ,
                Multiselect = true
                ,
                DefaultFileName = ""

            })
            {
                //フォルダが選択された場合
                if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    //選択したフォルダパスを保持
                    this.selectPathList = folderDialog.FileNames;
                    //選択設定
                    selectSetting(true);
                }
            }

        }

        /// <summary>
        /// ファイルを開くボタンの処理を行う
        /// </summary>
        /// <param name="sender">オブジェクト</param>
        /// <param name="e">ボタンクリックのイベントの種類</param>
        private void fileOpenBtn_Click(object sender, EventArgs e)
        {
            using (var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = @"D:"
                ,
                Filter = "mp3|*.mp3"
                ,
                Multiselect = true
                ,
                Title = "ファイルの選択"

            })
            {
                //ファイルが選択された場合
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //選択したファイルパスを保持
                    this.selectPathList = fileDialog.FileNames;
                    //選択設定
                    selectSetting(false);
                }

            }
        }

        /// <summary>
        /// ファイルパスを抽出してリストに追加を行う
        /// </summary>
        private void addFilePathLists()
        {
            //ファイルパスをクリア
            this.filePathLists.Clear();

            //フォルダの場合
            if (this.bFolder)
            {
                foreach (string path in this.selectPathList)
                {
                    List<string> filePathList = new List<string>();

                    //選択した各フォルダのファイルパスを取得
                    string[] filePaths = Directory.GetFiles(path, "*.mp3", System.IO.SearchOption.AllDirectories);
                    foreach (string filePath in filePaths)
                    {
                        filePathList.Add(filePath);
                    }
                    //フォルダごとのファイルパスをリストに追加
                    this.filePathLists.Add(filePathList);
                }
            }
            else
            {

                List<string> filePathList = new List<string>();
                foreach (string filePath in this.selectPathList)
                {
                    filePathList.Add(filePath);
                }
                //ファイルパスをリストに追加
                this.filePathLists.Add(filePathList);
            }

        }

        /// <summary>
        /// CDのファイル名のフォーマットの変更を行う
        /// </summary>
        /// <param name="filePath">変更するファイルパス</param>
        /// <returns>bool 成功したかどうか</returns>
        private bool editFileName(string filePath)
        {

            bool bSuccess = false;
            TagLib.File file = null;
            string errorMessage = null;

            try
            {

                //取得するID3情報のファイル
                string shellDirectory = Path.GetDirectoryName(filePath);
                string shellFileName = Path.GetFileName(filePath);
                ShellClass shell = new ShellClass();
                Folder folder = shell.NameSpace(shellDirectory);
                FolderItem folderItem = folder.ParseName(shellFileName);
                //アーティスト名取得
                string artist = folder.GetDetailsOf(folderItem, 13);
                artist = artist == "アーティスト情報なし" ? "" : "　" + artist;
                
                //ファイル名を取得
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                //ファイル名を編集
                int removeSize = 0;
                for (int i = 0; i < fileName.Length; i++)
                {
                    removeSize++;
                    if (fileName[i] == ' ')
                    {
                        break;
                    }
                }
                //ファイル名の変換
                string editFileName = fileName.Remove(0, removeSize);
                editFileName = editFileName + artist;
                editFileName = replaceEditFileName(editFileName);

                bool bUnexpected = false;
                if (removeSize > 3 || artist == "")
                {
                    DialogResult result = MessageBox.Show("「" + fileName + "」" + "は\n" + "「" + editFileName + "」" + "に変更されます", "想定外の変更", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if(result == System.Windows.Forms.DialogResult.No)
                    {
                        return bSuccess;
                    }
                    bUnexpected = true;
                }
                editFileName = editFileName + ".mp3";

                //ファイル名を変更
                FileSystem.RenameFile(filePath, editFileName);
                this.editFileNames += editFileName + "\n";
                if (bUnexpected)
                {
                    string unexpectedPath = filePath.Replace(fileName + ".mp3", editFileName);
                    this.unexpectedPathList.Add(unexpectedPath);
                }
                bSuccess = true;
            }
            catch (TagLib.CorruptFileException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "無効なファイルパスです";
            }
            catch (TagLib.UnsupportedFormatException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "存在しないまたは\n" +
                    "変更するファイル名が存在しません";
            }
            catch (UnauthorizedAccessException)
            {
                errorMessage = "ユーザーに必要なアクセス許可がありません";
            }
            catch (NotSupportedException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "パス内のファイルまたはディレクトリ名、コロン (:) が含まれてるか、無効です";
            }
            catch (ArgumentNullException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "ファイル形式がサポートされていません";
            }
            catch (ArgumentException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "ファイルパスが正しくありません";
            }
            catch (IndexOutOfRangeException)
            {
                errorMessage = "「" + filePath + "」" + "は\n" + "アーティスト名が設定されていません";
            }
            catch (FileNotFoundException)
            {
                errorMessage = "ディレクトリが存在していません";
            }
            catch (PathTooLongException)
            {
                errorMessage = "パスがシステム定義の最大長を超えています";
            }
            catch (IOException)
            {
                errorMessage = "変更するファイル名が既存のファイル名になっています";
            }
            catch (SecurityException)
            {
                errorMessage = "ユーザーにはパスを表示するために必要なアクセス許可が不足しています";
            }
            finally
            {
                if(file != null)
                {
                    //ファイルの解放
                    file.Dispose();
                }

                if(errorMessage != null)
                {
                    MessageBox.Show(errorMessage, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.errorPathList.Add(filePath);
                }
            }

            //処理結果を返す
            return bSuccess;

        }

        /// <summary>
        /// ファイルまたはフォルダ選択時に必要なデータの設定を行う
        /// </summary>
        /// <param name="bFolder">フォルダ選択フラグ</param>
        private void selectSetting(Boolean bFolder)
        {
            //ファイル選択時の設定
            this.bFolder = bFolder;
            this.pathTextBox.Text = "";
            //パスをテキストボックスに設定
            foreach (string path in this.selectPathList)
            {
                this.pathTextBox.Text += path;
            }
        }

        /// <summary>
        /// ファイル名を正しいフォーマットの置換を行う
        /// </summary>
        /// <param name="editFileName">編集ファイル名</param>
        /// <returns>string 置換後のファイル名</returns>
        private string replaceEditFileName(string editFileName)
        {
            editFileName = editFileName.Replace("Feat.", "feat.");
            editFileName = editFileName.Replace("\\", "￥");
            editFileName = editFileName.Replace("/", "／");
            editFileName = editFileName.Replace(":", "：");
            editFileName = editFileName.Replace("*", "＊");
            editFileName = editFileName.Replace("?", "？");
            editFileName = editFileName.Replace("\"", "”");
            editFileName = editFileName.Replace("<", "＜");
            editFileName = editFileName.Replace(">", "＞");
            editFileName = editFileName.Replace("|", "｜");

            return editFileName;
        }

        /// <summary>
        /// パスを文字列に連結を行う
        /// </summary>
        /// <param name="pathList">パスリスト</param>
        /// <returns>string 連結後のパス</returns>
        private string concatPathList(List<string> pathList)
        {
            string paths = "";
            foreach (string path in pathList)
            {
                //パスを連結
                paths += path + "\n";
            }
            return paths;
        }

    }
}
