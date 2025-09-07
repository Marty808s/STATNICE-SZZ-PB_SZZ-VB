// týkající se chování (Příkaz - DONE, Pozorovatel - DONE, Memo - DONE, Iterator - DONE, Strategy - DONE, Mediator - DONE, Visitor - DONE),
// https://refactoring.guru/design-patterns/behavioral-patterns
//-----------------------------------------------------------
//Příkaz - command
// Command drří referenci na recievera - toho obalí a pak je volán
using System.Linq.Expressions;

public interface ICommand
{
    void Execute();
}

//Receiver - ta logika kterou má command spustit
public class DummyAPI
{
    public void Insert(string key, string value)
    {
        Console.WriteLine($"Na insert: {key} : {value}");
    }

    public void Delete(string key, string value)
    {
        Console.WriteLine($"Na delete: {key} : {value}");
    }
}

public class InsertCommand : ICommand
{
    private DummyAPI _api;
    private string _key;
    private string _value;

    public InsertCommand(DummyAPI api, string key, string value)
    {
        _api = api;
        _key = key;
        _value = value;
    }

    public void Execute()
    {
        _api.Insert(this._key, this._value);
    }
}

public class DeleteCommand : ICommand
{
    private DummyAPI _api;
    private string _key;
    private string _value;

    public DeleteCommand(DummyAPI api, string key, string value)
    {
        _api = api;
        _key = key;
        _value = value;
    }
    public void Execute()
    {
        _api.Delete(this._key, this._value);
    }
}

//Invoker - vyvolává akci
public class Button
{
    private ICommand _command;

    public Button(ICommand command)
    { 
        this._command = command;
    }

    public void Click()
    {
        _command.Execute();
    }
}
//-----------------------------------------------------------
//Pozorovatel - observer

//rozhraní pro získání zprávy
public interface ISubcriber
{
    void Update(string message);
}

// poskytovatel zpráv - vytvoří a pošle, řídí seznam "kontatků"
public class Publisher
{
    private List<ISubcriber> _subscribers = new();

    public void Subscribe(ISubcriber subscriber)
    {
        _subscribers.Add(subscriber);
    }

    public void UnSubscribe(ISubcriber subscriber)
    {
        _subscribers.Remove(subscriber);
    }

    // pošli všem
    public void Notify(string message)
    {
        foreach (ISubcriber subscriber in _subscribers)
        {
            subscriber.Update(message);
        }
    }
}

//obejt subscribera, který bude přijímat zprávy
public class ConsoleSubscriber : ISubcriber
{
    private string _name;

    public ConsoleSubscriber(string name)
    {
        _name = name;
    }

    public void Update(string message)
    {
        Console.WriteLine($"Pro: {_name} - zpráva: {message}");
    }
}

//-----------------------------------------------------------
// Strategy - strategie

//mám strategie - soubor objektů, které implementují IStrategy
//mám kontext - ten má uloženou strategy kterou měním přes metodu

public interface IStrategy
{
    public double Calculate(List<int> list);
}

//strategie avg
public class AritmeticCalc : IStrategy
{
    public double Calculate(List<int> list)
    {

        return (double) list.Average();

    }
}

//strategie mean
public class MeanCalc : IStrategy
{
    public double Calculate(List<int> list)
    {
        if (list == null || !list.Any())
            throw new InvalidOperationException("Sekvence je prázdná.");

        var sorted = list.OrderBy(x => x).ToList();
        int count = sorted.Count;
        int middle = count / 2;

        return count % 2 == 0
            ? (sorted[middle - 1] + sorted[middle]) / 2.0
            : sorted[middle];
    }
}

// Využívá strategii
public class User
{
    private IStrategy _strategy;
    public List<int> ints = new();

    public User(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void UseStrategy()
    {
        Console.WriteLine(_strategy.Calculate(ints));
    }

    // jen pro přidání "známky"
    public void Update(int add) {
        ints.Add(add);
    }
}
//-----------------------------------------------------------
//Iterátor
// slouží k procházení kolekcí dat -> pomocí prostředníka

//Objekt DO kolekce - K ITEROVÁNÍ
public class Profile
{
    public string Name;

    public Profile(string name)
    {
        Name = name;
    }

    public void SayHello()
    {
        Console.WriteLine(Name);
    }
}

// iterátor pro objekt MyHomie
public interface ProfileIterator
{
    public Profile Next();
    public bool HasMore();
}

// rozhraní pro kolekci - vytvoř si iterator
public interface Kolekce
{
    public ProfileIterator CreateIterator();

}


// moje kolekce s objekty Profile
public class FriendList : Kolekce
{
    private List<Profile> _friends = new();
    public ProfileIterator CreateIterator()
    {
        return new FriendListIterator(_friends);
    }

    public void Add(Profile p)
    {
        _friends.Add(p); 
    }

    public void Remove(Profile p)
    {
        _friends.Remove(p);
    }
}


//můj iterátor přes třídu profile
public class FriendListIterator : ProfileIterator
{
    private List<Profile> _friends;
    private int _position = 0;

    public FriendListIterator(List<Profile> friends)
    {
        _friends = friends;
    }

    public bool HasMore()
    {
        return _position < _friends.Count;
    }

    public Profile Next()
    {
        if (!HasMore())
            throw new InvalidOperationException("Iterator je na konci.");
        return _friends[_position++];
    }
}

//-----------------------------------------------------------
//Návštěvník - visitor
//obejkty k navštívení - a následné manipulace

// interface objektu, který bude visitor navštěvovat
public interface IJustObject
{
    public void Accept(IVisitor visistor);
}


public class Dot : IJustObject
{
    public string Name = "Dot";
    public int Index = 0;

    public void Draw()
    {
        this.Index++;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitDot(this);
    }

    public void Move(int index)
    {
        this.Index--;
    }
}

public class Line : IJustObject
{
    public string Name = "Line";
    public int Index = 0;

    public void Draw()
    {
        this.Index++;
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitLine(this);
    }

    public void Move(int index)
    {
        this.Index--;
    }
}


//rozhraní Visitor 
public interface IVisitor
{
    public void VisitDot(Dot dot);
    public void VisitLine(Line line);
}


public class JustObjectVisitor : IVisitor
{
    public void VisitDot(Dot dot)
    {
        Console.WriteLine($"{dot.Name} | {dot.Index}");
    }

    public void VisitLine(Line line)
    {
        Console.WriteLine($"{line.Name} | {line.Index}");
    }
}

//-----------------------------------------------------------
//Mediator
// mediator ma jen metodu pro notifikaci - tu volaji komponenty
public interface IMediator
{
    void Notify(object sender, string message);
}


public class ComponentA
{
    private IMediator _mediator;

    // nastav mi mediatora
    public void SetMediator( IMediator mediator )
    {
        _mediator = mediator;
    }

    // nastav mi mediatora - tady by mela byt podminka, jinak nevolej notify a vrat zpravu
    public void OperationA()
    {
        Console.WriteLine("Tohle je operace A");
        _mediator.Notify(this, "A"); //upozorneni mediatorovi
    }

}

public class ComponentB
{
    private IMediator _mediator;

    // nastav mi mediatora
    public void SetMediator(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void OperationB()
    {
        Console.WriteLine("Tohle je operace B");
        _mediator.Notify(this, "B"); //upozorneni mediatorovi
    }

}

// má uložené jednotlivé komponenty - pokud by bylo potřeba zavolat metodu na reakci
public class MyMediator : IMediator
{
    //tady jsou jednotlivé přiřazené komponenty
    private ComponentA _componentaA;
    private ComponentB _componentaB;

    public MyMediator(ComponentA componentaA, ComponentB componentaB)
    {
        _componentaA = componentaA;
        _componentaB = componentaB;
    }

    // metody pro reakci na signály
    public void ReactOnA()
    {
        Console.WriteLine("Zpráva od A");
    }

    public void ReactOnB()
    {
        Console.WriteLine("Zpráva od B");
    }

    public void Notify(object sender, string message)
    {
        // ZDE ZPRACOVÁVÁ OPERACE - a návazně volám signály
        Console.WriteLine($"Zpráva od {sender} | {message}");
        Console.WriteLine("----------------------------------------");
        if (sender == _componentaA)
        {
            // reaguji nas signál
            ReactOnA();
            //component.Metoda() - kdyby bylo nutné
        }
        else
        {
            ReactOnB();
        }
    }
}

//-----------------------------------------------------------
//Mediator

// Memento - udržuje snapshot
public class OriginatorMemento
{
    public string SavedState { get; }

    internal OriginatorMemento(string state)
    {
        SavedState = state;
    }
}


// Originator – uchovává svůj stav, umí jej uložit/restaurovat
class Originator
{
    private string _state; //můj stav

    public void SetState(string state)
    {
        _state = state;
        Console.WriteLine($"Originator: Nastavený stav je '{_state}'");
    }

    public OriginatorMemento SaveToMemento()
    {
        Console.WriteLine("Originator: Ukládám stav do Mementa.");
        return new OriginatorMemento(_state);
    }

    public void RestoreFromMemento(OriginatorMemento memento)
    {
        _state = memento.SavedState;
        Console.WriteLine($"Originator: Stav obnoven z Mementa: '{_state}'");
    }

}

// Caretaker – spravuje uložené stavy (Mementa)
class Caretaker
{
    private readonly List<OriginatorMemento> _history = new();

    public void AddMemento(OriginatorMemento m)
    {
        _history.Add(m);
    }

    public OriginatorMemento GetMemento(int index)
    {
        return _history[index];
    }
}


//-----------------------------------------------------------
//Program
public class Program
{
    static void Main()
    {
        /*
        //Command
        //-reciever
        DummyAPI api = new DummyAPI();
        //-buttony
        Button insertBtn = new Button(new InsertCommand(api,"Pepa", "Hrubý"));
        Button deleteBtn = new Button(new DeleteCommand(api, "Pepa", "Hrubý"));

        insertBtn.Click();
        deleteBtn.Click();
        */

        /*
        //Observer
        Publisher publisher = new Publisher();

        // - příjemci
        var s1 = new ConsoleSubscriber("Petr");
        var s2 = new ConsoleSubscriber("Milan");
        publisher.Subscribe(s1);
        publisher.Subscribe(s2);
        publisher.Notify("Vítejte!");
        // odeberu
        publisher.UnSubscribe(s1);
        publisher.Notify("Přeživší!");
        */

        /*
        //Strategy
        IStrategy mean = new MeanCalc();
        IStrategy avg = new AritmeticCalc();
        var obj = new User(avg);
        obj.Update(5);
        obj.Update(10);
        obj.Update(7);
        obj.UseStrategy();
        obj.Update(5);
        obj.SetStrategy(mean);
        obj.UseStrategy();
        */

        //Iterátor
        /*
        FriendList list = new FriendList();
        list.Add(new Profile("Alice"));
        list.Add(new Profile("Bob"));
        list.Add(new Profile("Charlie"));

        ProfileIterator it = list.CreateIterator();

        while (it.HasMore())
        {
            Profile p = it.Next();
            p.SayHello();
        }
        */

        //Visitor
        /*
        var dot = new Dot();
        var line = new Line();
        var visitor = new JustObjectVisitor();
        dot.Accept(visitor);
        line.Accept(visitor);
        */

        //Mediator
        /*
        var c1 = new ComponentA();
        var c2 = new ComponentB();
        var mediator = new MyMediator(c1,c2);
        c1.SetMediator(mediator);
        c2.SetMediator(mediator);
        c1.OperationA();
        c2.OperationB();
        */

        //Memento
        var originator = new Originator();
        var caretaker = new Caretaker();

        originator.SetState("Stav 1");
        originator.SetState("Stav 2");

        caretaker.AddMemento(originator.SaveToMemento());

        originator.SetState("Stav 3");

        caretaker.AddMemento(originator.SaveToMemento());

        originator.SetState("Stav 4");

        Console.WriteLine("\nObnovuji na předchozí stav...");
        originator.RestoreFromMemento(caretaker.GetMemento(1));





    }
}
