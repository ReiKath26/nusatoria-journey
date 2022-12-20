using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public static class SaveManager
{
    public static readonly string save_folder = Application.persistentDataPath + "/GameSaves/";

    public static void Init()
    {
        if (!Directory.Exists(save_folder))
        {
            Directory.CreateDirectory(save_folder);
        }
    }

    public static void SaveSettings(string saveString)
    {
        File.WriteAllText(save_folder + "/playersetting.json", saveString);
    }

    public static string LoadSettings()
    {
         if (File.Exists(save_folder + "/playersettings.json"))
        {
            string contents = File.ReadAllText(save_folder + "/playersettings.json");
            return contents;
        }

        else
        {
            return null;
        }
    }

    public static void savePlayerSlot(string slotString, int slotNumber)
    {
        File.WriteAllText(save_folder + "/slotSave_" + slotNumber + ".json", slotString);
    }

    public static string LoadSlots(int slotNumber)
    {
         if (File.Exists(save_folder + "/slotSave_" + slotNumber + ".json"))
        {
            string contents = File.ReadAllText(save_folder + "/slotSave_" + slotNumber + ".json");
            return contents;
        }

        else
        {
            return null;
        }
    }
}
