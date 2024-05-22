using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

class Program
{
    static BazaDanych bazaDanych = new BazaDanych();

    static void Main(string[] args)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nWybierz opcję:");
            Console.WriteLine("1. Dodaj nowy album");
            Console.WriteLine("2. Dodaj utwór do albumu");
            Console.WriteLine("3. Dodaj wykonawcę do utworu");
            Console.WriteLine("4. Wyświetl wszystkie albumy");
            Console.WriteLine("5. Wyświetl wykonawców z albumu");
            Console.WriteLine("6. Wyświetl szczegóły utworu");
            Console.WriteLine("7. Wyświetl szczegóły albumu");
            Console.WriteLine("8. Zapisz bazę danych do pliku");
            Console.WriteLine("9. Odczytaj bazę danych z pliku");
            Console.WriteLine("10. Wyjście");

            string opcja = Console.ReadLine();

            switch (opcja)
            {
                case "1":
                    DodajNowyAlbum();
                    break;
                case "2":
                    DodajUtwórDoAlbumu();
                    break;
                case "3":
                    DodajWykonawcęDoUtworu();
                    break;
                case "4":
                    WyświetlWszystkieAlbumy();
                    break;
                case "5":
                    WyświetlWykonawcówZAlbumu();
                    break;
                case "6":
                    WyświetlSzczegółyUtworu();
                    break;
                case "7":
                    WyświetlSzczegółyAlbumu();
                    break;
                case "8":
                    ZapiszBazęDoPliku();
                    break;
                case "9":
                    OdczytajBazęZPliku();
                    break;
                case "10":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Nieznana opcja, spróbuj ponownie.");
                    break;
            }
        }
    }

    static void DodajNowyAlbum()
    {
        Console.WriteLine("Podaj tytuł albumu:");
        string tytuł = Console.ReadLine();

        Console.WriteLine("Podaj typ albumu (CD/DVD):");
        string typ = Console.ReadLine();

        Album nowyAlbum = new Album
        {
            Id = bazaDanych.Albumy.Count + 1,
            Tytuł = tytuł,
            Typ = typ
        };

        bazaDanych.DodajAlbum(nowyAlbum);
        Console.WriteLine("Album został dodany.");
    }

    static void DodajUtwórDoAlbumu()
    {
        Console.WriteLine("Podaj ID albumu, do którego chcesz dodać utwór:");
        int idAlbumu;
        if (int.TryParse(Console.ReadLine(), out idAlbumu))
        {
            var album = bazaDanych.PobierzSzczegółyAlbumu(idAlbumu);
            if (album != null)
            {
                Console.WriteLine("Podaj tytuł utworu:");
                string tytułUtworu = Console.ReadLine();

                Console.WriteLine("Podaj czas trwania utworu (w sekundach):");
                int czasTrwaniaSekundy;
                if (int.TryParse(Console.ReadLine(), out czasTrwaniaSekundy))
                {
                    Utwór nowyUtwór = new Utwór
                    {
                        Id = album.Utwory.Count + 1,
                        Tytuł = tytułUtworu,
                        CzasTrwania = TimeSpan.FromSeconds(czasTrwaniaSekundy)
                    };

                    album.Utwory.Add(nowyUtwór);
                    Console.WriteLine("Utwór został dodany do albumu.");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy czas trwania.");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono albumu o podanym ID.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID albumu.");
        }
    }

    static void DodajWykonawcęDoUtworu()
    {
        Console.WriteLine("Podaj ID albumu:");
        int idAlbumu;
        if (int.TryParse(Console.ReadLine(), out idAlbumu))
        {
            Console.WriteLine("Podaj ID utworu:");
            int idUtworu;
            if (int.TryParse(Console.ReadLine(), out idUtworu))
            {
                var utwór = bazaDanych.PobierzSzczegółyUtworu(idAlbumu, idUtworu);
                if (utwór != null)
                {
                    Console.WriteLine("Podaj imię wykonawcy:");
                    string imię = Console.ReadLine();
                    Console.WriteLine("Podaj nazwisko wykonawcy:");
                    string nazwisko = Console.ReadLine();

                    Wykonawca nowyWykonawca = new Wykonawca
                    {
                        Id = utwór.Wykonawcy.Count + 1,
                        Imię = imię,
                        Nazwisko = nazwisko
                    };

                    utwór.Wykonawcy.Add(nowyWykonawca);
                    Console.WriteLine("Wykonawca został dodany do utworu.");
                }
                else
                {
                    Console.WriteLine("Nie znaleziono utworu.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowe ID utworu.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID albumu.");
        }
    }

    static void WyświetlWszystkieAlbumy()
    {
        foreach (var album in bazaDanych.Albumy)
        {
            Console.WriteLine($"ID: {album.Id}, Tytuł: {album.Tytuł}, Typ: {album.Typ}");
        }
    }

    static void WyświetlWykonawcówZAlbumu()
    {
        Console.WriteLine("Podaj ID albumu:");
        int idAlbumu;
        if (int.TryParse(Console.ReadLine(), out idAlbumu))
        {
            var wykonawcy = bazaDanych.PobierzWykonawcówAlbumu(idAlbumu);
            if (wykonawcy != null && wykonawcy.Count > 0)
            {
                foreach (var wykonawca in wykonawcy)
                {
                    Console.WriteLine($"{wykonawca.Imię} {wykonawca.Nazwisko}");
                }
            }
            else
            {
                Console.WriteLine("Brak wykonawców lub nie znaleziono albumu.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID albumu.");
        }
    }

    static void WyświetlSzczegółyUtworu()
    {
        Console.WriteLine("Podaj ID albumu:");
        int idAlbumu;
        if (int.TryParse(Console.ReadLine(), out idAlbumu))
        {
            Console.WriteLine("Podaj ID utworu:");
            int idUtworu;
            if (int.TryParse(Console.ReadLine(), out idUtworu))
            {
                var utwór = bazaDanych.PobierzSzczegółyUtworu(idAlbumu, idUtworu);
                if (utwór != null)
                {
                    Console.WriteLine($"ID: {utwór.Id}, Tytuł: {utwór.Tytuł}, Czas trwania: {utwór.CzasTrwania}");
                    Console.WriteLine("Wykonawcy:");
                    foreach (var wykonawca in utwór.Wykonawcy)
                    {
                        Console.WriteLine($"{wykonawca.Imię} {wykonawca.Nazwisko}");
                    }
                }
                else
                {
                    Console.WriteLine("Nie znaleziono utworu.");
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowe ID utworu.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID albumu.");
        }
    }

    static void WyświetlSzczegółyAlbumu()
    {
        Console.WriteLine("Podaj ID albumu:");
        int idAlbumu;
        if (int.TryParse(Console.ReadLine(), out idAlbumu))
        {
            var album = bazaDanych.PobierzSzczegółyAlbumu(idAlbumu);
            if (album != null)
            {
                Console.WriteLine($"ID: {album.Id}, Tytuł: {album.Tytuł}, Typ: {album.Typ}, Czas trwania: {album.CzasTrwania}");
                Console.WriteLine("Utwory:");
                foreach (var utwór in album.Utwory)
                {
                    Console.WriteLine($"ID: {utwór.Id}, Tytuł: {utwór.Tytuł}, Czas trwania: {utwór.CzasTrwania}");
                }
            }
            else
            {
                Console.WriteLine("Nie znaleziono albumu.");
            }
        }
        else
        {
            Console.WriteLine("Nieprawidłowe ID albumu.");
        }
    }

    static void ZapiszBazęDoPliku()
    {
        Console.WriteLine("Podaj ścieżkę do pliku:");
        string ścieżka = Console.ReadLine();
        try
        {
            bazaDanych.ZapiszDoPliku(ścieżka);
            Console.WriteLine("Baza danych została zapisana.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas zapisu: {ex.Message}");
        }
    }

    static void OdczytajBazęZPliku()
    {
        Console.WriteLine("Podaj ścieżkę do pliku:");
        string ścieżka = Console.ReadLine();
        try
        {
            bazaDanych.OdczytajZPliku(ścieżka);
            Console.WriteLine("Baza danych została odczytana.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wystąpił błąd podczas odczytu: {ex.Message}");
        }
    }
}