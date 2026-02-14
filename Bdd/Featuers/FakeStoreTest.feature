Feature: Add product to the cart

As an online shopper, I want to view all available products and add the cheapest electronics item to my cart.

Req
Acceptance Criteria:
•	The product must belong to a specific category.
•	Only products that are in stock (have quantity available) should be considered.
•	Once added to the cart, the product should appear with correct price and quantity.

@tag1
Scenario Outline: Add the cheapest electronics product to the cart
	Given I retrieve all products in the "<Category>" category
	When I select the product with the lowest price in that category
	And I add <Qty> quantity of that product to user id <UserId> cart
	Then the cart should display the selected product for user id <UserId>
	And the product price should be correct
	And the quantity should be <Qty>
Examples: 
	| Category       | Qty | UserId |
	| men's clothing |   2 |      1 |
	| electronics    |   1 |      1 |
	| jewelery       |   3 |      1 |