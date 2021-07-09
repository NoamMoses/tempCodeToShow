using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rashmatz1 {
    public partial class viewItem : Form {
        // private string[] data;
        private Dictionary<string, string> data = new Dictionary<string, string>();
        private string title,
                       ID = "", name, desc, count, makat, makatFactory, location,
                       comments, sign;
        private char mode;
        mainWin main; // For refreshing the main LV
        handleLog myLog;
        handleDB DB;
        Sys mySys;

        public viewItem() {
            InitializeComponent();
            myLog = new handleLog();
        }

        public viewItem(char mode, Dictionary<string, string> data, mainWin main) {
            /* Init using data | for editing an item */
            Console.WriteLine("* viewItem constructor | with data *");
            InitializeComponent();
            myLog = new handleLog();
            DB = new handleDB();
            mySys = new Sys();
            this.title = parseTitle(mode);
            this.data = data;
            this.main = main;
        }

        public viewItem(char mode, string ID, mainWin main) {
            /* Init using just an ID | for creating a new item */
            Console.WriteLine("* viewItem constructor | without data, with ID *");
            InitializeComponent();
            myLog = new handleLog();
            DB = new handleDB();
            mySys = new Sys();
            this.title = parseTitle(mode);
            this.data = Sys.returnItemObj();
            /* Default values */
            this.data["ID"] = ID;
            this.data["count"] = "0";
            this.main = main;
        }

        private void viewItem_Load(object sender, EventArgs e) {
            credits.Text = Sys.credits;
            this.Text += this.title;Sys.setVersionLabel(version);
            Sys.setVersionLabel(version);
            this.ActiveControl = editFields_nameInput; // Sets the name-input to be active as default
            mainTitle.Text += title;
            if (mode == 'n') { // New item. No need for "delete" option.
                deleteItem.Visible = false;
            }
            // Prepares the list view, and textbos - with default values
            initLV();
            loadInputs();
        }

        /* Handling the label-input relationship. */
        private void editFields_nameTitle_Click(object sender, EventArgs e) { editFields_nameInput.Focus(); }
        private void editFields_makatTitle_Click(object sender, EventArgs e) { editFields_makatInput.Focus(); }
        private void editFields_locationTitle_Click(object sender, EventArgs e) { editFields_locatiomInput.Focus(); }
        private void editFields_descTitle_Click(object sender, EventArgs e) { editFields_descInput.Focus(); }
        private void editFields_makatFactoryTitle_Click(object sender, EventArgs e) { editFields_makatFactoryInput.Focus(); }
        private void editFields_signTitle_Click(object sender, EventArgs e) { editFields_signInput.Focus(); }
        private void editFields_commentsTitle_Click(object sender, EventArgs e) { editFields_commentsInput.Focus(); }
        private void editFields_countTitle_Click(object sender, EventArgs e) { editFields_countInput.Focus(); }

        private void cancelEdit_Click(object sender, EventArgs e) {
            /* Cancel */
            string msg = "האם אתה בטוח שברצונך לבטל את עריכת הפריט? כול השינויים יימחקו",
                title = "אישור פעולה | ביטול עריכת פריט";
            DialogResult confirm = Sys.showRTLConfirmationDialog(title, msg, MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes) {
                if (mode == 'e') {
                    myLog.write("DB Canceled editing an item");
                } else if (mode == 'n') {
                    myLog.write("DB Canceled creating an item");
                }
                this.Close(); // Just closing the form, no need to do anything special
            }
        }

        private void deleteItem_Click(object sender, EventArgs e) {
            /* Delete */
            // handleDB DB = new handleDB(); // TODO Check if this is important or not
            string msgCONFIRM = "האם אתה בטוח שברצונך למחוק את פריט זה? אין אפשרות לביטול הפעולה!",
                msgDONE = "הפריט נמחק בהצלחה",
                title = "אישור פעולה | מחיקת פריט";
            DialogResult confirm = Sys.showRTLConfirmationDialog(title, msgCONFIRM, MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes) {
                this.DB.deleteRow(this.data["ID"]);
                DialogResult complete = Sys.showRTLConfirmationDialog(title, msgDONE, MessageBoxButtons.OK);
                closeAndRefershMainLV();
            } else if (confirm == DialogResult.No || confirm == DialogResult.Cancel) {
            }
        }

        private void saveItem_Click(object sender, EventArgs e) {
            /* Save */
            // handleDB DB = new handleDB(); // TODO Check if this is important or not
            string msgOK = "הפריט נשמר בהצלחה!",
                msgERR = "לא הייתה אפשרות לשמור את הפריט." +
                "\nאנא וודא את תקינות הערכים ונסה שוב." +
                "\n\nשדות המסומנים ב-* הינם שדות חובה, והאורך המינימאלי הינו 2 תווים.",
                title = "אישור פעולה | שמירת עריכת פריט",
                newRow;
            if (validateInputs()) {
                DialogResult confirm = Sys.showRTLConfirmationDialog(title, msgOK, MessageBoxButtons.OK);
                if (confirm == DialogResult.OK) {
                    newRow = createNewDBRow();
                    if (mode == 'n') { // New-item mode
                        this.DB.addNewRow(newRow);
                    } else if (mode == 'e') { // Edit mode
                        this.DB.updateRow(this.DB.getIndexByID(this.data["ID"]), newRow); // Updating an existing row
                    }
                    main.refreshMainLV();
                    this.Close();
                }
            } else { // Inputs aren't valid
                DialogResult confirm = Sys.showRTLConfirmationDialog(title, msgERR, MessageBoxButtons.OK);
            }
        }



        /*
         * ----------------
         * | My Functions |
         * ----------------
        */
        private void closeAndRefershMainLV() {
            /*
             * Referesh the main LV (in main window)
             * and closes the edit/new item dialog
            */
            main.refreshMainLV();
            this.Close();
        }

        private bool validateInputs() {
            /* Checks whether the inputs are valid */
            string[] inputsToValidate = { // Inputs to check
                editFields_nameInput.Text,
            };
            return Sys.validateData(inputsToValidate);
        }
        private string createNewDBRow() {
            /* Creates a new CSV-style DB row */
            string newLine = "";
            newLine += editFields_IDinput.Text + ";";
            newLine += editFields_nameInput.Text + ";";
            newLine += editFields_descInput.Text + ";";
            newLine += editFields_countInput.Text + ";";
            newLine += editFields_makatInput.Text + ";";
            newLine += editFields_makatFactoryInput.Text + ";";
            newLine += editFields_locatiomInput.Text + ";";
            newLine += editFields_commentsInput.Text + ";";
            newLine += editFields_signInput.Text + "";
            return newLine;
        }

        string parseTitle(char mode) {
            /* Returns title according to the desired mode */
            this.mode = mode;
            if (mode == 'n') {
                return "יצירת פריט חדש";
            } else if (mode == 'e') {
                return "עריכת פריט";
            } else {
                return "";
            }
        }

        private void initLV() {
            /* Loads the list view */
            ListViewItem item = new ListViewItem(this.data["ID"]);
            for (int i = 1; i < Sys.indexesNames.Length; i++) {
                item.SubItems.Add(this.data[Sys.indexesNames[i]]);
            }
            mainList.Items.Add(item);
        }

        private void loadInputs() {
            /* Loads the textbox inputs, with default texts */
            editFields_IDinput.Text = this.data["ID"];
            editFields_nameInput.Text = this.data["name"];
            editFields_descInput.Text = this.data["data"];
            editFields_countInput.Value = Int32.Parse(this.data["count"]);
            editFields_makatInput.Text = this.data["makat"];
            editFields_makatFactoryInput.Text = this.data["makatFactory"];
            editFields_locatiomInput.Text = this.data["location"];
            editFields_commentsInput.Text = this.data["comments"];
            editFields_signInput.Text = this.data["sign"];
        }
    }
}
