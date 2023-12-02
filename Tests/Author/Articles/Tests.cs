using Delete = Author.Articles.DeleteArticle;
using Get = Author.Articles.GetArticle;
using List = Author.Articles.GetMyArticles;
using Save = Author.Articles.SaveArticle;

namespace Tests.Author.Articles;

public class Tests(Fixture f, ITestOutputHelper o) : TestClass<Fixture>(f, o)
{
    [Fact]
    public async Task Invalid_Article_Data()
    {
        var req = new Save.Request
        {
            Title = "blah",
            Content = "blah"
        };

        var (rsp, res) = await Fixture.Client.POSTAsync<Save.Endpoint, Save.Request, ErrorResponse>(req);

        rsp.IsSuccessStatusCode.Should().BeFalse();
        res.Errors.Count.Should().Be(2);
        res.Errors.Keys.Should().Equal(
            nameof(Save.Request.Title),
            nameof(Save.Request.Content));
    }

    [Fact, Priority(1)]
    public async Task Create_New_Articles()
    {
        for (var i = 0; i < 3; i++)
        {
            var req = Fake.SaveRequest();

            var (rsp, res) = await Fixture.Client.POSTAsync<Save.Endpoint, Save.Request, Save.Response>(req);

            rsp.IsSuccessStatusCode.Should().BeTrue();
            res.Message.Should().Be("Article saved!");
            res.ArticleID.Should().NotBeNullOrEmpty();

            Fixture.ArticleIDs.Add(res.ArticleID!);
        }

        Fixture.ArticleIDs.Count.Should().Be(3);
    }

    [Fact, Priority(2)]
    public async Task Get_Article_List()
    {
        var (rsp, res) = await Fixture.Client.GETAsync<List.Endpoint, IEnumerable<List.Article>>();

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.Count().Should().Be(3);
        res.Select(a => a.ArticleID).Should().Equal(Fixture.ArticleIDs);
    }

    [Fact, Priority(3)]
    public async Task Get_Article()
    {
        var articleID = Fixture.ArticleIDs[0];

        var (rsp1, res1) = await Fixture.Client.GETAsync<Get.Endpoint, Get.Request, Get.Response>(
                               new()
                               {
                                   ArticleID = articleID
                               });

        rsp1.IsSuccessStatusCode.Should().BeTrue();

        var article = await DB.Find<Dom.Article>().OneAsync(articleID);
        article.Should().NotBeNull();

        res1.ArticleID.Should().Be(article!.ID);
        res1.Title.Should().Be(article.Title);
        res1.Content.Should().Be(article.Content);

        var (rsp2, _) = await Fixture.Client.GETAsync<Get.Endpoint, Get.Request, Get.Response>(
                            new()
                            {
                                ArticleID = Fake.Random.Guid().ToString()
                            });
        rsp2.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact, Priority(4)]
    public async Task Delete_Article()
    {
        var articleID = Fixture.ArticleIDs[0];

        var (rsp, res) = await Fixture.Client.DELETEAsync<Delete.Endpoint, Delete.Request, Delete.Response>(
                             new()
                             {
                                 ArticleID = articleID
                             });

        rsp.IsSuccessStatusCode.Should().BeTrue();
        res.Message.Should().Be("Article Deleted!");

        var exists = await DB
                           .Find<Dom.Article>()
                           .MatchID(articleID)
                           .ExecuteAnyAsync();

        exists.Should().BeFalse();
    }
}