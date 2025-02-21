# TodoApi

A simple to-do task RESTful API with magic link authentication built with ASP.NET
Core Web APIs and Microsoft SQL Server. 

## Installation

1. Clone the repository:
	```bash
	git clone https://github.com/treemanw1/TodoApi.git
	```
2. Navigate to the project directory:
	```bash
	cd TodoApi
	```

## Usage
1. Run a Microsoft SQL Server docker container for development purposes!
	```
	docker pull mcr.microsoft.com/mssql/server:2022-latest

	docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123" -p 1433:1433 --name sql2022 --hostname sql2022 -d mcr.microsoft.com/mssql/server:2022-latest
	```
2. Create the `appsettings.json` file. For testing the email in development, I recommend using your personal gmail account and a [Google application password](https://support.google.com/accounts/answer/185833?hl=en) in `appsettings.json`.

3. Generate the database.
	```
	dotnet ef database update
	```

3. Start the server:
	```bash
	dotnet run	
	```
4. Access the API (Swagger UI) at `http://localhost:5261/swagger`.

## Auth Documentation

### Background

This project implements the backend of magic link authentication, a
password-less method of authentication that generates a cryptographically secure
token, embeds it in a link, and sends it to the user's email. Upon clicking the
link, the validity of the token is verified, authenticating the user.

The user is then provided with a new access token, enabling them to access
protected resources without needing to authenticate every request for some
period of time.

### POST /api/Auth

The user first submits their email to this endpoint, which generates a JWT
token, we'll call it the "magic" token, and sends it to the user's email.

In a full-stack implementation, the token would be embedded in a link, which
when clicked on, verifies the validity of the token and redirects the user to
their protected resources (say, an overview page).

### POST /api/Auth/Validate

We pass in the magic token to this endpoint, which validates it, marks the token
as used in the database, and returns the user a new separate stateless JWT
access token, which can be used to access their protected resources.