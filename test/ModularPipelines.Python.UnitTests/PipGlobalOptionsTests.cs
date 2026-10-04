using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.Python.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Python.UnitTests;

public class PipGlobalOptionsTests : TestBase
{
    [Test]
    public async Task General_Options_Render_Once_Before_Command_And_Package_Options()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new PipInstallOptions
        {
            Python = "python3",
            RequireVirtualenv = true,
            Target = "packages",
            IndexUrl = "https://example.test/simple",
            RequirementSpecifier = ["example-package"],
        };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "pip --python python3 --require-virtualenv install --target packages --index-url https://example.test/simple example-package");
        await Assert.That(typeof(PipInstallOptions).GetProperty(nameof(PipOptions.Python))!.DeclaringType)
            .IsEqualTo(typeof(PipOptions));
    }

    [Test]
    public async Task Repeatable_General_Options_Render_Each_Value()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new PipCheckOptions
        {
            ExistsAction = ["s", "i"],
            TrustedHost = ["first.test", "second.test"],
            UseDeprecated = ["legacy-one", "legacy-two"],
            UseFeature = ["feature-one", "feature-two"],
        };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "pip --exists-action s --exists-action i --trusted-host first.test --trusted-host second.test --use-deprecated legacy-one --use-deprecated legacy-two --use-feature feature-one --use-feature feature-two check");
    }

    [Test]
    public async Task Wrapped_Flags_And_Value_Options_Preserve_Arity()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var options = new PipCheckOptions
        {
            Debug = true,
            DisablePipVersionCheck = true,
            Isolated = true,
            KeyringProvider = "disabled",
            ResumeRetries = "3",
        };

        await Assert.That(builder.Build(options).ToString()).IsEqualTo(
            "pip --debug --disable-pip-version-check --isolated --keyring-provider disabled --resume-retries 3 check");
    }

    [Test]
    public async Task Inherited_Proxy_Credentials_Are_Masked_In_Dry_Run_Output()
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(
            new PipCheckOptions { Proxy = "https://user:password@proxy.test:443" },
            new CommandExecutionOptions { InternalDryRun = true });

        await Assert.That(result.CommandInput).IsEqualTo("pip --proxy ********** check");
    }

    [Test]
    public async Task Verbose_And_Version_Preserve_Distinct_Case_Sensitive_Aliases()
    {
        var verbose = (CliFlagAttribute) Attribute.GetCustomAttribute(
            typeof(PipOptions).GetProperty(nameof(PipOptions.Verbose))!, typeof(CliFlagAttribute))!;
        var version = (CliFlagAttribute) Attribute.GetCustomAttribute(
            typeof(PipOptions).GetProperty(nameof(PipOptions.Version))!, typeof(CliFlagAttribute))!;

        await Assert.That(verbose.ShortForm).IsEqualTo("-v");
        await Assert.That(version.ShortForm).IsEqualTo("-V");
    }
}
