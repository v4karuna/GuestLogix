# Guest Logix Interview - Vikram

Summary
- Algorithm uses BFS on a directed graph represented by an adjacency list. 
- The code is flexible to accomodate future use cases for multiple routes, number of connections, filter by airline etc.
- I ended up doing unit tests only for the algorithm because of time constraints.

Tech stack
- C#
- asp.net core
- xunit tests

Hosted Api
- https://guestlogixvikram.azurewebsites.net/api/routes/?origin=YYZ&destination=JFK
- Swagger: https://guestlogixvikram.azurewebsites.net/swagger

To run locally
- Clone repo
- Open in Visual Studio 2019 (recommended)
- Build Solution (optional: run unit tests) and Run
- URL: http://localhost:50719/api/routes/?origin=YYZ&destination=JFK
- Swagger URL: http://localhost:50719/swagger

Feel free to contact me if you run into any issues.
