using System;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Linq;

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

// Observer
public interface ISubscriber
{
    public void update(string? message = null);
}

// User je zároveň i samotný subscriber
public class User : Profile, ISubscriber
{
    public User(string name, string code, string password, string role)
        : base (name, code ,password, role) { }

    public override void info()
    {
        Console.WriteLine($"Jsem uživatel {name}, kód: {code}");
    }

    public void update(string? message = null)
    {
        Console.WriteLine($"{name} dostal notifikaci z kurzu: {message ?? ""}");
    }
}


// Jelikož samotný kurz obsahuje i příslušné studenty (subscribery, tak z něj udělám i publishera)
/*
 Po přihlášení rovnou vytvářím subscribera
 Publisher má metodu i na notifyOne podle parametru, následně notify pro včechny subscribery příslušného kurzu
 */
public class Kurz : Entita
{
    public List<Profile> users = new();
    private readonly List<ISubscriber> subscribers = new();
    public Dictionary<int, string> timetable = new();
    public Dictionary<User, string> evaluation = new();

    public Kurz(string name, string code)
        : base(name, code) { }

    public void subscribe(ISubscriber sub) => subscribers.Add(sub);
    public bool unSubscribe(ISubscriber sub) => subscribers.Remove(sub);

    public override void info()
    {
        Console.WriteLine($"Jsem kurz {name}, kód: {code}");
    }

    public void addUser(Profile user)
    {
        users.Add(user);

        if (user is ISubscriber sub)
        {
            subscribe(sub);
        }

    }

    public void notify(User user, string? message = null)
    {
        foreach (var s in subscribers) s.update(message);
    }

    public void notifyOne(ISubscriber subscriber, string? message = null)
    {
        subscriber.update(message);
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


// Strategy - na kalkulace udělat testy
public interface ICalculation<T>
{
    float calculation(List<T> input);
}

public class AritmeticCalc : ICalculation<float>
{
    public float calculation(List<float> input)
    {
        if (input == null || input.Count == 0)
            throw new ArgumentException("Seznam musí obsahovat alespoň jednu hodnotu.");

        return input.Average(); // spočítá průměr
    }
}

public class WeightedAverageCalc : ICalculation<(float value, float weight)>
{
    public float calculation(List<(float value, float weight)> input)
    {
        if (input == null || input.Count == 0)
            throw new ArgumentException("Seznam musí obsahovat alespoň jednu hodnotu.");

        float sumWeighted = input.Sum(x => x.value * x.weight);
        float sumWeights = input.Sum(x => x.weight);

        if (sumWeights == 0)
            throw new InvalidOperationException("Součet vah nesmí být nulový.");

        return sumWeighted / sumWeights;
    }
}

public class Calculator<T>
{
    public ICalculation<T> strategy;
    public Calculator(ICalculation<T> calc)
    {
        this.strategy = calc;
    }

    public float calculate(List<T> input)
    {
        return strategy.calculation(input);
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
            kurz.addUser(student);
            kurz.notifyOne(student, "Nazdaar a vítej!");
            //Console.WriteLine($"{student.name} byl přidán do kurzu {kurz.name}.");
        }
    }
}
