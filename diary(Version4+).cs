using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp13
{


    internal class Program
    {

        // Txt dosya yolu.
        static string path = "FILE_PATH"; // Kendi dosya yolunuzu seçin.
        // Program ilk defa mı çalışıyor?
        static bool firstTime = true;
        // Günlük metin ekleme limiti
        static bool dailyLimit = true;
        // Diary adlı listesinin başlangıç indexi.
        static int currentIndex = 0;
        // Kullanıcıları tutması için Dictionary. Key Value
        static Dictionary<string, string> userList = new Dictionary<string, string>();
        // Metinleri tutması için Liste
        static List<string> diary { get; set; } = new List<string>();


        static void Main(string[] args)
        {

            FirstMenu();

        }

        // Ana Menü oluşturma
        static void Menu()
        {
            while (true)
            {

                if (firstTime)
                {

                    firstTime = false;

                }

                else
                {
                    Thread.Sleep(2000);
                    Console.Clear();
                }

                Console.WriteLine("1.Yeni Kayıt Ekle");
                Console.WriteLine("2.Kayıtları Listele");
                Console.WriteLine("3.Tüm kayıtları sil");
                Console.WriteLine("4.Kayıt Ara");
                Console.WriteLine("5.Programı Durdur.");

                MenuSelection();

            }
        }

        // Menü Seçim
        static void MenuSelection()
        {
            int choice = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

            if (choice == 1)
            {

                AddNewRecord();
            }

            else if (choice == 2)
            {
                HideRecords();

            }

            else if (choice == 3)
            {
                DeleteAllRecords();

            }

            else if (choice == 4)
            {

                FindRecords();

            }

            else if (choice == 5)
            {
                Console.WriteLine("Program durduldu.");
                Environment.Exit(0);


            }

            else
            {

                Console.WriteLine("Hatalı tuşlama yaptınız!");
            }
        }

        // Tarih
        static DateTime GetDateNow()
        {

            return DateTime.Now;

        }

        // Kullanıcı kontrolü
        static void CheckUser()
        {
            Console.WriteLine("Giriş Yap sayfası");
            int sayac = 3;
            while (sayac >= 1)
            {
                Console.Write("Kullanıcı adınızı girin: ");
                string username = Console.ReadLine();
                Console.Write("Şifrenizi girin: ");
                string password = Console.ReadLine();

                if (userList.ContainsKey(username) && userList[username] == password)
                {
                    Console.Clear();
                    Console.WriteLine($"Giriş başarılı. Hoşgeldin {username}\n");
                    Console.WriteLine();
                    Menu();

                }

                else
                {
                    sayac -= 1;
                    Console.WriteLine($"Hatalı kullanıcı adı veya şifre! Kalan Hak: {sayac}");

                }

            }

        }

        // Yeni kayıt ekler.
        static void AddNewRecord()
        {
            Console.WriteLine();

            if (dailyLimit)
            {
                StreamWriter writer = new StreamWriter(path, true);
                Console.Write("Günlük Not: ");
                string diaryContent = Console.ReadLine();
                DateTime currentDate = GetDateNow();
                string newFormat = currentDate.ToString("dd/MM/yyyy");
                string record = $"{diaryContent}  - Kayıt edilen tarih: {newFormat}";
                writer.WriteLine(record);
                writer.WriteLine("--------------------------------");
                diary.Add(record);
                Console.WriteLine($"Notunuz {newFormat} tarihinde kayıt edildi.");
                dailyLimit = false;
                writer.Close();


            }

            else
            {
                Console.WriteLine("Bugün günlük kaydı girdin, aynı tarihte yeni bir kayıt eklemek ister misin?.(e) veya (h)");
                Console.Write("Seçiminiz: ");
                char choice = Convert.ToChar(Console.ReadLine());
                if (choice == 'e')
                {
                    dailyLimit = true;
                    AddNewRecord();

                }

                else
                {
                    Console.WriteLine("Ana menüye dönülüyor.");
                    Menu();
                }

            }
        }

        // Kayıtları listeler.
        static void ListRecords()
        {

            Console.Clear();

            Console.WriteLine();

            currentIndex = 0;
            while (true)
            {

                if (currentIndex < diary.Count)
                {
                    string text = diary[currentIndex];
                    Console.WriteLine(text);
                    Console.WriteLine("-------------------------");
                }
                else
                {
                    Console.WriteLine("Kayıt bulunamadı.");
                    currentIndex = 0;
                    Menu();
                    break;

                }

                ListRecordsProps();

            }

        }

        static void ListRecordsProps()
        {
            Console.WriteLine("(s)onraki kayıt | (d)üzenle | (x)sil | (a)na menu ");
            char choice = Convert.ToChar(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            if (choice == 's')
            {
                Console.Clear();
                Console.WriteLine();
                currentIndex++;

            }

            else if (choice == 'd')
            {
                diary[currentIndex] = EditRecord(diary[currentIndex]);


            }

            else if (choice == 'x')
            {
                Console.WriteLine("Emin misin? (e) / (h)");
                char youSure = Convert.ToChar(Console.ReadKey().KeyChar.ToString());
                Console.WriteLine();


                if (youSure == 'e')
                {
                    diary.RemoveAt(currentIndex);
                    currentIndex = 0;
                    Console.WriteLine("Kayıt silindi.");
                }

                else if (youSure == 'h')
                {

                    Console.WriteLine("İşlem iptal edildi. Ana menüye dönülüyor...");
                    Menu();
                }

                else
                {
                    Console.WriteLine("Hatalı harf! Ana menüye dönülüyor...");
                    Menu();
                }

            }

            else if (choice == 'a')
            {

                Console.WriteLine("Ana menüye dönülüyor...");
                Menu();

            }
        }

        // Kayıtları düzenler.
        static string EditRecord(string currentRecord)
        {
            Console.Write("Yeni notunuzu girin: ");
            string newContent = Console.ReadLine();
            DateTime currentDate = GetDateNow();
            string newFormat = currentDate.ToString("dd/MM/yyyy");
            string record = $"{newContent}  - Kayıt edilen tarih: {newFormat}";
            Console.WriteLine($"Notunuz: {newContent} olarak güncellendi ve {newFormat} tarihinde kayıt edildi.");
            return record;
        }

        //Tüm kayıtları siler.
        static void DeleteAllRecords()
        {
            Console.WriteLine();
            AreYouSureForAll();
            dailyLimit = true;



        }

        // Silmek istenen veri için emin misin sorusu sorar.                   
        static void AreYouSureForAll()
        {
            Console.WriteLine("Emin misin? (e) / (h)");
            char youSure = Convert.ToChar(Console.ReadKey().KeyChar.ToString());
            Console.WriteLine();

            if (youSure == 'e')
            {
                diary.Clear();
                Console.WriteLine("Tüm kayıtlar silindi.");
            }

            else if (youSure == 'h')
            {

                Console.WriteLine("İşlem iptal edildi. Ana menüye dönülüyor...");
                Menu();
            }

            else
            {
                Console.WriteLine("Hatalı seçim! Ana menüye dönülüyor...");
                Menu();
            }
        }

        // Kullanıcı menüsü
        static void FirstMenu()
        {

            Console.WriteLine("1-Giriş Yap 2- Kayıt Ol 3-Programı Durdur.");
            int choice = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

            if (choice == 1)
            {
                Console.Clear();

                CheckUser();
            }
            else if (choice == 2)
            {
                Console.Clear();

                Register();
            }

            else if (choice == 3)
            {
                Environment.Exit(0);

            }

            else
            {
                Console.WriteLine("Seçim hatalı!");
            }

        }

        // Kullanıcı kayıt
        static void Register()
        {
            Console.WriteLine("Kayıt Ol sayfası");

            Console.Write("Kullanıcı adınızı girin: ");
            string username = Console.ReadLine();
            Console.Write("Şifrenizi girin: ");
            string password = Console.ReadLine();


            if (userList.ContainsKey(username))
            {
                Console.WriteLine("Bu kullanıcı adı daha önce alınmış.");
                Console.Clear();
                FirstMenu();

            }

            else
            {

                userList.Add(username, password);
                Console.WriteLine("Kayıt başarılı, giriş sayfasına yönlendiriliyorsunuz.");
                Console.Clear();
                Thread.Sleep(1000);
                CheckUser();
            }
        }

        // Kayıtları Listelemeden önce şifre sorar.
        static void HideRecords()
        {
            Console.Clear();
            Console.Write("Şifrenizi girin: ");
            string password = Console.ReadLine();
            foreach (var record in userList)
            {

                if (record.Value == password)
                {
                    ListRecords();
                }

                else
                {
                    Console.WriteLine("Şifre Hatalı!");
                    Menu();
                }

            }

        }

        //Kelime veya tarih ile arama
        static void FindRecords()
        {
            Console.Clear();
            Console.Write("Kelime yada tarih giriniz(Tarih Formatı= xx.xx.xxxx): ");
            string search = Console.ReadLine();

            bool found = false;

            foreach (var record in diary)
            {
                if (record.Contains(search))
                {
                    Console.WriteLine(record);
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Bu kelimeyi içeren veya bu tarihte kayıt edilmiş içerik bulunmuyor.");
                Thread.Sleep(5000);
                Menu();
            }
        }

    }

}
