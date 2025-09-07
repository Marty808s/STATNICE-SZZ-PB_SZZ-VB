//1.Prototype: Pro efektivní vytváření a kopírování kontaktních záznamů. - DONE
//2. Command: Pro zaznamenávání a možné vrácení změn v kontaktech.
//3. Iterator: Pro navigaci skrze kolekci kontaktů.

// čas 3 hodiny
//--------------------------------------------------
// Prototype - pro mě kontakt
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

public abstract class Prototype
{
    public string Jmeno { get; set; }
    public string Prijmeni { get; set; }
    public string Telefon { get; set; }
    public string Email { get; set; }

    public Prototype(string jmeno, string prijmeni, string telefon, string email)
    {
        this.Jmeno = jmeno;
        this.Prijmeni = prijmeni;
        this.Telefon = telefon;
        this.Email = email;
    }

    public Prototype(Prototype other)
    {
        this.Jmeno = other.Jmeno;
        this.Prijmeni = other.Prijmeni;
        this.Telefon = other.Telefon;
        this.Email = other.Email;
    }

    public void SayHello()
    {
        Console.WriteLine($"{Jmeno} {Prijmeni}");
    }

    public abstract Prototype Clone();
}

public class Kontakt : Prototype
{
    //konstruktor pro vytvoření
    public Kontakt(string jmeno, string prijmeni, string telefon, string email)
        : base(jmeno, prijmeni, telefon, email)
    {
    }

    // konstruktor pro kopírování
    public Kontakt(Kontakt other)
        : base(other)
    {
    }

    public override Prototype Clone()
    {
        return new Kontakt(this);
    }
}

//--------------------------------------------------
// Iterator
//iterátor
public interface IKontaktIterator
{
    public Kontakt Next();
    public bool HasMore();
}

//kolekce
public interface IKolekce
{
    public KontaktIterator CreateIterator();

}

public class KontaktIterator : IKontaktIterator
{
    private List<Kontakt> _kontakty;
    private int _position = 0;

    public KontaktIterator(List<Kontakt> kontakty)
    {
        this._kontakty = kontakty;
    }

    public bool HasMore()
    {
        return _position < _kontakty.Count;
    }

    public Kontakt Next()
    {
        if (!HasMore())
        {
            Console.Write("Jsem na konci, jdu na začátek");
            this._position = 0;
            return _kontakty[0];


        }
        return _kontakty[_position++];
    }
}

// moje kolekce, která se rovnou stará jako reciever pro commandera - volá metody na kolekci dat
public class KontaktKolekce : IKolekce
{
    private List<Kontakt> _kontakty = new();

    public KontaktIterator CreateIterator()
    {
        return new KontaktIterator(_kontakty);
    }

    public int Count()
    {
        return _kontakty.Count;
    }

    public KontaktKolekce Search(string text)
    {
        var result = new KontaktKolekce();
        foreach (var kontakt in _kontakty)
        {
            if (!string.IsNullOrEmpty(kontakt.Jmeno) &&
                kontakt.Jmeno.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                result.Insert(kontakt);
            }
        }
        return result;
    }

    public bool Insert(Kontakt kontak)
    {
        try {
            var existing = _kontakty.FirstOrDefault(p => p.Jmeno == kontak.Jmeno);
            if (existing == null)
            {
                _kontakty.Add(kontak);
                return true;
            } else
            {
                return false;
            }
        } catch
        {
            return false;
        }

    }
    public bool Delete(Kontakt kontak)
    {
        try {
            _kontakty.Remove(kontak);
            return true;
        }
        catch
        {
            return false;
        }

    }

    public bool Edit(Kontakt kontak, int index)
    {
        /* - SOFISTIKOVANÉ ŘEŠENÍ, ALE PRO MĚ MOC OP
        var existing = _kontakty.FirstOrDefault(p => p.Jmeno == kontak.Jmeno);
        if (existing == null)
        {
            return false;
        }
        switch (key)
        {
            case nameof(Kontakt.Jmeno): //nameof -> název proměnné
                existing.Jmeno = newValue;
                break;
            case nameof(Kontakt.Prijmeni):
                existing.Prijmeni = newValue;
                break;
            case nameof(Kontakt.Telefon):
                existing.Telefon = newValue;
                break;
            case nameof(Kontakt.Email):
                existing.Email = newValue;
                break;
            default:
                return false;
        }

        return true;
        */
        var existing = _kontakty.FirstOrDefault(p => p.Jmeno == kontak.Jmeno);
        if (existing == null)
        {
            return false;
        }
        _kontakty[index] = kontak;
        return true;

    }

    public Kontakt GetById(int idx)
    {
        return _kontakty[idx];
    }


    public Kontakt Copy(Kontakt kontak)
    {
        var existing = _kontakty.FirstOrDefault(p => p.Jmeno == kontak.Jmeno);
        if (existing == null)
        {
            return existing;
        }
        var copy = existing.Clone();
        copy.Prijmeni = $"{kontak.Prijmeni} (kopie)";
        _kontakty.Add((Kontakt)copy);
        return (Kontakt)copy;

    }
}


//--------------------------------------------------
//Command
public interface ICommand
{
    bool Execute();
    bool Undo(); //pro vrácení
}


// Comand Manager - kvůli možnosti undo - buda stackovat commmandy
public class CommandManager
{
    private readonly Stack<ICommand> _history = new();

    public bool Execute(ICommand command)
    {
        var status = command.Execute();
        _history.Push(command);
        return status;
    }

    public bool Undo()
    {
        if (_history.Count > 0)
        {
            var last_command = _history.Pop();
            var status = last_command.Undo();
            return status;
        }
        else
        {
            Console.WriteLine("Není co vracet..");
            return false;
        }
    }

}

public class DeleteCommand : ICommand
{
    private KontaktKolekce _kolekce;
    private Kontakt _old_kontakt;

    public DeleteCommand(KontaktKolekce kolekce, Kontakt old_kontakt)
    {
        _kolekce = kolekce;
        _old_kontakt = old_kontakt;
    }

    public bool Execute()
    {
        var status = _kolekce.Delete(_old_kontakt);
        return status;
    }

    public bool Undo()
    {
        var status = _kolekce.Insert(_old_kontakt);
        return status;
    }
}


public class InsertCommand : ICommand
{
    private KontaktKolekce _kolekce;
    private Kontakt _new_kontakt;

    public InsertCommand(KontaktKolekce kolekce, Kontakt kontakt)
    {
        _kolekce = kolekce;
        _new_kontakt = kontakt;
    }

    public bool Execute()
    {
        var status = _kolekce.Insert(_new_kontakt);
        return status;
    }

    public bool Undo()
    {
        var status = _kolekce.Delete(_new_kontakt);
        return status;
    }
}

public class EditCommand : ICommand
{
    private KontaktKolekce _kolekce;
    private Kontakt _new_kontakt;
    private Kontakt _old_kontakt;

    private int _idx;

    public EditCommand(KontaktKolekce kolekce, Kontakt kontakt, int idx)
    {
        _kolekce = kolekce;
        _new_kontakt = kontakt;
        _idx = idx;
        _old_kontakt = kolekce.GetById(_idx);
    }

    public bool Execute()
    {
        var status = _kolekce.Edit(_new_kontakt, _idx);
        return status;
    }

    public bool Undo()
    {
        var status = _kolekce.Edit(_old_kontakt, _idx);
        return status;
    }
}


public class CopyCommand : ICommand
{
    private KontaktKolekce _kolekce;
    private Kontakt _kontakt;
    private Kontakt _new_object;

    public CopyCommand(KontaktKolekce kolekce, Kontakt kontakt)
    {
        _kolekce = kolekce;
        _kontakt = kontakt;
    }

    public bool Execute()
    {
        var new_obj = _kolekce.Copy(_kontakt);
        this._new_object = new_obj;
        return true;
        
    }

    public bool Undo()
    {
        var status = _kolekce.Delete(_new_object);
        return status;
    }
}


public class FileManager
{
    public string file_name = "kontakty";
    private KontaktKolekce _data;
    public string path;

    public FileManager(KontaktKolekce data)
    {
        this._data = data;
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        string exportDir = Path.Combine(projectPath, "exports");

        if (!Directory.Exists(exportDir))
        {
            Directory.CreateDirectory(exportDir);
        }

        this.path = Path.Combine(exportDir, $"{file_name}.txt");
    }

    public void Export()
    {
        using (StreamWriter writer = new StreamWriter(path))
        {
            for (int i = 0; i < _data.Count(); i++)
            {
                var kontakt = _data.GetById(i);
                writer.WriteLine($"{kontakt.Jmeno} | {kontakt.Prijmeni} | {kontakt.Telefon} | {kontakt.Email}");
            }
        }
    }

    public KontaktKolekce Import()
    {
        var kolekce = new KontaktKolekce();

        if (!File.Exists(path))
        {
            Console.WriteLine("Import: Soubor nenalezen, bude vytvořen nový při exportu.");
            return kolekce;
        }

        var lines = File.ReadAllLines(path);
        foreach (var line in lines)
        {
            var parts = line.Split('|');
            if (parts.Length == 3)
            {
                kolekce.Insert(new Kontakt(
                    parts[0].Trim(),
                    parts[1].Trim(),
                    parts[2].Trim(),
                    parts[3].Trim()
                ));
            }
        }

        Console.WriteLine($"Import: Načteno {kolekce.Count()} kontaktů ze souboru {path}.");
        return kolekce;
    }

}

public class ConsoleUI
{
    private KontaktKolekce _data = new();
    private CommandManager _cmd = new();

    public ConsoleUI()
    {
        _cmd.Execute(new InsertCommand(_data, new Kontakt("Adam", "Levý", "email", "34534234234")));
    }

    public void HledejKontakt()
    {
        Console.WriteLine("Hledejte podle jména:");
        Console.WriteLine("________________________________________________________");
        var search = Console.ReadLine();
        Console.WriteLine($"Hledaný výraz: {search.ToLower()}");
        Console.WriteLine("________________________________________________________");
        var results = _data.Search(search);

        KontaktIterator it = results.CreateIterator();

        var i = 0;

        while (it.HasMore())
        {
            var kontakt = it.Next();

            Console.WriteLine($"{i++}. {kontakt.Jmeno} {kontakt.Prijmeni} | {kontakt.Email} | {kontakt.Telefon}");
        }
    }

    public void SmazKontakt()
    {
        Console.WriteLine();
        Console.WriteLine("Z výpisu zadejte id kontaktu, který chcete smazat:");
        Console.WriteLine("________________________________________________________");
        Console.WriteLine();
        ListKontakty();

        var index = Console.ReadLine();
        int id = int.Parse(index);


        var obj = _data.GetById(id);
        //Console.WriteLine(obj);

        if (id != null)
        {
            _cmd.Execute(new DeleteCommand(_data, obj));
        }
        else
        {
            Console.WriteLine($"Došlo k chybě - existuje kontakt s indexem: {id}?");
        }
    }

    public void VratZmeny()
    {
        _cmd.Undo();
    }
    public void VytvorKopii()
    {
        Console.WriteLine();
        Console.WriteLine("Z výpisu zadejte id kontaktu, který chcete zkopírovat:");
        Console.WriteLine("________________________________________________________");
        Console.WriteLine();
        ListKontakty();

        var index = Console.ReadLine();
        int id = int.Parse(index);


        var obj = _data.GetById(id);
        //Console.WriteLine(obj);

        if (id != null)
        {
            _cmd.Execute(new CopyCommand(_data,obj));
        }
        else
        {
            Console.WriteLine($"Došlo k chybě - existuje kontakt s indexem: {id}");
        }

    }

    public void Nav()
    {
        Console.WriteLine("");
        Console.WriteLine("___________Kontakty___________");
        Console.WriteLine("1) Vypsat kontakty");
        Console.WriteLine("2) Vytvořit kontakt");
        Console.WriteLine("3) Duplikovat kontakt");
        Console.WriteLine("4) Smaz kontakt");
        Console.WriteLine("5) Hledej kontakt");
        Console.WriteLine("");
        Console.WriteLine("/) Vrať změny");
        Console.WriteLine("*) Konec");
    }

    public void VytvorKontakt()
    {
        Console.WriteLine();
        Console.WriteLine("Jméno");
        var jmeno = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine("Příjmení");
        var prijmeni = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine("Email");
        var email = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine("Telefon");
        var telefon = Console.ReadLine();

        //kontrola na emptry string!
        if (jmeno != null && prijmeni != null && email != null && telefon != null) 
        {
            // mám všechno vykonám insert
            _cmd.Execute(new InsertCommand(_data, new Kontakt(jmeno, prijmeni, telefon, email)));
        }
        else
        {
            Console.WriteLine("Vyplňte všechny údaje!");
        }

    }

    public void ListKontakty()
    {
        if (_data.Count() == 0)
        {
            Console.WriteLine("Seznam je prázdný");
        }

        int i = 0;

        KontaktIterator it = _data.CreateIterator();

        while (it.HasMore())
        {
            var kontakt = it.Next();
           
            Console.WriteLine($"{i++}. {kontakt.Jmeno} {kontakt.Prijmeni} | {kontakt.Email} | {kontakt.Telefon}");
        }
    }
    public void Run()
    {
        bool run = true;
        while (run) {
            Nav();
            var key = Console.ReadLine();
            Console.WriteLine("");

            switch (key)
            {
                case "1": ListKontakty(); break;
                case "2": VytvorKontakt(); break;
                case "3": VytvorKopii(); break;
                case "4": SmazKontakt(); break;
                case "5": HledejKontakt(); break;
                case "/": VratZmeny(); break;
                case "*": return;
                default: Console.WriteLine("Neplatná volba."); break;
            }
        }

    }


}

public class Program
{
    public static void Main()
    {
        var ui = new ConsoleUI();
        ui.Run();
    }
}