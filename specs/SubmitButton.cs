using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;

[Binding]
public class ClickButtonSteps
{
    private readonly IWebDriver _driver;
    private WebDriverWait _wait;
    private const int MaxRetryCount = 3; // Maximum number of retries

    public ClickButtonSteps(TestContext context)
    {
        _driver = context.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); // Set up WebDriverWait with a 15-second timeout
    }

    [When(@"I click the ""(.*)"" button")]
    public void WhenIClickTheButton(string buttonId)
    {
        int retryCount = 0; // Initialize retry count

        while (retryCount < MaxRetryCount)
        {
            try
            {
                // Wait until the button is present in the DOM
                var buttonElement = _wait.Until(ExpectedConditions.ElementExists(By.Id(buttonId)));

                // Ensure the button is clickable before clicking
                var planButton = _wait.Until(ExpectedConditions.ElementToBeClickable(buttonElement));
                planButton.Click();

                Console.WriteLine($"Clicked the '{buttonId}' button.");
                return; // Exit the method if the click is successful
            }
            catch (ElementClickInterceptedException)
            {
                retryCount++;
                Console.WriteLine($"Click on the '{buttonId}' button was intercepted. Retrying... (Attempt {retryCount}/{MaxRetryCount})");

                if (retryCount >= MaxRetryCount)
                {
                    throw new Exception($"Failed to click the '{buttonId}' button after {MaxRetryCount} attempts due to persistent interception.");
                }

                // Optionally, wait for a short period before retrying
                Thread.Sleep(500); // Adjust delay as needed
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"'{buttonId}' button was not present or clickable within the timeout period.");
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Failed to locate the '{buttonId}' button.");
            }
        }
    }


    
}
