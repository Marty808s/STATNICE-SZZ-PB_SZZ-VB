// ZÁKLADNÍ TYPY
using System.Runtime.CompilerServices;

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
    public Dictionary<User, List<(float value, float weight)>> evaluation = new();

    public Kurz(string name, string code)
        : base(name, code) { }

    public void subscribe(ISubscriber sub) => subscribers.Add(sub);
    public bool unSubscribe(ISubscriber sub) => subscribers.Remove(sub);

    public override void info()
    {
        Console.WriteLine($"Jsem kurz {name}, kód: {code}");

        if (timetable.Count == 0)
        {
            Console.WriteLine("Rozvrh: zatím není nastaven.");
        }
        else
        {
            Console.WriteLine("Rozvrh:");
            foreach (var entry in timetable)
            {
                Console.WriteLine($"  Den {entry.Key}: {entry.Value}");
            }
        }
    }

    public void addUser(Profile user)
    {
        users.Add(user);

        if (user is ISubscriber sub)
        {
            subscribe(sub);
        }

    }

    public void notify(string? message = null)
    {
        foreach (var s in subscribers) s.update(message);
    }

    public void notifyOne(ISubscriber subscriber, string? message = null)
    {
        subscriber.update(message);
    }

    public void rename(string newName)
    {
        name = newName;
        notify($"Kurz '{code}' změnil název na '{name}'.");
    }

    public void setTimetableEntry(int day, string description)
    {
        timetable[day] = description;
        notify($"Kurz '{name}' aktualizoval rozvrh: {day} -> {description}");
    }

    public void addGrade(User student, float value, float weight = 1f)
    {
        if (!users.Contains(student))
            throw new InvalidOperationException("Student není zapsán v tomto kurzu.");

        if (!evaluation.ContainsKey(student))
            evaluation[student] = new List<(float value, float weight)>();

        evaluation[student].Add((value, weight));
        notifyOne(student, $"V kurzu '{name}' přidána známka: {value} (váha {weight}).");
    }

    public List<User> getStudents()
    {
        return users
        .OfType<User>()
        .Where(u => u.role == "student")
        .ToList();
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
    private User? currentUser;

    private readonly Creator studentCreator = new StudentCreator();
    private readonly Creator teacherCreator = new UcitelCreator();
    private readonly Creator adminCreator = new SpravceCreator();
    private readonly Creator courseCreator = new KurzCreator();

    WeightedAverageCalc weightedAvgCalc = new WeightedAverageCalc();
    AritmeticCalc avgCalc = new AritmeticCalc();

    // jednoduchá „DB“ v paměti
    private readonly List<Entita> entities = new();

    // ===== Entry point =====
    public static void Main()
    {
        var app = new Program();
        app.SeedData();
        app.Run();
    }


    // Předvyplnění dat
    private void SeedData()
    {
        entities.Add(studentCreator.createProduct("Petr", "ST123", "heslo123"));     // student
        entities.Add(teacherCreator.createProduct("Eva", "TE001", "ucitelHeslo"));   // učitel
        entities.Add(adminCreator.createProduct("Adam", "AD001", "adminHeslo"));   // správce (admin)
        entities.Add(courseCreator.createProduct("Algoritmy", "ALG101"));
        entities.Add(courseCreator.createProduct("Datové struktury", "DS201"));
    }

    private void MainMenuCommon()
    {
        Console.Clear();
        Console.WriteLine("Obecné menu – zatím bez specifických akcí.");
        Pause();
    }

    // Hlavní běh aplikace
    private void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            if (currentUser == null)
            {
                ShowWelcome();
                HandleAuth();
            }
            else
            {
                if (currentUser.role == "student")
                    StudentMenu();
                else if (currentUser.role == "ucitel" || currentUser.role == "spravce")
                    TeacherMenu();
                else
                    MainMenuCommon();

                Console.Write("\n(P) Pokračovat  |  (O) Odhlásit  |  (K) Konec: ");
                var cmd = (Console.ReadLine() ?? "").Trim().ToUpperInvariant();
                if (cmd == "O") currentUser = null;
                else if (cmd == "K") break;
            }
        }
    }

    // ===== Přihlašovací obrazovka =====
    private void ShowWelcome()
    {
        Console.Clear();
        Console.WriteLine("====================================");
        Console.WriteLine("   STUDIJNÍ SYSTÉM – Konzolová app  ");
        Console.WriteLine("====================================");
        Console.WriteLine("1) Přihlášení");
        Console.WriteLine("2) Registrace");
        Console.WriteLine("0) Konec");
        Console.Write("\nVyberte akci: ");
    }

    private void HandleAuth()
    {
        switch ((Console.ReadLine() ?? "").Trim())
        {
            case "1": Login(); Pause(); break;
            case "2": Register(); Pause(); break;
            case "0": Environment.Exit(0); break;
            default: Console.WriteLine("Neplatná volba."); Pause(); break;
        }
    }

    // ===== Login / Registrace =====
    private void Login()
    {
        Console.Write("Uživatelský kód: ");
        var code = Console.ReadLine()?.Trim();
        Console.Write("Heslo: ");
        var pass = Console.ReadLine()?.Trim();

        var found = entities.OfType<User>()
            .FirstOrDefault(u => u.code == code && u.password == pass);

        if (found != null)
        {
            currentUser = found;
            Console.WriteLine($"\nPřihlášen jako {found.name} ({found.role}).");
        }
        else
        {
            Console.WriteLine("\nNeplatné přihlašovací údaje.");
        }
    }

    private void Register()
    {
        Console.Write("Role (student/ucitel): ");
        var role = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
        if (role != "student" && role != "ucitel")
        {
            Console.WriteLine("Povolené role: student, ucitel.");
            return;
        }

        Console.Write("Jméno: ");
        var name = (Console.ReadLine() ?? "").Trim();

        Console.Write("Kód: ");
        var code = (Console.ReadLine() ?? "").Trim();

        if (entities.OfType<User>().Any(u => u.code == code))
        {
            Console.WriteLine("Uživatel s tímto kódem už existuje.");
            return;
        }

        Console.Write("Heslo: ");
        var pass = (Console.ReadLine() ?? "").Trim();

        try
        {
            var creator = role == "student" ? studentCreator : teacherCreator;
            var ent = creator.createProduct(name, code, pass, role);
            if (ent is User u)
            {
                entities.Add(u);
                currentUser = u;
                Console.WriteLine($"\nRegistrace úspěšná, přihlášen jako {u.name} ({u.role}).");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba: {ex.Message}");
        }
    }

    // ===== Menu STUDENTA =====
    private void StudentMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"== MENU STUDENTA: {currentUser!.name} ==");
            Console.WriteLine("1) Zobrazit kurzy");
            Console.WriteLine("2) Zapsat se do kurzu");
            Console.WriteLine("3) Můj studijní plán (zapsané kurzy)");
            Console.WriteLine("4) Moje známky a průměry");
            Console.WriteLine("0) Zpět");
            Console.Write("Volba: ");

            var c = (Console.ReadLine() ?? "").Trim();
            if (c == "0") return;

            switch (c)
            {
                case "1": ListCourses(); Pause(); break;
                case "2": EnrollToCourse(currentUser!); Pause(); break;
                case "3": ShowMyPlan(currentUser!); Pause(); break;
                case "4": ShowMyGradesAndAverages(currentUser!); Pause(); break;
                default: Console.WriteLine("Neplatná volba."); Pause(); break;
            }
        }
    }

    // ===== Menu UČITELE / SPRÁVCE =====
    private void TeacherMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"== MENU UČITELE/SPRÁVCE: {currentUser!.name} ==");
            Console.WriteLine("1) Zobrazit kurzy");
            Console.WriteLine("2) Vytvořit kurz");
            Console.WriteLine("3) Upravit kurz (název/rozvrh)");
            Console.WriteLine("4) Zapsat studenta do kurzu");
            Console.WriteLine("5) Zadání známky studentovi");
            Console.WriteLine("6) Přehled výsledků a průměrů v kurzu");
            Console.WriteLine("0) Zpět");
            Console.Write("Volba: ");

            var c = (Console.ReadLine() ?? "").Trim();
            if (c == "0") return;

            switch (c)
            {
                case "1": ListCourses(); Pause(); break;
                case "2": CreateCourse(); Pause(); break;
                case "3": EditCourse(); Pause(); break;
                case "4": EnrollStudentToCourse(); Pause(); break;
                case "5": AddGradeToStudent(); Pause(); break;
                case "6": ShowCourseResultsAndAverages(); Pause(); break;
                default: Console.WriteLine("Neplatná volba."); Pause(); break;
            }
        }
    }

    // ===== Funkce UI =====
    private void ListCourses()
    {
        var courses = entities.OfType<Kurz>().ToList();
        if (courses.Count == 0) { Console.WriteLine("Žádné kurzy."); return; }

        Console.WriteLine("\n--- Kurzy ---");
        for (int i = 0; i < courses.Count; i++)
        {
            Console.Write($"{i + 1}) ");
            courses[i].info();
        }
    }

    private void CreateCourse()
    {
        Console.Write("Název kurzu: ");
        var name = (Console.ReadLine() ?? "").Trim();
        Console.Write("Kód kurzu: ");
        var code = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(code))
        {
            Console.WriteLine("Název i kód musí být vyplněny.");
            return;
        }
        if (entities.OfType<Kurz>().Any(k => k.code == code))
        {
            Console.WriteLine("Kurz s tímto kódem už existuje.");
            return;
        }

        entities.Add(courseCreator.createProduct(name, code));
        Console.WriteLine("Kurz vytvořen.");
    }

    private void EditCourse()
    {
        var kurz = PickCourse();
        if (kurz == null) return;

        Console.WriteLine("1) Změnit název");
        Console.WriteLine("2) Nastavit / změnit položku rozvrhu (den → popis)");
        Console.Write("Volba: ");
        var c = (Console.ReadLine() ?? "").Trim();

        switch (c)
        {
            case "1":
                Console.Write("Nový název: ");
                var newName = (Console.ReadLine() ?? "").Trim();
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    kurz.rename(newName); // Observer: notifikuje všechny studenty
                    Console.WriteLine("Název změněn (notifikace odeslány).");
                }
                break;

            case "2":
                Console.Write("Den (int): ");
                if (!int.TryParse(Console.ReadLine(), out int day))
                { Console.WriteLine("Neplatný den."); break; }

                Console.Write("Popis: ");
                var desc = Console.ReadLine() ?? "";
                kurz.setTimetableEntry(day, desc); // Observer: notifikuje
                Console.WriteLine("Rozvrh upraven (notifikace odeslány).");
                break;

            default:
                Console.WriteLine("Neplatná volba.");
                break;
        }
    }

    private void EnrollToCourse(User student)
    {
        var kurz = PickCourse();
        if (kurz == null) return;

        kurz.addUser(student);
        kurz.notifyOne(student, $"Byl jsi zapsán do kurzu '{kurz.name}'.");
        Console.WriteLine("Zápis proběhl.");
    }

    private void EnrollStudentToCourse()
    {
        var student = PickUser(roleFilter: "student");
        if (student == null) return;
        EnrollToCourse(student);
    }

    private void AddGradeToStudent()
    {
        var kurz = PickCourse();
        if (kurz == null) return;

        var studenti = kurz.getStudents();
        if (studenti.Count == 0) { Console.WriteLine("V kurzu nejsou žádní studenti."); return; }

        Console.WriteLine("\n--- Studenti v kurzu ---");
        for (int i = 0; i < studenti.Count; i++)
            Console.WriteLine($"{i + 1}) {studenti[i].name} ({studenti[i].code})");

        Console.Write("Vyber studenta: ");
        if (!int.TryParse(Console.ReadLine(), out int sidx) || sidx < 1 || sidx > studenti.Count)
        { Console.WriteLine("Neplatná volba."); return; }

        Console.Write("Známka (float): ");
        if (!float.TryParse(Console.ReadLine(), out float val))
        { Console.WriteLine("Neplatná známka."); return; }

        Console.Write("Váha (float, Enter = 1): ");
        var wraw = Console.ReadLine();
        float weight = 1f;
        if (!string.IsNullOrWhiteSpace(wraw) && !float.TryParse(wraw, out weight))
        { Console.WriteLine("Neplatná váha."); return; }

        try
        {
            kurz.addGrade(studenti[sidx - 1], val, weight);
            Console.WriteLine("Známka uložena (notifikace studentovi odeslána).");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba: {ex.Message}");
        }
    }

    private void ShowMyPlan(User me)
    {
        var myCourses = entities.OfType<Kurz>()
                                .Where(k => k.users.Contains(me))
                                .ToList();

        if (myCourses.Count == 0) { Console.WriteLine("Nejsi zapsán v žádném kurzu."); return; }

        Console.WriteLine("\n--- Můj studijní plán ---");
        foreach (var c in myCourses)
            Console.WriteLine($"• {c.name} ({c.code})");
    }

    private void ShowMyGradesAndAverages(User me)
    {
        var myCourses = entities.OfType<Kurz>()
                                .Where(k => k.users.Contains(me))
                                .ToList();

        if (myCourses.Count == 0) { Console.WriteLine("Žádné záznamy."); return; }

        Console.WriteLine("\nVýpočet průměru: 1) Aritmetický  |  2) Vážený");
        Console.Write("Volba: ");
        var c = (Console.ReadLine() ?? "").Trim();

        foreach (var kurs in myCourses)
        {
            var grades = kurs.evaluation.TryGetValue(me, out var list) ? list : new List<(float value, float weight)>();
            if (grades.Count == 0)
            {
                Console.WriteLine($"\n{kurs.name} ({kurs.code}): žádné známky.");
                continue;
            }

            float avg;
            if (c == "2")
            {
                var calc = new Calculator<(float, float)>(weightedAvgCalc);
                avg = calc.calculate(grades);
            }
            else
            {
                var calc = new Calculator<float>(avgCalc);
                avg = calc.calculate(grades.Select(g => g.value).ToList());
            }

            Console.WriteLine($"\n{kurs.name} ({kurs.code})");
            Console.WriteLine($"Známky: {string.Join(", ", grades.Select(g => $"{g.value} (w={g.weight})"))}");
            Console.WriteLine($"Průměr: {avg:F2}");
        }
    }

    private void ShowCourseResultsAndAverages()
    {
        var kurz = PickCourse();
        if (kurz == null) return;

        Console.WriteLine("\nVýpočet průměru: 1) Aritmetický  |  2) Vážený");
        Console.Write("Volba: ");
        var c = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine($"\n=== Výsledky – {kurz.name} ({kurz.code}) ===");
        foreach (var kv in kurz.evaluation)
        {
            var student = kv.Key;
            var grades = kv.Value;
            if (grades.Count == 0) continue;

            float avg;
            if (c == "2")
            {
                var calc = new Calculator<(float, float)>(new WeightedAverageCalc());
                avg = calc.calculate(grades);
            }
            else
            {
                var calc = new Calculator<float>(new AritmeticCalc());
                avg = calc.calculate(grades.Select(g => g.value).ToList());
            }

            Console.WriteLine($"{student.name} ({student.code}): " +
                              $"[{string.Join(", ", grades.Select(g => $"{g.value} (w={g.weight})"))}] → průměr {avg:F2}");
        }
    }

    // ===== Výběrové / pomocné metody =====
    private Kurz? PickCourse()
    {
        var courses = entities.OfType<Kurz>().ToList();
        if (courses.Count == 0) { Console.WriteLine("Žádné kurzy."); return null; }

        Console.WriteLine("\n--- Kurzy ---");
        for (int i = 0; i < courses.Count; i++)
            Console.WriteLine($"{i + 1}) {courses[i].name} ({courses[i].code})");
        Console.Write("Vyber kurz: ");

        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > courses.Count)
        { Console.WriteLine("Neplatná volba."); return null; }

        return courses[idx - 1];
    }

    private User? PickUser(string? roleFilter = null)
    {
        var users = entities.OfType<User>()
                            .Where(u => roleFilter == null || u.role == roleFilter)
                            .ToList();

        if (users.Count == 0) { Console.WriteLine("Žádní uživatelé."); return null; }

        Console.WriteLine("\n--- Uživatelé ---");
        for (int i = 0; i < users.Count; i++)
            Console.WriteLine($"{i + 1}) {users[i].name} ({users[i].role}, {users[i].code})");
        Console.Write("Vyber uživatele: ");

        if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > users.Count)
        { Console.WriteLine("Neplatná volba."); return null; }

        return users[idx - 1];
    }

    private static void Pause()
    {
        Console.Write("\nPokračujte stiskem Enter...");
        Console.ReadLine();
    }
}


