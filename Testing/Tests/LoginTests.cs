using NUnit.Framework;
using Testing.Pages;

namespace Testing.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        [Test]
        public void HU01_TC01_Login_CaminoFeliz()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Login(BaseUrl, "testdummy@email.com", "T3stDummy+p4$$w0rd!");

            Assert.That(Driver.Url, Does.Contain("/Index").Or.EqualTo($"{BaseUrl}/"));
        }

        [Test]
        public void HU01_TC02_Login_PruebaNegativa_ClaveInvalida()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.Login(BaseUrl, "testdummy@email.com", "ClaveIncorrecta");

            string error = loginPage.GetErrorMessage();
            Assert.That(error, Is.Not.Empty);
        }

        [Test]
        public void HU01_TC03_Login_PruebaLimites_CamposVacios()
        {
            var loginPage = new LoginPage(Driver);
            loginPage.NavigateTo(BaseUrl);
            loginPage.ClickLogin();

            string error = loginPage.GetErrorMessage();
            Assert.That(error, Is.Not.Empty);
        }
    }
}
