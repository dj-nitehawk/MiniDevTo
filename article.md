**the goal of this article** is to introduce you to an alternative, more developer friendly way of bulding web apis with asp.net 6 instead of the more commonly used mvc controllers.

**we will be exploring** the open source micro framework [FastEndpoints](https://fast-endpoints.com/) which is built on top of the new minimal api in .net 6, where we can get all the performance benefits without the pain-points of minimal api. the resulting code base is cleaner to navigate and project maintainability becomes a breeze when combined with [vertical slice architecture](https://www.ghyston.com/insights/architecting-for-maintainability-through-vertical-slices), no matter the size or complexity of the project, because the framework stays out of your way so you can focus on the engineering of your systems.

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
```

**create the folder structure** for our features so that it looks like the following:

![feature folder structure](https://dev-to-uploads.s3.amazonaws.com/uploads/articles/alci7aefu6bc9ib04pho.png)

each last level of the tree is going to be a single endpoint which could either be a command or a query which the ui of our app can perform. queries are prefixed with `Get` as a convention indicating it is a retrieval of data, whereas commands are prefixed with verbs such as `Save`, `Approve`, `Reject`, etc. indicating committing some state change.



