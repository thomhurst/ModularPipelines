using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Trivy.Enums;
using ModularPipelines.Trivy.Options;

namespace ModularPipelines.UnitTests.Attributes;

public class TrivyOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task Image_Renders_Target_Typed_Options_And_Secret()
    {
        var arguments = BuildArguments(new TrivyImageOptions
        {
            ImageName = "alpine:3.20",
            Format = TrivyImageFormat.Json,
            Severity = [TrivyImageSeverity.High, TrivyImageSeverity.Critical],
            Timeout = "2m",
            Password = ["registry-secret"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "alpine:3.20",
            "--format=json",
            "--severity=HIGH",
            "--severity=CRITICAL",
            "--timeout=2m",
            "--password=registry-secret",
        ]);
    }

    [Test]
    public async Task Image_Renders_Input_Without_Image_Name()
    {
        var arguments = BuildArguments(new TrivyImageOptions
        {
            Input = "ruby-3.1.tar",
        });

        await Assert.That(arguments).IsEquivalentTo(["--input=ruby-3.1.tar"]);
    }

    [Test]
    public async Task Nested_Commands_Render_Required_Operands()
    {
        await Assert.That(BuildArguments(new TrivyPluginInstallOptions("aquasecurity/trivy-plugin")))
            .IsEquivalentTo(["aquasecurity/trivy-plugin"]);
        await Assert.That(BuildArguments(new TrivyRegistryLoginOptions("registry.example.com")))
            .IsEquivalentTo(["registry.example.com"]);
    }

    [Test]
    public async Task Plugin_Upgrade_Renders_Each_Selected_Plugin()
    {
        var arguments = BuildArguments(new TrivyPluginUpgradeOptions
        {
            PluginNames = ["first", "second"],
        });

        await Assert.That(arguments).IsEquivalentTo(["first", "second"]);
    }

    [Test]
    public async Task Plugin_Run_Renders_Trailing_Plugin_Arguments()
    {
        var arguments = BuildArguments(new TrivyPluginRunOptions("kubectl")
        {
            PluginArguments = ["pod", "your-pod", "--", "--exit-code", "1"],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "kubectl",
            "pod",
            "your-pod",
            "--",
            "--exit-code",
            "1",
        ]);
    }

    [Test]
    public async Task Vex_Repo_Download_Renders_Each_Selected_Repository()
    {
        var arguments = BuildArguments(new TrivyVexRepoDownloadOptions
        {
            RepoNames = ["first", "second"],
        });

        await Assert.That(arguments).IsEquivalentTo(["first", "second"]);
    }

    [Test]
    public async Task Credential_Options_Are_Marked_As_Secrets()
    {
        var password = typeof(TrivyImageOptions).GetProperty(nameof(TrivyImageOptions.Password));
        var token = typeof(TrivyImageOptions).GetProperty(nameof(TrivyImageOptions.Token));

        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(token!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
