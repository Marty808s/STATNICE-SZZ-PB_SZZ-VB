using System;
using System.Security.Principal;

// ZÁKLADNÍ TYPY
public abstract class Entita
{
    public string name;
    public string code;

    protected Entita(string name, string code)
    {
        this.name = name;
        this.code = code;
    }

    public abstract void info(); // v C# se přepisuje 'override'
}

public abstract class Profile : Entita
{
    public string password;
    public string role;

    protected Profile(string name, string code, string password, string role)
        : base(name, code)
    {
        this.password = password;
        this.role = role;
    }
}

// PRODUKTY
public class User : Profile
{
    public User(string name, string code, string password, string role)
        : base (name, code ,password, role) { }

    public override void info()
    {
        Console.WriteLine($"Jsem uživatel {name}, kód: {code}");
    }
}

public class Kurz : Entita
{
    public List<Profile> users = new();
    public Kurz(string name, string code)
        : base(name, code) { }

    public override void info()
    {
        Console.WriteLine($"Jsem kurz {name}, kód: {code}");
    }
}

// FACTORY METHOD
public abstract class Creator
{
    public abstract Entita createProduct(string name, string code, string? password = null, string? role = null);
}
public class StudentCreator : Creator
{
    public override Entita createProduct(string name, string code, string? password = null, string? role = null)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Student vyžaduje password.", nameof(password));

        role ??= "student";
        return new User(name, code, password, role);
    }
}

public class UcitelCreator : Creator
{
    public override Entita createProduct(string name, string code, string? password = null, string? role = null)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Učitel vyžaduje password.", nameof(password));

        role ??= "ucitel"; 
        return new User(name, code, password, role);
    }
}

public class SpravceCreator : Creator
{
    public override Entita createProduct(string name, string code, string? password = null, string? role = null)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Správce vyžaduje password.", nameof(password));

        role ??= "spravce";
        return new User(name, code, password, role);
    }
}

public class KurzCreator : Creator
{
    public override Entita createProduct(string name, string code, string? password = null, string? role = null)
    {

        return new Kurz(name, code);
    }
}

public class Program
{
    public static void Main()
    {
        // --- Inicializace creatorů ---
        Creator studentCreator = new StudentCreator();
        Creator teacherCreator = new UcitelCreator();
        Creator adminCreator = new SpravceCreator();
        Creator courseCreator = new KurzCreator();

        // --- Vytvoření instancí pomocí factory ---
        List<Entita> entities = new List<Entita>
        {
            studentCreator.createProduct("Petr", "ST123", "heslo123"),
            teacherCreator.createProduct("Eva", "TE001", "ucitelHeslo"),
            adminCreator.createProduct("Adam", "AD001", "adminHeslo"),
            courseCreator.createProduct("Algoritmy", "ALG101")
        };

        Console.WriteLine("=== Seznam vytvořených entit ===");
        foreach (var entity in entities)
        {
            entity.info();
        }

        Console.WriteLine("\n=== Přihlašování na kurz ===");
        if (entities[0] is User student && entities[3] is Kurz kurz)
        {
            kurz.users.Add(student);
            Console.WriteLine($"{student.name} byl přidán do kurzu {kurz.name}.");
        }
    }
}
