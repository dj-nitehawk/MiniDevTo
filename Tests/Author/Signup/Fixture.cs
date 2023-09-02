using Xunit.Abstractions;

namespace Tests.Author.Signup;

public class Fixture : TestFixture<Program>
{
    public Fixture(IMessageSink s) : base(s) { }
}