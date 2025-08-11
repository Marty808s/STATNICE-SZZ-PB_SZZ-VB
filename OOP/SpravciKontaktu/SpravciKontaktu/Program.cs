//Console.WriteLine("Hello, World!");
/*
1. Prototype: Pro efektivní vytváření a kopírování kontaktních záznamů.
2. Command: Pro zaznamenávání a možné vrácení změn v kontaktech.
3. Iterator: Pro navigaci skrze kolekci kontaktů.
TO:DO - dodělat úpravu + jednotkové testy
*/

//1. Prototype - pro kontakty
//T je generický typ → návratová hodnota Clone() bude stejného typu jako objekt, který implementuje interface.
using System;

public interface IPrototype<T>
{
    T Clone();
}

public class Kontakt : IPrototype<Kontakt>
{
    public string name { get; set; }
    public string phone { get; set; }
    public string email { get; set; }

    public Kontakt Clone()
    {
        return new Kontakt {
            name = this.name, 
            phone = this.phone, 
            email = this.email 
        };
    }
}

// 2. Iterátor a UI - zásobník kontaktů + operace nad nimi, dodělat iterátor - UI JE RECIEVER U COMMANDU
public interface IIterator<T>
{
    T GetNext();
    bool HasNext();
}

public class KontaktIterator : IIterator<Kontakt>
{
    private readonly KontaktKolekce _kolekce;
    private int _currentIndex = 0;

    public KontaktIterator(KontaktKolekce kolekce)
    {
        _kolekce = kolekce;
    }

    public bool HasNext() => _currentIndex < _kolekce.Count;

    public Kontakt GetNext()
    {
        if (!HasNext()) throw new InvalidOperationException("Žádné další prvky");
        return _kolekce.Get(_currentIndex++);
    }
}

// Datová struktura spoelčně s operacemi nad ní -> Moje UI
public class KontaktKolekce
{
    private readonly List<Kontakt> _kontakty = new();
    public void Add(Kontakt kontakt) => _kontakty.Add(kontakt);
    public int Count => _kontakty.Count;
    public Kontakt Get(int index) => _kontakty[index];
    // Vytvoří iterátor
    public KontaktIterator CreateIterator() => new KontaktIterator(this);
}


//3. Command
public interface ICommand
{
    void Execute();
}

// Jeden command nad kontaktem
public class KontaktDelete : ICommand
{
    public Kontakt kontakt { get; set; }
    public KontaktKolekce _kolekce { get; set; }

    public KontaktDelete(KontaktKolekce data, Kontakt input)
    {
        this._kolekce = data;
        this.kontakt = input;
    }
    public void Execute()
    {
        // zpráva do konzole + dodělat vrácení stavu
        Console.WriteLine($"Mazání kontaktu: {this.kontakt.name}");
        // zavolání operace na KontaktKolekce
    }
}


//UI
public class ConsoleUI
{
    private readonly KontaktKolekce _data = new();
    //TO:DO - dodělat commandy, všechny přes set vytvořit jako prop, potom je přidat níže přes Execute

    public ConsoleUI()
    {
        // dummy
        _data.Add(new Kontakt { name = "Karel", phone = "123", email = "karel@example.com" });
        _data.Add(new Kontakt { name = "Petr", phone = "456", email = "petr@example.com" });
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("==== Kontakty ====");
            Console.WriteLine("1) Vypsat kontakty");
            Console.WriteLine("2) Přidat kontakt");
            Console.WriteLine("3) Smazat kontakt");
            Console.WriteLine("4) Klonovat kontakt (Prototype)");
            Console.WriteLine("0) Konec");
            Console.Write("Volba: ");

            var key = Console.ReadLine();
            Console.WriteLine();

            switch (key)
            {
                case "1": ListKontakty(); break;
                case "2": AddKontakt(); break;
                case "3": DeleteKontakt(); break;
                case "4": CloneKontakt(); break;
                case "0": return;
                default: Console.WriteLine("Neplatná volba."); break;
            }
        }
    }

    // UI funkce
    private void ListKontakty()
    {
        if (_data.Count == 0)
        {
            Console.WriteLine("Seznam je prázdný.");
            return;
        }

        //použití iterátoru
        var it = _data.CreateIterator();
        int i = 1;
        while (it.HasNext())
        {
            var k = it.GetNext();
            Console.WriteLine($"{i++}. {k.name} | {k.phone} | {k.email}");
        }
    }

    private void AddKontakt()
    {
        var k = new Kontakt
        {
            name = ReadNonEmpty("Jméno: "),
            phone = ReadNonEmpty("Telefon: "),
            email = ReadNonEmpty("E-mail: ")
        };
        _data.Add(k);
        Console.WriteLine("Kontakt přidán.");
    }

    private void DeleteKontakt()
    {
        if (_data.Count == 0)
        {
            Console.WriteLine("Není co mazat.");
            return;
        }

        ListKontakty();
        int index = ReadIndex($"Zadej číslo (1–{_data.Count}) ke smazání: ", 1, _data.Count) - 1;
        var target = _data.Get(index);
        // execute commandu - přesunou do konstruktoru - vytvořím všechny commandy
        var cmd = new KontaktDelete(_data, target);
        cmd.Execute();
    }

    private void CloneKontakt()
    {
        if (_data.Count == 0)
        {
            Console.WriteLine("Není co klonovat.");
            return;
        }

        ListKontakty();
        int index = ReadIndex($"Zadej číslo (1–{_data.Count}) ke klonování: ", 1, _data.Count) - 1;
        var copy = _data.Get(index).Clone();
        copy.name = copy.name + " (kopie)";
        _data.Add(copy);
        Console.WriteLine("Kontakt naklonován a přidán.");
    }

    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
            Console.WriteLine("Zadej neprázdnou hodnotu.");
        }
    }

    private static int ReadIndex(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int n) && n >= min && n <= max)
                return n;
            Console.WriteLine("Neplatné číslo.");
        }
    }
}


// Aplikace
public class Program
{
    public static void Main(string[] args)
    {
        new ConsoleUI().Run();
    }
}
