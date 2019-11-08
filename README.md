# Admin Dashboard

Reference to the [design document](https://docs.google.com/document/d/1SUOwBpVA55hXOANGyHlSXm_SZFIHivj3G8LcsxXD0Jk/edit)

## Table of Contents

- [Setup](#setup)
- [Workflow](#workflow)
- [Tests](#tests)
- [Improvements to make](#improvements)

## Setup
1. Install MySQL on local machine (or dockerize)
2. Navigate to the AdminDashboard/ directory and run `dotnet restore` where the .sln file is
3. Run dotnet user-secrets init`
4. Run `dotnet user-secrets "AuthorizationConfiguration:Secret" "RY25waYFu+VlSdPUikfbLJEUpt2SuD5rF2bkQMAqwJ+N+6hm` (For purposes of running the application, the credentials are exposed)
5. Navigate to the client/ directory and run `npm install`
6. Run `npm start`. Make sure the `webpack.config.js` points at the exposed URL from the server.

## Workflow
1. Log in. Username is Minh@gravitational.com and password is GravitationalInterviewByMinhNovember2019
2. Through a REST client like Postman, register users on the account (Note the account's ID is exposed on the UI)
3. Once the user limit is reached, upgrade the plan to enterprise
4. Dismiss the congratulatory upgrade message
5. Log out

Feel free to remove any sections that aren't applicable to your project.

## Tests
1. In the `TestFixture.cs`, insert line             `Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", <token>);` on line 29. To get the token, call the login endpoint on the web API with the correct username and password.

## Improvements to make
1. Have the integration tests run without any manual changes by overriding Startup.cs and bypassing authorization.
2. Refactor server side so there is no concept of "Account ID", and have a default account to use by the client. On the client side, have the account already created by the time the admin logs in and simply get the default account.
3. Use react router correctly and include the account ID as query parameters between components
4. Upon server side errors, handle correctly in the client to redirect to an Error page