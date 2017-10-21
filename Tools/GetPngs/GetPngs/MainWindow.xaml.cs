using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
namespace GetPngs
{
    public partial class MainWindow : Window
    {
        private string selectDirPath;
        WindowApplicationSettings settings = new WindowApplicationSettings();
        private int selectIndex = 0;
        public MainWindow()
        {
            InitializeComponent();
            settings.Reload();
            this.StartDirectoryName.Text = settings.Path;
            selectDirPath = settings.Path;
        }

        //private string FixPath(string path)
        //{
        //    //return path.Replace("\\", "/");
        //    return path;
        //}
        private void CopyDirectory(string curPath,string desPath)
        {
            
        }


        private void GetPngBtn(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(selectDirPath))
            {
                string[] dirs = Directory.GetDirectories(selectDirPath,"*",SearchOption.TopDirectoryOnly);
                for(int i=0;i<dirs.Length;i++)
                {
                    string dirPath = dirs[i];
                    if(dirPath.Contains("_out"))
                    {                    
                        Directory.Delete(dirPath, true);
                        continue;
                    }
                    string newDirPath = string.Format("{0}_out", dirPath);

                    Directory.CreateDirectory(newDirPath);
                  
                    string[] subFiles = Directory.GetFiles(dirPath, "*", SearchOption.AllDirectories);
                    for (int j = 0; j < subFiles.Length; j++)
                    {
                        string file = subFiles[j];
                        FileInfo fileInfo = new FileInfo(file);

                        string extension = System.IO.Path.GetExtension(file);
                        if (selectIndex == 0 && extension.Equals(".png"))
                        {
                            string newFile = string.Format("{0}/{1}", newDirPath, fileInfo.Name);
                            File.Move(file, newFile);
                            
                        }
                    }
                    
                }
            }
            else
            {
                System.Windows.MessageBox.Show("提取路径为空", "提示");
            }
        }

        private void SelectStartPath(object sender, RoutedEventArgs e)
        {

            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            m_Dialog.SelectedPath = settings.Path;
            
            DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string dir = m_Dialog.SelectedPath.Trim();
            this.StartDirectoryName.Text = dir;
            settings.Path = dir;
            settings.Save();
            selectDirPath = dir;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectIndex = comboBox.SelectedIndex;
        }
    }
    public class WindowApplicationSettings : ApplicationSettingsBase
     {
         [UserScopedSettingAttribute()]
         public String Path
         {
             get { return (String)this["Path"]; }
             set { this["Path"] = value; }
         }
    }
}
