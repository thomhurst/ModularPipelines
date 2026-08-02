namespace ModularPipelines.Git.UnitTests;

public class GitCommitMapperTests
{
    private const string Separator = "%\n%";

    private readonly GitCommitMapper _mapper = new();

    [Test]
    public async Task Maps_Commit_Fields_And_Normalizes_Date_Offsets()
    {
        var output = JoinFields(
            "Author Name",
            "author@example.com",
            "2026-07-31T18:30:45+05:30",
            "Committer Name",
            "committer@example.com",
            "2026-07-31T08:15:30-04:00",
            "0123456789abcdef0123456789abcdef01234567",
            "0123456",
            "Commit subject",
            "Commit body\nwith another line");

        var commit = _mapper.Map(output);

        await Assert.That(commit.Author!.Name).IsEqualTo("Author Name");
        await Assert.That(commit.Author.Email).IsEqualTo("author@example.com");
        await Assert.That(commit.Author.Date).IsEqualTo(new DateTime(2026, 7, 31, 13, 0, 45, DateTimeKind.Utc));
        await Assert.That(commit.Committer!.Name).IsEqualTo("Committer Name");
        await Assert.That(commit.Committer.Email).IsEqualTo("committer@example.com");
        await Assert.That(commit.Committer.Date).IsEqualTo(new DateTime(2026, 7, 31, 12, 15, 30, DateTimeKind.Utc));
        await Assert.That(commit.Hash!.Long).IsEqualTo("0123456789abcdef0123456789abcdef01234567");
        await Assert.That(commit.Hash.Short).IsEqualTo("0123456");
        await Assert.That(commit.Message!.Subject).IsEqualTo("Commit subject");
        await Assert.That(commit.Message.Body).IsEqualTo("Commit body\nwith another line");
    }

    [Test]
    public async Task Preserves_Empty_Fields_Without_Shifting_Later_Values()
    {
        var output = JoinFields(
            "Author Name",
            string.Empty,
            "2026-07-31T13:00:45Z",
            "Committer Name",
            "committer@example.com",
            "2026-07-31T12:15:30Z",
            "0123456789abcdef0123456789abcdef01234567",
            "0123456",
            "Commit subject",
            string.Empty);

        var commit = _mapper.Map(output);

        await Assert.That(commit.Author!.Email).IsEmpty();
        await Assert.That(commit.Committer!.Name).IsEqualTo("Committer Name");
        await Assert.That(commit.Message!.Subject).IsEqualTo("Commit subject");
        await Assert.That(commit.Message.Body).IsEmpty();
    }

    [Test]
    [Arguments(9)]
    [Arguments(11)]
    public async Task Rejects_Unexpected_Field_Count(int fieldCount)
    {
        var output = string.Join(Separator, Enumerable.Repeat("value", fieldCount));

        var exception = Assert.Throws<ArgumentException>(() => _mapper.Map(output));

        await Assert.That(exception.Message).Contains("Expected exactly 10 fields");
    }

    private static string JoinFields(params string[] fields)
    {
        return string.Join(Separator, fields);
    }
}
