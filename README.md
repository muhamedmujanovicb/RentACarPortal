# FleetDrive — Rent-a-Car Web Application

## Project goal
FleetDrive is a web application designed to help rent-a-car businesses manage their fleet and contracts data. The application has a user side as well which helps the user book a vehicle easily or get a recommendation through the app's integrated recommendation algorithm. The application's goal is to display the contributor's skills and knowledge on ASP.NET and Entity Core frameworks.

## Technologies
* **Backend**
  - C#, ASP.NET, Entity Framework Core
* **Database**
  - Localized DB with SQL Lite
* **Frontend**
  - Bootstrap 5, HTML5, CSS3, JavaScript
* **IDE**
  - Visual Studio Community, DB Browser for SQL Lite

## Database Architecture
The application relies on a relational database structured around three core entities linked via foreign keys and shared reference properties:
* **Vehicles**
  - Stores fleet details
* **Contracts**
  - Manages rental agreements, linking clients to specific vehicles, alongside rental periods and driver details
* **Users**
  - Handles authentication and ownership scoping for fleet items

 There are also secondary entities
 * **Booking Contract Requests**
    - Designed to package information for constructing a `Contract`
*  **Vehicle Recommendations**
    - Passes user input data to the vehicle recommender algorithm
      
## Application Structure
* **Controllers**
  - Handle HTTP requests, LINQ data queries, and pass data to views via `ViewBag`
* **Models**
  - Define the data schema and database entities (`Vehicle`, `Contract`, `User`)
* **Views**
  - Razor templates utilizing Bootstrap components for a responsive UI, featuring custom-built dashboards and analytics charts
 
## Key Modules & Services
* **Fleet & Contract Services**
  - Core components managing data flow, validation, and relational integrity between vehicle registrations and client contracts
* **Recommendation Algorithm**
  - A custom matching module that evaluates user preferences against available fleet inventory, filtering and scoring vehicles to provide personalized rental suggestions
* **Interactive Statistics Dashboard**
  - Pulls aggregated contract data to render real-time visualizations covering fleet utilization rates, revenue generation by vehicle type, rental popularity, and average rental durations

## Conclusion
This project provided hands-on experience implementing architectural patterns (like Dependency Injection and Repository patterns), building custom LINQ joins for complex relational data, and integrating client-side charting libraries into an MVC pipeline. Building this project was a practical exercise in moving past theory and implementing core backend patterns firsthand. Specifically, it allowed me to put into practice:
* **MVC Pattern & Routing**
  - Structured controllers, models, and Razor views to handle data flow and clean separation of concerns
* **EF Core Migrations**
  - Managed database schemas and evolution code-first through migrations
* **Dependency Injection & Lifetimes**
  - Configured service registration (`scoped` vs `transient`) to handle database context scopes properly across requests
* **Relational Data Management**
  - Handled shared state and complex data querying using LINQ joins mapped to real database entities without foreign key constraints
* **Data Binding & Views**
  - Implemented row-level data binding and data rendering to dynamic UI components like Chart.js dashboards
