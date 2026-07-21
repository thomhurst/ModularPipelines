using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Snyk.Enums;
using ModularPipelines.Snyk.Options;

namespace ModularPipelines.UnitTests.Attributes;

public class SnykOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Auth_Renders_Secret_Positional_And_OAuth_Options()
    {
        var arguments = BuildArguments(new SnykAuthOptions
        {
            ApiToken = "token-value",
            AuthType = "token",
            ClientSecret = "client-secret",
            ClientId = "client-id",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "token-value",
            "--auth-type=token",
            "--client-secret=client-secret",
            "--client-id=client-id",
        ]);
    }

    [Test]
    public async Task ContainerTest_Renders_Image_Flags_Enums_And_Integer()
    {
        var arguments = BuildArguments(new SnykContainerTestOptions("alpine:3.20")
        {
            PrintDeps = true,
            SeverityThreshold = SnykSeverityThreshold.High,
            FailOn = SnykFailOn.Patchable,
            NestedJarsDepth = 2,
            Username = "registry-user",
            Password = "registry-password",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "alpine:3.20",
            "--print-deps",
            "--severity-threshold=high",
            "--fail-on=patchable",
            "--nested-jars-depth=2",
            "--username=registry-user",
            "--password=registry-password",
        ]);
    }

    [Test]
    public async Task Credential_Values_Are_Marked_As_Secrets()
    {
        var apiToken = typeof(SnykAuthOptions).GetProperty(nameof(SnykAuthOptions.ApiToken));
        var clientSecret = typeof(SnykAuthOptions).GetProperty(nameof(SnykAuthOptions.ClientSecret));
        var password = typeof(SnykContainerTestOptions).GetProperty(nameof(SnykContainerTestOptions.Password));

        await Assert.That(apiToken!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(clientSecret!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    [Test]
    public async Task Policy_Renders_Optional_Path_And_Debug_Flag()
    {
        var arguments = BuildArguments(new SnykPolicyOptions
        {
            PathToPolicyFile = ".snyk",
            Debug = true,
        });

        await Assert.That(arguments).IsEquivalentTo([".snyk", "-d"]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
