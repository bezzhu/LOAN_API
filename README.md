# LOAN API
## Description
This project implements a Web API for managing loans, including functionalities for both accountants and users.

## Components
### Accountant Controller:

Allows accountants to view, update, and delete loans.
Provides functionality to block users for a certain period, restricting their loan-taking rights.

### User Controller:

Supports user registration and authentication.
Allows users to view their information based on their ID.
Users can request loans, but their ability to do so depends on their account status.

### Loan Entity:

Defines the fields for loan requests, including type, amount, currency, period, and status.

### Authorization:
Implements role-based authorization, allowing only accountants to perform certain actions.
Users have limited rights, such as requesting loans and viewing their own information.

### Database Setup:
Describes the setup process for the database, including schema and initial data seeding.


## API Reference

#### Get all loans

```http
  GET /api/Accoutant/getAllLoans/${userId}
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `userId` | `int` | **Required**. Registered Accountant token for authentication|




## 🚀 About Me
I'm a Software developer...


## 🔗 Links
[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/bekabezhuashvili/)

