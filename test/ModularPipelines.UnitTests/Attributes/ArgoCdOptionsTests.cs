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
    public async Task ApplicationSetDelete_Renders_Multiple_Names()
    {
        var arguments = BuildArguments(new ArgoCdApplicationSetDeleteOptions(["first", "second"])
        {
            Wait = true,
            Yes = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "first",
            "second",
            "--wait",
            "--yes",
        ]);
    }

    [Test]
    public async Task ApplicationSetGenerate_Renders_File()
    {
        var arguments = BuildArguments(new ArgoCdApplicationSetGenerateOptions("apps.yaml")
        {
            Output = ArgoCdApplicationSetGenerateOutput.Json,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "apps.yaml",
            "--output=json",
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

    [Test]
    public async Task RbacCan_Renders_Role_Or_Subject_As_One_Operand()
    {
        var arguments = BuildArguments(new ArgoCdAdminSettingsRbacCanOptions(
            "role:admin",
            "create",
            "application"));

        await Assert.That(arguments).IsEquivalentTo(["role:admin", "create", "application"]);
    }

    [Test]
    public async Task UpdateRolePolicy_Renders_Explicit_False_For_Default_True_DryRun()
    {
        var arguments = BuildArguments(new ArgoCdAdminProjUpdateRolePolicyOptions(
            "project-*",
            "add-permission",
            "get")
        {
            DryRun = false,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "project-*",
            "add-permission",
            "get",
            "--dry-run=false",
        ]);
    }

    [Test]
    [Arguments("sync")]
    [Arguments("wait")]
    public async Task App_Targets_Render_Before_Options(string command)
    {
        var options = command == "sync"
            ? (object) new ArgoCdAppSyncOptions
            {
                ApplicationNames = ["first", "second"],
                DryRun = true,
            }
            : new ArgoCdAppWaitOptions
            {
                ApplicationNames = ["first", "second"],
                Health = true,
            };

        var arguments = BuildArguments(options);
        string[] expectedArguments = command == "sync"
            ? ["first", "second", "--dry-run"]
            : ["first", "second", "--health"];

        await Assert.That(arguments).IsEquivalentTo(expectedArguments);
    }

    [Test]
    public async Task Configure_Renders_Explicit_False_For_Prompts_Enabled()
    {
        var arguments = BuildArguments(new ArgoCdConfigureOptions
        {
            PromptsEnabled = false,
        });

        await Assert.That(arguments).IsEquivalentTo(["--prompts-enabled=false"]);
    }

    [Test]
    [Arguments("get")]
    [Arguments("rm")]
    public async Task Cluster_Server_Or_Name_Renders_As_One_Operand(string command)
    {
        var options = command == "get"
            ? (object) new ArgoCdClusterGetOptions("production")
            : new ArgoCdClusterRmOptions("production");

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["production"]);
    }

    [Test]
    public async Task ProjAddDestination_Preserves_Name_Flag()
    {
        var arguments = BuildArguments(new ArgoCdProjAddDestinationOptions(
            "platform",
            "production",
            "apps")
        {
            Name = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "platform",
            "production",
            "apps",
            "--name",
        ]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
