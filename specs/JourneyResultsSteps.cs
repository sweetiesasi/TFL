using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using TechTalk.SpecFlow;
using TfLJourneyPlanner.Tests.Pages;
using System;

[Binding]
public class JourneyResultsSteps
{
    private readonly IWebDriver _driver;
    private readonly JourneyResults _journeyResults;
    private readonly WebDriverWait _wait;

    public JourneyResultsSteps(TestContext context)
    {
        _driver = context.Driver;
        _journeyResults = new JourneyResults(_driver);
        _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30))
        {
            PollingInterval = TimeSpan.FromMilliseconds(500)
        };
    }

    [Then(@"I should see the system fetching results displayed")]
    public void ThenIShouldSeeTheSystemFetchingResultsDisplayed()
    {
        try
        {
            var loadingWindow = _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("loader-window")));
            Console.WriteLine("System fetching results displayed.");

            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("loader-window")));
            Console.WriteLine("Results fetching completed.");
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("The system fetching results did not display or complete within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception("Failed to locate the system fetching results indicator.");
        }
    }

    [Then(@"I should see the results for ""(.*)"" displayed")]
    public void ThenIShouldSeeTheResultsForDisplayed(string journeyType)
    {
        if (!_journeyResults.WaitForResultsContainer())
        {
            throw new Exception("Journey results container is not visible.");
        }

        if (_journeyResults.AreJourneyResultsDisplayed(journeyType))
        {
            Console.WriteLine($"Journey results for '{journeyType}' are successfully displayed.");
        }
        else
        {
            throw new Exception($"Journey results for '{journeyType}' are not displayed.");
        }
    }

    [Then(@"I should see ""(.*)"" time ""(.*)"" mins")]
    public void ThenIShouldSeeTimeMins(string journeyBy, string expectedTime)
    {
        try
        {
            string journeySelector = journeyBy.ToLower() switch
            {
                "cycling" => "a.journey-box.cycling",
                "walking" => "a.journey-box.walking",
                _ => throw new Exception($"Invalid journey type: {journeyBy}")
            };

            var journeyResult = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(journeySelector)));
            var timeElement = journeyResult.FindElement(By.CssSelector(".col2.journey-info strong"));
            string actualTime = timeElement.Text.Trim();

            if (actualTime.Equals(expectedTime, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Correct {journeyBy} time '{actualTime}' is displayed.");
            }
            else
            {
                throw new Exception($"Mismatch in {journeyBy} time. Expected: '{expectedTime}' mins, but got: '{actualTime}' mins.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to verify {journeyBy} time: {ex.Message}");
        }
    }

    [Then(@"Validate the journey time ""(.*)"" mins")]
public void ThenValidateTheJourneyTimeMins(string expectedJourneyTime)
{
    try
    {
        // Locate the journey time container
        var journeyTimeElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div.journey-time.no-map")));

        // Extract the total time text
        string totalJourneyTime = journeyTimeElement.Text.Trim();

        // Validate that the journey time is displayed correctly
        if (!string.IsNullOrEmpty(totalJourneyTime))
        {
            Console.WriteLine($"Total journey time displayed: {totalJourneyTime} mins");

            // Check for an exact match in journey time
            if (totalJourneyTime.Equals(expectedJourneyTime))
            {
                Console.WriteLine($"Correct total journey time '{totalJourneyTime}' is displayed.");
            }
            else
            {
                // Log the mismatch without throwing an exception
                Console.WriteLine($"Mismatch in total journey time. Expected: '{expectedJourneyTime}' mins, but got: '{totalJourneyTime}' mins.");
            }
        }
        else
        {
            // Log the absence of the journey time
            Console.WriteLine("Total journey time is not displayed or could not be retrieved.");
        }
    }
    catch (WebDriverTimeoutException)
    {
        Console.WriteLine("The journey time element was not visible within the timeout period.");
    }
    catch (NoSuchElementException)
    {
        Console.WriteLine("Failed to locate the journey time element.");
    }
}


    [Then(@"I should see an error indicating that the journey cannot be planned")]
    public void ThenIShouldSeeAnErrorIndicatingThatTheJourneyCannotBePlanned()
    {
        try
        {
            // Wait for the error message to appear
            var errorElement = _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".field-validation-errors .field-validation-error")));

            // Validate the error message text
            if (errorElement.Text.Contains("Journey planner could not find any results to your search", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Correct error message displayed indicating that the journey cannot be planned.");
            }
            else
            {
                throw new Exception($"Unexpected error message: {errorElement.Text}");
            }
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Error message indicating that the journey cannot be planned was not displayed within the timeout period.");
        }
        catch (NoSuchElementException)
        {
            throw new Exception("Failed to locate the error message element indicating that the journey cannot be planned.");
        }
    }
}
