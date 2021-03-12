# Assessment documentation

## Build and run using docker

Clone the repository. From the solution root folder execute:

`docker-compose -f docker-compose.yml up webapp`

Application can then be tested on the http://localhost:5000

## Statically added users

There are two users hardcoded in the application just for demonstration perposes:

* Admin: 
Email: admin@assessment.no
Password: Admin1234!

* Customer:
Email: customer@assessment.no
Password: Customer1234!

## Endpoints

### User

* Register: `POST /api/user/register`
**Body example**
```json
{
    "Email": username@domain.no,
    "Passsword": "some password"
}
```
* Login: `POST /api/user/login`
**Body example**
```json
{
    "Email": username@domain.no,
    "Passsword": "some password"
}
```

### Product

* Get all: `GET /api/product/getall`

* Get filtered: `GET /api/product/filter`
**URL Parameter** : `filter=[string]`

* Get paginated: `GET /api/product/pagination`
**URL Parameters** : `page=[integer] and perpage=[integer]`

* Get single product: `GET /api/product/get`
**URL Parameter** : `id=[integer]`

* Add new: `POST /api/product`
**Body example**
```json
{
    "Name": "Headphone",
    "Description": "Something",
    "Price": 299.99
}
```
This requrest needs admin autorization. to test, login with admin account.

* Remove: `DELETE /api/product`
**URL Parameter** : `id=[integer]`
This requrest needs admin autorization. to test, login with admin account.

* Update: `PUT /api/product`
**Body example**
```json
{
    "Id": 1,
    "Name": "Headphone",
    "Description": "Something",
    "Price": 299.99
}
```
This requrest needs admin autorization. to test, login with admin account.

### News

* Get single News: `GET /api/news/get`
**URL Parameter** : `id=[integer]`

* Get all: `GET /api/news/getall`

* Add new: `POST /api/news`
**Body example**
```json
{
    "Title": "News title",
    "Body": "News body"
}
```
This requrest needs admin autorization. to test, login with admin account.

### Shopping card

* Add item to shopping card: `POST /api/shoppingcard/add`
**Body example**
```json
{
    "ProductId": 1,
    "Count": 2
}
```

* Get shoping card: `GET /api/shopingcard/get`
**URL Parameter** : `id=[integer]`

* Get user´s active shopping card: `GET /api/shopingcard/getuseractivecard`

* Checkout: `POST /api/shopingcard/checkout`

* History: `GET /api/shopingcard/history`
Returns list of all current user´s shoping cards

* History: `GET /api/shopingcard/history`
**URL Parameter** : `userid=[string]`
This requrest needs admin autorization. to test, login with admin account.
User id is the same as email address.