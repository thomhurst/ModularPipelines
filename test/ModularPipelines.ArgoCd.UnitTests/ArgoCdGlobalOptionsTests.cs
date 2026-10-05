using System.Reflection;
using ModularPipelines.ArgoCd.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.ArgoCd.UnitTests;

public class ArgoCdGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Persistent_Values_Render_After_Command_With_Explicit_False_And_Zero()
    {
        var options = new ArgoCdAppGetOptions("demo")
        {
            GrpcWeb = CliOptionValue.Bare,
            HttpRetryMax = 0,
            Insecure = "false",
        };
        await AssertArguments(BuildArguments(options),
            ["demo", "--grpc-web", "--http-retry-max=0", "--insecure=false"]);
        await Assert.That(await RenderCommand(options))
            .IsEqualTo("argocd app get demo --grpc-web --http-retry-max=0 --insecure=false");
    }

    [Test]
    public async Task Repeated_Headers_Keep_Alias_And_Mask_Credential_Values()
    {
        var options = new ArgoCdAppGetOptions("demo")
        {
            AuthToken = "private-token",
            Header = ["Authorization: Bearer private-header", "X-Trace: trace-value"],
            ClientCrtKey = "public-key-path",
        };
        var arguments = BuildArguments(options);
        await Assert.That(arguments).Contains("--header=Authorization: Bearer private-header");
        await Assert.That(arguments).Contains("--header=X-Trace: trace-value");
        await Assert.That(typeof(ArgoCdOptions).GetProperty(nameof(ArgoCdOptions.Header))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-H");
        var obfuscator = await GetService<ISecretObfuscator>();
        var output = obfuscator.Obfuscate(string.Join(' ', arguments), options);
        await Assert.That(output).DoesNotContain("private-token");
        await Assert.That(output).DoesNotContain("private-header");
        await Assert.That(output).Contains("public-key-path");
    }

    [Test]
    public async Task Admin_Server_Override_Shares_Storage_And_Emits_Once()
    {
        var options = new ArgoCdAdminImportOptions("backup.yaml") { Server = "https://initial" };
        ((ArgoCdOptions)options).Server = "https://kubernetes";
        await Assert.That(options.Server).IsEqualTo("https://kubernetes");
        await AssertArguments(BuildArguments(options), ["backup.yaml", "--server=https://kubernetes"]);
        await Assert.That(typeof(ArgoCdAdminImportOptions).GetProperty(nameof(ArgoCdOptions.Server))!.DeclaringType)
            .IsEqualTo(typeof(ArgoCdAdminImportOptions));
    }

    [Test]
    public async Task Shared_Settings_Do_Not_Promote_Command_Local_Options()
    {
        await Assert.That(typeof(ArgoCdOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Count().IsEqualTo(26);
        await Assert.That(typeof(ArgoCdOptions).GetProperty("AppNamespace")).IsNull();
        await Assert.That(typeof(ArgoCdAppGetOptions).GetProperty(nameof(ArgoCdOptions.AuthToken))!.DeclaringType)
            .IsEqualTo(typeof(ArgoCdOptions));
        var options = new ArgoCdLoginOptions("cd.example") { Server = "api.example" };
        await AssertArguments(BuildArguments(options), ["cd.example", "--server=api.example"]);
    }
}
