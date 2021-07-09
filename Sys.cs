using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rashmatz1 {
    class Sys {
        public const string DB_NAME = "db.txt",
            LOG_NAME = "log.txt",
            SETTINGS_NAME = "settings.txt",
            credits = "פותח על-ידי נעם מערכות\"ש.    חתומוזס 2021 ©",
            version = "1.0",
            lastUpdated = "03/07/2021";
        public const int ID_MIN = 0, ID_MAX = 1000, // IDs range
            INPUT_MIN_LEN = 2; // Minimum valid input length
        public static string[] indexesNames = {
            "ID", "name", "data", "count", "makat", "makatFactory", "location", "comments", "sign"
        };

        public static Dictionary<string, string> returnItemObj() {
            Dictionary<string, string> data = new Dictionary<string, string>();
            for (int i = 0; i < indexesNames.Length; i++) {
                data.Add(indexesNames[i], "");
            }
            /*
            data.Add("ID", "");
            data.Add("name", "");
            data.Add("data", "");
            data.Add("count", "");
            data.Add("makat", "");
            data.Add("makatFactory", "");
            data.Add("location", "");
            data.Add("comments", "");
            data.Add("sign", "");
            */

            return data;
        }


        public static DialogResult showRTLConfirmationDialog(string title, string msg, MessageBoxButtons buttons) {
            /* Shows a confirmation dialog, in RTL */
            DialogResult confirm = MessageBox.Show(msg, title, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading);
            return confirm;
        }

        public static bool validateData(string[] data) {
            /*
             * Checks whether the data provided is valid or not.
             * Right now, the only condition is that the input is at least INPUT_MIN_LEN (2) charachters long.
            */
            int i;
            bool valid = true;
            for (i = 0; i < data.Length; i++) {
                if (data[i].Length < INPUT_MIN_LEN) {
                    valid = false;
                    break;
                }
            }
            return valid;
        }

        public static void setVersionLabel(Label label) {
            /*
             * Sets the version (and last updated) label.
             * Gets the label via parameter, since the function can't access controls.
            */
            label.Text = "גרסה: " + version + " | " + lastUpdated;
        }


        /*
         * Returns the date and hour - with an option to use a specific format.
         * Default format is en-GB: 03/07 00:27:11
        */
        public static string getDate(string format) {
            return DateTime.Now.ToString(new CultureInfo(format));
        }

        public static string getDate() {
            return DateTime.Now.ToString(new CultureInfo("en-GB"));
        }
    }
}
