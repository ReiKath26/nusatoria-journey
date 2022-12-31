using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnboardingManager : MonoBehaviour
{
    private int genderChoose = 0;
    private int slotNumber = 1;
    private string chooseName;

    [SerializeField] private GameObject onboardPanelOne;
    [SerializeField] private GameObject onboardPanelTwo;
    [SerializeField] private GameObject onboardPanelThree;

    [SerializeField] private GameObject female_picture_highlight;
    [SerializeField] private GameObject male_picture_highlight;
    [SerializeField] private GameObject userNameInput;

    [SerializeField] private GameObject female_picture;
    [SerializeField] private GameObject male_picture;
    [SerializeField] private TextMeshProUGUI player_name_text;

    void Awake()
    {
        slotNumber = PlayerPrefs.GetInt("choosenSlot");
    }

    public void choosePicture(int choosen)
    {
        if (choosen == 0)
        {
            male_picture_highlight.SetActive(true);
            female_picture_highlight.SetActive(false);
        }

        else
        {
            male_picture_highlight.SetActive(false);
            female_picture_highlight.SetActive(true);
        }

        genderChoose = choosen;
    }

    public void ReadStringInput(string s)
    {
        chooseName = s;
    }

    public void saveName()
    {

        if (chooseName == "")
        {
            return;
        }
        SceneManage.instance.loadPopUp(onboardPanelThree);
        SceneManage.instance.closePopUp(onboardPanelTwo);
        displayData();
    }

    public void displayData()
    {
        if (genderChoose == 0)
        {
            male_picture.SetActive(true);
            female_picture.SetActive(false);
        }

        else
        {
            male_picture.SetActive(false);
            female_picture.SetActive(true);
        }

        player_name_text.text = chooseName;
    }

    public void initGame()
    {
        Glossary glossary = new Glossary();

        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 0, keyName = "Sultan Agung", 
        keyDesc = "Sultan Agung adalah raja dari Kerajaan Mataram. Pada masanya, Kerajaan Mataram mencapai masa kejayaan. Ia memiliki cita-cita untuk menyatukan seluruh tanah jawa dan menyingkirkan kekuasaan asing dari bumi Nusantara.",
        unlocked = false});
        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 1, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 2, keyName = "Latar Belakang Serangan Sultan Agung", 
        keyDesc = "Awalnya Belanda dan Mataram memiliki hubungan yang baik. Hal ini berubah karena keserakahan dari Belanda. J.P. Coen yang saat itu telah diangkat menjadi Gubernur Jendral meskipun belum secara resmi, pada tanggal 18 November 1618 memerintahkan Van der Marct menyerang Jepara dan menyebabkan kerugian yang besar bagi Mataram. Hal ini menyebabkan hubungan Belanda dan Mataram jadi buruk.",
        unlocked = false});
        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 3, keyName = "Serangan Pertama Sultan Agung", 
        keyDesc = "Pada Agustus 1628, pasukan Mataram dibawah pimpinan Tumenggung Baureksa menyerang Batavia. Selanjutnya, menyusul pasukan Tumenggung Sura Agul-Agul, dan kedua bersaudara yaitu Kiai Dipati Mandurojo dan Upa Santa pada Oktober 1628. Terjadi peperangan sengit antara pasukan Mataram dan VOC. Namun serangan ini gagal karena kurangnya perbekalan serta persenjataan VOC yang lebih modern. Akhirnya, Mataram terpaksa mundur terlebih dahulu pada 6 Desember 1628.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 4, keyName = "Serangan Kedua Sultan Agung", 
        keyDesc = "Sultan Agung tidak menyerah dan terus melancarkan serangan, tetapi tidak ada yang berhasil. Sepeninggal Sultan Agung di tahun 1645, Kerajaan Mataram semakin lemah sehingga akhirnya dapat dikuasai VOC.",
        unlocked = false});

        glossary.conceptList.Add(new KeyConcepts() {keyNumber = 5, keyName = "Akhir Serangan Sultan Agung", 
        keyDesc = "Sultan Agung tidak menyerah dan terus melancarkan serangan, tetapi tidak ada yang berhasil. Sepeninggal Sultan Agung di tahun 1645, Kerajaan Mataram semakin lemah sehingga akhirnya dapat dikuasai VOC.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 6, keyName = "Latar belakang Perang Padri", 
        keyDesc = "Bermula dari adanya pertentangan antara kaum Padri dengan kaum Adat dalam masalah praktik keagamaan sehingga terjadi bentrokan antar keduanya.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 7, keyName = "Kaum Padri", 
        keyDesc = "Mereka melakukan pembaruan atau pemurnian ajaran Islam di Minangkabau. Mereka menentang adat dan kebiasaan kaum Adat yang tidak sesuai dengan ajaran Islam.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 8, keyName = "Kaum Adat", 
        keyDesc = "Mereka melakukan hal-hal yang dilarang ajaran Islam seperti berjudi, sabung ayam, dan minum-minuman keras. Akibatnya, kaum Padri menentang perbuatan tersebut. Sehingga, kaum Adat yang mendapat dukungan dari beberapa pejabat penting kerajaan menolak gerakan kaum Padri.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 9, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});

         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 10, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 11, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 12, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 13, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 14, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});

         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 15, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 16, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});
         glossary.conceptList.Add(new KeyConcepts() {keyNumber = 17, keyName = "Jan Pieterzoon Coen", 
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false});




        Inventory inventory = new Inventory();
        inventory.slotList.Add(new InventorySlots() {slotNumber = 0, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 1, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 2, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 3, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 4, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 5, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 6, itemSaved = null});
        inventory.slotList.Add(new InventorySlots() {slotNumber = 7, itemSaved = null});

        PlayerPosition initialPosition = new PlayerPosition() {x_pos = 1125.646f, y_pos = 201.7f, z_pos = 796.8095f};
        SaveSlots slot = new SaveSlots() {slot = slotNumber, playerName = chooseName, playerGender = genderChoose,
        time = 0, chapterNumber = 0, lastPosition = initialPosition, understandingLevel = 0, missionNumber = 0, goalNumber = 0, 
        player_glossary = glossary, player_inventory = inventory};
        SaveHandler.instance.saveSlot(slot, slotNumber);
        SceneManage.instance.LoadScene(4);
    }

    public void resetData()
    {
        genderChoose = 0;
        SceneManage.instance.loadPopUp(onboardPanelOne);
        SceneManage.instance.closePopUp(onboardPanelThree);
    }

}
