using ModularPipelines.GitHub;

namespace ModularPipelines.GitHub.UnitTests;

[NotInParallel("ProcessEnvironment")]
public class GitHubEnvironmentVariablesTests
{
    [Test]
    public async Task EventPath_Reads_GitHub_Event_Path()
    {
        var previous = Environment.GetEnvironmentVariable("GITHUB_EVENT_PATH");
        try
        {
            Environment.SetEnvironmentVariable("GITHUB_EVENT_PATH", "/tmp/event.json");

            await Assert.That(new GitHubEnvironmentVariables().EventPath).IsEqualTo("/tmp/event.json");
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_EVENT_PATH", previous);
        }
    }

    [Test]
    public async Task RunNumber_Is_Null_When_Missing_Or_Invalid()
    {
        var previous = Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER");
        try
        {
            (string? Value, int? Expected)[] cases =
            [
                (null, null),
                ("", null),
                ("invalid", null),
                ("2147483648", null),
                ("42", 42),
            ];

            foreach (var (value, expected) in cases)
            {
                Environment.SetEnvironmentVariable("GITHUB_RUN_NUMBER", value);

                var environment = new GitHubEnvironmentVariables();
                await Assert.That(environment.RunNumber).IsEqualTo(expected);
            }
        }
        finally
        {
            Environment.SetEnvironmentVariable("GITHUB_RUN_NUMBER", previous);
        }
    }
}
