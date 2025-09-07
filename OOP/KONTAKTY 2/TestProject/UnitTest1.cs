namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void PrototypeCopyKontakt()
        {
            var k1 = new Kontakt("Petr", "Rychlý", "411 236 541", "tohle je email");
            var k2 = k1.Clone();

            Assert.Equal(k1.Jmeno, k2.Jmeno);
            Assert.Equal(k1.Prijmeni, k2.Prijmeni);

        }

        [Fact]
        public void OneKontaktInsert()
        {
            var cmdMng = new CommandManager();
            var kontakty = new KontaktKolekce();

            var k1 = new Kontakt("Petr", "Rychlý", "411 236 541", "tohle je email");
            cmdMng.Execute(new InsertCommand(kontakty, k1));

            var iterator = kontakty.CreateIterator();
            Assert.True(iterator.HasMore());

        }

        [Fact]
        public void InsertKontaktsIntoCollectionAndIterate()
        {
            var cmdMng = new CommandManager();
            var kontakty = new KontaktKolekce();

            var k1 = new Kontakt("Petr", "Rychlý", "411 236 541", "tohle je email");
            var k2 = new Kontakt("Adam", "Rychlý", "411 236 541", "tohle je email");

            cmdMng.Execute(new InsertCommand(kontakty, k1));
            cmdMng.Execute(new InsertCommand(kontakty, k2));

            var iterator = kontakty.CreateIterator();

            Assert.True(iterator.HasMore());
            Assert.Equal(k1, iterator.Next());
            Assert.True(iterator.HasMore());
            Assert.Equal(k2, iterator.Next());
            Assert.False(iterator.HasMore());
        }
    }
}