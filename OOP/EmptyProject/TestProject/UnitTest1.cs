/*
dotnet new xunit -n SchoolApp.Tests
dotnet add SchoolApp.Tests/SchoolApp.Tests.csproj reference path/k/tvemu/hlavnimu/Projektu.csproj
dotnet test

--> j�t do z�vislost�, p�idat referenci na projekt --> p�id�m dll

spu�t�n� testu/prav�m na test cs soubor a d�t testovat. N�sledn� ve v�stupech p�ej�t na testy:
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