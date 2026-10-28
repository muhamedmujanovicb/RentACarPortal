# FleetDrive — Rent-a-Car Web Platform

## Project goal 🎯
FleetDrive is a web application designed to help rent-a-car businesses manage their fleet and contracts data. The application has a user side as well which helps the user book a vehicle easily or get a recommendation through the app's integrated recommendation algorithm. 

The application's goal is to display the contributor's skills and knowledge on ASP.NET Core and Entity Framework Core frameworks. Everything listed as implemented in the project is claimed as theoretically understood by the contributor.

## Technologies 🛠
* **Backend**
  - C#, ASP.NET Core 8 (MVC), Entity Framework Core, Cookie authentication
* **Database**
  - Local SQLite database
* **Frontend**
  - Bootstrap 5, HTML5, CSS3, JavaScript, Razor views
* **IDE**
  - Visual Studio Community, DB Browser for SQL Lite

## Database Architecture 📘
The application relies on a relational database structured around four core entities linked via foreign keys:
1. **Vehicles** - Stores fleet details, linked to the company that owns each vehicle
2. **Contracts** - Manages rental agreements, linking clients to specific vehicles, alongside rental periods and driver details
3. **Clients** - Handles authentication for users who book vehicles
4. **Companies** - Handles authentication and ownership for fleet groups
 
Lookup entities (vehicle make, model and type) are seeded into the database and shared by all vehicles.

 There is also a secondary assistance entity
*  **Vehicle Recommendations**
    - Passes user input data to the vehicle recommendor algorithm
 
Unique indexes protect usernames and vehicle registration numbers at database level.
      
## Application Structure 📕
* **Controllers** - One controller per feature. They handle HTTP requests, are protected by role-based authorization, and pass data to views via `ViewBag`
* **Services** - Hold the business logic and LINQ data queries (booking, dashboard, fleet, statistics and notification services)
* **Models** - Define the data schema and database entities (`Vehicle`, `Contract`, `Client`, `Company` and the lookup models)
* **Views** - Razor templates utilizing Bootstrap components for a responsive UI, featuring custom-built dashboards and analytics charts. Each controller has its own view folder (`Views/<Controller>/<Action>.cshtml`)
 
## Key Modules 🗝
* **Authentication and Authorization**
  - Cookie authentication with claims (id, username and role), salted password hashing, role-based access for the Company and Client sides, ownership checks on every company-owned record, and anti-forgery validation on every POST request
* **Fleet & Contract Services**
  - Core components managing data flow, validation, and relational integrity between vehicle registrations and client contracts
* **Recommendation Algorithm**
  - A custom matching module that evaluates user preferences against available fleet inventory, filtering and scoring vehicles to provide personalized rental suggestions
* **Interactive Statistics Dashboard**
  - Pulls aggregated contract data to render real-time visualizations covering fleet utilization rates, revenue generation by vehicle type, rental popularity, and average rental durations
* **Simple booking system**
  - Allows the client user to easily create a vehicle booking request through a well constructed forms pipeline. A vehicle is held while its request is pending and can only be booked while it is available

## Conclusion ⭕
This project provided hands-on experience in implementing some of the core elements of web development. Building this project was a practical exercise in moving past theory and implementing core backend patterns firsthand. Specifically, it allowed me to put into practice:

* **SECTION 1 - CORE WEB DEVELOPMENT ELEMENTS**

  * **MVC Pattern & Routing**
    - Structured controllers, models, and Razor views to handle data flow and clean separation of concerns, with one controller and one view folder per feature
    
  * **Application Architecture**
    - Structured a monolithic application designed to fit enterprise application standards across different application types
    
  * **EF Core Migrations**
    - Managed database schemas and evolution code-first through migrations, including unique indexes
    
  ##
  * **Dependency Injection & Lifetimes**
    - Configured service registration (`scoped` for services that use the database context, `singleton` for stateless services) to handle lifetimes properly across requests
    
  * **Relational Data Management**
    - Handled shared state and complex data querying using LINQ joins and eager loading (`Include` and `ThenInclude`) mapped to real database entities with foreign keys
    
  * **Data Binding & Views**
    - Implemented row-level data binding and data rendering to dynamic UI components like Chart.js dashboards, and passed one-time messages across redirects with `TempData`
    
  ##
  * **Authentication**
    - Stopped trusting identity sent by the client: the logged-in user now comes from a signed authentication cookie with claims-based identity, and passwords are stored as salted hashes instead of plain text
    
  * **Authorization**
    - Applied role-based authorization with `[Authorize(Roles = ...)]` and an access denied page, and configured the ASP.NET Core middleware pipeline in the correct order (`UseAuthentication` before `UseAuthorization`)
    
  * **Asynchronous programming**
    - Used `async` and `await` for sign-in and data access where the framework provides asynchronous operations
  
* **SECTION 2 - SECURITY THREATS IN DEVELOPMENT**
  * **Broken access control and Insecure Direct Object Reference (IDOR)**
    - Never trusting identity or record IDs sent by the client. Identity comes from the cookie, queries are scoped by owner, and a record that is not yours returns 404 instead of revealing that it exists
    
  * **Cross-Site Request Forgery (CSRF)**
    - Validated anti-forgery tokens on every POST request
    
  * **Insecure input handling**
    - Validated allowed status values, blocked duplicate usernames and registration numbers, and checked vehicle availability on the server instead of trusting the browser
  
* **SECTION 3 - CONCURRENCY CONTROL**
  * **Optimistic concurrency**
    - Protected vehicle bookings with optimistic concurrency, so two clients can no longer book the same vehicle at the same moment
    
  * **Race condition (check-then-act)**
    - Found that checking "Available" and then saving "Pending" in separate steps lets two requests both pass, and reproduced it with two requests paused at debugger breakpoints
    
  * **Lost update** 
    - Understood how two requests can read the same row, both change it, and the second save silently overwrites the first
    
  * **Optimistic concurrency and concurrency tokens**
    - Added a `[ConcurrencyCheck]` column to `Vehicle`, so a stale `UPDATE` matches zero rows and is rejected instead of overwriting
    
  * **EF Core change tracking** 
    - Learned how EF Core compares each property's original value with its current value when generating the `UPDATE`
    
  * **`DbUpdateConcurrencyException`** 
    - Caught it when a booking loses the race and showed a "vehicle no longer available" message instead of a server error
    
  * **GUID as a version stamp** 
    - Used a Globally Unique Identifier, regenerated on every vehicle status change, as the row version, since SQLite doesn't generate one automatically
    
  * **Nullable reference types & model binding**
    - Learned that MVC model binding turns an empty form field into `null`, and handled it with a nullable `string?` parameter so an empty comment no longer crashes a booking

  * **Server-side price calculation**
    - Calculated the total price on the server from the vehicle's daily rate, ignoring any price sent from the browser
