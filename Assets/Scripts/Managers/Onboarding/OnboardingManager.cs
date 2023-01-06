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
        List<KeyConcepts> concepts = new List<KeyConcepts>
        {
                 new KeyConcepts() {keyNumber = 0, keyName = "Sultan Agung",
        keyDesc = "Sultan Agung adalah raja dari Kerajaan Mataram. Pada masanya, Kerajaan Mataram mencapai masa kejayaan. Ia memiliki cita-cita untuk menyatukan seluruh tanah jawa dan menyingkirkan kekuasaan asing dari bumi Nusantara.",
        unlocked = false},
            new KeyConcepts() {keyNumber = 1, keyName = "Latar Belakang Serangan Sultan Agung",
        keyDesc = "Awalnya Belanda dan Mataram memiliki hubungan yang baik. Hal ini berubah karena keserakahan dari Belanda. J.P. Coen yang saat itu telah diangkat menjadi Gubernur Jendral meskipun belum secara resmi, pada tanggal 18 November 1618 memerintahkan Van der Marct menyerang Jepara dan menyebabkan kerugian yang besar bagi Mataram. Hal ini menyebabkan hubungan Belanda dan Mataram jadi buruk.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 2, keyName = "Jan Pieterzoon Coen",
        keyDesc = "J.P. Coen adalah Gubernur Jenderal yang berkuasa dari tahun 1619-1623 dan 1627-1629. Dalam masa kekuasaannya, ia mengeksploitasi hasil bumi setelah memperoleh kontrol penuh atas kota Batavia pada tahun 1619. Hal ini menyebabkan para penguasa di pulau Jawa tidak suka dengan keberadaannya dan VOC secara keseluruhan.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 3, keyName = "Serangan Pertama Sultan Agung",
        keyDesc = "Pada Agustus 1628, pasukan Mataram dibawah pimpinan Tumenggung Baureksa menyerang Batavia. Selanjutnya, menyusul pasukan Tumenggung Sura Agul-Agul, dan kedua bersaudara yaitu Kiai Dipati Mandurojo dan Upa Santa pada Oktober 1628. Terjadi peperangan sengit antara pasukan Mataram dan VOC. Namun serangan ini gagal karena kurangnya perbekalan serta persenjataan VOC yang lebih modern. Akhirnya, Mataram terpaksa mundur terlebih dahulu pada 6 Desember 1628.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 4, keyName = "Serangan Kedua Sultan Agung",
        keyDesc = "Sultan Agung tidak menyerah dan terus melancarkan serangan, tetapi tidak ada yang berhasil. Sepeninggal Sultan Agung di tahun 1645, Kerajaan Mataram semakin lemah sehingga akhirnya dapat dikuasai VOC.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 5, keyName = "Akhir Serangan Sultan Agung",
        keyDesc = "Sultan Agung tidak menyerah dan terus melancarkan serangan, tetapi tidak ada yang berhasil. Sepeninggal Sultan Agung di tahun 1645, Kerajaan Mataram semakin lemah sehingga akhirnya dapat dikuasai VOC.",
        unlocked = false},

        //keyconcept chapter 2
        new KeyConcepts() {keyNumber = 6, keyName = "Latar belakang Perang Padri",
        keyDesc = "Bermula dari adanya pertentangan antara kaum Padri dengan kaum Adat dalam masalah praktik keagamaan sehingga terjadi bentrokan antar keduanya.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 7, keyName = "Kaum Padri",
        keyDesc = "Mereka melakukan pembaruan atau pemurnian ajaran Islam di Minangkabau. Mereka menentang adat dan kebiasaan kaum Adat yang tidak sesuai dengan ajaran Islam.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 8, keyName = "Kaum Adat",
        keyDesc = "Mereka melakukan hal-hal yang dilarang ajaran Islam seperti berjudi, sabung ayam, dan minum-minuman keras. Akibatnya, kaum Padri menentang perbuatan tersebut. Sehingga, kaum Adat yang mendapat dukungan dari beberapa pejabat penting kerajaan menolak gerakan kaum Padri.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 9, keyName = "Meletusnya Perang Padri",
        keyDesc = "James Du Puy diangkat menjadi residen di Minangkabau pada tahun 1821. Ia bekerja sama dengan kaum Adat melalui sebuah perjanjian persahabatan dengan tokoh Adat yaitu Tuanku Suruaso dan 14 Penghulu Minangkabau pada 10 Februari 1821. Belanda yang telah diberi kemudahan oleh kaum Adat berhasil menduduki Simawang. Tindakan Belanda ini ditentang keras oleh kaum Padri. Pada tahun 1821, meletuslah Perang Padri.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 10, keyName = "Serangan Tuanku Lintau",
        keyDesc = "Tuanku Lintau menggerakan 20.000 sampai 25.000 pasukan untuk mengadakan serangan di sekitar hutan di sebelah timur gunung. Mereka menggunakan senjata tradisional seperti tombak dan parang. Sedangkan, Belanda dengan kekuatan 200 orang serdadau Eropa ditambah 10.000 orang pribumi termasuk kaum Adat. Belanda menggunakan senjata yang lebih modern seperti meriam dan senjata api lainnya.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 11, keyName = "Dampak Serangan Tuanku Lintau",
        keyDesc = "Pertempuran ini memakan banyak orang. Tuanku Lintau kehilangan 350 orang perajurit, termasuk putranya. Tuanku Lintau dan sisa pasukannya kemudian mengundurkan diri ke Lintau. Setelah pasukan Belanda berhasil menguasai seluruh lembah Tanah Datar, mereka mendirikan sebuah benteng di Batusangkar.",
        unlocked = false},
        new KeyConcepts() {keyNumber = 12, keyName = "Fase Pertama Perang Padri (1821-1825)",
        keyDesc = "Pada tahun 1821, Kaum Padri menyerang pos-pos dan pencegatan terhadap patroli-patroli Belanda. Tuanku Lintau memusatkan perjuangannya di Lintau. Tuanku Nan Renceh memimpin pasukannya di sekitar Baso. Bulan September 1822, kaum Padri berhasil mengusir Belanda dari Sungai Puar, Guguk Sigandang, dan Tajong Alam. Pada tahun 1823 pasukan Padri berhasil mengalahkan tentara Belanda di Kapau.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 13, keyName = "Perjanjian Masang",
        keyDesc = "Belanda merasa kewalahan sehingga tercapailah perundingan damai yang disebut dengan Perjanjian Masang yang terjadi di wilayah Alahan Panjang pada tanggal 26 Januari 1824. Tetapi, Belanda memanfaatkan perjanjian tersebut untuk menduduki daerah-daerah lain. Akhirnya, perjanjian ini dibatalkan akibat ditangkapnya Tuanku Mensiangan yang menimbulkan amarah kaum Padri. Kaum Padri menyatakan pembatalan kesepakatan dalam Perjanjian Masang. Tuanku Imam Bonjol kembali melawan Belanda dan perlawanan masih berlangsung di berbagai tempat.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 14, keyName = "Fase kedua Perang Padri (1825-1830)",
        keyDesc = "Belanda mengusahakan upaya damai karena mereka berkonsentrasi pada Perang Diponegoro. Tetapi, kaum Padri tidak begitu menghiraukannya. Sehingga, Belanda mengutus Sulaiman Aljufri untuk mendekati dan membujuk kaum Padri untuk berdamai. Tuanku Imam Bonjol menolak, tetapi Tuanku Lintau dan Tuanku Nan Renceh merespon ajakan damai itu sehingga terjadilah Perjanjian Padang.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 15, keyName = "Perjanjian Padang",
        keyDesc = "Perjanjian damai dari Belanda yang ditandatangani pada tanggal 15 November 1825. Isinya adalah sebagai berikut: Belanda mengakui kekuasaan pemimpin Padri di Batusangkar, Saruaso, Padang Guguk Sigandang, Agam, Bukittinggi dan menjamin pelaksanaan sistem agama di daerahnya; kedua belah pihak tidak akan saling menyerang; kedua pihak akan melindungi para pedagang dan orang-orang yang sedang melakukan perjalanan; secara bertahap Belanda akan melarang praktik adu ayam.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 16, keyName = "Fase Ketiga Perang Padri (1830-1837/1838)",
        keyDesc = "Pada pertempuran fase ketiga ini kaum Padri mulai mendapatkan simpati dari kaum Adat. Dengan demikian, kekuatan para pejuang di Sumatera Barat meningkat. Orang-orang Padri yang mendapatkan dukungan kaum Adat itu bergerak ke pos-pos tentara Belanda. ",
        unlocked = false},
         new KeyConcepts() {keyNumber = 17, keyName = "Penyerangan Gillavary",
        keyDesc = "Tindakan ini membuat Belanda menyerang Koto Tuo di Ampek Angkek, yang pasukannya dipimpin oleh Gillavary.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 18, keyName = "Penyerangan Elout",
        keyDesc = "Pada tahun 1831, Gillavary digantikan oleh Jacob Elout. Pada Agustus 1831 Belanda yang dipimpin Jacob Elout dapat menguasai Benteng Marapalam, yang merupakan kunci untuk menguasai Lintau, akibatnya beberapa nagari di sekitarnya ikut menyerah. Datangnya bantuan pasukan dari Jawa pada tahun 1832 membuat Belanda semakin ofensif terhadap kekuatan kaum Padri di berbagai daerah.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 19, keyName = "Penyerangan Francis",
        keyDesc = "Pada tahun 1833, dengan kekuatan yang berlipat ganda Belanda melakukan penyerangan terhadap pos-pos pertahanan kaum Padri. Elout digantikan oleh E. Francis.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 20, keyName = "Plakat Panjang",
        keyDesc = "Plakat Panjang adalah pernyataan atau janji khidmat yang isinya tidak akan ada lagi peperangan antara Belanda dan kaum Padri. Setelah pengumuman Plakat Panjang ini kemudian Belanda mulai menawarkan perdamaian kepada para pemimpin Padri. Dengan kebijakan baru itu beberapa tokoh Padri dikontak oleh Belanda dalam rangka mencapai perdamaian. Beberapa tokoh memenuhi ajakan Belanda untuk berdamai, sementara yang tidak setuju masih melanjutkan perlawanan. Pertahanan terakhir kaum Padri berada di tangan Tuanku Imam Bonjol.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 21, keyName = "Perjuangan pasukan Tuanku Imam Bonjol",
        keyDesc = "Pada tahun 1834 Belanda memusatkan kekuatannya untuk menyerang pasukan Imam Bonjol di Bonjol. Pada bulan Agustus 1835 benteng di perbukitan dekat Bonjol jatuh ke tangan Belanda. Belanda mencoba berdamai, tapi Imam Bonjol mengajukan syarat agar rakyatnya dibebaskan dan nagari tidak diduduki Belanda. Namun, Belanda tidak memberi jawaban dan terus mengepung pertahanan di Bonjol yang dipimpin oleh Francis.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 22, keyName = "Kelanjutan perjuangan pasukan Tuanku Imam Bonjol",
        keyDesc = "Sampai tahun 1836 benteng Bonjol tetap dipertahankan oleh pasukan Padri, tetapi satu per satu pemimpin Padri ditangkap sehingga memperlemah pertahanan pasukan Padri. Namun, di bawah komando Imam Bonjol mereka terus berjuang. Pada tanggal 16 Agustus 1837 Benteng Bonjol berhasil dikepung dari empat penjuru dan berhasil dilumpuhkan. Imam Bonjol dan pejuang lainnya dapat meloloskan diri.",
        unlocked = false},
         new KeyConcepts() {keyNumber = 23, keyName = "Akhir dari Imam Bonjol",
        keyDesc = "Francis kembali menyerukan Imam Bonjol untuk berdamai. Demi menjamin keselamatan warganya, pada tanggal 28 Oktober 1837, Imam Bonjol menerima tawaran damai dari Residen Francis. Ternyata ajakan berunding itu hanya tipu muslihat, karena pada saat datang di tempat perundingan, Imam Bonjol langsung ditangkap. Imam Bonjol kemudian dibawa ke Batavia. Akhirnya, Tuanku Imam Bonjol dibuang ke Cianjur, Jawa Barat. ",
        unlocked = false},
        };

        List<InventorySlots> inventory = new List<InventorySlots>
        {
            new InventorySlots() {slotNumber = 0, itemSaved = null},
            new InventorySlots() {slotNumber = 1, itemSaved = null},
            new InventorySlots() {slotNumber = 2, itemSaved = null},
            new InventorySlots() {slotNumber = 3, itemSaved = null},
            new InventorySlots() {slotNumber = 4, itemSaved = null},
            new InventorySlots() {slotNumber = 5, itemSaved = null},
            new InventorySlots() {slotNumber = 6, itemSaved = null},
            new InventorySlots() {slotNumber = 7, itemSaved = null},
        };
      

        if(genderChoose == 0)
        {
            PlayerPosition initialPosition = new PlayerPosition() {x_pos = 1125.646f, y_pos = 201.7f, z_pos = 796.8095f};
            SaveSlots slot = new SaveSlots() {slot = slotNumber, playerName = chooseName, playerGender = genderChoose, chapterNumber = 0, lastPosition = initialPosition, understandingLevel = 0, missionNumber = 0, goalNumber = 0};
            SaveHandler.instance.saveSlot(slot, slotNumber);
        }

        else
        {
            PlayerPosition initialPosition = new PlayerPosition() {x_pos = 1130.106f, y_pos = 206.8f, z_pos = 804.8292f};
           SaveSlots slot = new SaveSlots() {slot = slotNumber, playerName = chooseName, playerGender = genderChoose, chapterNumber = 0, lastPosition = initialPosition, understandingLevel = 0, missionNumber = 0, goalNumber = 0};
            SaveHandler.instance.saveSlot(slot, slotNumber);
        }

        for(int i=0;i<18;i++)
        {
            SaveHandler.instance.saveKeyconcepts(concepts[i], slotNumber, i);
        }

        for(int j=0;j<8;j++)
        {
            SaveHandler.instance.saveInventory(inventory[j], slotNumber, j);
        }
        
        SceneManage.instance.LoadScene(4);
    }

    public void resetData()
    {
        genderChoose = 0;
        SceneManage.instance.loadPopUp(onboardPanelOne);
        SceneManage.instance.closePopUp(onboardPanelThree);
    }

}
