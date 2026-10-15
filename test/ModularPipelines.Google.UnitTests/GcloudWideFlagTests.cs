using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Extensions;
using ModularPipelines.Google.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudWideFlagTests : TestBase
{
    [Test]
    public async Task Project_Renders_Once_After_The_Complete_Command_Path()
    {
        var command = await RenderCommand(new GcloudProjectsListOptions { Project = "build-project" });

        await Assert.That(command).IsEqualTo("gcloud projects list --project=build-project");
    }

    [Test]
    [Arguments("Account")]
    [Arguments("BillingProject")]
    [Arguments("Configuration")]
    [Arguments("FlagsFile")]
    [Arguments("Flatten")]
    [Arguments("Format")]
    [Arguments("Project")]
    [Arguments("Quiet")]
    [Arguments("Verbosity")]
    [Arguments("AccessTokenFile")]
    [Arguments("ImpersonateServiceAccount")]
    [Arguments("LogHttp")]
    [Arguments("TraceToken")]
    [Arguments("UserOutputEnabled")]
    public async Task Commands_Inherit_One_Definition_Of_Each_Wide_Flag(string name)
    {
        var property = typeof(GcloudProjectsListOptions).GetProperty(name)!;

        await Assert.That(property.DeclaringType).IsEqualTo(typeof(GcloudOptions));
        await Assert.That(typeof(GcloudProjectsListOptions)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Any(candidate => candidate.Name == name)).IsFalse();
    }

    [Test]
    public async Task Local_Project_Override_Shares_The_Base_Value_And_Preserves_Validation()
    {
        var options = new GcloudAccessApprovalRequestsListOptions();
        GcloudOptions global = options;
        global.Project = "resource-project";

        await Assert.That(options.Project).IsEqualTo("resource-project");
        await AssertArguments(BuildArguments(options), ["--project=resource-project"]);
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsTrue();

        options.Folder = "folder";
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsFalse();
    }

    [Test]
    public async Task Required_Local_Project_Takes_Precedence_Over_The_Inherited_Default()
    {
        var options = new GcloudAssetAnalyzeMoveOptions("resource-project") { DestinationFolder = "folder" };
        ((GcloudOptions) options).Project = "global-project";

        await Assert.That(options.Project).IsEqualTo("resource-project");
        var arguments = BuildArguments(options);
        await Assert.That(arguments.Count(argument => argument.StartsWith("--project=", StringComparison.Ordinal)))
            .IsEqualTo(1);
        await Assert.That(arguments).Contains("--project=resource-project");
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsTrue();
    }

    [Test]
    public async Task Scalar_Values_Stay_In_One_Equals_Separated_Argument()
    {
        var arguments = BuildArguments(new GcloudProjectsListOptions
        {
            Project = "resource-project",
            AccessTokenFile = "credentials/access token.txt",
        });

        await Assert.That(arguments.Count).IsEqualTo(2);
        await Assert.That(arguments).Contains("--project=resource-project");
        await Assert.That(arguments).Contains("--access-token-file=credentials/access token.txt");
    }

    [Test]
    public async Task Flags_Files_Repeat_Their_Switch()
    {
        await AssertArguments(BuildArguments(new GcloudProjectsListOptions
        {
            FlagsFile = ["first.yaml", "second.json"],
        }), ["--flags-file=first.yaml", "--flags-file=second.json"]);
    }

    [Test]
    public async Task List_Values_Use_One_Comma_Separated_Argument_Per_Flag()
    {
        var arguments = BuildArguments(new GcloudProjectsListOptions
        {
            Flatten = ["labels", "bindings"],
            ImpersonateServiceAccount = ["first@example.com", "second@example.com"],
        });

        await Assert.That(arguments.Count).IsEqualTo(2);
        await Assert.That(arguments).Contains("--flatten=labels,bindings");
        await Assert.That(arguments).Contains("--impersonate-service-account=first@example.com,second@example.com");
    }

    [Test]
    public async Task Empty_Collections_Omit_Their_Flags()
    {
        await AssertArguments(BuildArguments(new GcloudProjectsListOptions
        {
            FlagsFile = [],
            Flatten = [],
            ImpersonateServiceAccount = [],
        }), []);
    }

    [Test]
    public async Task Quiet_Preserves_Its_Alias_And_Renders_Only_The_Long_Form()
    {
        var flag = typeof(GcloudOptions).GetProperty(nameof(GcloudOptions.Quiet))!
            .GetCustomAttribute<CliFlagAttribute>()!;

        await Assert.That(flag.ShortForm).IsEqualTo("-q");
        await AssertArguments(BuildArguments(new GcloudProjectsListOptions { Quiet = true }), ["--quiet"]);
    }

    [Test]
    [Arguments(true, "--user-output-enabled")]
    [Arguments(false, "--no-user-output-enabled")]
    [Arguments(null, null)]
    public async Task Output_Flag_Preserves_True_False_And_Unset(bool? enabled, string? expected)
    {
        await AssertArguments(BuildArguments(new GcloudProjectsListOptions { UserOutputEnabled = enabled }),
            expected is null ? [] : [expected]);
    }

    [Test]
    public async Task Trace_Token_Is_Masked_While_The_Credential_File_Path_Remains_Visible()
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<TrueModule>().BuildAsync();
        var options = new GcloudProjectsListOptions
        {
            TraceToken = "private-trace-token",
            AccessTokenFile = "credentials/access-token.txt",
        };
        var obfuscator = pipeline.Services.GetRequiredService<ISecretObfuscator>();

        var output = obfuscator.Obfuscate("private-trace-token credentials/access-token.txt", options);

        await Assert.That(output).IsEqualTo($"{new SecretMaskingOptions().MaskValue} credentials/access-token.txt");
        await Assert.That(typeof(GcloudOptions).GetProperty(nameof(GcloudOptions.TraceToken))!
            .GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
        await Assert.That(typeof(GcloudOptions).GetProperty(nameof(GcloudOptions.AccessTokenFile))!
            .GetCustomAttribute<SecretValueAttribute>()).IsNull();
    }
}
