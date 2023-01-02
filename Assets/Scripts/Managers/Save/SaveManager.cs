using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public static class SaveManager
{
    public static readonly string save_folder = Application.dataPath + "/GameSaves/";
    public static readonly string keyconcept_object_save_folder = Application.dataPath + "/KeyConcepts/";
    public static readonly string inventory_object_save_folder = Application.dataPath + "/Inventory/";

    public static void Init()
    {
        if (!Directory.Exists(save_folder))
        {
            Directory.CreateDirectory(save_folder);
        }

        if (!Directory.Exists(keyconcept_object_save_folder))
        {
            Directory.CreateDirectory(keyconcept_object_save_folder);
        }

        if (!Directory.Exists(inventory_object_save_folder))
        {
            Directory.CreateDirectory(inventory_object_save_folder);
        }
    }

    public static void SaveSettings(string saveString)
    {
        File.WriteAllText(save_folder + "/playersetting.json", saveString);
    }

    public static string LoadSettings()
    {
        if (File.Exists(save_folder + "/playersetting.json"))
        {
            string contents = File.ReadAllText(save_folder + "/playersetting.json");
            return contents;
        }

        else
        {
            Debug.Log("File Not Found");
            return null;
        }
    }

    public static void saveGlossary(string saveString, int slotNumber, int keyNumber)
    {
        File.WriteAllText(keyconcept_object_save_folder + "/playerGlossary_" + slotNumber + "_" + keyNumber + ".json", saveString);
    }

    public static string loadGlossary(int slotNumber, int keyNumber)
    {
        if (File.Exists(keyconcept_object_save_folder + "/playerGlossary_" + slotNumber + "_" + keyNumber + ".json"))
        {
            string contents = File.ReadAllText(keyconcept_object_save_folder + "/playerGlossary_" + slotNumber + "_" + keyNumber + ".json");
            return contents;
        }

        else
        {
            Debug.Log("File Not Found");
            return null;
        }
    }

    public static void saveInventory(string saveString, int slotNumber, int keyNumber)
    {
        File.WriteAllText(inventory_object_save_folder + "/playerInventory_" + slotNumber + "_" + keyNumber + ".json", saveString);
    }

    public static string loadInventory(int slotNumber, int keyNumber)
    {
        if (File.Exists(inventory_object_save_folder + "/playerInventory_" + slotNumber + "_" + keyNumber + ".json"))
        {
            string contents = File.ReadAllText(inventory_object_save_folder + "/playerInventory_" + slotNumber + "_" + keyNumber + ".json");
            return contents;
        }

        else
        {
            Debug.Log("File Not Found");
            return null;
        }
    }

    public static void savePlayerSlot(string slotString, int slotNumber)
    {
        File.WriteAllText(save_folder  + "/slotSave_" + slotNumber + ".json", slotString);
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
