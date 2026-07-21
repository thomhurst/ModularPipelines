using ModularPipelines.ArgoCd.Enums;
using ModularPipelines.ArgoCd.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class ArgoCdOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task AppGet_Renders_Positional_Options_Enum_And_Secret()
    {
        var arguments = BuildArguments(new ArgoCdAppGetOptions("guestbook")
        {
            AppNamespace = "argocd",
            HardRefresh = true,
            Output = ArgoCdAppGetOutput.Json,
            Timeout = 30,
            AuthToken = "token-value",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "guestbook",
            "--app-namespace=argocd",
            "--hard-refresh",
            "--output=json",
            "--timeout=30",
            "--auth-token=token-value",
        ]);
    }

    [Test]
    public async Task ApplicationSetCreate_Renders_Multiple_Files_And_Flags()
    {
        var arguments = BuildArguments(new ArgoCdApplicationSetCreateOptions(["apps.yaml", "more-apps.yaml"])
        {
            DryRun = true,
            Output = ArgoCdApplicationSetCreateOutput.Yaml,
            Upsert = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "apps.yaml",
            "more-apps.yaml",
            "--dry-run",
            "--output=yaml",
            "--upsert",
        ]);
    }

    [Test]
    public async Task Credential_Options_Are_Marked_As_Secrets()
    {
        var authToken = typeof(ArgoCdAppGetOptions).GetProperty(nameof(ArgoCdAppGetOptions.AuthToken));
        var password = typeof(ArgoCdRepoAddOptions).GetProperty(nameof(ArgoCdRepoAddOptions.Password));

        await Assert.That(authToken!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
