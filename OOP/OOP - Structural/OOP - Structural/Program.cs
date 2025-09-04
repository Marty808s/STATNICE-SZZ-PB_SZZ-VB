// strukturální návrhové vzory (Adaptér - DONE, Dekorátor - DONE, Most - DONE, Muší váha - IN PROGRESS)
// https://refactoring.guru/design-patterns/structural-patterns
//-----------------------------------------------------------------------------------
//Adaptér
using System.Drawing;
using System.Xml.Linq;

public interface App1
{
    void Method(); 
}

public class Adapter : App1
{
    private Service adaptTo;

    public Adapter(Service sluzba)
    {
        this.adaptTo = sluzba;
    }


    public void Method()
    {
        List<int> list = new List<int> { 1,3, 4 };
        var output_list = Formater(list);

        adaptTo.ServiceMethod(output_list);

    }

    // pomocná funkce na konvertování a zavolání service funkce
    public List<float> Formater(List<int> input_data)
    {
        List<float> output_list = new List<float>();

        foreach (var item in input_data)
        {
            output_list.Add((float)(item%2));
        }
        return output_list;
    }
}

public class Service
{
    public void ServiceMethod(List<float> data)
    {
        foreach (var item in data)
        {
            Console.WriteLine(item);
        }
    }
}
//-----------------------------------------------------------------------------------
//Dekorátor
// rozhraní - které všichni musí implementovat
public interface IWriter
{
    void Write(string message);
}

// první implementace - základní implementace - tu budu wrapovat
public class ConsoleWriter : IWriter
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }

}

// první dekorace třídy ConsoleWriter
public class UpperWriter : IWriter
{
    private IWriter _base;

    public UpperWriter(IWriter component)
    {
        this._base = component;
    }

    public void Write(string message)
    {
        //rozčíření funkčnosti objektu
        _base.Write(message.ToUpper());
    }

}

// druhá dekorace třídy ConsoleWriter
public class LowerWriter : IWriter
{
    private IWriter _base;

    public LowerWriter(IWriter component)
    {
        _base = component;
    }

    public void Write(string message)
    {
        //rozčíření funkčnosti objektu
        _base.Write(message.ToLower());
    }

}

//-----------------------------------------------------------------------------------
//Most

public abstract class Color
{
    public string name { get; set; }
}

public class Red : Color
{
    public Red() { name = "Red"; }
}

public class Blue : Color
{
    public Blue() { name = "Blue"; }
}


public abstract class Shape2D
{
    private Color color; //tohle je už ten bridge
    public string name { get; set; }
    public int x_vector { get; set; }
    public int y_vector { get; set; }
    public bool fill { get; set; } = false;

    public Shape2D(Color _color)
    {
        color = _color;
    }

    public void setColor(Color _color)
    {
        color = _color;
    }

    public void Render()
    {
        Console.WriteLine($"Jsem {name}, mám barvu {color.name}, fill: {fill}");
    }

}

public class Rect : Shape2D
{
    public int side { get; set; }

    public Rect(Color color, int _side) : base(color)
    {
        side = _side;
        name = "Obdélník";
    }
}

public class Circle : Shape2D
{
    public int r { get; set; }
    public Circle(Color color, int _r) : base(color)
    {
        r = _r;
        name = "Kruh";
    }
}

//-----------------------------------------------------------------------------------
//Muší váha
// mám dva stavy: intrinsic - sdílený, neměnný - textury, tvuky atd.
//                extrinsic - dodává se při každém volání - pozice, rotace





//-----------------------------------------------------------------------------------
//Program
class Program
{
    static void Main()
    {
        //Adapter
        /*
        Service service = new Service();
        var adapter = new Adapter(service);
        adapter.Method();
        */

        //Dekorátor
        /*
        var text = "Hello World!!!";
        IWriter console_writer = new ConsoleWriter();
        IWriter upper_writer = new UpperWriter(console_writer);
        IWriter lower_writer = new LowerWriter(console_writer);

        console_writer.Write(text);
        upper_writer.Write(text);
        lower_writer.Write(text);
        */

        //Most
        /*
        Shape2D redCircle = new Circle(new Red(),4);
        Shape2D blueRec= new Rect(new Blue(), 8);

        redCircle.Render();
        blueRec.Render();
        */

    }
}