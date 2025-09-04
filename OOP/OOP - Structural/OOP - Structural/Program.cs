// strukturální návrhové vzory (Adaptér - DONE, Dekorátor, Most, Muší váha)
// https://refactoring.guru/design-patterns/structural-patterns
//-----------------------------------------------------------------------------------
//Adaptér

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


//-----------------------------------------------------------------------------------
//Program
class Program
{
    static void Main()
    {
        Service service = new Service();
        var adapter = new Adapter(service);
        adapter.Method();
    }
}