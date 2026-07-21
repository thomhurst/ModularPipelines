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
    public async Task App_Delete_Renders_Optional_Multiple_Targets()
    {
        var arguments = BuildArguments(new ArgoCdAppDeleteOptions
        {
            ApplicationNames = ["first", "second"],
            Yes = true,
        });

        await Assert.That(arguments).IsEquivalentTo(["first", "second", "--yes"]);
    }

    [Test]
    public async Task Repo_Rm_Renders_Multiple_Repositories()
    {
        var arguments = BuildArguments(new ArgoCdRepoRmOptions(
            ["https://first.example/repo.git", "https://second.example/repo.git"]));

        await Assert.That(arguments).IsEquivalentTo(
        [
            "https://first.example/repo.git",
            "https://second.example/repo.git",
        ]);
    }

    [Test]
    public async Task Cluster_Set_Preserves_Name_Option()
    {
        var arguments = BuildArguments(new ArgoCdClusterSetOptions("production")
        {
            Name = "renamed-production",
        });

        await Assert.That(arguments).IsEquivalentTo(["production", "--name=renamed-production"]);
    }

    [Test]
    public async Task Source_Positions_Render_As_Repeated_Options()
    {
        var arguments = BuildArguments(new ArgoCdAppSyncOptions
        {
            ApplicationNames = ["my-app"],
            SourcePositions = ["1", "2"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "my-app",
            "--source-positions=1",
            "--source-positions=2",
        ]);
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
    public async Task ClusterRotateAuth_Renders_Server_Or_Name_As_One_Operand()
    {
        var arguments = BuildArguments(new ArgoCdClusterRotateAuthOptions("production"));

        await Assert.That(arguments).IsEquivalentTo(["production"]);
    }

    [Test]
    public async Task AdminClusterKubeconfig_Renders_Optional_Operands()
    {
        var listArguments = BuildArguments(new ArgoCdAdminClusterKubeconfigOptions());
        var generateArguments = BuildArguments(new ArgoCdAdminClusterKubeconfigOptions
        {
            ClusterUrl = "production",
            OutputPath = "cluster.yaml",
            InsecureSkipTlsVerify = true,
        });

        await Assert.That(listArguments).IsEmpty();
        await Assert.That(generateArguments).IsEquivalentTo(
        [
            "production",
            "cluster.yaml",
            "--insecure-skip-tls-verify",
        ]);
    }

    [Test]
    public async Task ProjRemoveDestination_Preserves_Server_Option()
    {
        var arguments = BuildArguments(new ArgoCdProjRemoveDestinationOptions(
            "platform",
            "https://destination.example",
            "apps")
        {
            Server = "https://argocd.example",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "platform",
            "https://destination.example",
            "apps",
            "--server=https://argocd.example",
        ]);
    }

    [Test]
    public async Task CertAddTls_Preserves_ServerName_Option()
    {
        var arguments = BuildArguments(new ArgoCdCertAddTlsOptions("repository.example")
        {
            ServerName = "argocd.example",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "repository.example",
            "--server-name=argocd.example",
        ]);
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

    [Test]
    public async Task AccountUpdatePassword_Renders_All_Values()
    {
        var arguments = BuildArguments(new ArgoCdAccountUpdatePasswordOptions
        {
            Account = "alice",
            CurrentPassword = "old-secret",
            NewPassword = "new-secret",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--account=alice",
            "--current-password=old-secret",
            "--new-password=new-secret",
        ]);

        var currentPassword = typeof(ArgoCdAccountUpdatePasswordOptions)
            .GetProperty(nameof(ArgoCdAccountUpdatePasswordOptions.CurrentPassword));
        var newPassword = typeof(ArgoCdAccountUpdatePasswordOptions)
            .GetProperty(nameof(ArgoCdAccountUpdatePasswordOptions.NewPassword));

        await Assert.That(currentPassword!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(newPassword!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    [Test]
    public async Task Context_Renders_Optional_Name_Before_Delete_Flag()
    {
        var arguments = BuildArguments(new ArgoCdContextOptions
        {
            ContextName = "cd.argoproj.io",
            Delete = true,
        });

        await Assert.That(arguments).IsEquivalentTo(["cd.argoproj.io", "--delete"]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
