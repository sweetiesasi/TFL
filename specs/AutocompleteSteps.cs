using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using System;
using System.Threading;

[Binding]
public class AutocompleteSteps
{
    private readonly IWebDriver _driver;
    private readonly WebDriverWait _wait;
    private const int MaxRetryCount = 3; // Maximum retries

    public AutocompleteSteps(TestContext context)
    {
        _driver = context.Driver;
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15)); // Set 15-second timeout
    }

    [When(@"I select ""(.*)"" from the suggestions")]
    public void WhenISelectFromTheSuggestions(string selection)
    {
        int retryCount = 0;

        while (retryCount < MaxRetryCount)
        {
            try
            {
                // Wait for the suggestions to appear
                _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".stop-name")));

                // Locate all suggestions
                var suggestions = _driver.FindElements(By.CssSelector(".stop-name"));

                // Loop through suggestions and select the matching one
                foreach (var suggestion in suggestions)
                {
                    if (suggestion.Text.Contains(selection, StringComparison.OrdinalIgnoreCase))
                    {
                        // Scroll to the suggestion to ensure visibility
                        ScrollIntoView(suggestion);

                        // Click the suggestion
                        suggestion.Click();
                        Console.WriteLine($"'{selection}' successfully selected from suggestions.");
                        return; // Exit once successful selection is made
                    }
                }

                // If no matching suggestion is found, throw an exception
                throw new Exception($"'{selection}' was not found in the autocomplete suggestions.");
            }
            catch (ElementClickInterceptedException)
            {
                // Handle intercepted clicks by retrying
                retryCount++;
                Console.WriteLine($"Click on '{selection}' suggestion was intercepted. Retrying... (Attempt {retryCount}/{MaxRetryCount})");
                Thread.Sleep(500); // Adjust delay as needed

                if (retryCount >= MaxRetryCount)
                {
                    throw new Exception($"Failed to select '{selection}' after {MaxRetryCount} attempts.");
                }
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception($"Suggestions did not appear within the timeout period.");
            }
            catch (NoSuchElementException)
            {
                throw new Exception($"Failed to locate suggestions for the autocomplete field.");
            }
        }
    }

    // Utility method to scroll the element into view
    private void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
    }
}
