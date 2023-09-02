using Bogus;
using SaveRequest = Author.Articles.SaveArticle.Request;

namespace Tests.Author.Articles;

internal static class Fakes
{
    internal static SaveRequest SaveRequest(this Faker f) => new()
    {
        Content = f.Lorem.Paragraphs(3),
        Title = f.Hacker.Phrase()
    };
}
