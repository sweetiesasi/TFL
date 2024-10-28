using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace TfLJourneyPlanner.Tests.Pages
{
    public class JourneyResults
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public JourneyResults(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30)); // Increased timeout to 30 seconds
        }

        // Method to check if the main results container is visible
        public bool AreResultsContainerDisplayed()
        {
            try
            {
                var resultsContainer = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".results-container")));
                return resultsContainer.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Results container was not displayed within the timeout period.");
                return false;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Failed to locate the results container element.");
                return false;
            }
        }

        // Wait method for the results container
        public bool WaitForResultsContainer()
{
    try
    {
        // Wait for the 'extra-journey-options' container to become visible
        var resultsContainer = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".extra-journey-options.multi-modals.clearfix")));

        // Return true if the container is displayed
        return resultsContainer.Displayed;
    }
    catch (WebDriverTimeoutException)
    {
        Console.WriteLine("Results container was not displayed within the timeout period.");
        return false;
    }
    catch (NoSuchElementException)
    {
        Console.WriteLine("Failed to locate the results container.");
        return false;
    }
}


        // Method to check if a specific type of journey result is displayed
   public bool AreJourneyResultsDisplayed(string journeyType)
{
    try
    {
        var journeyTypeHeader = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath($"//h2[contains(text(), '{journeyType}')]")));
        string actualHeaderText = journeyTypeHeader.Text.Trim();

        if (actualHeaderText.Equals(journeyType, StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"'{journeyType}' results header is correctly displayed.");
            return true;
        }
        else
        {
            Console.WriteLine($"Mismatch in journey type header. Expected: '{journeyType}', but got: '{actualHeaderText}'.");
            return false;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error checking journey type: {ex.Message}");
        return false;
    }
}


        // Method to get the journey time from the results page
        public string? GetJourneyTime()
        {
            try
            {
                var journeyTimeElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".journey-time")));
                return journeyTimeElement.Text.Trim();
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Journey time was not displayed within the timeout period.");
                return null;
            }
            catch (NoSuchElementException)
            {
                Console.WriteLine("Failed to locate the journey time element.");
                return null;
            }
        }
    }
}
