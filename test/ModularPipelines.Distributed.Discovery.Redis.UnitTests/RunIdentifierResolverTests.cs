using ModularPipelines.Distributed.Discovery.Redis;

namespace ModularPipelines.Distributed.Discovery.Redis.UnitTests;

[TUnit.Core.NotInParallel]
public class RunIdentifierResolverTests
{
    private static readonly string[] CiEnvironmentVariables =
    [
        "GITHUB_SHA",
        "BUILD_SOURCEVERSION",
        "CI_COMMIT_SHA",
    ];

    [Test]
    public async Task Returns_Configured_Value_When_Provided()
    {
        var result = RunIdentifierResolver.Resolve("my-run-id");

        await Assert.That(result).IsEqualTo("my-run-id");
    }

    [Test]
    public async Task Returns_First_Available_CI_Commit()
    {
        await WithEnvironmentAsync(
            new Dictionary<string, string?>
            {
                ["GITHUB_SHA"] = "github-sha",
                ["BUILD_SOURCEVERSION"] = "azure-sha",
                ["CI_COMMIT_SHA"] = "gitlab-sha",
            },
            async () =>
            {
                var result = RunIdentifierResolver.Resolve(null);

                await Assert.That(result).IsEqualTo("github-sha");
            });
    }

    [Test]
    public async Task Falls_Back_When_No_Value_Is_Configured()
    {
        await WithEnvironmentAsync(
            CiEnvironmentVariables.ToDictionary(name => name, _ => (string?) null),
            async () =>
            {
                var result = RunIdentifierResolver.Resolve(null);

                await Assert.That(result).IsNotNull();
                await Assert.That(result).IsNotEqualTo(string.Empty);
            });
    }

    private static async Task WithEnvironmentAsync(
        IReadOnlyDictionary<string, string?> values,
        Func<Task> assertion)
    {
        var originals = values.Keys.ToDictionary(name => name, Environment.GetEnvironmentVariable);

        try
        {
            foreach (var (name, value) in values)
            {
                Environment.SetEnvironmentVariable(name, value);
            }

            await assertion();
        }
        finally
        {
            foreach (var (name, value) in originals)
            {
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }
}
