/*
1. Prototype: Pro efektivní vytváření a kopírování kontaktních záznamů.
2. Command: Pro zaznamenávání a možné vrácení změn v kontaktech.
3. Iterator: Pro navigaci skrze kolekci kontaktů.
TO:DO - dodělat úpravu + jednotkové testy
TO:DO - hledání kontaktů podle jména
TO:DO - export kontaktů a jeho načtení při spuštění
*/

//1. Prototype - pro kontakty
//T je generický typ → návratová hodnota Clone() bude stejného typu jako objekt, který implementuje interface.
using System;
using System.Reflection;

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
    public void Insert(int index, Kontakt kontakt) => _kontakty.Insert(index, kontakt);
    public int Count => _kontakty.Count;
    public Kontakt Edit(int index, Kontakt kontakt) => _kontakty[index] = kontakt;
    public Kontakt Get(int index) => _kontakty[index];

    // NEW: vrať smazaný kontakt, ať jde vrátit zpět
    public Kontakt Delete(int index)
    {
        var removed = _kontakty[index];
        _kontakty.RemoveAt(index);
        return removed;
    }
    public KontaktIterator CreateIterator() => new KontaktIterator(this);
}


//3. Command rozhraní
public interface ICommand
{
    void Execute();
    void Undo();
}

// Commandy nad kontaktem
public class KontaktDelete : ICommand
{
    private readonly KontaktKolekce _kolekce;
    private readonly int _index;
    private Kontakt _deletedKontakt;

    public KontaktDelete(KontaktKolekce data, Kontakt input, int index)
    {
        _kolekce = data;
        _index = index;
        _deletedKontakt = input;
    }

    public void Execute()
    {
        Console.WriteLine($"Mazání kontaktu: {_deletedKontakt.name}");
        _deletedKontakt = _kolekce.Delete(_index); // vezmi reálně smazaný kus
    }

    public void Undo()
    {
        _kolekce.Insert(_index, _deletedKontakt);
    }
}

public class KontaktEdit : ICommand
{
    private readonly KontaktKolekce _kolekce;
    private readonly int _index;
    private readonly Kontakt _newKontakt;
    private Kontakt _oldKontakt;

    public KontaktEdit(KontaktKolekce data, int index, Kontakt input)
    {
        _kolekce = data;
        _index = index;
        _newKontakt = input;
    }

    public void Execute()
    {
        _oldKontakt = _kolekce.Get(_index).Clone(); // ulož původní stav
        _kolekce.Edit(_index, _newKontakt);
    }

    public void Undo()
    {
        _kolekce.Edit(_index, _oldKontakt);
    }
}

public class KontaktCreate : ICommand
{
    private readonly KontaktKolekce _kolekce;
    private readonly Kontakt _kontakt;
    private int? _insertIndex;

    public KontaktCreate(KontaktKolekce data, Kontakt input)
    {
        _kolekce = data;
        _kontakt = input;
    }

    public void Execute()
    {
        _kolekce.Add(_kontakt);
        _insertIndex = _kolekce.Count - 1;
    }

    public void Undo()
    {
        if (_insertIndex.HasValue)
            _kolekce.Delete(_insertIndex.Value);
    }
}

// Command Manager - ten bude executovat a ukládat commandy - připádně je vracet pomocí Undo ze zásobníku
public class CommandManager
{
    private readonly Stack<ICommand> _history = new();

    public void Execute(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void Undo()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("Není co vracet.");
            return;
        }

        var cmd = _history.Pop();
        cmd.Undo();
    }
}


//UI
public class ConsoleUI
{
    private readonly KontaktKolekce _data = new();
    private readonly CommandManager _cmd = new();

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
            Console.WriteLine("5) Upravit kontakt");
            Console.WriteLine("6) Vrátit poslední akci (Undo)");
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
                case "5": EditKontakt(); break;
                case "6": _cmd.Undo(); break;
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

    private void EditKontakt()
    {
        ListKontakty();
        int index = ReadIndex($"Zadej číslo (1–{_data.Count}) k úpravě: ", 1, _data.Count) - 1;
        var target = _data.Get(index);

        Console.WriteLine($"Jméno [{target.name}] (nech prázdné pro zachování):");
        string nameInput = Console.ReadLine();

        Console.WriteLine($"Telefon [{target.phone}] (nech prázdné pro zachování):");
        string phoneInput = Console.ReadLine();

        Console.WriteLine($"E-mail [{target.email}] (nech prázdné pro zachování):");
        string emailInput = Console.ReadLine();

        var newObj = new Kontakt
        {
            name = string.IsNullOrWhiteSpace(nameInput) ? target.name : nameInput,
            phone = string.IsNullOrWhiteSpace(phoneInput) ? target.phone : phoneInput,
            email = string.IsNullOrWhiteSpace(emailInput) ? target.email : emailInput
        };

        _cmd.Execute(new KontaktEdit(_data, index, newObj));

    }
    private void AddKontakt()
    {
        var k = new Kontakt
        {
            name = ReadNonEmpty("Jméno: "),
            phone = ReadNonEmpty("Telefon: "),
            email = ReadNonEmpty("E-mail: ")
        };

        _cmd.Execute(new KontaktCreate(_data, k));
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

        _cmd.Execute(new KontaktDelete(_data, target, index));
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

        _cmd.Execute(new KontaktCreate(_data, copy));
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

// Aplikace - běh
public class Program
{
    public static void Main(string[] args)
    {
        new ConsoleUI().Run();
    }
}
