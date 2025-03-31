using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WOFFRandomizer
{
    public class Uninstall
    {
        private static (string, string) defineFile(string basepath, string name, RichTextBox log)
        {
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + ".csh");
            string backupCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + "_original.csh");

            if (!File.Exists(backupCSH))
            {
                return ("", "");
            }
            return (sourceCSH, backupCSH);
        }

        private static void copyAndRemoveFile(string source, string backup)
        {
            // Simple way to handle if file is already uninstalled.
            if (source != "")
            {
                File.Copy(backup, source, true);
                File.Delete(backup);
            }
            
        }

        public static void clearLogs()
        {
            string currDir = Directory.GetCurrentDirectory();
            System.IO.File.WriteAllText(currDir + "/logs/seed.txt", "");
            System.IO.File.WriteAllText(currDir + "/logs/monster_log.txt", "");
        }
        public static void Run(string basepath, RichTextBox log)
        {
            
            (string sourceMBCSH, string backupMBCSH) = defineFile(basepath, "mirageboard_data", log);
            (string sourceEGLSH, string backupEGLSH) = defineFile(basepath, "enemy_group_list", log);
            (string sourceCESLSH, string backupCESLSH) = defineFile(basepath, "character_enemy_status_list", log);
            (string sourceSLSH, string backupSLSH) = defineFile(basepath, "shop_list", log);
            (string sourceMPSH, string backupMPSH) = defineFile(basepath, "monster_place", log);

            copyAndRemoveFile(sourceMBCSH, backupMBCSH);
            copyAndRemoveFile(sourceEGLSH, backupEGLSH);
            copyAndRemoveFile(sourceCESLSH, backupCESLSH);
            copyAndRemoveFile(sourceSLSH, backupSLSH);
            copyAndRemoveFile(sourceMPSH, backupMPSH);
            clearLogs();

            log.AppendText("Uninstallation complete. Thank you for playing!\n\n");
        }
    }
}
