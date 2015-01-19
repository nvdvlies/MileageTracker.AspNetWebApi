MileageTracker.AspNetWebApi
===========================

# MileageTracker API

## Account
| Endpoint | Description   |
| ------------- | ------------- |
| [POST api/account](#register-account) | Register a new account |
| [POST api/account/token](#get-authorization-token) | Retrieve an authorization token |

## Trips
| Endpoint | Description   |
| ------------- | ------------- |
| [GET api/trips?pageNumber={pageNumber}&pageSize={pageSize}](#get-trips) | Retrieve a paginated list of trips created by the user. |
| [GET api/trips/template](#get-trip-template) | Retrieve a template of a trip containing the current date, last destination address as the origin address and last used car. |
| [GET api/trips/{id}](#get-trip-by-id) | Retrieve a trip with a given ID |
| [POST api/trips](#create-trip) | Create a new trip |
| [PUT api/trips/{id}](#update-trip) | Update an existing trip |
| [DELETE api/trips/{id}](#delete-trip) | Delete a trip |

## Addresses
| Endpoint | Description   |
| ------------- | ------------- |
| [GET api/addresses?pageNumber={pageNumber}&pageSize={pageSize}](#get-addresses)	| Retrieve a paginated list of addresses created by the user. |
| [GET api/addresses/{id}](#get-address-by-id) | Retrieve an address with a given ID. |
| [POST api/addresses](#create-address) | Create a new address |
| [PUT api/addresses/{id}](#update-address) | Update an existing address |
| [DELETE api/addresses/{id}](#delete-address) | Delete an address |

## Cars
| Endpoint | Description   |
| ------------- | ------------- |
| [GET api/cars?pageNumber={pageNumber}&pageSize={pageSize}](#get-cars) | Retrieve a paginated list of cars created by the user. |
| [GET api/cars/{id}](#get-car-by-id) | Retrieve an car with a given ID. |
| [POST api/cars](#create-car) | Create a new car |
| [PUT api/cars/{id}](#update-car) | Update an existing car |
| [DELETE api/cars/{id}](#delete-car) | Delete a car |

### Register account
Register a new account

*POST api/account*

#### Headers
| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |

#### Sample request

```json
{
  "userName": "sample string 1",
  "password": "sample string 2",
  "confirmPassword": "sample string 3"
}
```

### Get authorization token

Retrieve an authorization token

*POST api/account/token*

#### Headers
| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/x-www-form-urlencoded |

#### Sample request

```
grant_type=password&username={username}&password={password}
```

#### Sample response

```json
{
  "access_token": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "token_type": "bearer",
  "expires_in": 86399
}
```

### Get trips

Retrieve a paginated list of trips created by the user.

*GET api/trips?pageNumber={pageNumber}&pageSize={pageSize}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| pageNumber | Current page number | integer | Default value is 1 |
| pageSize | Maximum number of items to return per page | integer | Default value is 15 |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
[
  {
    "id": 1,
    "date": "2015-01-19T12:06:59.7202676Z",
    "addressOrigin": {
      "id": 1,
      "name": "sample string 2",
      "addressLine": "sample string 3",
      "postalCode": "sample string 4",
      "city": "sample string 5",
      "remarks": "sample string 6"
    },
    "addressDestination": {
      "id": 1,
      "name": "sample string 2",
      "addressLine": "sample string 3",
      "postalCode": "sample string 4",
      "city": "sample string 5",
      "remarks": "sample string 6"
    },
    "car": {
      "id": 1,
      "numberPlate": "sample string 2",
      "make": "sample string 3",
      "model": "sample string 4",
      "remarks": "sample string 5"
    },
    "remarks": "sample string 3",
    "distanceInKm": 4
  },
  {
    "id": 1,
    "date": "2015-01-19T12:06:59.7202676Z",
    "addressOrigin": {
      "id": 1,
      "name": "sample string 2",
      "addressLine": "sample string 3",
      "postalCode": "sample string 4",
      "city": "sample string 5",
      "remarks": "sample string 6"
    },
    "addressDestination": {
      "id": 1,
      "name": "sample string 2",
      "addressLine": "sample string 3",
      "postalCode": "sample string 4",
      "city": "sample string 5",
      "remarks": "sample string 6"
    },
    "car": {
      "id": 1,
      "numberPlate": "sample string 2",
      "make": "sample string 3",
      "model": "sample string 4",
      "remarks": "sample string 5"
    },
    "remarks": "sample string 3",
    "distanceInKm": 4
  }
]
```

### Get trip template

Retrieve a template of a trip containing the current date, last destination address as the origin address and last used car.

*GET api/trips/template*

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
{
  "date": "2015-01-19T12:08:51.5038334Z",
  "addressOrigin": {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "addressDestination": null,
  "car": {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  },
  "remarks": null
}
```

### Get trip by ID

Retrieve a trip with a given ID.

*GET api/trips/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of trip | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
{
  "id": 1,
  "date": "2015-01-19T12:09:59.969874Z",
  "addressOrigin": {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "addressDestination": {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "car": {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  },
  "remarks": "sample string 3",
  "distanceInKm": 4
}
```

### Create trip

Create a new trip.

*POST api/trips*

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "date": "2015-01-19T12:11:24.0605093Z",
  "addressOrigin": {
    "id": 1
  },
  "addressDestination": {
    "id": 2
  },
  "car": {
    "id": 1
  },
  "remarks": "sample string 3"
}
```

#### Sample response

```json
{
  "id": 1,
  "date": "2015-01-19T12:11:24.0605093Z",
  "addressOrigin": {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "addressDestination": {
    "id": 2,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "car": {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  },
  "remarks": "sample string 3",
  "distanceInKm": 4
}
```

### Update trip

Update an existing trip.

*PUT api/trips/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of address | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "id": 1,
  "date": "2015-01-19T12:29:51.3317288Z",
  "addressOrigin": {
    "id": 1
  },
  "addressDestination": {
    "id": 2
  },
  "car": {
    "id": 1
  },
  "remarks": "sample string 3"
}
```

#### Sample response

```json
{
  "id": 1,
  "date": "2015-01-19T12:29:51.3317288Z",
  "addressOrigin": {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "addressDestination": {
    "id": 2,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  "car": {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  },
  "remarks": "sample string 3",
  "distanceInKm": 4
}
```

### Delete trip

Delete a trip.

*DELETE api/trips/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of address | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Authorization | Bearer {token} |


### Get addresses

Retrieve a paginated list of addresses created by the user.

*GET api/addresses?pageNumber={pageNumber}&pageSize={pageSize}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| pageNumber | Current page number | integer | Default value is 1 |
| pageSize | Maximum number of items to return per page | integer | Default value is 15 |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
[
  {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  },
  {
    "id": 1,
    "name": "sample string 2",
    "addressLine": "sample string 3",
    "postalCode": "sample string 4",
    "city": "sample string 5",
    "remarks": "sample string 6"
  }
]
```

### Get address by ID

Retrieve an address with a given ID.

*GET api/addresses/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of address | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
{
  "id": 1,
  "name": "sample string 2",
  "addressLine": "sample string 3",
  "postalCode": "sample string 4",
  "city": "sample string 5",
  "remarks": "sample string 6"
}
```

### Create address

Create a new address.

*POST api/addresses*

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "name": "sample string 2",
  "addressLine": "sample string 3",
  "postalCode": "sample string 4",
  "city": "sample string 5",
  "remarks": "sample string 6"
}
```

#### Sample response

```json
{
  "id": 1,
  "name": "sample string 2",
  "addressLine": "sample string 3",
  "postalCode": "sample string 4",
  "city": "sample string 5",
  "remarks": "sample string 6"
}
```

### Update address

Update an existing address

*PUT api/addresses/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of address | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "id": 1,
  "name": "sample string 2",
  "addressLine": "sample string 3",
  "postalCode": "sample string 4",
  "city": "sample string 5",
  "remarks": "sample string 6"
}
```

#### Sample response

```json
{
  "id": 1,
  "name": "sample string 2",
  "addressLine": "sample string 3",
  "postalCode": "sample string 4",
  "city": "sample string 5",
  "remarks": "sample string 6"
}
```
 
### Delete address

Delete an address

*DELETE api/addresses/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of address | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Authorization | Bearer {token} |

### Get cars

Retrieve a paginated list of cars created by the user.

*GET api/cars?pageNumber={pageNumber}&pageSize={pageSize}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| pageNumber | Current page number | integer | Default value is 1 |
| pageSize | Maximum number of items to return per page | integer | Default value is 15 |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
[
  {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  },
  {
    "id": 1,
    "numberPlate": "sample string 2",
    "make": "sample string 3",
    "model": "sample string 4",
    "remarks": "sample string 5"
  }
]
```

### Get car by ID

Retrieve a car with a given ID.

*GET api/cars/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of car | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Authorization | Bearer {token} |

#### Sample response

```json
{
  "id": 1,
  "numberPlate": "sample string 2",
  "make": "sample string 3",
  "model": "sample string 4",
  "remarks": "sample string 5"
}
```

### Create car

Create a new car.

*POST api/cars*

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "numberPlate": "sample string 2",
  "make": "sample string 3",
  "model": "sample string 4",
  "remarks": "sample string 5"
}
```

#### Sample response

```json
{
  "id": 1,
  "numberPlate": "sample string 2",
  "make": "sample string 3",
  "model": "sample string 4",
  "remarks": "sample string 5"
}
```

### Update car

Update an existing car

*PUT api/cars/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of car | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Accept | application/json |
| Content-Type | application/json |
| Authorization | Bearer {token} |

#### Sample request

```json
{
  "id": 1,
  "numberPlate": "sample string 2",
  "make": "sample string 3",
  "model": "sample string 4",
  "remarks": "sample string 5"
}
```

#### Sample response

```json
{
  "id": 1,
  "numberPlate": "sample string 2",
  "make": "sample string 3",
  "model": "sample string 4",
  "remarks": "sample string 5"
}
```
 
### Delete car

Delete a car

*DELETE api/cars/{id}*

#### URI parameters
| Name | Description | Type | Additional information |
| ------------- | ------------- | ------------- | ------------- |
| id | ID of car | integer | Required |

#### Headers

| Header | Value |
| ------------- | ------------- |
| Authorization | Bearer {token} |
