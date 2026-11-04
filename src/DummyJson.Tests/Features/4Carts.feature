@carts
Feature: Carts
As an API consumer
I want to create and delete carts
So that I can manage a user's shopping

Scenario: 1. Add 5 products to a cart and validate the total discount percentage
    When I add the following products to a new cart for user 1
        | ProductId | Quantity |
        |         1 |        1 |
        |         2 |        2 |
        |         3 |        1 |
        |         4 |        3 |
        |         5 |        1 |
    Then the response status code should be 201
    And the cart should contain 5 distinct products with a total quantity of 8
    And each cart line should match the requested product and quantity
    And the total discount percentage of the cart should be 62.11
    And the cart totals should be consistent with its lines

Scenario: 2. Delete a cart
    When I delete the cart with id 1
    Then the response status code should be 200
    And the response should contain cart id 1
    And the cart should be flagged as deleted with a deletion timestamp