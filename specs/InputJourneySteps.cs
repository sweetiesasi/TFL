using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using System;
using System.Threading;

[Binding]
public class InputJourneySteps
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;

    public InputJourneySteps(TestContext context)
    {
        _driver = context.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); // 15-second timeout
    }

    [When(@"I type ""(.*)"" in the ""(.*)"" field")]
    public void WhenITypeInField(string location, string fieldId)
    {
        try
        {
            // Locate the input field and ensure it is clickable
            var inputField = _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id(fieldId)));
            ScrollIntoView(inputField);
            inputField.Clear();
            inputField.Click();

            // Type slowly into the input field
            TypeSlowly(inputField, location);
            Console.WriteLine($"Entered '{location}' in the '{fieldId}' field.");
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"The '{fieldId}' field did not become clickable within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception($"Failed to locate the '{fieldId}' field.");
        }
    }

    [Then(@"It should show ""(.*)"" in the ""(.*)"" field")]
    public void ThenItShouldShowInField(string expectedValue, string fieldId)
    {
        try
        {
            // Locate the input field using the provided ID
            var inputField = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id(fieldId)));

            // Get the actual value from the input field
            string actualValue = inputField.GetAttribute("value").Trim();

            // Compare the actual value with the expected value
            if (actualValue.Equals(expectedValue, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Correct value '{actualValue}' is displayed in the '{fieldId}' field.");
            }
            else
            {
                throw new Exception($"Mismatch in '{fieldId}' field value. Expected: '{expectedValue}', but got: '{actualValue}'.");
            }
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception($"The '{fieldId}' field did not contain the expected value within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception($"Failed to locate the '{fieldId}' field.");
        }
    }
   [Then(@"Validate the ""(.*)"" is invalid with error")]
public void ThenValidateTheIsInvalidWithError(string fieldId)
{
    try
    {
        // Locate the error element for the given field
        var errorElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector($"span[data-valmsg-for='{fieldId}']")));

        // Validate that the error message is displayed
        if (errorElement.Displayed && errorElement.Text.Contains("The"))
        {
            Console.WriteLine($"Error message displayed for field '{fieldId}': {errorElement.Text}");
        }
        else
        {
            throw new Exception($"Error message for field '{fieldId}' is not displayed or incorrect.");
        }

        // Check if the warning icon is displayed (the icon might be part of the error element container)
        var errorIcon = errorElement.FindElement(By.CssSelector("span"));

        if (errorIcon.Displayed)
        {
            Console.WriteLine($"Error icon displayed alongside message for field '{fieldId}'.");
        }
        else
        {
            throw new Exception($"Error icon not displayed for field '{fieldId}'.");
        }
    }
    catch (WebDriverTimeoutException)
    {
        throw new Exception($"Error message for field '{fieldId}' was not displayed within the timeout period.");
    }
    catch (NoSuchElementException)
    {
        throw new Exception($"Failed to locate the error message element or icon for field '{fieldId}'.");
    }
}



    private void TypeSlowly(IWebElement element, string text)
    {
        foreach (char c in text)
        {
            element.SendKeys(c.ToString());
            Thread.Sleep(100); // Adjust delay as needed
        }
    }

    private void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }



}
