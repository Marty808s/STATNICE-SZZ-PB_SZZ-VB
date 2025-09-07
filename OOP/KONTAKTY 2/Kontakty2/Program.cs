//1.Prototype: Pro efektivní vytváření a kopírování kontaktních záznamů. - DONE
//2. Command: Pro zaznamenávání a možné vrácení změn v kontaktech.
//3. Iterator: Pro navigaci skrze kolekci kontaktů.

// čas 2 hodiny
//--------------------------------------------------
// Prototype - pro mě kontakt
using System.Runtime.CompilerServices;
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
            //throw new InvalidOperationException("Iterator nemá kam jít - je na konci, jdu na začátek");
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
        copy.Jmeno = $"{kontak.Jmeno} (kopie)";
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
            var status = last_command.Execute();
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


public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello World!");
    }
}