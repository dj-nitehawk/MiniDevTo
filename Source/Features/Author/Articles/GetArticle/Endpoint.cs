﻿namespace Author.Articles.GetArticle;

public class Endpoint : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/author/articles/get-article/{ArticleID}");
        Claims(Claim.AuthorID);
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        var article = await Data.GetArticle(r.ArticleID);

        if (article is null)
            await SendNotFoundAsync();
        else
            await SendAsync(article);
    }
}