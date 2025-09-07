/*
dotnet new xunit -n SchoolApp.Tests
dotnet add SchoolApp.Tests/SchoolApp.Tests.csproj reference path/k/tvemu/hlavnimu/Projektu.csproj
dotnet test

--> jít do závislostí, pøidat referenci na projekt --> pøidám dll

spuštìní testu/pravím na test cs soubor a dát testovat. Následnì ve výstupech pøejít na testy:
dotnet test
*/

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
    }
}