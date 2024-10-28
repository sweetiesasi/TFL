Feature: TFL Functionality

  Scenario: Plan a valid journey from Leicester Square Underground Station to Covent Garden Underground Station
    Given I am on the TfL Journey Planner page
    When I type "Leicester Square" in the "InputFrom" field 
    And I select "Leicester Square Underground Station" from the suggestions
    Then It should show "Leicester Square Underground Station" in the "InputFrom" field
    When I type "Covent Garden U" in the "InputTo" field 
    And I select "Covent Garden Underground Station" from the suggestions    
    Then It should show "Covent Garden Underground Station" in the "InputTo" field
    When I click the "plan-journey-button" button
    Then I should see the system fetching results displayed
    Then I should see the results for "Walking and cycling" displayed
    Then I should see "Cycling" time "2" mins
    Then I should see "Walking" time "9" mins
    When I click the "Edit preferences" link
    And I select routes with least walking 
    And I update Journey
    Then I should see the system fetching results displayed
    Then Validate the journey time "11" mins
    When I click on "View details" button
    Then I should see access information for "Covent Garden Underground Station" including 'Up stairs, Up lift, Level walkway'

  Scenario: Plan an invalid journey
    Given I am on the TfL Journey Planner page
    When I type "@#$@$@@$" in the "InputFrom" field
    And I type "@#$#@$#@$$" in the "InputTo" field
    When I click the "plan-journey-button" button
    Then I should see the system fetching results displayed
    Then I should see an error indicating that the journey cannot be planned

  Scenario: Plan a journey with no locations
    Given I am on the TfL Journey Planner page
    When I click the "plan-journey-button" button
    Then Validate the "InputFrom" is invalid with error
    Then Validate the "InputTo" is invalid with error

  Scenario: Plan a journey using different modes 
  Scenario: Plan a journey using different preferences
  Scenario: Plan a journey using different Leaving date and time
  Scenario: Plan a journey and verify the access options
  Scenario: Plan a journey between two stations that are extremely close to each other
  Scenario: Plan a journey using hire bike
  Scenario: Validate the street View
  Scenario: Test the planner on different network conditions (e.g., 3G, 4G, 5G, Wi-Fi)
  Scenario: Test the planner on different browsers and devices
  Scenario: Simulate a high number of concurrent users accessing the journey planner
  Scenario: Simulate a server failure during journey planning












