using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using TechTalk.SpecFlow;

[Binding]
public class NavigationSteps
{
    private readonly TestContext _context;
    private readonly WebDriverWait _wait;

    public NavigationSteps(TestContext context)
    {
        _context = context;

        // Initialize WebDriver only if it hasn't been initialized yet
        if (_context.Driver == null)
        {
            _context.Driver = new ChromeDriver(); // Initialize WebDriver once
        }

        // Set up WebDriverWait with the initialized WebDriver
        _wait = new WebDriverWait(_context.Driver, TimeSpan.FromSeconds(10));
    }

    [Given(@"I am on the TfL Journey Planner page")]
    public void GivenIAmOnTheTfLJourneyPlannerPage()
    {
        // Navigate to the TfL Journey Planner page
        _context.Driver.Navigate().GoToUrl("https://tfl.gov.uk/plan-a-journey");

        try
        {
            // Attempt to click the accept cookies button if present
            var acceptCookiesButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowAll")));
            acceptCookiesButton.Click();
            Console.WriteLine("Accepted cookies on the TfL Journey Planner page.");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("Cookie banner not displayed.");
        }

        // Ensure the page is fully loaded by checking the URL
        _wait.Until(ExpectedConditions.UrlContains("tfl.gov.uk"));
        Console.WriteLine("Navigation to the TfL Journey Planner page completed.");
    }

    [AfterScenario]
    public void CloseBrowser()
    {
        try
        {
            // Check if WebDriver is initialized and quit the driver
            if (_context.Driver != null)
            {
                _context.Driver.Quit(); // Close the browser
                _context.Driver.Dispose(); // Release the WebDriver resources
                Console.WriteLine("Closed the Chrome browser after scenario execution.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to close the browser. Error: {ex.Message}");
        }
    }
}
