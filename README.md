# Gift delivery management system
#### Authors: _Maksym Tautkevychuis, Maryna Kamienieva, Dauirzhan Akmurat_

![Blank diagram-3](https://github.com/user-attachments/assets/76b55227-a9ff-4a50-a1a1-e289e4844865)


## Description:
### Gift delivery management system

A company that decided to open a new field - gift delivery that is specific on good and cheap products that can be delivered -  needs a system. The system allows users to browse various gift types (items), place orders, manage payments, and track the delivery of gifts to recipients and if it’s needed - refund them. 

The system supports different types of users, such as customer, employee, foundation account, business account. Users contain their id, phone number and email. Business accounts have information about their documentation(for business, corporation), business name, list of authorised users, address, corporation discount that is 3%, verification. Fondation accounts contain documentation that is needed for working with charity organisations, foundation name and their type (product based, delivery based, location-based).  Each employee contains his specific working schedule, name, address. In the schedule there can be just 5 holidays per year and scheduled dates when exactly the employee is working. Besides that, the employee can advise himself. 
Each client has their name, birthday and wishlist for adding their dreams for a birthday. Beside that, each client can have one type of subscription and one type of subscription can have many clients. The subscription contains the list of features, price and fixed tax value 10.2. There are two subscriptions for client- premium or standard. Premium account has some privileges such as a 0.5% discount for all products 0.5%, free delivery of orders, free priority of delivery. The Standard account doesn't have that privileges: no discount for each product, just for some holidays/events, no priority of delivery, the order and playful delivery. However, that contains a list of available dates to deliver and a list of free gifts. Also in the system a client contains his own Wallet. Client can contain just one wallet in his account and the wallet can have a unique customer. In the wallet is an amount of money and the currency. 
Based on that information users can place multiple orders. Each order contains location, status(arrived, shipping, added), description.
The system contains id, a Treker with the location, so that order can be trekked uniquely and estimated timer for arrival. Also order should contain at least one item. Each item has an id, name, price that is holded and the date of production. Moreover, each item contains an exporter for the product with their quantity. Exporter contain their name and documentation, country, address, shipping cost, some amount of supplied items, time lead date and phone number. Each item contains the exporter with their quantity.
The System sends Notifications to users regarding order status, payment confirmations, and updates. Notifications can be delivered via email, SMS. Each Notification includes text.  
After receiving a gift, users can leave a Review(of the catalogue list)  comment and rating which can be from 1 to 10, for both the gift and the overall experience with the products as many times as he want. 
There is also the last option - Refund. The user can do the process of returning funds to an employee after they request a refund for an order or specific item. Refund contains: date, status(isApproved), processed by which employee.

