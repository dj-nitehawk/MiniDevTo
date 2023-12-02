using Bogus;
using SaveRequest = Author.Articles.SaveArticle.Request;

namespace Tests.Author.Articles;

static class Fakes
{
    internal static SaveRequest SaveRequest(this Faker f)
        => new()
        {
            Content = f.Lorem.Paragraphs(),
            Title = f.Hacker.Phrase()
        };
}