using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rashmatz1 {
    public partial class Settings : Form {

        handleSettings mySettings;
        mainWin main; // For refreshing the main LV
        string currentDir, chosenPath;
        public Settings(mainWin main) {
            InitializeComponent();
            Sys.setVersionLabel(version);
            credits.Text = Sys.credits;
            mySettings = new handleSettings();
            this.main = main;
            currentDir = mySettings.getSettingsDir();
            chosenPath = currentDir; // Just to have a default value, and not empty ""
        }

        private void Settings_Load(object sender, EventArgs e) {
            folderLocation.Text = currentDir;
        }

        private void chooseFolder_Click(object sender, EventArgs e) {
            chosenPath = selectFolderDialog(currentDir);
            folderLocation.Text = chosenPath;
        }

        private string selectFolderDialog(string defaultPath) {
            FolderBrowserDialog chooseFolder = new FolderBrowserDialog();
            chooseFolder.SelectedPath = defaultPath;
            string selectedPath = defaultPath;
            if (chooseFolder.ShowDialog() == DialogResult.OK) {
                selectedPath = chooseFolder.SelectedPath;
            }
            return selectedPath;
        }

        private void cancelEdit_Click(object sender, EventArgs e) {
            /* Cancel */
            string msg = "האם אתה בטוח שברצונך לבטל את עריכת ההגדרות? כול השינויים יימחקו",
                title = "אישור פעולה | ביטול עריכת הגדרות";
            DialogResult confirm = Sys.showRTLConfirmationDialog(title, msg, MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes) {
                this.Close(); // Just closing the form, no need to do anything special
            }
        }

        private void saveItem_Click(object sender, EventArgs e) {
            /* Save */
            string msgOK = "ההגדרות נשמרו בהצלחה!",
                msgERR = "לא הייתה אפשרות לשמור את ההגדרות." +
                "\nאנא וודא את תקינות הערכים ונסה שוב.",
                title = "אישור פעולה | שמירת עריכת הגדרות";
            DialogResult confirm = Sys.showRTLConfirmationDialog(title, msgOK, MessageBoxButtons.OK);
            if (confirm == DialogResult.OK) {
                Console.WriteLine("saveItem_Click before change - " + mySettings.getSettingsDir());
                mySettings.updateSettings("data", chosenPath);
                Console.WriteLine("saveItem_Click after change - " + mySettings.getSettingsDir());
                // Console.WriteLine("Saving edit settings - new value is: " + chosenPath);
                File.Move(currentDir + "\\" + Sys.DB_NAME, chosenPath + "\\" + Sys.DB_NAME);
                File.Move(currentDir + "\\" + Sys.LOG_NAME, chosenPath + "\\" + Sys.LOG_NAME);
                Console.WriteLine("Moves file: " + currentDir + "\\" + Sys.DB_NAME + " => " + chosenPath + "\\" + Sys.DB_NAME);
                Console.WriteLine("Moves file: " + currentDir + "\\" + Sys.LOG_NAME + " => " + chosenPath + "\\" + Sys.LOG_NAME);
                main.refreshMainLV();
                this.Close();
            }
        }
    }
}
