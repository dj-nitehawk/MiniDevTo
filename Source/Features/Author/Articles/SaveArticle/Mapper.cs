﻿namespace Author.Articles.SaveArticle;

public class Mapper : Mapper<Request, Response, Dom.Article>
{
    public override async Task<Dom.Article> ToEntityAsync(Request r, CancellationToken ct = default)
        => new()
        {
            ID = r.ArticleID!,
            AuthorID = r.AuthorID,
            Title = r.Title,
            Content = r.Content,
            AuthorName = await Data.GetAuthorName(r.AuthorID) ?? "Unknown"
        };
}