# Gift delivery management system
#### Authors: _Maksym Tautkevychuis, Maryna Kamienieva, Dauirzhan Akmurat_
![Blank diagram-3](https://github.com/user-attachments/assets/c9572693-a870-431a-9bff-3aa6204b8f33)


## Description:
A company that decided to open a new field - gift delivery that is specific on good and cheap products that can be delivered -  needs a system. The system allows users to browse various gift types(items), place orders, manage payments, and track the delivery of gifts to recipients and if it’s needed - refund them. 

The system supports different types of users, such as customer, employee, foundation account, business account. Users contain their id and phone number. Business accounts have information about their documentation(for business, corporation). Fondation accounts contain documentation that is needed for working with charity organisations.  Each employee contains their working schedule. Besides that, the employee can advise himself. 
Each client has their name and wishlist for adding their dreams for a birthday. Beside that, each client can have one type of subscription and one type of subscription can have many clients. The subscription contains the list of features and a price. There are two subscriptions for client- premium or standard. Premium account has some privileges such as a 0.5% discount for all products 0.5%, free delivery of orders, free priority of delivery. The Standard account doesn't have that privileges: no discount for each product, just for some holidays/events, no priority of delivery the order and playful delivery. Also in the system a client contains his own Wallet. Customer can contain just one wallet in his account and the wallet can have a unique customer. In the wallet is an amount of money and the currency. 
Assumption, user will be deleted if new account created with the same phone number.
Based on that information users can place multiple orders. Each order contains location, status(arrived, shipping, added), description.
The system contains a Treker with the location, so that order can be trekked uniquely. Also order should contain at least one item. Each item has an id, name, price that is holded and the date of production. Moreover, each item contains an exporter for the product with their quantity. Exporter contain their name and documentation. Each item contains the exporter with their quantity.
The System sends Notifications to users regarding order status, payment confirmations, and updates. Notifications can be delivered via email, SMS. Each Notification includes text.  
After receiving a gift, users can leave a Review(of the catalogue list)  for both the gift and the overall experience with the products as many times as he want. 
There is also the last option - Refund. The user can do the process of returning funds to an employee after they request a refund for an order or specific item. Refund contains: id, date, status(isApproved), processed by which employee.

## The system allows to do:
1. For foundation accounts will be a possibility to find free propositions
2. Add wishes/gifts to a wishlist.
3. Employee have an option to add a refund
4. Add order to bucket
5. Track Delivery for the order
6. User can view notification regarding to the order status
7. The notification can be sended to a user regarding to the status of an order
8. User can add wishlist to your profile
9. User can do a refund of order
10. In wallet you can add money to the card and change the currency
11. System can list categories of an items
