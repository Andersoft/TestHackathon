# Getting Started
Run the following docker command to get SQL set up locally `docker run -d -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MargretThatcher!1" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-CU13-ubuntu-20.04`
Next connect to sql and create a database called `OakbrookShop` once the database has been created run the init sql script.