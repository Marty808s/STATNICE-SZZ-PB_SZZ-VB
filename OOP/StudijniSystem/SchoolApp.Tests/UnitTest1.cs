/*
dotnet new xunit -n SchoolApp.Tests
dotnet add SchoolApp.Tests/SchoolApp.Tests.csproj reference path/k/tvemu/hlavnimu/Projektu.csproj
dotnet test

--> jít do závislostí, pøidat referenci na projekt --> pøidám dll

spuštìní testu/pravím na test cs soubor a dát testovat. Následnì ve výstupech pøejít na testy:
dotnet test
*/

namespace SchoolApp.Tests;

public class UnitTest1
{
    [Fact]
    public void KurzCreator_ShouldIgnorePasswordAndRoleParameters()
    {
        var creator = new KurzCreator();

        // I když pošleme password/role, mìlo by se vytvoøit Kurz bez ohledu na nì
        var entita = creator.createProduct("Algoritmy", "ALG101", "ignoredPass", "ignoredRole");

        var kurz = Assert.IsType<Kurz>(entita);
        Assert.Equal("Algoritmy", kurz.name);
        Assert.Equal("ALG101", kurz.code);
    }
}