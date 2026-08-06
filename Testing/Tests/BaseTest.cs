using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Testing.Tests
{
    [SetUpFixture]
    public class GlobalReportSetup
    {
        public static ExtentReports Extent;
        public static ExtentSparkReporter SparkReporter;

        [OneTimeSetUp]
        public void StartReport()
        {
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "TestReport.html");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

            SparkReporter = new ExtentSparkReporter(reportPath);
            Extent = new ExtentReports();
            Extent.AttachReporter(SparkReporter);
        }

        [OneTimeTearDown]
        public void FlushReport()
        {
            Extent.Flush();
        }
    }

    public class BaseTest
    {
        protected IWebDriver Driver;
        protected ExtentTest Test;
        protected string BaseUrl = "https://localhost:7296";

        [SetUp]
        public void Setup()
        {
            Test = GlobalReportSetup.Extent.CreateTest(TestContext.CurrentContext.Test.Name);

            var options = new ChromeOptions();
            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            string screenshotPath = CaptureScreenshot(TestContext.CurrentContext.Test.Name);

            if (status == TestStatus.Failed)
            {
                Test.Fail("Prueba Fallida: " + TestContext.CurrentContext.Result.Message)
                    .AddScreenCaptureFromPath(screenshotPath);
            }
            else
            {
                Test.Pass("Prueba Exitosa")
                    .AddScreenCaptureFromPath(screenshotPath);
            }

            Driver?.Quit();
            Driver?.Dispose();
        }

        private string CaptureScreenshot(string name)
        {
            try
            {
                string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "Screenshots");
                Directory.CreateDirectory(screenshotsDir);

                string filePath = Path.Combine(screenshotsDir, $"{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                ITakesScreenshot ts = (ITakesScreenshot)Driver;
                Screenshot screenshot = ts.GetScreenshot();
                screenshot.SaveAsFile(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al tomar captura: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
