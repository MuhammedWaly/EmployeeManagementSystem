# Employee Managment System

## Technologies Used
 **Backend:**
  - C#
  - .NET 8
  - Entity Framework Core
  - Identity and JWT for Authentication and Authorization
    
 **Frontend:**
  - Blazor WASM


- **Development Environment:**
  - Visual Studio 2022

  ## Getting Started 

2. Open the project in Visual Studio 2022.

3. Set up your database connection in the `appsettings.json` file.

5. Run the project.


## Application Scenarios and Tasks

### Authentication :
- Authentication in this project is handled through JWT tokens. When a user logs in with their credentials, the server validates these credentials and generates a JWT token. This token contains encoded information about the user and serves as proof of their identity. The client includes this token in subsequent requests to the server to authenticate their identity.
Authorization

### Authorization 
- implemented using role-based access control. Each JWT token includes information about the user's role, such as 'admin' or 'user'. When a request is made to the server, the server checks the user's role in the JWT token to ensure they have the necessary permissions to perform the requested action. For example, certain endpoints or functionalities may only be accessible to users with the 'admin' role.
Token and Refresh Token

To enhance security and manage user sessions, this project uses both access tokens and refresh tokens. The access token is short-lived and is used to authenticate and authorize requests. Once it expires, the client can use a refresh token to obtain a new access token without requiring the user to log in again. This helps maintain security while providing a seamless user experience.

### Employees :
1. **Adding a New Employee [Admin]:**
2. **Get all Employees with relevant details Like General depart, Department, Country, and Town**
 
3. **Updating Product or Category Information[Admin]:**

4. **Deleting a Employee:**

5. **Get a Employee By Id:** 
 
### User Management :
1. **Deleting a User:**
2. **Get All Users[Admin]**
3. **Get User By Id[Admin]**   
4. **Get User by Username**


### Location Management
  **Countries - Cities - Towns**
1. **Get All**
2. **Get By Id**
3. **Delete**
4. **Update**

### Dapartments Management
  **General Dep - Department - Branches**
1. **Get All**
2. **Get By Id**
3. **Delete**
4. **Update**

### Beautiful UI
- This project features a stunning user interface designed using Blazor WebAssembly. It leverages modern UI frameworks like Bootstrap to create an intuitive and   visually appealing user experience.
