using NUnit.Framework;
using Testing.Pages;

namespace Testing.Tests
{
    [TestFixture]
    public class ListingCrudTests : BaseTest
    {
        [SetUp]
        public void Authenticate()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Login(BaseUrl, "testdummy@email.com", "T3stDummy+p4$$w0rd!");
        }

        [Test]
        public void HU02_TC01_CrearPublicacion_CaminoFeliz()
        {
            var listingPage = new ListingPage(Driver);
            string uniqueTitle = "Casa de Playa " + Guid.NewGuid().ToString().Substring(0, 5);

            listingPage.ClickCreateNew();
            listingPage.FillForm(uniqueTitle, "Hermosa propiedad frente al mar", "150000");
            listingPage.ClickSave();

            Assert.That(listingPage.IsListingPresentInTable(uniqueTitle), Is.True);
        }

        [Test]
        public void HU02_TC02_CrearPublicacion_PruebaNegativa_SinTitulo()
        {
            var listingPage = new ListingPage(Driver);

            listingPage.ClickCreateNew();
            listingPage.FillForm("", "Descripción sin título", "1000");
            listingPage.ClickSave();

            // Debe de mantenerse en pantalla Create por fallo de validacion
            Assert.That(Driver.Url, Does.Contain("/Create"));
        }

        [Test]
        public void HU04_TC01_EditarPublicacion_CaminoFeliz()
        {
            var listingPage = new ListingPage(Driver);
            string originalTitle = "Casa Original " + Guid.NewGuid().ToString().Substring(0, 4);
            string editedTitle = "Casa Editada " + Guid.NewGuid().ToString().Substring(0, 4);

            // Se crea
            listingPage.ClickCreateNew();
            listingPage.FillForm(originalTitle, "Detalles", "5000");
            listingPage.ClickSave();

            // Se edita
            listingPage.ClickEditFor(originalTitle);
            listingPage.FillForm(editedTitle, "Detalles Modificados", "6000");
            listingPage.ClickSave();

            Assert.That(listingPage.IsListingPresentInTable(editedTitle), Is.True);
        }

        [Test]
        public void HU05_TC01_EliminarPublicacion_CaminoFeliz()
        {
            var listingPage = new ListingPage(Driver);
            string titleToDelete = "Item a Eliminar " + Guid.NewGuid().ToString().Substring(0, 4);

            // Se crea
            listingPage.ClickCreateNew();
            listingPage.FillForm(titleToDelete, "Para borrar", "200");
            listingPage.ClickSave();

            // Se elimina
            listingPage.ClickDeleteFor(titleToDelete);
            listingPage.ConfirmDelete();

            Assert.That(listingPage.IsListingPresentInTable(titleToDelete), Is.False);
        }
    }
}