using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

public class TestContext
{
    public IWebDriver Driver { get; set; }

    public TestContext()
    {
        // Initialize the WebDriver once
        Driver = new ChromeDriver();
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    // Cleanup method to close the driver after all scenarios are executed
    public void Cleanup()
    {
        if (Driver != null)
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
