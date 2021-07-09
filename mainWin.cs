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


/*

TODO

- To check "formBorderStyle"
    it is used to make the main form not resizable. Need to check more deeply what each option means

*/

namespace Rashmatz1 {
    public partial class mainWin : Form {

        handleDB DB;
        handleLog myLog;
        handleSettings mySettings;

        public mainWin() {
            InitializeComponent();
            /* Calling constructors and creating files if not exist */
            DB = new handleDB();
            myLog = new handleLog();
            mySettings = new handleSettings();
        }

        private void mainWin_Load(object sender, EventArgs e) {
            Console.WriteLine("* Programme started *");
            Console.WriteLine("* Data directiory is: " + mySettings.getSettingsDir());

            /*
             * A new handleDB instance is needed for "refreshing" the data, and specifically the location.
             * Otherwise, when you close the programme after changing the data directiory (via settings),
             *   It will re-create a log.txt file and write in "programme closed" (mainWin_Close() event) - just once.
             *   After one time it happens, it never happens again -
             *   Because in next time it will read the "new", correct data-directiory path.
            */
            handleLog refreshedLog = new handleLog();
            refreshedLog.write("Programme started");

            DB.removeEmptyLines(); // Removing all empty lines from DB, whenever the software is up
            Sys.setVersionLabel(version);
            initMainLV();
        }
        private void mainWin_Close(object sender, EventArgs e) {
            /* Same as above in mainWin_Load() function */
            handleLog refreshedLog = new handleLog();
            refreshedLog.write("Programme closed");
        }

        private void mainList_DoubleClick(object sender, EventArgs e) {
            /* Opens dialog for edit-item */
            openEditItem();
        }

        private void newItem_Click(object sender, EventArgs e) {
            /* Opens dialog for new-item */
            openCreateNewItem();
        }

        private void about_Click(object sender, EventArgs e) {
            /* Opens "about" dialog */
            about about = new about();
            about.ShowDialog();
        }

        private void settingsButton_Click(object sender, EventArgs e) {
            Settings settings = new Settings(this);
            settings.ShowDialog();
        }

        private void debugButton_Click(object sender, EventArgs e) {
            /* Just a debugging button */
            Console.WriteLine("Debug button has been pressed");
            string meinOrdner = @"C:\Users\Noam\Documents\Programming\C#\Rashmatz1\bin\Debug\meinOrdner";
            mySettings.updateSettings("data", "will it work?");
            mySettings.updateSettings("data", meinOrdner);
        }



        /*
         * ----------------
         * | My Functions |
         * ----------------
        */

        public void refreshMainLV() {
            /* Refresh the main list view */
            mainListView.Items.Clear();
            initMainLV();
        }

        private void initMainLV() {
            /* Init the main list view - setting all values */
            handleDB DB = new handleDB(); // TODO Check if it was important or not
            List<Item> dbData = DB.getData();
            int i;
            for (i = 0; i < dbData.Count; i++) {
                ListViewItem item = new ListViewItem(dbData[i].itemID);
                item.SubItems.Add(dbData[i].itemName);
                item.SubItems.Add(dbData[i].itemData);
                item.SubItems.Add(dbData[i].itemCount);
                item.SubItems.Add(dbData[i].itemMakat);
                item.SubItems.Add(dbData[i].itemMakatFactory);
                item.SubItems.Add(dbData[i].itemLocation);
                item.SubItems.Add(dbData[i].itemComments);
                item.SubItems.Add(dbData[i].itemSign);
                mainListView.Items.Add(item);
            }

            /* TODO check if possible to remove this hard-coded setup */
        }
        private void openEditItem() {
            /* Opens dialog for edit-item */
            if (mainListView.SelectedIndices.Count > 0) {
                int itemIndex = mainListView.SelectedIndices[0];
                Dictionary<string, string> data = new Dictionary<string, string>();
                for (int i = 0; i < Sys.indexesNames.Length; i++) {
                    data.Add(Sys.indexesNames[i], mainListView.Items[itemIndex].SubItems[i].Text);
                }

                Form viewItem = new viewItem('e', data, this); // Opns in edit mode
                viewItem.ShowDialog();
            }
        }

        private void openCreateNewItem() {
            /* Opens dialog for mew-item */
            handleDB DB = new handleDB(); // TODO Check if it was important or not
            Form viewItem = new viewItem('n', DB.genNewID().ToString(), this); // Opens in create-new mode
            viewItem.ShowDialog();
        }
    }
}
