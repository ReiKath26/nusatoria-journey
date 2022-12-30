using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionProvider : MonoBehaviour
{
    public static MissionProvider instance;

    void Awake()
    {
        instance = this;
    }

    public List<Mission> getChapterMissions(int chapter)
    {
        List<Mission> mission;
        if(chapter == 0)
        {
            mission = new List<Mission>
            {
                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke Kota Terdekat untuk Mencari Informasi", 1, new string[] {"Sultan Agung"}, new int[] {2}, new Story[] {
                        new Story("Kamu bertemu dengan seseorang didekat gerbang kota...", new List<Dialogs>
                        {
                            new NPCDialog("???", "Hei, kamu!", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Eh? Apa orang itu berbicara padaku?)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Aneh...jangan-jangan aku benar-benar terlempar ke masa lalu)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi sebaiknya aku menanggapinya agar tidak tampak mencurigakan)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Iya...pak?", null),
                            new NPCDialog("???", "Hahaha, baru kali ini saya dipanggil pak oleh warga saya", null),
                            new NPCDialog("???", "Atau jangan-jangan kamu orang baru disini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Bisa...dibilang begitu?", null),
                            new NPCDialog("???", "Oh kalau begitu, selamat datang di Mataram!", null),
                            new NPCDialog("???", "Saya harap saya dapat menjamu tamu seperti kamu dengan lebih baik", null),
                            new NPCDialog("???", "Tapi kami ditengah situasi cukup genting", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Situasi cukup genting?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Oh iya, tapi saya belum tau siapa bapak", null),
                            new NPCDialog("???", "Ah iya benar, saya belum memperkenalkan diri", null),
                            new NPCDialog("Sultan Agung", "Saya Sultan Agung, raja dari Kerajaan Mataram ini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Raja?!)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aduh, maafkan ketidaksopanan saya, paduka raja", null),
                            new NPCDialog("Sultan Agung", "Sudah-sudah tidak perlu meminta maaf, dan panggil saya sultan saja ya", null),
                            new NPCDialog("Sultan Agung", "Dan sekali lagi saya meminta maaf karena saya tidak menjamu dek...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Player, Sultan", null),
                            new NPCDialog("Sultan Agung", "Ah iya, dek Player dengan lebih baik, karena kami sedang merencanakan sebuah perang", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Merencanakan perang? Dengan siapa?", null),
                            new NPCDialog("Sultan Agung", "Oh tentu dengan orang-orang Belanda menyebalkan itu...", null),
                            new NPCDialog("Sultan Agung", "Siapa nama mereka...ah iya, VOC, terutama pimpinan mereka itu, J.P. Coen", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Aku sepertinya pernah mendengar tentang mereka sebelumnya di sekolah...", null),
                            new NPCDialog("Sultan Agung", "Saya tidak tau ada sekolah yang mengajarkan tentang itu", null),
                            new NPCDialog("Sultan Agung", "Tapi intinya orang-orang di VOC itu ingin sekali menguasai perdagangan di pulau Jawa ini", null),
                            new NPCDialog("Sultan Agung", "Dan itu membawa penderitaan bagi rakyat disini", null),
                            new NPCDialog("Sultan Agung", "Lagi, pekerjaan mereka itu juga menghalangi saya menggapai cita-cita saya", null),
                            new NPCDialog("Sultan Agung", "Untuk menyatukan seluruh tanah jawa dan menghilangkan pengaruh mereka dari tanah ini", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi apa yang mereka lakukan sampai Sultan merencanakan untuk berperang dengan mereka...", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "...dan tidak menempuh jalur damai terlebih dahulu? ", null),
                            new NPCDialog("Sultan Agung", "Orang-orang Belanda itu tidak bisa dipercaya sama sekali, dek Player", null),
                            new NPCDialog("Sultan Agung", "Baru saat mereka mencoba menjalin hubungan baik dengan kerajaan saya...", null),
                            new NPCDialog("Sultan Agung", "Mereka malah menyerang kota Jepara 18 November tahun kemarin, 1618", null),
                            new NPCDialog("Sultan Agung", "Dan itu adalah ulah dari pemimpin mereka, si J.P Coen itu", null),
                            new NPCDialog("Sultan Agung", "Ia memerintahkan si Van der Marct itu untuk menyerang, dan membawa kerugian besar bagi kami", null),
                            new NPCDialog("Sultan Agung", "Itu semua karena mereka serakah dan gila kekuasaan", null),
                            new NPCDialog("Sultan Agung", "Karena itu lah, sudah saatnya Mataram membawa urusan ini ke Medan Perang", null),
                            new NPCDialog("Sultan Agung", "....sekarang kita tinggal melihat situasi disana", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Situasi di tempat anda akan menyerang, Sultan?", null),
                            new NPCDialog("Sultan Agung", " Iya, di pusat pemerintahan mereka di Batavia", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Tapi bukannya itu jauh sekali dari sini...pasukan anda akan menempuh perjalanan yang cukup panjang untuk itu...", null),
                            new NPCDialog("Sultan Agung", "Tidak apa-apa, semuanya akan dipersiapkan dengan baik, dek Player", null),
                            new NPCDialog("Sultan Agung", "Bicara soal itu...apa kamu ada rencana tertentu, dek Player?", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Bicara soal itu...apa kamu ada rencana tertentu, dek Player?", null),
                            new NPCDialog("Sultan Agung", "Kalau kamu tidak keberatan, saya ingin meminta bantuanmu untuk mengintai situasi di Batavia", null),
                            new NPCDialog("Sultan Agung", "Supaya saat serangan besok, kami sudah lebih siap dengan medan dan situasi yang ada", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, tapi kemana arah menuju Batavia?", null),
                            new NPCDialog("Sultan Agung", "Ikuti saja jalur ini dan nanti kamu akan sampai ke Batavia", null),
                            new NPCDialog("Sultan Agung", "Saya akan menunggu kabar dari kamu", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Jadi Sultan Agung ini adalah raja Mataram pada masa pendudukan VOC)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Dan karena serangan VOC ke Jepara tanggal 18 November 1618 lalu, ia jadi akan melakukan serangan pada VOC)", null),
                            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencatat info ini, siapa tau aku memerlukannya nanti)", null),

                        }, false)})}, null), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi Mengintai ke Kota Batavia", 1, new string[] {"Batavia Fort Gate"}, new int[] {-1}, new Story[] {
                        new Story("Setelah perjalanan cukup panjang, kamu sampai di Batavia...", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Huftt...hufftt...", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Lelah sekali...untungnya jarak Batavia dan Mataram tidak sebegitu jauh seperti Jakarta dan Jawa Tengah di masaku...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi apakah memang seharusnya begitu?)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Memang ada sesuatu yang aneh tentang tempat ini...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Sebaiknya aku mencari petunjuk di kota ini untuk aku laporkan kepada Sultan Agung)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Dan siapa tau juga aku bisa menemukan petunjuk tentang kemana mesin aneh tadi itu membawaku)", null),

        }, false)
                    })
                }, null), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Mencari petunjuk di Batavia", 3, new string[] {"Red Book", "Crates Box", "Belanda Soldier"}, new int[] {1, -1, -1}, new Story[] {
                        new Story("Kamu menemukan sebuah buku merah tergeletak di tanah...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Siapa yang menaruh file-file berserakan seperti ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi file apa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, " (Hmmm....)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Eh kenapa ada info-info terkait J.P. Coen disini?)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Mari kita lihat...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi J.P. Coen memerintah dari 1618-1623 dan 1627-sekarang...aku asumsikan 1628...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ada hal lain kah yang harus aku tau...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Hmmmm...dari data-data disini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Pengeluaran yang dikeluarkan VOC untuk membeli sebanyak ini hasil bumi sangat sedikit)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak perlu belajar akuntansi untuk mengetahui ini aneh...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Mereka pasti mengeksploitasi hasil bumi disini dan angkanya terus meningkat)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Memang serakah orang-orang itu, tapi sebaiknya aku menyimpan info ini untuk aku review kembali nantinya)", null),
    

        }, false),
                        new Story("Diujung kota terdapat banyak kotak-kotak barang yang tertata rapi...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Kotak-kotak ini semua hasil bumi...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Tapi terutama rempah-rempah)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Disini dituliskan semuanya akan dikirimkan ke VOC)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Jadi mereka memang menguasai seluruh jual beli disini ya...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Atau itu hasil dari monopoli mereka...)", null),

        }, false),
                        new Story("Kamu memperhatikan penjaga yang berjaga di barak secara seksama...", new List<Dialogs>
        {
          new MainCharacterDialog(true, characterExpression.neutral, "(Penjaga-penjaga itu...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Sepertinya mereka membawa senapan yang cukup modern...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak tau senjata semacam itu sudah ada di masa ini...)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Ini bisa jadi berbahaya jika Sultan Agung tidak mempersiapkan senjata yang setara)", null),
          new MainCharacterDialog(true, characterExpression.neutral, "(Aku sebaiknya memberitahunya)", null),
        }
, false)
                    })
                }, null), 

                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[] {
                        new Story("Kamu menemukan sebuah buku merah tergeletak di tanah...", new List<Dialogs>
        {
         new MainCharacterDialog(true, characterExpression.neutral, "Sultan, aku kembali!", null),
         new NPCDialog("Sultan Agung", "Ah, kamu sudah kembali, dek Player", null),
         new NPCDialog("Sultan Agung", "Jadi bagaimana hasil pengintaianmu?", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Semuanya sepertinya normal disana", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Dan sepertinya VOC sudah menguasai perdagangan disana karena aku menemukan banyak kotak dan dokumen...", null),
         new MainCharacterDialog(true, characterExpression.neutral, "...yang menunjukan demikian, mereka memonopoli semuanya disana", null),
         new NPCDialog("Sultan Agung", "Hmph, orang-orang serakah itu", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Aku juga melihat kalau para penjaga disana dibekali senapan yang cukup modern", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Aku penasaran...senjata apa yang anda siapkan untuk perang, Sultan?", null),
         new NPCDialog("Sultan Agung", "Senjata perang pada umumnya, bambu runcing dan lainnya", null),
         new NPCDialog("Sultan Agung", "Kita juga memiliki cukup senapan, jadi aku rasa kita sudah cukup siap", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...kalau kau bilang begitu", null),
         new MainCharacterDialog(true, characterExpression.neutral, "(Aku tidak yakin itu cukup tapi ya sudahlah...)", null),
         new NPCDialog("Sultan Agung", "Tapi memang ada baiknya kami mulai mempersiakan perbekalan dengan baik...", null),
         new NPCDialog("Sultan Agung", "Agar tidak dimanfaatkan musuh kalau kami sampai kekurangan perbekalan", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Apakah ada yang bisa aku bantu terkait itu Sultan?", null),
         new NPCDialog("Sultan Agung", "Kamu ini baik sekali, dek Player ", null),
         new NPCDialog("Sultan Agung", "Terkait itu, kamu bisa temui pemimpin pasukan saya, Tumenggung Baurekhsa", null),
         new NPCDialog("Sultan Agung", "Seharusnya dia sekarang ada di gudang perbekalan di dekat Pelabuhan Jepara", null),
         new NPCDialog("Sultan Agung", "Coba kamu kesana dan nanti ikuti saja instruksi darinya", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Baik, Sultan!", null),
        }, false)
                    })
                }, new Story("Setelah pencarian panjang, kamu merasa sudah mendapat cukup informasi..", new List<Dialogs>
        {
         new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, sepertinya aku sudah mendapat informasi yang cukup", null),
         new MainCharacterDialog(true, characterExpression.neutral, "Sebaiknya aku kembali ke tempat Sultan Agung", null),
        }, false)), 

                 new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke gudang perbekalan pasukan Mataram", 1, new string[] {"Tumenggung Baurekhsa"}, new int[] {-1}, new Story[] {
                        new Story("Sesampainya di gudang perbekalan...", new List<Dialogs>
        {
            new NPCDialog("???", "Hmmm...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Permisi pak, apakah anda Tumenggung Baurekhsa?", null),
            new NPCDialog("T.Baurekhsa", " Iya saya sendiri, adek ini siapa", null),
            new NPCDialog("T.Baurekhsa", "Saya Player, saya dikirim Sultan Agung untuk membantu persiapan pasukan disini", null),
            new MainCharacterDialog(true, characterExpression.neutral, "", null),
            new NPCDialog("T.Baurekhsa", "Begitu ya, tapi kami baru saja selesai mempersiapkan semuanya sebenarnya", null),
            new NPCDialog("T.Baurekhsa", "Jadi saya tidak yakin kamu bisa membantu apa lagi disini, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Sebentar ya, saya sedang mengecek daftar perbekalan yang ada", null),
            new NPCDialog("T.Baurekhsa", "Hmmmm....", null),
            new NPCDialog("T.Baurekhsa", " ....", null),
            new NPCDialog("T.Baurekhsa", " Hmmm..kok masih ada yang kurang...", null),
            new NPCDialog("T.Baurekhsa", "Padahal aku sudah meminta pasukan yang lainnya untuk mengecek ulang sebelum menutup gudang...", null),
            new NPCDialog("T.Baurekhsa", "Sekarang aku jadi harus membuka gudang lagi sekaligus mengambil perbekalan yang kurang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Pak, apakah saya bisa membantu mengambilkan perbekalan-perbekalan tersebut?", null),
            new NPCDialog("T.Baurekhsa", "Kamu yakin kamu bisa? Barang-barang perbekalan itu berat, lho?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak apa-apa, saya yakin saya bisa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Kan tinggal aku masukan ke fitur inventory canggih di smartphoneku dan aku bisa membawa barang-barang itu seakan-akan mereka microchip)", null),
            new NPCDialog("T.Baurekhsa", "Baiklah kalau kamu bersikeras", null),
            new NPCDialog("T.Baurekhsa", "Kami membutuhkan 150 ekor sapi, 5.900 karung gula, 25.000 buah kelapa dan 12.000 karung beras lagi", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Itu banyak sekali??", null),
            new NPCDialog("T.Baurekhsa", "Haha..itu jumlah total semuanya saja, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Kami hanya kurang 15 karung gula, 2 buah kelapa, dan 10 karung beras lagi..", null),
            new NPCDialog("T.Baurekhsa", "Biasanya tumpukan karung gula dan beras ada 5 karung sekaligus jadi kamu cari 3 dan 2 tumpukan masing-masing saja", null),
            new NPCDialog("T.Baurekhsa", "Aku tunggu disini saat kamu sudah mendapatkan semua barang itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baik, pak", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, saatnya bekerja..", null),
        }, false)
                    })
                }, null), 

                 new Mission(new List<Goal>
                {
                    new GatherGoal("Mengambil 2 karung beras untuk perbekalan", 2, new string[] {"Rice Sack", "Rice Sack (1)", "Rice Sack (2)", "Rice Sack (3)"}, null),
                    new GatherGoal("Mengambil 3 karung gula untuk perbekalan", 3, new string[] {"Sugar Sack", "Sugar Sack (1)", "Sugar Sack (2)", "Sugar Sack (3)"}, null),
                    new GatherGoal("Mengambil 2 buah kelapa untuk perbekalan", 2, new string[] {"Coconut nut", "Coconut nut (1)", "Coconut nut (2)"}, null)
                }, null), 
                 new Mission(new List<Goal>
                {
                    new SubmitGoal("Berikan perbekalan ke Tumenggung Baurekhsa", 1, "Tumenggung Baurekhsa", new Story[]{
                        new Story("Kamu kembali ke Tumenggung Baurekhsa dengan membawa seluruh perbekalan yang dibutuhkan", new List<Dialogs>
        {
            new NPCDialog("T.Baurekhsa", "Oh, kamu sudah membawakan semua perbekalan yang kami perlukan", null),
            new NPCDialog("T.Baurekhsa", "Terimakasih, dek Player", null),
            new NPCDialog("T.Baurekhsa", "Kalau begitu kami sudah siap untuk melakukan serangan kepada Batavia ", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Apakah anda sudah membuat rencana terkait serangan tersebut?", null),
            new NPCDialog("T.Baurekhsa", "Kami akan membawa semua perbekalan ini menggunakan kapal", null),
            new NPCDialog("T.Baurekhsa", "Dan kami akan bertingkah seakan kami ingin berdagang, untuk menginfiltrasi benteng mereka", null), 
            new NPCDialog("T.Baurekhsa", "Setelah itu kami akan memulai serangan dan mulai mendatangkan pasukan-pasukan tambahan pada bulan-bulan ke depan", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aku masih khawatir terkait persenjataan yang dimiliki pasukan mereka...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Tapi bukan urusanku juga, mereka sudah aku peringatkan)", null),
            new NPCDialog("T.Baurekhsa", "Baiklah, karena persiapan semuanya sudah beres, bisakah kamu sampaikan ini kepada Sultan?", null),
            new NPCDialog("T.Baurekhsa", "Beritahu padanya kami siap menyerang kapan pun ia perintahkan", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sepertinya aku akan kembali ke Mataram)", null),
            new NPCDialog("T.Baurekhsa", "Hmmm...tapi sempat tidak ya...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Kenapa tidak sempat? Aku saja berjalan dari Mataram ke Batavia hanya dalam hitungan menit kok!", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sesuatu yang aneh sih...)", null),
            new NPCDialog("T.Baurekhsa", "ya, tapi waktu sepertinya berjalan secara berbeda untuk orang-orang seperti kamu, dek Player", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Orang seperti aku?", null),
            new NPCDialog("T.Baurekhsa", ".......", null), 
            new NPCDialog("T.Baurekhsa", "Lupakan saja, sebaiknya kamu mulai berjalan ke Mataram sekarang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "......", null),


        }, false)
                    }, new Item[]{
                        new Item("Karung_beras", "Karung Beras", "Karung berisi beras yang merupakan makanan pokok rakyat Nusantara", 2),
                        new Item("Karung_gula", "Karung Gula", "Karung berisi gula dari perkebunan bambu Mataram", 3),
                        new Item("Coconut", "Kelapa", "Kelapa yang tentunya tidak jatuh dari pohon dimana tidak ada pohon kelapa di sekitarnya...", 2)
                    })
                }, new Story("Setelah semua perbekalan terkumpul...", new List<Dialogs>
        {
            new MainCharacterDialog(true, characterExpression.neutral, "Baiklah, saatnya kembali ke Tumenggung Baurekhsa", null),
            
        }, false)), 
                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi untuk mengantarkan pesan kepada Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[]{
                        new Story("Sesampainya kembali ke kota untuk bertemu dengan Sultan Agung...", new List<Dialogs>
        {
            new NPCDialog("Sultan Agung", "Dek Player, kamu akhirnya sampai...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Akhirnya?", null),
            new NPCDialog("Sultan Agung", " Iya, aku mendapat pesan dari Tumenggung Baurekhsa kalau kamu sedang berjalan kemari sekitar sebulan yang lalu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sebulan?! Aku hanya berjalan selama beberapa menit?!", null),
            new NPCDialog("Sultan Agung", "Mungkin karena kamu menikmati keindahan alam dan perjalanan jadi tidak terasa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Aneh sekali...tadi itu benar-benar tidak terasa seperti satu bulan)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Bahkan aku tidak tau tanggal berapa sekarang di dunia antaberanta ini...)", null),
            new NPCDialog("Sultan Agung", "Kamu baik-baik saja, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sultan, kalau boleh bertanya, sekarang ini bulan dan tahun berapa ya?", null),
            new NPCDialog("Sultan Agung", " Agustus 1628, dek MC...Tumenggung Baurekhsa dan pasukannya baru saja akan berangkat ke Batavia", null),
            new NPCDialog("Sultan Agung", "Dan waktu aku pergi ke Batavia waktu itu...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Berapa lama Sultan menunggu sampai aku kembali kemari?", null),
            new NPCDialog("Sultan Agung", "Hmmm...sekitar 3 atau 4 bulan? Saya lupa sih", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Oke...ini tidak benar...waktu memang berjalan sangat aneh di tempat ini....)", null),
            new NPCDialog("Sultan Agung", "Ngomong-ngomong soal itu, saya ingin mengecek kondisi pasukan di Batavia", null),
            new NPCDialog("Sultan Agung", "Tapi jika saya pergi kesana secara langsung sekarang, nanti akan menimbulkan curiga", null),
            new NPCDialog("Sultan Agung", "Bisakah saya meminta bantuan lagi, dek Player?", null),
            new MainCharacterDialog(true, characterExpression.neutral, " Anda ingin saya...kembali ke Batavia lagi?", null),
            new NPCDialog("Sultan Agung", "Kira-kira begitu, saya tau itu perjalanan yang panjang tapi...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tidak apa-apa, Sultan, saya akan kesana", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Lebih baik daripada saya berdiam disini tanpa melakukan apapun", null),
            new NPCDialog("Sultan Agung", "Baik kalau begitu, terimakasih dek Player, kembalilah kemari ketika kamu sudah mendapat info terkait perang yang terjadi", null),
            
        }, false)
                    })
                }, null), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke Batavia untuk memantau jalannya perang", 1, new string[] {"Batavia NPC"}, new int[] {1}, new Story[]{
                        new Story("Dari luar pintu gerbang benteng, terlihat derunya perang antara pasukan Mataram dan Belanda", new List<Dialogs>

        {
            new NPCDialog("Warga", " Hei, kalau saya jadi kamu, saya tidak akan pergi masuk ke benteng", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Memangnya kenapa, pak?", null),
            new NPCDialog("Warga", "Perang besar sedang terjadi disana, warga-warga dari Mataram itu menyerang dan terjadi perang yang sudah berjalan berbulan-bulan sekarang", null),
            new MainCharacterDialog(true, characterExpression.neutral, "....tidak lagi...", null),
            new NPCDialog("Warga", "ya saya setuju, saya juga sudah lelah dengan berbagai perang yang terjadi", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Sekarang ini...memangnya bulan apa, pak?", null),
             new NPCDialog("Warga", "Bulan Desember, tanggal 6, saya rasa perang itu akan mulai mereda beberapa saat lagi...", null),
            new NPCDialog("T. Baurekhsa", " PASUKAN! MUNDUR!", null),
            new MainCharacterDialog(true, characterExpression.neutral, "tu...suara Tumenggung...", null),
            new NPCDialog("Warga", "Yap, orang-orang Mataram itu tidak punya kesempatan melawan Belanda dengan senjata canggih seperti itu", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Tapi...apa yang sebenarnya terjadi disitu...", null),
            new NPCDialog("Warga", "Oh, kamu mau tau? Baiklah, saya tidak biasa memberikan informasi secara gratis, tapi khusus kali ini tidak apa-apa", null),
            new NPCDialog("Warga", "Jadi pada awalnya kapal-kapal dari pasukan orang bernama Tumenggung Baurekhsa itu tiba di Batavia", null),
            new NPCDialog("Warga", "Tidak ada serangan sampai hari ketiga, tapi Belanda curiga dan menaruh pasukan-pasukan mereka di dekat Benteng", null),
            new NPCDialog("Warga", "Sore harinya itu, pasukan Mataram mulai turun dan perang pun diluncurkan", null),
            new NPCDialog("Warga", "Terjadi perlawanan tanpa henti, dan disaat inilah Belanda memanggil pasukan tambahan", null),
            new NPCDialog("Warga", "Kini Belanda memiliki lebih banyak pasukan dan pertempuran menjadi sengit", null),
            new NPCDialog("Warga", "Sekitar 3 bulan setelahnya, pasukan Mataram lain mendarat di Batavia", null),
            new NPCDialog("Warga", " Mereka adalah pasukan dari Tumenggung Sura Agul-Agul, dan kedua bersaudara yaitu Kiai Dipati Mandurojo dan Upa Santa. ", null),
            new NPCDialog("Warga", "Terjadi perang sengit antara kedua kubu", null),
            new NPCDialog("Warga", "Tapi pada akhirnya karena sepertinya mereka kekurangan perbekalan mereka mengalami kekalahan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, " (Aku sudah tau ini akan terjadi)", null),
            new NPCDialog("Warga", "Dan hari ini, akhirnya mereka mundur juga kan...", null),
            new MainCharacterDialog(true, characterExpression.neutral, "Jadi setelah perjuangan itu mereka kalah juga ", null),
            new NPCDialog("Warga", " Iya tapi memang kita tidak akan selalu menang dalam hidup...hanya bisa berusaha sekuat yang kita bisa", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Wow...warga ini filosofikal juga...)", null),
            new MainCharacterDialog(true, characterExpression.neutral, "(Sebaiknya aku mencatat semua informasi itu dan kembali ke Sultan Agung)", null),

        }, false)
                    })
                }, null), 
                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {-1}, new Story[]{
                        new Story("Sultan Agung nampak seperti sedang banyak pikiran, tentu hal ini terkait dengan kekalahan mereka dalam perang", null, false)
                    })
                }, null), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Karawang", 2, new string[] {"Spot Tree 2", "Spot Tree 1"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan lokasi di dekat rumah-rumah warga", null, false),
                        new Story("Kamu menemukan lokasi di dekat pantai", null, false)
                    }),
                    new ExplorationGoal("Membantu mencarikan lokasi lumbung di Cirebon", 2, new string[] {"Spot Tree 4", "Spot Tree 3"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan lokasi di pinggir jalan", null, false),
                        new Story("Kamu menemukan lokasi di balik kawasan rumah warga", null, false)
                    })
                }, null), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Mencari pedagang sekitar yang akan ke Mataram", 1, new string[] {"Pedagang Mataram NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu menemukan seorang pedagang yang nampak mempersiapkan bawaannya...", null, false),
                    }),
                     new SubmitGoal("Berikan peta yang sudah ditandai ke pedagang", 1, "Pedagang Mataram NPC", null, new Item[]{
                        new Item("Map", "Peta yang sudah ditandai", "Peta yang sudah ditandai untuk kebutuhan khusus", 1)
                    })
                }, new Story("Setelah menandai seluruh tempat yang kamu temukan di petamu", null, false)), 

                  new Mission(new List<Goal>
                {
                    new ExplorationGoal("Pergi ke arah yang berlawanan ke Batavia", 1, new string[] {"Batavia NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu bertemu kembali dengan warga yang waktu itu...", null, false)
                    })
                }, null), 

                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Kembali ke Sultan Agung", 1, new string[] {"Sultan Agung"}, new int[] {1}, new Story[]{
                        new Story("Sultan Agung tampak mulai lebih banyak kerut di wajahnya", null, false)
                    })
                }, null), 
                    new Mission(new List<Goal>
                {
                    new ExplorationGoal("Lari ke Pelabuhan Jepara", 1, new string[] {"Kepala Pedagang NPC"}, new int[] {1}, new Story[]{
                        new Story("Sesampainya di pelabuhan, ada sebuah kapal yang mulai bersiap untuk berangkat", null, false)
                    })
                }, null), 

                  new Mission(new List<Goal>
                {
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", true)
                }, null), 

                  new Mission(new List<Goal>
                {
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama Kepala Pedagang", 1, "Kepala Pedagang NPC", new Story[]{
                        new Story("Kepala pedagang selesai memberikan review singkat...", null, false)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", false)
                }, new Story("Kepala pedagang nampak sedikit ragu-ragu", null, false)), 

                 new Mission(new List<Goal>
                {
                     new ExplorationGoal("Lari ke sisi lain pelabuhan", 1, new string[] {"Pedagang Anak NPC"}, new int[] {-1}, new Story[]{
                        new Story("Kamu melihat seorang pedagang yang seperti memanggilmu dari kejauhan...", null, false)
                    }),
                    new ReviewGoal("Mereview kembali beberapa kejadian bersama pedagang", 1, "Pedagang Anak NPC", new Story[]{
                        new Story("Pedagang selesai memberikan review singkat...", null, false)
                    }),
                    new JudgementGoal("Menyelesaikan tantangan Judgement dari Kepala Pedagang", 1, "Kepala Pedagang NPC", false)
                }, new Story("Kepala pedagang terlihat emosi", null, false)), 
                new Mission(new List<Goal>
                {
                    new GatherGoal("Berbicara dengan kepala pedagang", 1, new string[] {"Kepala Pedagang NPC"}, new Story[]{
                        new Story("Setelah perjuangan cukup panjang menjawab semua pertanyaan kepala pedagang, sepertinya kamu sudah mulai mendapat kepercayaannya", null, false)
                    })
                }, null), 
                new Mission(new List<Goal>
                {
                    new ExplorationGoal("Menghampiri orang yang mencurigakan", 1, new string[] {"Yudha"}, new int[] {1}, new Story[]{
                        new Story("Kamu menghampiri orang itu yang sedang sibuk mencari sesuatu", null, false)
                    }),
                     new SubmitGoal("Memberikan lencana kembali kepada Yudha", 1, "Yudha", new Story[]{
                        new Story("Yudha dengan cepat mengambil kembali lencananya darimu", null, false)
                    }, new Item[]{
                        new Item("police_badge", "Lencana Polisi", "Apa ada polisi di sekitar sini?", 1)
                    })
                }, new Story("Kamu memutuskan untuk santai sejenak dan melihat kembali penemuan-penemuan yang sudah kamu temukan", null, false))
            };
        }

        else
        {
            mission = new List<Mission>
            {

            };
        }

        return mission;
    }

    public Story getEndChapterStory(int chapter)
    {
        Story story_end;

        if(chapter == 0)
        {
             story_end = new Story("Dan perjalananmu pun masih berlanjut..", new List<Dialogs>
        {
            new CutsceneDialog("rumah_kosong", "Setelah perjuangan yang cukup panjang dari berbagai daerah lainnya, akhirnya kongsi dagang itu bubar pada tahun 31 Desember 1799", null),
            new CutsceneDialog("rumah_kosong", "Tapi Nusantara belum lepas dari tangan Belanda", null),
            new CutsceneDialog("rumah_kosong", "Pemimpin-pemimpin dari Belanda, dan bahkan Inggris sempat membawa alur pemerintahan di Nusantara", null),
            new CutsceneDialog("rumah_kosong", "Tapi tidak ada dari mereka yang membawa kemakmuran kepada Nusantara, atau Bumiputera, atau Hindia Belanda pada masanya masing-masing", null),
            new CutsceneDialog("rumah_kosong", "Jalan Anyer-Panarukan hingga tanam paksa, semua hasil pengerjaan paksa dari sekian banyak rakyat pada masanya", null),
            new CutsceneDialog("rumah_kosong", "Bahkan penguasa pribumi yang penuh keserakahan justru menambah penderitaan dari rakyat biasa", null),
            new CutsceneDialog("rumah_kosong", "Dan perjuangan dari rakyat Hindia Belanda pun masih terus berlangsung, untuk mengusahakan kemerdekaan mereka...", null)
        }, true);

        }

        else
        {
            story_end = new Story("", null, true);
        }

        return story_end;
    }
}
