An Example Payment Gateway
==========================

[![Build Status](https://dev.azure.com/robertfpickering/robertfpickering/_apis/build/status/robertpi.checkout?branchName=master)](https://dev.azure.com/robertfpickering/robertfpickering/_build/latest?definitionId=1&branchName=master)

This is an example of what a simple payment gateway API might look like.

A swagger API browser can be found here: https://robertpi-checkout.azurewebsites.net/swagger

# Architecture

The API is implemented using ASP.NET Core MVC to provide JSON REST services. All API features are implemented 
in the project Checkout.csproj and unit tests are implemented in Checkout.Tests.csproj 

In the main project in coming data
is validated with [Fluent Validation](https://fluentvalidation.net/). JSON Serialization is provided
by [JSON.NET](https://www.newtonsoft.com/json) to be able to support [NodaTime](https://nodatime.org/) 
serialization. An API browser is provided by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) 
and [Swagger](https://swagger.io/).

Unit Testing is via [NUnit](https://nunit.org) and [Moq](https://github.com/Moq/moq4/wiki/Quickstart).

The main Checkout.csproj that implements the services has been compiled with C# 8.0's 
["Nullable Reference Type" Feature](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references)
to try and avoid errors due to nulls. The unit test project Checkout.Tests.csproj does not use this feature
as the structure of unit tests don't lend themselves to this.

# Deliverables

## Requirements

1. Build an API that allows a merchant
  * To process a payment through your payment gateway. 
  
    *This is implemented in **PaymentController** class.*

  * To retrieve details of a previously made payment. 
  
    *This is implemented in **PaymentHistoryController** class.*

2. Build a simulator to mock the responses from the bank to test the API from your first deliverable. 

      *This is implemented in **BankSimulator** class. 
      It is used to run the test version of the website and can be swapped out by changing the service configuration in **Startup** class. 
      No alertative implementation is provided. The simulator is not used in unit testing as mocks provide better control.*

## Extensions

### Application logging 

I have added minimalist logging where I thought necessary. I'm assuming the default ASP.NET Core MVC 
configuration is good enough for our perposes and the ASP.NET core does necessary logging of requests
and exceptions by default. To be really useful the logs would need to be shipped
somehow to a centralize log storage service, but this has not been implmented.

### Application metrics 

Not done

### Containerization 

The application runs inside a docker container. The docker files is based on the template
provided by Visual Studio 2019.

### Authentication 

Not done

### API client 

Not done

### Build script / CI 

The build script is made up a yaml file of the CI build and csproj files for all other build tasks.
I didn't not add a custom build script using a scripting language, like FAKE, because it seemed like over kill 
for a project of this size. I think the limits of this apporach would be quickly reached and a custom
build script would become necessary.

The CI build runs the unit tests and builds the docker image then pushes the image to an Azure repository. 
Once pushed to the Azure repository, the image is automatically deployed to an Azure web service.
Ideally the CI script would also measure test coverage and linting / fxcop / other static analysis but 
this has not been implemented.

### Performance testing 

Not done 

### Encryption 

Not done (beyond basic HTTPS which works by default)

### Data storage

Not done

# Further Work and Improvments

Aside from the extensions that have not been completed there are other improvments that could be made: 

* The APIs are incomplete:
  - /payment requires more attributes are required like client name, address, etc.
  - /paymenthistory currently it's only possible to retrive an payment by it's id, 
  allowing users to search by other attributes, such as the payment date, would be useful.
* The same data objects (**Checkout.PublicDtos** namspace) are used to store the data and send the data over the wire. To allow more 
flexible versioning it could be better to use two different sets of objects.
* The objects in **Checkout.PublicDtos** are not immutable, as this can cause problems with serialization 
frameworks, immutable data objects are generally easier to reason about so benefit program correctness.
* The data objects could be structured better, for example **CheckoutPaymentParameters** might be 
decomposed into **CreditCardDetails**, containing only attributes relating to the credit card and 
**Payment** containting the currency and amount.
* There's no way for clients using the bank similator to specify if they would like the simulator to
fail a payment.
* Better validation of incoming data, for example the currency could be matched against 
a list of ISO currency values. 