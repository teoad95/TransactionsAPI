# Transactions API

TransactionsAPI is a .NET 6-based RESTful API designed to manage transactions data stored in an SQL Server database. This API provides four basic actions through the Transaction controller:

1. **POST Transaction**: Add or update transactions by uploading a CSV file, which will be processed and stored in our database.

2. **GET All Transactions**: Retrieve a list of all transactions with support for pagination.

3. **GET Transaction**: Retrieve information about a specific transaction by providing its unique identifier.

4. **DELETE Transaction**: Delete a transaction by its unique identifier.

5. **UPDATE Transaction**: Update an existing transaction or create a new one if it does not exist.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
  - [POST Transaction](#post-transaction)
  - [GET All Transactions](#get-all-transactions)
  - [GET Transaction](#get-transaction)
  - [DELETE Transaction](#delete-transaction)
  - [UPDATE Transaction](#update-transaction)


## Getting Started

### Prerequisites

Before you can start using the TransactionsAPI, ensure you have the following prerequisites installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- SQL Server
- A CSV file containing transaction data

### Installation

1. Clone this repository to your local machine:

   ```shell
   git clone [https://github.com/teoad95/TransactionsAPI.git]
2. Navigate to the project directory
3. Configure your SQL Server connection string in the appsettings.json file and run the update database command through the Package Manager Console in order to create your database.

## Usage

### POST Transaction

To add or update transactions, make a POST request with a CSV file containing transaction data. The API will process the CSV and store the transactions in the database.

### GET All Transactions

Retrieve a list of all transactions with optional pagination support.

### GET Transaction

Retrieve information about a specific transaction by providing its unique identifier.

### DELETE Transaction

Delete a transaction by its unique identifier.

### UPDATE Transaction

Update an existing transaction or create a new one if it does not exist.
