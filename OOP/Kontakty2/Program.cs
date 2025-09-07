//1.Prototype: Pro efektivní vytváření a kopírování kontaktních záznamů. - DONE
//2. Command: Pro zaznamenávání a možné vrácení změn v kontaktech.
//3. Iterator: Pro navigaci skrze kolekci kontaktů.

// čas 18 minut
//--------------------------------------------------
// Prototype - pro mě kontakt
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



//--------------------------------------------------
//Command
public interface ICommand
{
    void Execute();
}

//Reaciever - ten spustí logiku - jednotlivé operace
public class KontaktReciever
{

}