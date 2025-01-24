using System;
using System.Collections.Generic;


class Program
{
    static void Main()
    {
        Kutuphane kutuphanem = new Kutuphane();

        // Öncelikle kitapları kütüphaneye ekleriz

        kutuphanem.KitapEkle(new Kitap("9781234567890", "Yüzüklerin Efendisi", "J.R.R. Tolkien"));
        kutuphanem.KitapEkle(new Kitap("9780451524935", "1984", "George Orwell"));
        kutuphanem.KitapEkle(new Kitap("9789753638024", "Kürk Mantolu Madonna", "Sabahattin Ali"));
        kutuphanem.KitapEkle(new Kitap("9789754700113", "İnce Memed", "Yaşar Kemal"));
        kutuphanem.KitapEkle(new Kitap("9786053601989", "Aşk", "Elif Şafak"));
        kutuphanem.KitapEkle(new Kitap("9789754703633", "Eylül", "Mehmet Rauf"));
        kutuphanem.KitapEkle(new Kitap("9786050921981", "Serenad", "Zülfü Livaneli"));
        kutuphanem.KitapEkle(new Kitap("9789754589022", "Saatleri Ayarlama Enstitüsü", "Ahmet Hamdi Tanpınar"));
        kutuphanem.KitapEkle(new Kitap("9789754700113", "Bir Bilim Adamının Romanı", "Oğuz Atay"));
        kutuphanem.KitapEkle(new Kitap("9786050948797", "Beyaz Zambaklar Ülkesinde", "Grigory Petrov"));
        kutuphanem.KitapEkle(new Kitap("9786051852307", "Şu Çılgın Türkler", "Turgut Özakman"));
        kutuphanem.KitapEkle(new Kitap("9789754586991", "Çalıkuşu", "Reşat Nuri Güntekin"));

        Console.WriteLine("Kütüphaneye hoş geldiniz!");
   
        while (true)
        {
            // Kullanıcı bilgilerini alırız

            Console.Write("Kisi adi: ");
            string ad = Console.ReadLine();
            Console.Write("Kisi soyadi: ");
            string soyad = Console.ReadLine();
            Console.Write("Dogum yili: ");
            int dogumYili = Convert.ToInt32(Console.ReadLine());
            Console.Write("Kimlik numarasi (gizli tutulacak): ");
            string kimlikNumarasi = Console.ReadLine();

            // Bir kişi nesnesi oluştururuz

            Kisi kisi = new Kisi(ad, soyad, dogumYili, kimlikNumarasi);

            // kullanıcıya hangi işlemi yapmak istediğini seçtiririz

            while (true)
            {
                Console.Write("=======================================================\n");
                Console.WriteLine("\nMevcut Kitaplar:");
                kutuphanem.KitaplariListele();

                Console.WriteLine("\nYapmak istediginiz islemi girin:");
                Console.WriteLine("a. Kitap odunc al");
                Console.WriteLine("b. Kitap iade et");
                Console.WriteLine("q. Cikis yapmak icin 'q' tusuna basin.");
                string islem = Console.ReadLine();

                Console.Write("=======================================================\n");

                if (islem.ToLower() == "a")
                {
                    Console.WriteLine("\nOdunc almak istediginiz kitap numarasini (tek kitap alabilirsiniz) giriniz: ");
                    int kitapNumarasi = Convert.ToInt32(Console.ReadLine());
                    kutuphanem.KitapOduncAl(kisi, kitapNumarasi);
                }
                else if (islem.ToLower() == "b")
                {
                    Console.WriteLine("\nIade etmek istediginiz kitap numarasini (tek kitap alabilirsiniz) giriniz: ");
                    int iadeKitapNumarasi = Convert.ToInt32(Console.ReadLine());
                    kutuphanem.KitapIadeEt(kisi, iadeKitapNumarasi);
                }
                else if (islem.ToLower() == "q")
                {
                    Console.WriteLine("Cikis yapiliyor...");
                    return; // Programdan çıkış yapılır
                }
                else
                {
                    Console.WriteLine("Gecersiz islem. Lutfen tekrar deneyin.");
                }
            }
        }
    }
}

// Kişi sınıfı oluştururuz
class Kisi
{
    public string Ad { get; private set; }
    public string Soyad { get; private set; }
    public int DogumYili { get; private set; }
    private string KimlikNumarasi; // Kimlik numarası gizli tutulacak

    // Kişi sınıfından constructor (yapıcı metod) oluştururuz
    public Kisi(string ad, string soyad, int dogumYili, string kimlikNumarasi)
    {
        Ad = ad;
        Soyad = soyad;
        DogumYili = dogumYili;
        KimlikNumarasi = kimlikNumarasi;
    }
}

// Kitap sınıfı oluştururuz
class Kitap
{
    public string ISBN { get; private set; }
    public string Baslik { get; private set; }
    public string Yazar { get; private set; }
    public DateTime? OduncAlinmaTarihi { get; private set; }
    public string OncekiOduncAlan { get; private set; }

    // Kitap sınıfından constructor (yapıcı metod) oluştururuz
    public Kitap(string isbn, string baslik, string yazar)
    {
        ISBN = isbn;
        Baslik = baslik;
        Yazar = yazar;
    }

    public void OduncAl(string oduncAlan)
    {
        OncekiOduncAlan = OduncAlinmaTarihi == null ? null : OncekiOduncAlan;
        OduncAlinmaTarihi = DateTime.Now;
        OncekiOduncAlan = oduncAlan; // Önceki ödünç alan kişi adı güncellenir
    }

    public void IadeEt()
    {
        OduncAlinmaTarihi = null; // Kitap geri verildiği için ödünç alma tarihi sıfırlanır
    }

    public bool MevcutMu()
    {
        return OduncAlinmaTarihi == null;
    }

    public void BilgileriYazdir()
    {
        Console.WriteLine($"Kitap Adi: {Baslik}, ISBN: {ISBN}, Odunc Alindigi Tarih: {OduncAlinmaTarihi?.ToString("dd.MM.yyyy HH:mm:ss")}, Onceki Odunc Alan Kisi: {OncekiOduncAlan}");
    }
}

// Kütüphane sınıfı oluştururuz
class Kutuphane
{
    private List<Kitap> Kitaplar;
    private Dictionary<Kisi, List<Kitap>> OduncAlinanKitaplar;

    public Kutuphane()
    {
        Kitaplar = new List<Kitap>();
        OduncAlinanKitaplar = new Dictionary<Kisi, List<Kitap>>();
    }

    public void KitapEkle(Kitap kitap)
    {
        Kitaplar.Add(kitap);
    }

    public void KitaplariListele()
    {
        for (int i = 0; i < Kitaplar.Count; i++)
        {
            Kitap kitap = Kitaplar[i];
            Console.WriteLine($"{i + 1}. {kitap.Baslik} - {kitap.Yazar} (ISBN: {kitap.ISBN})");
        }
    }

    public void KitapOduncAl(Kisi kisi, int kitapNumarasi)
    {
        if (kitapNumarasi < 1 || kitapNumarasi > Kitaplar.Count)
        {
            Console.WriteLine("Gecersiz kitap numarasi.");
            return;
        }

        Kitap kitap = Kitaplar[kitapNumarasi - 1];

        if (kitap.MevcutMu())
        {
            kitap.OduncAl($"{kisi.Ad} {kisi.Soyad}");
            if (!OduncAlinanKitaplar.ContainsKey(kisi))
            {
                OduncAlinanKitaplar[kisi] = new List<Kitap>();
            }
            OduncAlinanKitaplar[kisi].Add(kitap);
            Console.WriteLine($"{kitap.Baslik} kitabi odunc alindi.");
            KitaplariGoster(kisi);
        }
        else
        {
            Console.WriteLine("Kitap mevcut degil veya daha once odunc alinmis.");
        }
    }

    public void KitapIadeEt(Kisi kisi, int kitapNumarasi)
    {
        if (kitapNumarasi < 1 || kitapNumarasi > Kitaplar.Count)
        {
            Console.WriteLine("Gecersiz kitap numarasi.");
            return;
        }

        Kitap kitap = Kitaplar[kitapNumarasi - 1];
        if (OduncAlinanKitaplar.ContainsKey(kisi) && OduncAlinanKitaplar[kisi].Contains(kitap))
        {
            kitap.IadeEt();
            OduncAlinanKitaplar[kisi].Remove(kitap);
            Console.WriteLine($"{kitap.Baslik} kitabi iade edildi.");
        }
        else
        {
            Console.WriteLine("Bu kitap daha once odunc alinmamis.");
        }
    }

    public void KitaplariGoster(Kisi kisi)
    {
        Console.WriteLine($"\n{kisi.Ad} {kisi.Soyad} tarafindan odunc alinan kitaplar:");
        if (OduncAlinanKitaplar.ContainsKey(kisi))
        {
            foreach (var kitap in OduncAlinanKitaplar[kisi])
            {
                kitap.BilgileriYazdir();
            }
        }
        else
        {
            Console.WriteLine("Hic kitap odunc alinmamis.");
        }
    }
}
