using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using System;
using System.Threading;

[Binding]
public class ViewDetailsSteps
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public ViewDetailsSteps(TestContext context)
    {
        _driver = context.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); // Adjust timeout as needed
    }

    [When(@"I click on ""(.*)"" button")]
    public void WhenIClickOnButton(string buttonText)
    {
        try
        {
            // Attempt to locate the "View details" button directly
            IWebElement viewDetailsButton = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//button[contains(@class, 'view-hide-details') and contains(text(), '{buttonText}')]")));

            // Scroll to the button to ensure visibility
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", viewDetailsButton);

            // Attempt to click the button
            viewDetailsButton.Click();
            Console.WriteLine($"Clicked the '{buttonText}' button.");
        }
        catch (NoSuchElementException)
        {
            // Use Tab navigation if direct locating fails
            Actions actions = new Actions(_driver);
            Console.WriteLine($"The '{buttonText}' button was not found directly. Attempting to use Tab navigation...");
            bool buttonFound = false;
            int maxTabPresses = 50; // Set a limit to prevent infinite loops

            for (int i = 0; i < maxTabPresses && !buttonFound; i++)
            {
                actions.SendKeys(Keys.Tab).Perform();
                Thread.Sleep(100); // Adjust delay for smoother tabbing

                // Check if the focused element is the "View details" button
                IWebElement activeElement = _driver.SwitchTo().ActiveElement();
                string activeElementText = activeElement.Text;

                if (activeElementText == buttonText)
                {
                    Console.WriteLine($"Found the '{buttonText}' button using Tab navigation.");
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", activeElement);
                    activeElement.Click();
                    buttonFound = true;
                    Console.WriteLine($"Clicked the '{buttonText}' button.");
                }
            }

            if (!buttonFound)
            {
                throw new Exception($"Failed to find the '{buttonText}' button using Tab navigation.");
            }
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"The '{buttonText}' button was not clickable within the timeout period.");
        }
        catch (ElementClickInterceptedException)
        {
            // Retry clicking using JavaScript as a fallback
            Console.WriteLine("Standard click intercepted, attempting JavaScript click...");
            IWebElement viewDetailsButton = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//button[contains(@class, 'view-hide-details') and contains(text(), '{buttonText}')]")));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", viewDetailsButton);
            Console.WriteLine($"Clicked the '{buttonText}' button using JavaScript.");
        }
    }

    [Then(@"I should see access information for ""(.*)"" including '(.*)'")]
    public void ThenIShouldSeeAccessInformationFor(string stationName, string expectedAccess)
    {
        try
        {
            // Step 1: Locate the specific location name, "Covent Garden Underground Station"
            var locationElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//span[contains(@class, 'location-name') and text()='{stationName}']")));

            if (locationElement.Displayed)
            {
                Console.WriteLine($"'{stationName}' location is displayed.");
            }
            else
            {
                throw new Exception($"Location '{stationName}' is not displayed.");
            }

            // Step 2: Validate the presence of specified access features
            string[] accessFeatures = expectedAccess.Split(',');

            foreach (string feature in accessFeatures)
            {
                string trimmedFeature = feature.Trim();
                string cssSelector = trimmedFeature.ToLower() switch
                {
                    "up stairs" => "a.up-stairs",
                    "up lift" => "a.up-lift",
                    "level walkway" => "a.level-walkway",
                    _ => throw new Exception($"Invalid access feature: {trimmedFeature}")
                };

                // Wait for the access element to be visible
                var accessElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(cssSelector)));

                if (accessElement.Displayed)
                {
                    Console.WriteLine($"'{trimmedFeature}' is displayed for '{stationName}'.");
                }
                else
                {
                    throw new Exception($"'{trimmedFeature}' is not displayed for '{stationName}'.");
                }
            }
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"Timed out while verifying access information for '{stationName}'.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception($"Failed to locate access information elements for '{stationName}'.");
        }
    }
}
