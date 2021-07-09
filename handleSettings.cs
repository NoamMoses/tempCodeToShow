using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rashmatz1 {
    class handleSettings {
        private string settingsPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
        // handleLog myLog;
        public string[] SETTINGS_CONTENT;
        public int SETTINGS_ROWS_LEN;
        private void createSettingsIfNotExist() {
            /* Checks if a db.txt file exists - and creating one if not */
            string fullPath = settingsPath + "\\" + Sys.SETTINGS_NAME; // Path with file name
            if (!File.Exists(fullPath)) {
                using (FileStream settingsFile = File.Create(fullPath)) {
                }
                // File.Create(fullPath).Close(); // Just a different way for creating and closing a file
                this.settingsPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                setSettingsDir(this.settingsPath);
                handleLog myLog = new handleLog();
                myLog.write("Settings file created");
            } else { // Already exists
                this.settingsPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath); // getProperty() hasn't been set yet??? returns empty
            }
        }

        public handleSettings() {
            Console.WriteLine("* handleSettings constructor *");
            createSettingsIfNotExist();
            SETTINGS_CONTENT = File.ReadAllLines(settingsPath + "\\" + Sys.SETTINGS_NAME);
            SETTINGS_ROWS_LEN = SETTINGS_CONTENT.Length;
        }

        public void setSettingsDir(string dir) {
            /* Sets the data-direction, path is given via parameter (dir) */
            string[] content = { // New content to be written to file, in case there was no previous data
                "data;" + dir
            };
            string currentDir = getSettingsDir();
            if (currentDir == "") { // No previous data, need to hard-code one
                File.AppendAllLines(dir + "\\" + Sys.SETTINGS_NAME, content);
            } else { // There is a previous data, just need to update it
                updateSettings("data", dir);
            }
        }

        public string getSettingsDir() {
            /* Returns the current data-directory */
            return getProperty("data");
        }

        public int getIndexByProperty(string index) {
            /* Returns the index (line number) of a settings' field, by its PROPERTY name */
            int i,
                foundAt = -1; // Default value. Means the item was not found
            string[] lineSplit;
            string currentID;
            for (i = 0; i < SETTINGS_ROWS_LEN; i++) {
                lineSplit = SETTINGS_CONTENT[i].Split(';');
                currentID = lineSplit[0];
                if (currentID == index) { // Item found
                    foundAt = i;
                    break; // Just in case
                }
            }
            return foundAt;
        }

        public string getProperty(string property) {
            /*
             * Returns a settings'-property.
             * Example:
             *   Settings-line: "data;C:\\.."
             *   Input: "data"
             *   Returns: "C:\\.."
            */
            string[] content = File.ReadAllLines(settingsPath + "\\" + Sys.SETTINGS_NAME),
                    newContent = new string[content.Length];
            int i;
            string[] lineSplit;
            string propertyValue = "";
            for (i = 0; i < content.Length; i++) {
                lineSplit = content[i].Split(';');
                if (lineSplit[0] == property) { // Desired property
                    propertyValue = lineSplit[1];
                    break; // Just in case
                }
            }
            return propertyValue;
        }

        public void updateSettings(string property, string newValue) {
            /*
             * Gets a settings-property via parameter (property)
             * And updating it to a new value, provided via parameter (newValue)
            */
            string fullPath = settingsPath + "\\" + Sys.SETTINGS_NAME;
            string[] content = File.ReadAllLines(fullPath),
                    newContent = new string[content.Length];
            int i;
            string[] lineSplit;
            string newLine, lastValue = "";
            handleLog myLog = new handleLog();
            for (i = 0; i < content.Length; i++) {
                // Copy the file
                newContent[i] = content[i];
            }
            for (i = 0; i < newContent.Length; i++) {
                lineSplit = newContent[i].Split(';');
                if (lineSplit[0] == property) { // Desired line
                    newLine = property + ";" + newValue;
                    lastValue = lineSplit[1]; // Saving the last data, for the log file
                    newContent[i] = newLine; // Update the line
                    break; // Just in case
                }
            }
            File.WriteAllLines(fullPath, newContent); // Writes the new data
            myLog.write("Data folder has been changed. " + lastValue + " => " + newValue);
        }
    }
}
