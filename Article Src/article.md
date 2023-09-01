**The goal of this article** is to introduce you to an alternative, more developer friendly way of building Web APIs with ASP.NET 7 instead of the more commonly used MVC controllers.

**We will be exploring** the open source library [FastEndpoints](https://fast-endpoints.com/) which is built on top of Minimal APIs introduced in .NET 6, where we can get all the performance benefits without the pain-points of Minimal APIs. The resulting code-base is cleaner to navigate and project maintainability becomes a breeze when combined with [Vertical Slice Architecture](https://www.ghyston.com/insights/architecting-for-maintainability-through-vertical-slices), no matter the size or complexity of the project, because the framework stays out of your way so you can focus on the engineering & functional aspects of your systems.

**Let's get our hands dirty** by building a REST API for a simplified version of Dev.to where authors/posters can sign up for an account and publish articles. New articles will go into a moderation queue where the site administrator would have to approve it before being publicly available.

**The main entities** in our system would be the following:
- Admin
- Author
- Article

**The features/user stories** of the system could be classified as follows:
- Admin
  - Login to site
  - Get a list articles to be moderated
  - Approve an article to publish it
  - Reject an article with a reason
- Author
  - Sign up on site
  - Login to site
  - Get a list of their own articles
     - See status [pending/approved/rejected]
  - Create new article
  - Edit existing article
- Public Area
  - Get a list of 50 latest articles
  - Get article by id
  - Get latest 50 comments for an article
  - Post a comment on an article

**The tech stack** used will be the following:
- *Base framework:* ASP.NET 7
- *Endpoint framework:* FastEndpoints
- *Authentication scheme:* JWT Bearer
- *Input validation:* FluentValidations
- *Data storage:* MongoDB
- *API visualization:* SwaggerUI

## Let's go...

**Create a new web project** and install the dependencies either using visual studio or by running the following commands in a terminal window:
```bash
dotnet new web -n MiniDevTo
dotnet add package FastEndpoints
dotnet add package FastEndpoints.Swagger
dotnet add package MongoDB.Entities
```

**Create the folder structure** for our features so that it looks like the following:

![feature folder structure](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/alci7aefu6bc9ib04pho.png)

Each last level of the tree is going to be a single endpoint which could either be a command or a query which the ui/frontend of our app can call. Queries are prefixed with `Get` as a convention indicating it is a retrieval of data, whereas commands are prefixed with verbs such as `Save`, `Approve`, `Reject`, etc. indicating committing of some state change. This might sound familiar if you've come across `CQRS` before, but we're not separating reads vs. writes here as done in CQRS. Instead, we're organizing our our features/endpoints in accordance with `Vertical Slice Architecture`.

FastEndpoints is an implementation of [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). This will be the last pattern I'll talk about in this article, I promise!

> The REPR Design Pattern defines web API endpoints as having three components: a Request, an Endpoint, and a Response. It simplifies the frequently-used MVC pattern and is more focused on API development.

So, in order to give us some relief from the boring, repetitive task of creating the multiple class files needed for an endpoint, go ahead and install either the [Visual Studio ](https://marketplace.visualstudio.com/items?itemName=dj-nitehawk.FastEndpoints) or [VS Code](https://marketplace.visualstudio.com/items?itemName=drilko.fastendpoints) extension provided by FastEndpoints. Or you can just create those files manually.

### Program.cs
First thing's first... Let's update `Program.cs` file to look like the following:
```csharp
global using FastEndpoints;
global using FluentValidation;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();
```
This is all that's needed for this to be a web API project. but... If you try to run the program now, you will be greeted with a run-time exception such as the following:

> **InvalidOperationException:** 'FastEndpoints was unable to find any endpoint declarations!'

Let's fix that by creating our very first endpoint using the VS Extension.

### Author Signup Endpoint

right-click the `Author/Signup` folder in visual studio > add > new item. then select `FastEndpoints Feature FileSet` new item template located under the `Installed > Visual C#` node. then for the file name, enter `Author.Signup.cs` as shown below:

![vs extension](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/jfet1qmz5c3whihjxz56.gif)

What that will do is, it'll create a new set of files under the folder you selected with a namespace specific for this endpoint. open up `Endpoint.cs` file and have a look at the namespace at the top. It is what we typed in as the file name earlier.

While we have the endpoint class opened, go ahead and replace it's contents with the code below:
```csharp
namespace Author.Signup;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Post("/author/signup");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        await SendAsync(new Response()
        {
            //blank for now
        });
    }
}
```
What do we have here? We have an endpoint class definition that inherits from the generic base class `Endpoint<TRequest, TResponse, TMapper>`. it has 2 overridden methods `Configure()` and `HandleAsync()`.

In the configure method, we're specifying that we want the endpoint to listen for the http verb/method `POST` on the route `/author/signup`. We're also saying that unauthenticated users should be allowed to access this endpoint by using the `AllowAnonymous()` method. 

The `HandleAsync()` method is where you'd write the logic for handling the incoming request. For now it's just sending a blank response because we haven't yet added any fields/properties to our request & response DTO classes.

### Models.cs

Open up the `Models.cs` file and replace request and response classes with the following:
```csharp
public class Request
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class Response
{
    public string Message { get; set; }
}
```

### Swagger UI
Now, let's setup Swagger so we have a way to interact with our endpoints using a web browser as opposed to using something like Postman. Open up `Program.cs` again and make it look like this:

```csharp
global using FastEndpoints;
global using FastEndpoints.Validation;
using FastEndpoints.Swagger; //add this

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument() //add this

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen(); //add this
app.Run();
```

Then open up `Properties/launchSettings.json` file and replace contents with this:
```java
{
  "profiles": {
    "MiniDevTo": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:8080",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```
Updating launch-settings is not mandatory but let's fix the listening port of our API server to `8080` for the purpose of this article.

Next build and run your project in debug mode (CTRL+F5 in Visual Studio). Fire up your web browser and head on over to the url `http://localhost:8080/swagger` to see the Swagger UI.

You should now be seeing something like this:

<img loading="lazy" src="https://dev-to-uploads.s3.amazonaws.com/uploads/articles/c0mrzzv5i3wt2uaqgsxx.png">

Expand the `/author/signup` endpoint, and modify the request body/json to look like this (click `Try It Out` button to do so):
```java
{
  "FirstName": "Johnny",
  "LastName": "Lawrence",
  "Email": "what@is.uber",
  "UserName": "EagleFang",
  "Password": "death2kobra"
}
```

Before executing the request, head on over to the `Endpoint.cs` file and place a breakpoint on line 14. Then go ahead and hit the execute button in swagger. Once the breakpoint is hit, inspect the request DTO parameter of the `HandleAsync()` method where you will see something like this:

<img src="https://dev-to-uploads.s3.amazonaws.com/uploads/articles/2x3adqpigbe2nc914bnf.png" loading="lazy">

That is basically how you receive a request from a client (Swagger UI in this case). The handler method is supplied with a fully populated POCO from the incoming http request. For a detailed explanation of how this model binding works, please have a look at the documentation page [here](https://fast-endpoints.com/docs/model-binding#built-in-request-binding).

**Let's return a response** from our endpoint now. Stop debugging and update the `HandleAsync()` method to look like the following:
```csharp
public override async Task HandleAsync(Request r, CancellationToken c)
{
    await SendAsync(new Response()
    {
        Message = $"hello {r.FirstName} {r.LastName}! your request has been received!"
    });
}
```

Start the app again and execute the same request in Swagger UI. Which should display the response from the server as follows:

<img loading="lazy" src="https://dev-to-uploads.s3.amazonaws.com/uploads/articles/el5fgxodte3nexdi4py0.png">

There are [multiple ways to send responses](https://fast-endpoints.com/docs/get-started#sending-responses) back to the client from a handler. here we're sending a new instance of a response DTO populated with a custom message.

### Input Validation

Open the `Models.cs` file and make the validator class look like the following:
```csharp
public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("your name is required!")
            .MinimumLength(3).WithMessage("name is too short!")
            .MaximumLength(25).WithMessage("name is too long!");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("email address is required!")
            .EmailAddress().WithMessage("the format of your email address is wrong!");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("a username is required!")
            .MinimumLength(3).WithMessage("username is too short!")
            .MaximumLength(15).WithMessage("username is too long!");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("a password is required!")
            .MinimumLength(10).WithMessage("password is too short!")
            .MaximumLength(25).WithMessage("password is too long!");
    }
}
```
Here we're defining the input validation requirements using [Fluent Validation](https://fluentvalidation.net/) rules. Let's see what happens when the user input doesn't meet the above  criteria. Execute the same request in swagger with the following incorrect json content:
```java
{
  "LastName": "Lawrence",
  "Email": "what is email?",
  "UserName": "EagleFang",
  "Password": "123"
}
```

The server will respond with this:
```java
{
  "StatusCode": 400,
  "Message": "One or more errors occured!",
  "Errors": {
    "FirstName": [ "your name is required!" ],
    "Email": [ "the format of your email address is wrong!" ],
    "Password": ["password is too short!" ]
  }
}
```

As you can see, if the incoming request data does not meet the validation criteria, a `http 400` bad response is returned with the details of what is wrong. The handler logic will not be executed in case there is a validation error in the incoming request. This default behavior can be changed [like this](https://fast-endpoints.com/docs/validation#disable-automatic-failure-response) if need be.

### Handler Logic

Let's go ahead and persists a new `Author` entity to the database by modifying the handler logic.
```csharp
public override async Task HandleAsync(Request r, CancellationToken c)
{
    var author = Map.ToEntity(r);

    var emailIsTaken = await Data.EmailAddressIsTaken(author.Email);

    if (emailIsTaken)
        AddError(r => r.Email, "Sorry! Email address is already in use...");

    var userNameIsTaken = await Data.UserNameIsTaken(author.UserName);

    if (userNameIsTaken)
        AddError(r => r.UserName, "Sorry! Ehat username is not available...");

    ThrowIfAnyErrors();

    await Data.CreateNewAuthor(author);

    await SendAsync(new()
    {
        Message = "Thank you for signing up as an author!"
    });
}
```
 First, we're using the `ToEntity()` method on the `Map` property of the endpoint class to transform the request dto into an `Author` [domain entity](https://github.com/dj-nitehawk/MiniDevTo/tree/main/Source/Entities/Author.cs). The logic for mapping is in the `Mapper.cs` file which can be [found here](https://github.com/dj-nitehawk/MiniDevTo/tree/main/Source/Features/Author/Signup/Mapper.cs). you can read more about the mapper class [here](https://fast-endpoints.com/docs/domain-entity-mapping#mapping-logic-in-a-separate-class).

Then we're asking the database if this email address is already taken by someone ([code here](https://github.com/dj-nitehawk/MiniDevTo/tree/main/Source/Features/Author/Signup/Data.cs)). If it's already taken we're adding a validation error to the collection of errors of the endpoint using the `AddError()` method.

Next, we're asking the db if the username is already taken by someone and add an error if it's taken.

After all the business rules are checked, we want to send an error response to the client if any of the previous business rule checks have failed. that's what the `ThrowIfAnyErrors()` does. When either the username or email address is taken, a response like the following will be sent to the client. Execution is stopped at that point and the proceeding lines of code are not executed.

```java
{
  "StatusCode": 400,
  "Message": "One or more errors occured!",
  "Errors": {
    "Email": [ "sorry! email address is already in use." ],
    "UserName": [ "sorry! that username is not available." ]
  }
}
```

If there are no validation errors added, and author creation worked, the following json response will be received by the client.

```java
{
  "Message": "Thank you for signing up as an author!"
}
```

## Congratulations!
You've persevered thus far and have your first working endpoint. If you're interested in completing this exercise, head on over to github and have a look through the [full source code](https://github.com/dj-nitehawk/MiniDevTo). Things should now be self explanatory. If something is unclear, please comment here or open a [github issue](https://github.com/dj-nitehawk/MiniDevTo/issues/new). I will try my best to answer within 24hrs. Also have a look through the following resources which will explain most of the code.


## Resources
- **[Project Source Code](https://github.com/dj-nitehawk/MiniDevTo)**
- **[FastEndpoints Documentation](https://fast-endpoints.com/)**
- **[MongoDB.Entities Documentation](https://mongodb-entities.com/)**
- **[Introductory Article To MongoDB](https://dev.to/djnitehawk/tutorial-mongodb-with-c-the-easy-way-1g68)**
- **[Vertical Slices Starter Template](https://github.com/dj-nitehawk/MongoWebApiStarter)**
- **[A Compendium Of VSA Resources](https://mehdihadeli.github.io/awesome-software-architecture/architectural-style/vertical-slice-architecture/)**