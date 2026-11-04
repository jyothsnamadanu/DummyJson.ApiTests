@auth
Feature: Auth
As an API consumer
I want to authenticate against DummyJSON
So that I can call endpoints that require a bearer token

Scenario: 1. Authenticate a user and store the access token
    When I log in with the configured user credentials
    Then the response status code should be 200
    And the response should contain an access token and a refresh token
    And the access token should be stored for subsequent requests

Scenario: 2. Validate the id, first name and last name of the current authenticated user
    Given I am authenticated as the configured user
    When I request the current authenticated user
    Then the response status code should be 200
    And the authenticated user should have the following details
        | Id | FirstName | LastName |
        |  1 | Emily     | Johnson  |
