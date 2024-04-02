# Tax calculator app

This tax calculator app is a small PoWICDiaSAoT (Proof of what Iulia can do in a small amount of time).

## Disclaimers

In the previous ETL-related project I was able to invest more time, so that application may be more appealing from an architecture/design point of view. Down in the document, a small comparison section outlines the main differences and maybe what concepts could've been used here as well. However, some things got better (or so I hope) from the best practices point of view - these are also covered in the dedicated section.

In the initial requirements, the application is explained as a simple Tax Calculator based on the tax bands in the UK. 
However, the requirements also state that I should think about it as an enterprise-level application, therefore I tried 
to add a simple (yet, somewhat realistic) twist: 
    - The application should provide an administrator portal, allowing a designated person the add (and perform basic CRUD operations) a new country with its own dynamic list of tax bands. 
    - The regular calculator page should now contain a dropdown, allowing the regular (and not authenticated) user to select the desired country in order for the calculation to be based. 

Later on in the development other ideas occurred (and I think those could've been more suitable), but because I had to remain within some time boundaries, I will limit myself to only writing them down in the 'Improvements' section. 



## Overview

### Resource links
- Backend API : https://taxcalculatorapi.azurewebsites.net/swagger/index.html
- Frontend Client : https://taxcalculatorclient.azurewebsites.net/ 

## Technical details

The application has 2 main components: 

### Backend - .Net Core Web API 

Since the application itself is pretty straight forward I opted for a layered approach:
    - Controllers/interface
    - Services
    - Repositories

The API exposes the following capabilities:
    - Auth endpoints(login, register)
    - Tax Bands CRUD operations
    - Tax Calculator

The persistence layer is based on EF Core. It is used as an ORM on top of a SQL DB hosted in Azure.

### - Blazor UI Application

Here I was in a completely new field. I have close to zero experience when it comes to UI development so I was faced with a tough decision of what technologies to use. 
(My first thought was a plain JS/HTML/CSS application but I didn't want to bother with plain JS/AJAX HTTP requests, so I went with Blazor.)

Since the backend is based on .Net and C# I decided to give Blazor a chance. 
To spend as little time on the design as possible I integrated and used the Blazor Mud components (for dropdowns, input fields, buttons, grids, tables.. pretty much everything)

It is a nice framework, but my lack of experience with it can be seen in the project type (Server-Client instead of just web assemblies for a static web app deployment).

The application itself is formed of 4 main screens:
    - Landing page where you can choose between Administrator Portal or Tax Calculator
    - The Tax Calculator
    - The Authentication page
    - The Administrator Portal


### Cloud Infrastructure -- this is in progress

- The backend application is deployed using Azure App Service;
- All the secrets are manually placed in the service's Environment Variables

- The UI is deployed (unfortunately) in the same way... because I didn't use the proper Blazor Project to deploy it as a static app.

### Flow
- I did not expose the Register User functionality in the UI because of safety reasons. In a real enterprise level application, the Register part would be in another application that only a super-user has access to and can register Admins that are able to 
make the CRUD opperations on tax bands. You can make an API request to the Register endpoint to register an admin user for further use. 

### Hr ETL Project vs Tax Calculator Project

As mentioned in the beginning, I had the chance to spend more time on the first project and this reflected mainly on the cloud infrastructure used (although a discussion can be made as if more services would've been relevant for the Calculator).

Maybe the most obvious thing that could've been brought over was using the Azure Key Vault to store secrets. 

From the code-oriented best practices some things I learned and applied in the meantime are:
- Use of a middleware to handle unexpected errors (instead of trying catches in every controller)
- Use of fluent result instead of a custom Result<T> object.
- Use of a mapping nugget (Mapster) instead of manually converting the Entities to Models and vice-versa.
- Dedicate extension methods per project to handle DI.
- Options pattern for passing app configuration around



## Improvements:

- UI can be improved quite a lot by adding additional validations, adding nice-to-have elements in order to improve UX, optimizing the authentication mechanism, and creating reusable components - not to mention writing it better.
- The TaxCalculator service from the backend should/could implement a combination of Strategy and Factory patterns. In the current state of the application, it doesn't make a difference because regardless of the selected country the same algorithm is applied. However, if we add a different calculation approach (see how the IT industry in Romania has different legislation when it comes to taxes), a simple checkbox in the UI (I am a developer!) would completely change the algorithm (strategy) while the input/output format should/could remain the same.

Known issues:
- Double-clicking on log in
- Not properly formatted UI errors
- UI side user session handling


