@users
Feature: Users
As an API consumer
I want to query and create users
So that I can manage user records

Scenario: 1. Verify how many users have red hair
    When I filter all users by "hair.color" with value "Red"
    Then the response status code should be 200
    And the response should contain 25 users
    And the total reported by the response should be 25
    And every returned user should have hair colour "Red"

Scenario: 2. Add a new user with personal details, hair and full address
    When I add a new user with the following details
        | Field      | Value             |
        | FirstName  | Jane              |
        | LastName   | Doe               |
        | Age        | 34                |
        | Gender     | female            |
        | HairColour | Auburn            |
        | HairType   | Wavy              |
        | Address    | 221B Baker Street |
        | City       | London            |
        | State      | Greater London    |
        | StateCode  | GLD               |
        | PostalCode | NW1 6XE           |
        | Latitude   | 51.523767         |
        | Longitude  | -0.158555         |
        | Country    | United Kingdom    |
    Then the response status code should be 201
    And the response should contain a newly generated user id
    And the created user in the response should match the request