**the goal of this article** is to introduce you to an alternative, more developer friendly way of bulding web apis with asp.net 6 instead of the more commonly used mvc controllers.

**we will be exploring** the open source endpoint library [FastEndpoints](https://fast-endpoints.com/) which is built on top of the new minimal api in .net 6, where we can get all the performance benefits without the pain-points of minimal api. the resulting code-base is cleaner to navigate and project maintainability becomes a breeze when combined with [vertical slice architecture](https://www.ghyston.com/insights/architecting-for-maintainability-through-vertical-slices), no matter the size or complexity of the project, because the framework stays out of your way so you can focus on the engineering aspects of your systems.

**let's get our hands dirty** by building a rest api for a simplified version of dev.to where authors/posters can sign up for an account and publish articles. new articles will go into a moderation queue where the site administrator would have to approve it before being publicly available.

**the main entities** in our system would be the following:
- Admin
- Author
- Article

**the features/user stories** of our system could be classified as follows:
- Admin
  - login to site
  - get a list articles to be moderated
  - approve an article to publish it
  - reject an article with a reason
- Author
  - sign up on site
  - login to site
  - get a list of their own articles
     - see status [pending/approved/rejected]
  - create new article
  - edit existing article
- Public Area
  - get a list of 50 latest articles
  - get article by id
  - get latest 50 comments for an article
  - post a comment on an article

**the tech stack** used will be the following:
- *base framework:* Asp.Net 6
- *endpoint framework:* FastEndpoints
- *auth provider:* JWT Bearer
- *input validation:* FluentValidations
- *data storage:* MongoDB
- *api visualization:* SwaggerUI

## let's go...

**create a new web project** and add FastEndpoints to it either using visual studio or by running the following commands in a terminal window:
```bash
dotnet new web -n MiniDevTo
dotnet add package FastEndpoints
dotnet add package FastEndpoints.Validation
dotnet add package FastEndpoints.Security
dotnet add package FastEndpoints.Swagger
```

**create the folder structure** for our features so that it looks like the following:

![feature folder structure](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/alci7aefu6bc9ib04pho.png)

each last level of the tree is going to be a single endpoint which could either be a command or a query which the ui/frontend of our app can call. queries are prefixed with `Get` as a convention indicating it is a retrieval of data, whereas commands are prefixed with verbs such as `Save`, `Approve`, `Reject`, etc. indicating committing of some state change. this might sound familiar if you've come across `CQRS` before, but we're not separating reads vs. writes here as done in CQRS. instead, we're organizing our our features/endpoints in accordance with `Vertical Slice Architecter`.

FastEndpoints is an implementation of [REPR pattern](https://deviq.com/design-patterns/repr-design-pattern). (this will be the last pattern i'll talk about in this article, i promise!)

> The REPR Design Pattern defines web API endpoints as having three components: a Request, an Endpoint, and a Response. It simplifies the frequently-used MVC pattern and is more focused on API development.

so, in order to give us some relief of the boring, repetitive task of creating the multiple class files needed for an endpoint, go ahead and install [this visual studio extension](https://fast-endpoints.com/wiki/VS-Extension.html) provided by FastEndpoints. no worries if you're not using visual studio, you can create those files manually.

### Program.cs
first thing's first... let's update `Program.cs` file to look like the following:
```csharp
global using FastEndpoints;
global using FastEndpoints.Validation;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.Run();
```
this is all that's needed for this to be a web api project. but... if you try to run the program now, you will be greeted with a run-time exception such as the following:

> **InvalidOperationException:** 'FastEndpoints was unable to find any endpoint declarations!'

let's fix that by creating our very first endpoint using the vs extension we just installed.

### Author Signup Endpoint

right-click the `Author/Signup` folder in visual studio > add > new item. then select `FastEndpoints Feature FileSet` new item template located under the `Installed > Visual C#` node. then for the file name, enter `Author.Signup.cs` as shown below:

<img loading="lazy" src="https://dev-to-uploads.s3.amazonaws.com/uploads/articles/b34139su76mm3toq9dps.gif">

what that will do is, it'll create a new set of files under the folder you selected with a namespace specific for this endpoint. open up `Endpoint.cs` file and have a look at the namespace at the top. it is what we typed in as the file name earlier.

while we have the endpoint class opened, go ahead and replace it's contents with the code below:
```csharp
namespace Author.Signup;

public class Endpoint : Endpoint<Request, Response, Mapper>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/author/signup");
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
what do we have here? we have an endpoint class definition that inherits from the generic base class `Endpoint<TRequest, TResponse, TMapper>`. it has 2 overridden methods `Configure()` and `HandleAsync()`.

in the configure method, we're specifying that we want the endpoint to listen for the http verb/method `post` on the route `/author/signup`. we're also saying that unauthenticated users should be allowed to access this endpoint by using the `AllowAnonymous()` method. 

the `HandleAsync()` method is where you'd write the logic for handling the incoming request. for now it's just sending a blank response because we haven't yet added any fields/properties to our request & response dto classes.

### Models.cs

open up the `Models.cs` file and replace request and response classes with the following:
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

