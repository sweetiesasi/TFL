# TfL Journey Planner Test Automation

This repository contains automated tests for the **TfL Journey Planner** to ensure the functionality of journey planning, error handling, preferences updates, and more. The tests cover a range of scenarios, including valid journeys, invalid journeys, and edge cases like no input or unsupported locations.

## Table of Contents

1. [Overview](#overview)
2. [Test Scenarios](#test-scenarios)
3. [Project Structure](#project-structure)
4. [Prerequisites](#prerequisites)
5. [Installation](#installation)
6. [Configuration](#configuration)
7. [Running Tests](#running-tests)
8. [Test Reports](#test-reports)

## Overview

The **TfL Journey Planner Test Automation Project** is designed to validate the core functionalities of the **TfL Journey Planner** on the Transport for London website. The tests are written using **SpecFlow**, **Selenium WebDriver**, and **C#**. This project aims to ensure that the journey planner works as expected, handling both positive and negative test cases.

## Test Scenarios

The following test scenarios are covered:

### Functional Scenarios
1. **Valid Journey Planning**
   - Planning a journey from **Leicester Square** to **Covent Garden**.
   - Verifying travel preferences, journey options, and access information.
2. **Invalid Journey Handling**
   - Handling unsupported or non-existent locations.
   - Displaying appropriate error messages when fields are left blank.
3. **Preferences Adjustments**
   - Selecting different preferences, such as routes with least walking, step-free access, etc.
4. **Edge Case Handling**
   - Planning journeys with very short distances.
   - Planning journeys with difference distances.
   - Handling scenarios where inputs are invalid or not supported.

### Non-Functional Scenarios
1. **Performance Testing**
   - Simulating high user load conditions.
2. **Compatibility Testing**
   - Ensuring compatibility across different browsers and devices.
3. **Error Handling**
   - Verifying error messages and handling UI errors gracefully.
4. **Accessibility Testing**
   - Ensuring correct display of accessibility options for users with special requirements.

## Project Structure

The project is structured as follows:
TestTLF1/
│
├── bin/                                  # Compiled binaries
│
├── features/                             # Feature files with Gherkin syntax
│   ├── TFLPlanAJourney.feature           # Main feature file for journey planning scenarios
│
├── obj/                                  # Object files generated during build
│
├── specs/                                # Step definitions and page object models
│   ├── AutocompleteSteps.cs              # Step definitions for autocomplete functionality
│   ├── EditPreferencesSteps.cs           # Step definitions for editing preferences
│   ├── InputJourneySteps.cs              # Step definitions for handling input fields
│   ├── JourneyResults.cs                 # Page object model for the journey results page
│   ├── JourneyResultsSteps.cs            # Step definitions for validating journey results
│   ├── NavigationSteps.cs                # Step definitions for navigation and setup
│   ├── SubmitButton.cs                   # Common utility for handling submit buttons
│   ├── TextContext.cs                    # Test context setup and initialization
│   ├── ViewDetailSteps.cs                # Step definitions for viewing journey details
│
├── .editorconfig                         # Code formatting settings
├── TestTLF1.csproj                       # .NET project file
├── TestTLF1.sln                          # Solution file for Visual Studio
└── README.md                             # Documentation file



## Prerequisites

Ensure you have the following installed before proceeding:
- **.NET Core SDK** (latest version)
- **ChromeDriver** or another WebDriver for your preferred browser
- **Visual Studio** or **Visual Studio Code**
- **SpecFlow** and **Selenium WebDriver** packages for **.NET**
- **NuGet** package manager for dependencies

## Installation

## Configuration
1. **Clone the repository:**
   ```bash
   git clone https://github.com/yourusername/tfl-journey-planner-tests.git
   cd tfl-journey-planner-tests

2. **Clone the repository:**
  ```bash
  git clone https://github.com/yourusername/TestTLF1.git
  cd TestTLF1```

3. **Open the solution:**
  ```bash    
  Open TestTLF1.sln using Visual Studio or Visual Studio Code.

4. **Install NuGet packages:**

Use Visual Studio's built-in NuGet Package Manager or run:
```bash
Copy code
dotnet restore
Set up WebDriver:```

Ensure ChromeDriver (or another WebDriver) is in your system's PATH.
Configuring WebDriver
To switch browsers, update the WebDriver setup in TestContext or NavigationSteps.cs.
Supported browsers: Chrome, Firefox, Edge.

## Running Test
1. **Open the solution in Visual Studio or Visual Studio Code.**
2. **Build the solution:**
  ```bash
  dotnet build```
3. **Run the tests using the Test Explorer in Visual Studio or from the command line:**
  ```bash
  dotnet test
4. **You can also run specific test scenarios using the SpecFlow extension in Visual Studio.**

## Test Reports
  Generate Test Reports Using dotnet test
  When running tests via the command line, you can generate reports using the --logger option:
  ```bash
  dotnet test --logger "trx;LogFileName=TestResults.trx"

This will create a TRX report (Test Run XML) in the project directory, usually saved in the TestResults folder.

