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
        private static void DefineCopyAndRemoveFile(string basepath, string name)
        {
            string sourceCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + ".csh");
            string backupCSH = Path.GetFullPath(basepath + "/resource/finalizedCommon/mithril/system/csv/" + name + "_original.csh");

            if (!File.Exists(backupCSH))
            {
                return;
            }
            else if (sourceCSH != "")
            {
                File.Copy(backupCSH, sourceCSH, true);
                File.Delete(backupCSH);
            }
        }

        private static void DefineCopyAndRemoveOtherFile(string basepath, string path, string name)
        {
            string source = Path.GetFullPath(basepath + path + name);
            string backup = Path.GetFullPath(basepath + path + name + "_original");

            if (!File.Exists(backup))
            {
                return;
            }
            else if (source != "")
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
            System.IO.File.WriteAllText(currDir + "/logs/item_log.txt", "");
            System.IO.File.WriteAllText(currDir + "/logs/quest_log.txt", "");
            System.IO.File.WriteAllText(currDir + "/logs/mirageboard_log.txt", "");
        }

        public static void Run(string basepath, RichTextBox log, Button button1, Button button2, Button button3)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;

            DefineCopyAndRemoveFile(basepath, "mirageboard_data");
            DefineCopyAndRemoveFile(basepath, "enemy_group_list");
            DefineCopyAndRemoveFile(basepath, "character_enemy_status_list");
            DefineCopyAndRemoveFile(basepath, "shop_list");
            DefineCopyAndRemoveFile(basepath, "monster_place");
            DefineCopyAndRemoveOtherFile(basepath, "/resource", "/script64.bin");
            DefineCopyAndRemoveFile(basepath, "character_resource_list");
            DefineCopyAndRemoveFile(basepath, "command_ability_param");
            DefineCopyAndRemoveFile(basepath, "arena_reward_table_list");
            DefineCopyAndRemoveFile(basepath, "quest_data_sub_reward_table_list");
            DefineCopyAndRemoveFile(basepath, "character_list");

            clearLogs();

            log.AppendText("Uninstallation complete. Thank you for playing!\n\n");
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }
    }
}
