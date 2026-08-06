using OpenQA.Selenium;

namespace Testing.Pages
{
    public class ListingPage
    {
        private readonly IWebDriver _driver;

        public ListingPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Nvegacion
        private IWebElement CreateNewButton => _driver.FindElement(By.XPath("//a[contains(@href, '/Create')]"));

        // Elementos del formulario
        private IWebElement TitleInput => _driver.FindElement(By.Name("NewListing.Name"));
        private IWebElement DescriptionInput => _driver.FindElement(By.Name("NewListing.Description"));
        private IWebElement PriceInput => _driver.FindElement(By.Name("NewListing.Price"));
        private IWebElement SaveButton => _driver.FindElement(By.CssSelector("button[type='submit']"));

        public void ClickCreateNew() => CreateNewButton.Click();

        public void FillForm(string name, string description, string price)
        {
            TitleInput.Clear();
            TitleInput.SendKeys(name);

            DescriptionInput.Clear();
            DescriptionInput.SendKeys(description);

            PriceInput.Clear();
            PriceInput.SendKeys(price);
        }

        public void ClickSave() => SaveButton.Click();

        public bool IsListingPresentInTable(string name)
        {
            try
            {
                var element = _driver.FindElement(By.XPath($"//td[contains(text(), '{name}')]"));
                return element != null;
            }
            catch
            {
                return false;
            }
        }

        public void ClickEditFor(string name)
        {
            var editButton = _driver.FindElement(By.XPath($"//tr[td[contains(text(), '{name}')]]//a[contains(@href, '/Edit')]"));
            editButton.Click();
        }

        public void ClickDeleteFor(string name)
        {
            var deleteButton = _driver.FindElement(By.XPath($"//tr[td[contains(text(), '{name}')]]//a[contains(@href, '/Delete')]"));
            deleteButton.Click();
        }

        public void ConfirmDelete()
        {
            var confirmBtn = _driver.FindElement(By.CssSelector("button[type='submit']"));
            confirmBtn.Click();
        }
    }
}
