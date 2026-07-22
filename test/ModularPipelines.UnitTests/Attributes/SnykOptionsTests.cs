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
            FailOn = SnykContainerTestFailOn.Upgradable,
            NestedJarsDepth = 2,
            Username = "registry-user",
            Password = "registry-password",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "alpine:3.20",
            "--print-deps",
            "--severity-threshold=high",
            "--fail-on=upgradable",
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

    [Test]
    public async Task Previously_Bare_Value_Options_And_Code_Path_Render_Values()
    {
        var describeArguments = BuildArguments(new SnykIacDescribeOptions
        {
            TfcToken = "terraform-token",
            TfcEndpoint = "https://terraform.example.com",
            ConfigDir = ".snyk-config",
        });
        var aibomArguments = BuildArguments(new SnykAibomOptions
        {
            Upload = true,
            Repo = "https://github.com/example/repository",
        });
        var codeArguments = BuildArguments(new SnykCodeTestOptions
        {
            Path = "src",
        });

        await Assert.That(describeArguments).IsEquivalentTo(
        [
            "--tfc-token=terraform-token",
            "--tfc-endpoint=https://terraform.example.com",
            "--config-dir=.snyk-config",
        ]);
        await Assert.That(aibomArguments).IsEquivalentTo(
        [
            "--upload",
            "--repo=https://github.com/example/repository",
        ]);
        await Assert.That(codeArguments).IsEquivalentTo(["src"]);

        var tfcToken = typeof(SnykIacDescribeOptions).GetProperty(nameof(SnykIacDescribeOptions.TfcToken));
        await Assert.That(tfcToken!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    [Test]
    public async Task Root_Test_Renders_Optional_Target()
    {
        var arguments = BuildArguments(new SnykTestOptions
        {
            Target = "github.com/example/repository",
            FailOn = SnykFailOn.Patchable,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "github.com/example/repository",
            "--fail-on=patchable",
        ]);
    }

    [Test]
    public async Task Monitor_Renders_Optional_Target()
    {
        var arguments = BuildArguments(new SnykMonitorOptions
        {
            Target = "src/MyApp",
        });

        await Assert.That(arguments).IsEquivalentTo(["src/MyApp"]);
    }

    [Test]
    public async Task IacDescribe_Renders_Filter_Value()
    {
        var arguments = BuildArguments(new SnykIacDescribeOptions
        {
            Filter = "resource.type == 'aws_s3_bucket'",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--filter=resource.type == 'aws_s3_bucket'",
        ]);
    }

    [Test]
    public async Task Sbom_Commands_Render_Documented_Options()
    {
        var sbomArguments = BuildArguments(new SnykSbomOptions(SnykFormat.Cyclonedx16Json)
        {
            Org = "security",
            Dev = true,
            Exclude = "fixtures",
            DetectionDepth = 3,
            PruneRepeatedSubdependencies = true,
            AllowIncompleteSbom = true,
            JsonFileOutput = "bom.json",
        });
        var testArguments = BuildArguments(new SnykSbomTestOptions("bom.json")
        {
            Json = true,
        });
        var containerArguments = BuildArguments(new SnykContainerSbomOptions(
            SnykFormat.Spdx23Json,
            "registry.example.com/app:latest")
        {
            Org = "security",
            ExcludeAppVulns = true,
            ExcludeNodeModules = true,
            NestedJarsDepth = 2,
            Username = "registry-user",
            Password = "registry-password",
        });

        await Assert.That(sbomArguments).IsEquivalentTo(
        [
            "--format=cyclonedx1.6+json",
            "--org=security",
            "--dev",
            "--exclude=fixtures",
            "--detection-depth=3",
            "--prune-repeated-subdependencies",
            "--allow-incomplete-sbom",
            "--json-file-output=bom.json",
        ]);
        await Assert.That(testArguments).IsEquivalentTo(["--file=bom.json", "--json"]);
        await Assert.That(containerArguments).IsEquivalentTo(
        [
            "--format=spdx2.3+json",
            "registry.example.com/app:latest",
            "--org=security",
            "--exclude-app-vulns",
            "--exclude-node-modules",
            "--nested-jars-depth=2",
            "--username=registry-user",
            "--password=registry-password",
        ]);
    }

    [Test]
    public async Task Terraform_Headers_And_Container_Password_Are_Secrets()
    {
        var headers = typeof(SnykIacDescribeOptions)
            .GetProperty(nameof(SnykIacDescribeOptions.FetchTfstateHeaders));
        var password = typeof(SnykContainerSbomOptions)
            .GetProperty(nameof(SnykContainerSbomOptions.Password));

        await Assert.That(headers!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
        await Assert.That(password!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
