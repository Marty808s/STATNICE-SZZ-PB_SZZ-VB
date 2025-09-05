// týkající se chování (Příkaz - DONE, Pozorovatel - DONE, Memo, Iterator, Observer, Strategy),
//-----------------------------------------------------------
//Příkaz - command
// Command drří referenci na recievera - toho obalí a pak je volán
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
// Memo - memento





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



    }
}
