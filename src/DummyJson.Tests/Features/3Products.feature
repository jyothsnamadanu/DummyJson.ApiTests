@products
Feature: Products
As an API consumer
I want to search, page through and update products
So that I can work with the product catalogue

Scenario: 1. Verify the price of iPhone 13 Pro
    When I search for products matching "iPhone 13 Pro"
    Then the response status code should be 200
    And the search results should contain exactly one product titled "iPhone 13 Pro"
    And the product "iPhone 13 Pro" should have a price of 1099.99

Scenario: 2. Retrieve the 15th product using pagination
    When I request page 15 of the products list with a page size of 1, selecting only "id,title,price"
    Then the response status code should be 200
    And the response should contain only 1 product
    And the returned product should have the following details
        | Id | Title                            | Price  |
        | 15 | Wooden Bathroom Sink With Mirror | 799.99 |
    And the returned product should only contain the fields "id,title,price"

Scenario: 3. Update the title of product 121
    When I update the title of product 121 to "iPhone 5s - Refurbished"
    Then the response status code should be 200
    And the response should contain product id 121
    And the product title in the response should be "iPhone 5s - Refurbished"