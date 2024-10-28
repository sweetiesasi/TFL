using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;
using TechTalk.SpecFlow;

[Binding]
public class EditPreferencesSteps
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public EditPreferencesSteps(TestContext context)
    {
        _driver = context.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); // 15-second timeout
    }

    [When(@"I click the ""(.*)"" link")]
    public void WhenIClickTheLink(string buttonText)
    {
        try
        {
            // Locate the button using XPath
            var button = _wait.Until(ExpectedConditions.ElementExists(By.XPath($"//button[contains(text(), '{buttonText}')]")));

            // Scroll the button into view using JavaScript
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);

            // Wait for the button to be clickable
            button = _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath($"//button[contains(text(), '{buttonText}')]")));

            // Click the button
            button.Click();
            Console.WriteLine($"Clicked the '{buttonText}' button.");
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"The '{buttonText}' button was not clickable within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception($"Failed to locate the '{buttonText}' button.");
        }
        catch (ElementClickInterceptedException)
        {
            throw new Exception($"Click on the '{buttonText}' button was intercepted by another element.");
        }
    }

  [When(@"I select routes with least walking")]
public void WhenISelectRoutesWithLeastWalking()
{
    try
    {
        // Wait for the label element associated with 'Routes with least walking' to be visible
        var leastWalkingLabel = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("label[for='JourneyPreference_2']")));
        Console.WriteLine("Label for 'Routes with least walking' is visible.");

        // Scroll the label into view using JavaScript
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", leastWalkingLabel);

        // Locate the radio button input element
        var leastWalkingInput = _wait.Until(ExpectedConditions.ElementExists(By.Id("JourneyPreference_2")));

        // Ensure the radio button is enabled
        if (leastWalkingInput.Enabled)
        {
            try
            {
                // Click the radio button input
                leastWalkingInput.Click();
                Console.WriteLine("Selected the 'Routes with least walking' radio button.");
            }
            catch (ElementClickInterceptedException)
            {
                // If the radio button click is intercepted, attempt to click the label instead
                leastWalkingLabel.Click();
                Console.WriteLine("Clicked the label for 'Routes with least walking' as a fallback.");
            }
        }
        else
        {
            throw new Exception("The 'Routes with least walking' radio button is not enabled.");
        }
    }
    catch (WebDriverTimeoutException)
    {
        throw new Exception("The 'Routes with least walking' radio button or its label was not clickable within the timeout period.");
    }
    catch (NoSuchElementException)
    {
        throw new Exception("Failed to locate the radio button or its label for 'Routes with least walking'.");
    }
}


[When(@"I update Journey")]
public void WhenIUpdateJourney()
{
    try
    {
        // Locate the container with update buttons
        var updateButtonsContainer = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.update-buttons-container")));

        // Find the 'Update journey' button within the container
        var updateJourneyButton = updateButtonsContainer.FindElement(By.CssSelector("input.primary-button.plan-journey-button[value='Update journey']"));

        // Scroll to the button to ensure visibility
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", updateJourneyButton);

        // Attempt to click the button directly
        try
        {
            updateJourneyButton.Click();
            Console.WriteLine("Clicked the 'Update journey' button.");
            return; // Exit the method if the click is successful
        }
        catch (ElementClickInterceptedException)
        {
            Console.WriteLine("Standard click intercepted, attempting JavaScript click...");

            // Use JavaScript to force-click the button as a fallback
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", updateJourneyButton);
            Console.WriteLine("Clicked the 'Update journey' button using JavaScript.");
            return; // Exit the method if the JavaScript click is successful
        }
    }
    catch (Exception)
    {
        Console.WriteLine("Direct click on 'Update journey' button failed. Attempting tab navigation...");
    }

    // Fallback: Use Tab navigation to reach the 'Update journey' button
    try
    {
        Actions actions = new Actions(_driver);
        bool buttonFound = false;
        int maxTabPresses = 50; // Set a limit to prevent infinite loops
        int tabPresses = 0;

        // Loop to press the Tab key until the 'Update journey' button is focused
        while (!buttonFound && tabPresses < maxTabPresses)
        {
            // Simulate pressing the Tab key
            actions.SendKeys(Keys.Tab).Perform();
            Thread.Sleep(100); // Adjust delay as needed for smoother tabbing

            // Check if the focused element is the 'Update journey' button
            IWebElement activeElement = _driver.SwitchTo().ActiveElement();
            string activeElementValue = activeElement.GetAttribute("value");

            if (activeElementValue == "Update journey")
            {
                Console.WriteLine("Found the 'Update journey' button using Tab navigation.");
                buttonFound = true;

                // Attempt to click the button
                try
                {
                    activeElement.Click();
                    Console.WriteLine("Clicked the 'Update journey' button using Tab navigation.");
                }
                catch (ElementClickInterceptedException)
                {
                    // Use JavaScript to force-click the button as a fallback
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", activeElement);
                    Console.WriteLine("Clicked the 'Update journey' button using JavaScript after tabbing.");
                }
                return; // Exit the method after successfully clicking
            }

            tabPresses++;
        }

        // Check if the button was found within the limit
        if (!buttonFound)
        {
            throw new Exception("Failed to find the 'Update journey' button using Tab navigation.");
        }
    }
    catch (Exception ex)
    {
        throw new Exception("Unexpected error occurred while attempting to click the 'Update journey' button using tab navigation.", ex);
    }
}


}