// https://refactoring.guru/design-patterns/creational-patterns
//Factory - objektová - DONE, abstract - DONE
//Builder - DONE
//Singleton - DONE
//Prototype - DONE

//---------------------------------------------------------------------------------------------------------
//Factory - basic
using System.Globalization;

public interface IProduct
{
    public void doStuff();
}

//Produkt A
public class ProductA: IProduct
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt A!");
    }
}

//Produkt A
public class ProductB : IProduct
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt B!");
    }
}

// Abstract class Creatora pro jednotlivé factory
public abstract class Creator
{
    public abstract IProduct createProduct();
}

// přepisují createProduct a vrací svůj produkt
// override - přepisuje abstraktní metodu z rodiče
public class CreatorA : Creator
{
    public override IProduct createProduct()
    {
        return new ProductA();
    }
}

// přepisují createProduct a vrací svůj produkt
// override - přepisuje abstraktní metodu z rodiče
public class CreatorB : Creator
{
    public override IProduct createProduct()
    {
        return new ProductB();
    }
}

//---------------------------------------------------------------------------------------------------------
//Factory - abstract

//interface pro jednotlivé typy produktů - u mě A a B podle jejich rozhraních -> které vytváří 2 factory - 1 a 2
public interface IProductA
{
    public void doStuff();
}

public interface IProductB
{
    public void doStuff();
}

// produkty pro factory 1
public class Product1A: IProductA
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt A z factory 1");
    }
}

public class Product1B : IProductB
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt B z factory 1");
    }
}


// produkty pro factory 2
public class Product2A: IProductA
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt A z factory 2");
    }
}

public class Product2B: IProductB
{
    public void doStuff()
    {
        Console.WriteLine("Jsem produkt B z factory 2");
    }
}


// interface který na rozdíl od classic factory vytváří rozhraní pro tvorbu více produktů
public interface IAbstractFactory
{
    public IProductA createStuff1();
    public IProductB createStuff2();
}

// factory 1 na produkty z IAbstractFactory
public class Product1Factory: IAbstractFactory
{
    public IProductA createStuff1()
    {
        return new Product1A();
    }
    public IProductB createStuff2()
    {
        return new Product1B();
    }
}

// factory 2 na produkty z IAbstractFactory
public class Product2Factory : IAbstractFactory
{
    public IProductA createStuff1()
    {
        return new Product2A();
    }
    public IProductB createStuff2()
    {
        return new Product2B();
    }
}

//---------------------------------------------------------------------------------------------------------
//Singleton
public class Singleton
{
    public static Singleton instance;
    // private konstruktor - nemůřu vytvořit instanci mimo třídu
    private Singleton() { }

    public static Singleton getInstance()
    {
        if (instance == null)
        {
            Console.WriteLine("Nastavuji sess");
            instance = new Singleton();
        }
        Console.WriteLine("Vracím instanci");
        return instance;
    }
}
//---------------------------------------------------------------------------------------------------------
// Builder
public interface IBuilderProduct
{
    string type { get; set; }
    int wheels { get; set; }
    int cylinders { get; set; }
    float hp { get; set; }
}
public interface IBuilder
{
    void stepOne();
    void stepTwo();
    void stepThree();
    IBuilderProduct getResult();

}

public class BuildingA: IBuilderProduct
{
    public string type { get; set; } = "TypeA";
    public int wheels { get; set; }
    public int cylinders { get; set; }
    public float hp { get; set; }
}

public class BuildingB: IBuilderProduct
{
    public string type { get; set; } = "TypeB";
    public int wheels { get; set; }
    public int cylinders { get; set; }
    public float hp { get; set; }
}

public class BuilderOne: IBuilder
{
    private BuildingA result;
    public BuilderOne()
    {
        this.result = new BuildingA();
    }

    public void stepOne()
    {
        this.result.cylinders = 4;
    }

    public void stepTwo()
    {
        this.result.wheels = 4;
    }

    public void stepThree()
    {
        this.result.hp = 1400.0f;
    }

    public IBuilderProduct getResult()
    {
        return this.result;
    }
}


public class BuilderTwo: IBuilder
{
    private BuildingB result;
    public BuilderTwo()
    {
        this.result = new BuildingB();
    }

    public void stepOne()
    {
        this.result.cylinders = 2;
    }

    public void stepTwo()
    {
        this.result.wheels = 3;
    }

    public void stepThree()
    {
        this.result.hp = 100.0f;
    }

    public IBuilderProduct getResult()
    {
        return this.result;
    }
}


public class Director
{
    private IBuilder builder;

    public Director(IBuilder builder)
    {
        this.builder = builder;
    }

    public void changeBuilder(IBuilder builder)
    {
        this.builder = builder;
    }

    public IBuilderProduct make(string type)
    {
        if (type == "no_engine")
        {
            builder.stepOne();
            builder.stepTwo();
        } else
        {
            builder.stepOne();
            builder.stepTwo();
            builder.stepThree();
        }
        return builder.getResult();
    }
}
//---------------------------------------------------------------------------------------------------------
// Prototype
public abstract class Prototype
{
    public string field { get; set; }
    public abstract Prototype clone();
}


public class ConcretePrototype : Prototype
{
    public ConcretePrototype(string _field)
    {
        this.field = _field;
    }

    public ConcretePrototype(Prototype prototype)
    {
        this.field = prototype.field;
    }

    public override Prototype clone()
    {
        return new ConcretePrototype(this);
    }
}


//---------------------------------------------------------------------------------------------------------
// Program sekce
public class Program
{
    public static void Main()
    {
        // Factory - basic
        /*
        Creator creatorA = new CreatorA();
        IProduct productA = creatorA.createProduct();
        productA.doStuff();  // -> "Jsem produkt A!"

        Creator creatorB = new CreatorB();
        IProduct productB = creatorB.createProduct();
        productB.doStuff();  // -> "Jsem produkt B!"
        */

        //Factory abstract
        // Továrny Product1Factory/Product2Factory vrací konzistentní „rodiny“ produktů A a B a klient může pracovat jen přes IAbstractFactory.
        /*
        IAbstractFactory factory = new Product1Factory();
        IProductA p1f1 = factory.createStuff1();
        IProductB p1f2 = factory.createStuff2();
        p1f1 .doStuff();
        p1f2 .doStuff();

        IAbstractFactory factory2 = new Product2Factory();
        IProductA p2f1 = factory2.createStuff1();
        IProductB p2f2 = factory2.createStuff2();
        p2f1.doStuff();
        p2f2.doStuff();
        */

        //Singleton
        /*
        var s1 = Singleton.getInstance();
        var s2 = Singleton.getInstance();
        Console.WriteLine(ReferenceEquals(s1, s2)); // True
        */

        //Builder

        BuilderOne b1 = new BuilderOne();
        BuilderTwo b2 = new BuilderTwo();
        Director director = new Director(b1);
        IBuilderProduct prod = director.make("");
        Console.WriteLine(prod.hp);
        director.changeBuilder(b2);
        IBuilderProduct p2 = director.make("no_engine");
        Console.WriteLine(p2.hp);


        //Prototype
        /*
        var p1 = new ConcretePrototype("Originál");
        var p2 = p1.clone();

        Console.WriteLine(p1.field); // Originál
        Console.WriteLine(p2.field); // Originál
        Console.WriteLine(Object.ReferenceEquals(p1, p2)); // False
        */
 


    }
}


