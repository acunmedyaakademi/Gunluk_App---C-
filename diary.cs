using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApp13
{


    internal class Program
    {

        static string path = "C:\\FILE_PATH\\Proje\\diary.txt";
        static bool firstTime = true;
        static bool dailyLimit = true;
        static int currentIndex = 0;


        static List<string> diary { get; set; } = new List<string>();


        static void Main(string[] args)
        {
            CheckUser();
            

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
                    Console.WriteLine("4. Kayıt Ara");
                    Console.WriteLine("5.Çıkış Yap");


                    int choice = Convert.ToInt32(Console.ReadKey().KeyChar.ToString());

                    if (choice == 1)
                    {

                        AddNewRecord();
                    }

                    else if (choice == 2)
                    {
                        ListRecords();

                    }

                    else if (choice == 3)
                    {
                        DeleteAllRecords();

                    }

                    else if (choice == 4)
                    {
                        //TODO
                        FindRecords();

                    }

                    else if (choice == 5)
                    {
                        Environment.Exit(0);


                    }

                    else
                    {

                        Console.WriteLine("Hatalı tuşlama yaptınız!");
                    }


                }
            }

            // Tarihi Tutar
            static DateTime GetDateNow()
            {

                return DateTime.Now;

            }

            // Kullanıcı kontrolü
            static void CheckUser()
            {
                int sayac = 3;
                while (sayac >= 1)
                {
                    Console.Write("Kullanıcı adınızı girin: ");
                    string username = Console.ReadLine();
                    Console.Write("Şifrenizi girin: ");
                    string password = Console.ReadLine();

                    if (username == "admin" || password == "password")
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
                Console.WriteLine();


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
                        Console.WriteLine("Kayıt bulunamadı!");
                        Menu();
                        break;
                       
                    }
                    Console.WriteLine("(s)onraki kayıt | (d)üzenle | (x)sil | (a)na menu ");
                    char choice = Convert.ToChar(Console.ReadKey().KeyChar.ToString());
                    Console.WriteLine();

                    if (choice == 's')
                    {
                        Console.Clear();
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
         

            static void DeleteAllRecords()
            {
                Console.WriteLine();
                AreYouSureForAll();


            }

            // Emin misin sorusu sorar.
                      

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
                    Console.WriteLine("Hatalı harf! Ana menüye dönülüyor...");
                    Menu();
                }
            }

            //TODO

            static void FindRecords()
            {
                Console.Write("Gün.Ay.Yıl Giriniz: ");
                string day = Console.ReadLine();
                if (diary.Contains(day))
                {
                    Console.WriteLine("TODO");
                }
              

            }

        }
    }

}
