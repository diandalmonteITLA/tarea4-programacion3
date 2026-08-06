using OpenQA.Selenium;

namespace Testing.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Elementos Web
        private IWebElement EmailInput => _driver.FindElement(By.Name("LoginInput.UserName")); // Ajustar atributo Name/Id según tu Login.cshtml
        private IWebElement PasswordInput => _driver.FindElement(By.Name("LoginInput.Password"));
        private IWebElement LoginButton => _driver.FindElement(By.CssSelector("button[type='submit']"));
        private IWebElement ErrorMessage => _driver.FindElement(By.ClassName("text-danger"));

        // Acciones
        public void NavigateTo(string url)
        {
            _driver.Navigate().GoToUrl($"{url}/Login");
        }

        public void EnterEmail(string email)
        {
            EmailInput.Clear();
            EmailInput.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public void Login(string url, string email, string password)
        {
            NavigateTo(url);
            EnterEmail(email);
            EnterPassword(password);
            ClickLogin();
        }

        public string GetErrorMessage()
        {
            try
            {
                var errorElements = _driver.FindElements(
                    By.CssSelector(".text-danger, .field-validation-error, .validation-summary-errors li, .alert-danger"));

                foreach (var element in errorElements)
                {
                    if (!string.IsNullOrWhiteSpace(element.Text))
                        return element.Text;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}