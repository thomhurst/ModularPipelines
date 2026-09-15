using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    public async Task Scraped_Optional_Operand_Bundle_Validates_Conditional_Members(bool nested, bool optionalParent)
    {
        var help = """
            NAME
                gcloud example create - create a resource
            SYNOPSIS
                gcloud example create [RESOURCE --parent=PARENT]
            POSITIONAL ARGUMENTS
                 [RESOURCE]
                    The resource name.
            FLAGS
                 --parent=PARENT
                    The parent name.
            """;
        if (optionalParent)
        {
            help = help.Replace("[RESOURCE --parent=PARENT]", "[RESOURCE [--parent=PARENT]]", StringComparison.Ordinal);
        }
        if (nested)
        {
            help = help.Replace("create [RESOURCE --parent=PARENT]", "create ([ROOT] [RESOURCE --parent=PARENT])", StringComparison.Ordinal)
                .Replace("POSITIONAL ARGUMENTS", "POSITIONAL ARGUMENTS\n     [ROOT]\n        Optional root.", StringComparison.Ordinal);
        }
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("example create", help)).Single();
        var group = command.RequiredAlternativeGroups.Single();
        await Assert.That(group.IsRequired).IsFalse();
        await Assert.That(group.IsChoice).IsFalse();
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            for (var mask = 0; mask < 4; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Resource")!.SetValue(instance, (mask & 1) != 0 ? "resource" : null);
                type.GetProperty("Parent")!.SetValue(instance, (mask & 2) != 0 ? "parent" : null);
                var errors = ((IValidatableObject) instance).Validate(new(instance));
                await Assert.That(!errors.Any()).IsEqualTo(mask is 0 or 3 || (optionalParent && mask == 1));
            }
        });
    }

    [Test]
    [Arguments("FLAGS")]
    [Arguments("OPTIONAL FLAGS")]
    public async Task Gcloud_Optional_Resource_Bundle_Preserves_Conditional_Requirements(string section)
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "container-hub-ingress-enable.txt"));
        helpText = helpText.ReplaceLineEndings("\n").Replace("\nFLAGS\n", $"\n{section}\n", StringComparison.Ordinal);
        var command = (await CreateGcloudScraper().Parse(["gcloud", "container", "hub", "ingress", "enable"], helpText))!;
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        var constraint = command.RequiredAlternativeGroups.Single();
        await Assert.That(constraint.IsRequired).IsFalse();
        await Assert.That(constraint.IsChoice).IsFalse();
        await Assert.That(constraint.Members.Single(member => member.OptionSwitch == "--config-membership").IsRequired).IsTrue();

        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            for (var mask = 0; mask < 4; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("ConfigMembership")!.SetValue(instance, (mask & 1) != 0 ? "membership" : null);
                type.GetProperty("Location")!.SetValue(instance, (mask & 2) != 0 ? "location" : null);
                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                await Assert.That(!errors.Any()).IsEqualTo(mask != 2);
            }
        });
    }

    [Test]
    [Arguments("FLAGS")]
    [Arguments("OPTIONAL FLAGS")]
    public async Task Gcloud_Optional_Plain_Bundle_Preserves_Conditional_Negatable_Member(string section)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            {section}
                 Configuration bundle.
                   --[no-]confirm
                      This flag argument must be specified if any of the other arguments in this group are specified.
                   --location
                      Select the location.
            """;
        await VerifyGcloudChoice(helpText, ["Confirm", "NoConfirm", "Location"], mask =>
            (mask & 3) != 3 && ((mask & 4) == 0 || (mask & 3) != 0), requiresOptions: false);
    }

    [Test]
    public async Task Gcloud_Network_Interface_References_Do_Not_Make_Standalone_Flags_Repeatable()
    {
        // Unmodified Google Cloud SDK 550.0.0 help captured on Windows.
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "compute-instances-create-550.txt"));
        var command = await CreateGcloudScraper().Parse(["gcloud", "compute", "instances", "create"], helpText);

        using (Assert.Multiple())
        {
            foreach (var switchName in new[]
                     {
                         "--external-ipv6-address", "--internal-ipv6-address", "--internal-ipv6-prefix-length",
                         "--ipv6-network-tier", "--private-network-ip", "--stack-type",
                     })
            {
                var option = command!.Options.Single(option => option.SwitchName == switchName);
                await Assert.That(option.AcceptsMultipleValues).IsFalse();
                await Assert.That(option.CSharpType).IsEqualTo("string?");
            }

            await Assert.That(command!.Options.Single(option => option.SwitchName == "--network-interface")
                .AcceptsMultipleValues).IsTrue();
        }
    }

    [Test]
    [Arguments("FLAGS")]
    [Arguments("OPTIONAL FLAGS")]
    [Arguments("REQUIRED FLAGS")]
    public async Task Gcloud_Optional_Exclusive_Choices_Validate_Without_Requiring_Options(string section)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            {section}
                 At most one of these can be specified:
                   --first
                      Select the first behavior.
                   --second
                      Select the second behavior.
            """;
        await VerifyGcloudChoice(helpText, ["First", "Second"], mask => mask != 3, requiresOptions: false);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Gcloud_Nested_Choices_Preserve_Outer_Presence_And_Inner_Exclusivity(bool exclusive)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            FLAGS
                 {(exclusive ? "Exactly" : "At least")} one of these must be specified:
                   --token
                      Authenticate with a token.
                   Or exactly one of these must be specified:
                     --profile
                        Select a saved profile.
                     --interactive
                        Sign in interactively.
            """;
        await VerifyGcloudChoice(helpText, ["Token", "Profile", "Interactive"], mask =>
            mask != 0 && (mask & 6) != 6 && (!exclusive || (mask & 1) == 0 || (mask & 6) == 0),
            requiresOptions: true);
    }

    [Test]
    [Arguments("Or resource configuration. This must be specified:")]
    [Arguments("Or configuration bundle. This must be specified:")]
    public async Task Gcloud_Required_Choices_Preserve_Conditional_Bundle_Requirements(string bundleHeading)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            FLAGS
                 At least one of these must be specified:
                   --token
                      Authenticate with a token.
                   {bundleHeading}
                     --account
                        This flag argument must be specified if any of the other arguments in this group are specified.
                     --location
                        Select the location.
            """;
        await VerifyGcloudChoice(helpText, ["Token", "Account", "Location"], mask =>
            mask != 0 && ((mask & 4) == 0 || (mask & 2) != 0), requiresOptions: true);
    }

    [Test]
    public async Task Gcloud_Optional_Choice_Counts_A_Plain_Bundle_As_One_Branch()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example
            OPTIONAL FLAGS
                 At most one of these can be specified:
                   --all
                      Select every extension.
                   Specify exact extensions to copy.
                     --by-id
                        Select extensions by ID.
                     --known
                        Select known extensions.
            """;
        var generated = await VerifyGcloudChoice(helpText, ["All", "ById", "Known"], mask =>
            (mask & 1) == 0 || (mask & 6) == 0, requiresOptions: false);
        await Assert.That(generated).Contains("At most one of All or (ById or Known) may be specified.");
    }

    [Test]
    public async Task Gcloud_Privateca_Optional_Exclusivity_Preserves_The_Selector_Bundle()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "privateca-templates-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "privateca", "templates", "create"], helpText))!;
        var constraint = command.RequiredAlternativeGroups.Single(group =>
            group.PropertyNames.Contains("CopyAllRequestedExtensions"));

        await Assert.That(constraint.IsRequired).IsFalse();
        await Assert.That(constraint.IsMutuallyExclusive).IsTrue();
        await Assert.That(constraint.Members.Single().PropertyName).IsEqualTo("CopyAllRequestedExtensions");
        await Assert.That(constraint.Groups.Single().PropertyNames)
            .IsEquivalentTo(["CopyExtensionsByOid", "CopyKnownExtensions"]);
        await Assert.That(constraint.Groups.Single().IsChoice).IsFalse();
    }

    [Test]
    public async Task Gcloud_Conditional_Bundle_Requires_One_Negatable_Flag_Form()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example
            FLAGS
                 At least one of these must be specified:
                   --token
                      Authenticate with a token.
                   Or configuration bundle. This must be specified:
                     --[no-]confirm
                        This flag argument must be specified if any of the other arguments in this group are specified.
                     --location
                        Select the location.
            """;
        await VerifyGcloudChoice(helpText, ["Token", "Confirm", "NoConfirm", "Location"], mask =>
            mask != 0 && (mask & 6) != 6 && ((mask & 14) == 0 || (mask & 6) != 0), requiresOptions: true);
    }

    [Test]
    public async Task Gcloud_Conditional_Bundle_Requires_Its_Nested_Choice_Only_When_Selected()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example
            FLAGS
                 At least one of these must be specified:
                   --token
                      Authenticate with a token.
                   Or configuration bundle. This must be specified:
                     --extra
                        Enable extra settings.
                     Exactly one of these must be specified:
                       --first
                          Select the first source.
                       --second
                          Select the second source.
            """;
        await VerifyGcloudChoice(helpText, ["Token", "Extra", "First", "Second"], mask =>
            mask != 0 && (mask & 12) != 12 && ((mask & 14) == 0 || (mask & 12) != 0), requiresOptions: true);
    }

    private static async Task<string> VerifyGcloudChoice(
        string helpText, string[] properties, Func<int, bool> isValid, bool requiresOptions)
    {
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var service = string.Join("\n", (await new SubDomainClassGenerator().GenerateAsync(tool)).Select(file => file.Content));
        await Assert.That(service.Contains("GcloudExampleCreateOptions? options = null", StringComparison.Ordinal))
            .IsEqualTo(!requiresOptions);
        await Assert.That(generated).Contains("IValidatableObject");
        await VerifyGeneratedValidation(generated, "GcloudExampleCreateOptions", async type =>
        {
            for (var mask = 0; mask < 1 << properties.Length; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                for (var index = 0; index < properties.Length; index++)
                {
                    type.GetProperty(properties[index])!.SetValue(instance, (mask & (1 << index)) != 0);
                }

                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                await Assert.That(!errors.Any()).IsEqualTo(isValid(mask));
            }
        });
        return generated;
    }

    [Test]
    [Arguments("oauth2-client-credentials-config-id", "The client identifier.", false)]
    [Arguments("security-settings-aws-v4-access-key-id", "The AWS access key ID.", false)]
    [Arguments("proxy-secret-version-id", "The ID of the secret version containing proxy credentials.", false)]
    [Arguments("properties-secret-id", "The ID of the Oracle Cloud Infrastructure vault secret.", false)]
    [Arguments("password", "The password.", true)]
    [Arguments("secret-access-key", "The secret access key.", true)]
    [Arguments("private-key", "Private key material.", true)]
    [Arguments("body", "The value for the secret.", true)]
    public async Task Gcloud_Generation_Masks_Secret_Values_But_Not_Identifiers(
        string switchName, string description, bool expectedSecret)
    {
        var helpText = $"""
            NAME
                gcloud example update - Update an example.

            FLAGS
                --{switchName}=VALUE
                    {description}

            GCLOUD WIDE FLAGS
                --help
            """;
        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var generated = await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command!],
        });

        await Assert.That(command!.Options.Single().IsSecret).IsEqualTo(expectedSecret);
        await Assert.That(generated.Single().Content.Contains("[SecretValue]", StringComparison.Ordinal))
            .IsEqualTo(expectedSecret);
    }

    [Test]
    public async Task Gcloud_Required_Flag_Tables_Do_Not_Declare_Separator_Flags()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "dataproc-clusters-gke-create.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "dataproc", "clusters", "gke", "create"], helpText))!;
        var arguments = command.ArgumentGroups.SelectMany(group => group.FlattenArguments()).ToArray();
        await Assert.That(arguments.All(argument => argument.SwitchName.Any(char.IsLetterOrDigit))).IsTrue();
        await Assert.That(arguments.Where(argument => !argument.IsPositional).Select(argument => argument.SwitchName))
            .IsEquivalentTo(command.Options.Where(option => !option.SwitchName.StartsWith("--no-", StringComparison.Ordinal))
                .Select(option => option.SwitchName));
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    public async Task Gcloud_Required_Presence_Flag_Rejects_False_And_Missing_Values(bool negatable, bool descriptionNegation)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            REQUIRED FLAGS
                 --{(negatable ? "[no-]" : "")}confirm
                    Confirm the operation. {(descriptionNegation ? "Specify --no-confirm to reject it." : string.Empty)}
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        string[] expectedProperties = negatable || descriptionNegation ? ["Confirm", "NoConfirm"] : ["Confirm"];
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(expectedProperties);
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, "GcloudExampleCreateOptions", async type =>
        {
            foreach (var value in new bool?[] { null, false, true })
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Confirm")!.SetValue(instance, value);
                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(value == true);
            }

            if (negatable || descriptionNegation)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("NoConfirm")!.SetValue(instance, true);
                await Assert.That(((IValidatableObject) instance).Validate(new ValidationContext(instance))).IsEmpty();
                type.GetProperty("Confirm")!.SetValue(instance, true);
                await Assert.That(((IValidatableObject) instance).Validate(new ValidationContext(instance))).IsNotEmpty();
            }
        });
    }

    [Test]
    public async Task Gcloud_Required_Value_Option_Accepts_Documented_Negation()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example
            REQUIRED FLAGS
                 --name=NAME
                    Set the name, or use --no-name to clear it.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.RequiredOptions).IsEmpty();
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var name in new string?[] { null, "value" })
            {
                foreach (var negate in new bool?[] { null, false, true })
                {
                    var instance = Activator.CreateInstance(type)!;
                    type.GetProperty("Name")!.SetValue(instance, name);
                    type.GetProperty("NoName")!.SetValue(instance, negate);
                    var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                    await Assert.That(!errors.Any()).IsEqualTo((name is not null) ^ (negate == true));
                }
            }
        });
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Gcloud_Required_Choices_Include_Negated_Flags(bool exclusive)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            REQUIRED FLAGS
                 {(exclusive ? "Exactly" : "At least")} one of these must be specified:
                   --[no-]confirm
                      Confirm the operation.
                   --other
                      Select the other operation.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(["Confirm", "NoConfirm", "Other"]);
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyGeneratedValidation(generated, "GcloudExampleCreateOptions", async type =>
        {
            for (var mask = 0; mask < 8; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                string[] properties = ["Confirm", "NoConfirm", "Other"];
                var selected = 0;
                for (var index = 0; index < properties.Length; index++)
                {
                    var enabled = (mask & (1 << index)) != 0;
                    type.GetProperty(properties[index])!.SetValue(instance, enabled);
                    selected += enabled ? 1 : 0;
                }

                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                await Assert.That(!errors.Any()).IsEqualTo((mask & 3) != 3 && (exclusive ? selected == 1 : selected > 0));
            }
        });
    }

    [Test]
    [Arguments("Exactly one of these must be specified. Or choose the other source:", true)]
    [Arguments("At least one of these must be specified. Or combine both sources:", false)]
    public async Task Gcloud_Required_Cardinality_Precedes_Alternative_Prose(string heading, bool exclusive)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            REQUIRED FLAGS
                 {heading}
                   --file=FILE
                      Input file.
                   --directory=DIRECTORY
                      Input directory.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames).IsEquivalentTo(["File", "Directory"]);
        await Assert.That(command.RequiredAlternativeGroups.Single().IsMutuallyExclusive).IsEqualTo(exclusive);
    }

    [Test]
    [Arguments("At most one of these can be specified:")]
    [Arguments("At least one of these must be specified:")]
    public async Task Gcloud_Conditional_Branches_Do_Not_Require_Their_Nested_Flags(string heading)
    {
        var helpText = $"""
            NAME
                gcloud example create - create an example
            REQUIRED FLAGS
                 {heading}
                   --token=TOKEN
                      Authenticate with a token.
                   Or at least one of these must be specified:
                     --profile=PROFILE
                        Select a saved profile.
                     --interactive
                        Sign in interactively.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.Options.Count).IsEqualTo(3);
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(["Token", "Profile", "Interactive"]);
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        })).Single().Content;
        await VerifyGeneratedValidation(generated, "GcloudExampleCreateOptions", async type =>
        {
            for (var mask = 0; mask < 8; mask++)
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Token")!.SetValue(instance, (mask & 1) != 0 ? "token" : " ");
                type.GetProperty("Profile")!.SetValue(instance, (mask & 2) != 0 ? "profile" : "");
                type.GetProperty("Interactive")!.SetValue(instance, (mask & 4) != 0);
                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance));
                var valid = heading.StartsWith("At most", StringComparison.Ordinal)
                    ? (mask & 1) == 0 || (mask & 6) == 0
                    : mask != 0;
                await Assert.That(!errors.Any()).IsEqualTo(valid);
            }
        });
    }

    [Test]
    public async Task Gcloud_Unsplit_Flags_Preserve_Required_Choices_Without_Requiring_Other_Options()
    {
        const string helpText = """
            NAME
                gcloud artifacts files upload - upload a file
            FLAGS
                 --async
                    Run asynchronously.
                 Exactly one of these must be specified:
                   --source=SOURCE
                      Input file.
                   --source-directory=SOURCE_DIRECTORY
                      Input directory.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "artifacts", "files", "upload"], helpText))!;
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(["Source", "SourceDirectory"]);
        await Assert.That(command.RequiredAlternativeGroups.Single().IsMutuallyExclusive).IsTrue();
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await VerifyUploadValidation(generated);
    }

    [Test]
    [Arguments("REQUIRED FLAGS")]
    [Arguments("FLAGS")]
    public async Task Gcloud_Required_Resource_Requires_Selector_Without_Configurable_Attributes(string sectionHeading)
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "dataproc-clusters-gke-create.txt"));
        helpText = helpText.Replace("REQUIRED FLAGS", sectionHeading, StringComparison.Ordinal);
        var command = (await CreateGcloudScraper().Parse(["gcloud", "dataproc", "clusters", "gke", "create"], helpText))!;
        await Assert.That(command.RequiredOptions.Select(option => option.SwitchName)).Contains("--gke-cluster");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--gke-cluster-location").IsRequired).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--history-server-cluster").IsRequired).IsFalse();
    }

    [Test]
    public async Task Gcloud_Optional_Resource_Selector_Remains_Optional_In_Unsplit_Flags()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "dataproc-clusters-gke-create.txt"));
        helpText = helpText.Replace("REQUIRED FLAGS", "FLAGS", StringComparison.Ordinal)
            .Replace("This must be specified.", string.Empty, StringComparison.Ordinal);
        var command = (await CreateGcloudScraper().Parse(["gcloud", "dataproc", "clusters", "gke", "create"], helpText))!;
        await Assert.That(command.RequiredOptions).IsEmpty();
        await Assert.That(command.RequiredAlternativeGroups.All(group => !group.IsRequired && !group.IsChoice)).IsTrue();
        await Assert.That(command.RequiredAlternativeGroups.SelectMany(group => group.Members)
                .Where(member => member.IsRequired).Select(member => member.OptionSwitch ?? member.PropertyName))
            .IsEquivalentTo(["Cluster", "--gke-cluster", "--history-server-cluster", "--metastore-service"]);
    }

    [Test]
    public async Task Gcloud_Upload_Parses_Required_And_Optional_Flag_Sections()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "artifacts-files-upload.txt"));
        var command = await CreateGcloudScraper().Parse(["gcloud", "artifacts", "files", "upload"], helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName)).IsEquivalentTo(
            ["--source", "--source-directory", "--async", "--file", "--skip-existing", "--location", "--repository"]);
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--async").IsFlag).IsTrue();
        await Assert.That(command.ArgumentGroups.SelectMany(group => group.Groups))
            .Contains(group => group.Kind.HasFlag(CliArgumentGroupKind.AtLeastOne | CliArgumentGroupKind.AtMostOne));
    }

    [Test]
    [Arguments("REQUIRED FLAGS", 1)]
    [Arguments("OPTIONAL FLAGS", 2)]
    public async Task Gcloud_Upload_Requires_Exactly_One_Source_In_Generated_Options_And_Service(string section, int expectedGroups)
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "artifacts-files-upload.txt"));
        helpText = helpText.Replace("REQUIRED FLAGS", section, StringComparison.Ordinal);
        var scraper = new TestGcloudScraper(new UploadHelpExecutor(helpText));
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        var upload = commands.Single(command => command.FullCommand == "gcloud artifacts files upload");
        await Assert.That(upload.RequiredAlternativeGroups).Count().IsEqualTo(expectedGroups);
        var choice = upload.RequiredAlternativeGroups.Single(group => group.IsMutuallyExclusive);
        await Assert.That(choice.PropertyNames).IsEquivalentTo(["Source", "SourceDirectory"]);
        await Assert.That(choice.IsMutuallyExclusive).IsTrue();
        await Assert.That(upload.Options.All(option => !option.IsRequired)).IsTrue();
        await Assert.That(upload.PositionalArguments).IsEmpty();

        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [upload],
        };
        var options = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var service = string.Join("\n", (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Select(file => file.Content));
        await Assert.That(options).Contains(section == "REQUIRED FLAGS"
            ? "Exactly one of Source or SourceDirectory must be specified."
            : "At most one of Source or SourceDirectory may be specified.");
        await Assert.That(service).Contains("GcloudArtifactsFilesUploadOptions options,");
        await Assert.That(service).DoesNotContain("GcloudArtifactsFilesUploadOptions? options = null");
        await VerifyUploadValidation(options);
    }

    private static Task VerifyUploadValidation(string generatedOptions) =>
        VerifyGeneratedValidation(generatedOptions, "GcloudArtifactsFilesUploadOptions", async type =>
        {
            foreach (var (source, directory, valid) in new (string?, string?, bool)[]
            {
                (null, null, false), (" ", "", false), ("file.txt", "directory", false),
                ("file.txt", null, true), (null, "directory", true),
            })
            {
                var instance = Activator.CreateInstance(type)!;
                type.GetProperty("Source")!.SetValue(instance, source);
                type.GetProperty("SourceDirectory")!.SetValue(instance, directory);
                var errors = ((IValidatableObject) instance).Validate(new ValidationContext(instance)).ToArray();
                await Assert.That(errors.Length).IsEqualTo(valid ? 0 : 1);
            }
        });

    private static async Task VerifyGeneratedValidation(string generatedOptions, string typeName, Func<Type, Task> verify)
    {
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            "gcloud-upload-validation",
            [
                CSharpSyntaxTree.ParseText("global using System; global using System.Collections.Generic; global using System.Linq; "
                    + "global using ModularPipelines.Attributes; "
                    + "namespace ModularPipelines.Google.Options { public record GcloudOptions; } "
                    + "namespace ModularPipelines.Secrets { public sealed class SecretValueAttribute : Attribute; } "
                    + "namespace ModularPipelines.Attributes { "
                    + "public enum OptionFormat { EqualsSeparated } "
                    + "public sealed class CliOptionAttribute(string name) : Attribute { public OptionFormat Format { get; set; } } "
                    + "public sealed class CliFlagAttribute(string name) : Attribute; "
                    + "public sealed class CliArgumentAttribute(int position) : Attribute { public CommandLinePhase Phase { get; set; } } "
                    + "public sealed class CliSubCommandAttribute(params string[] parts) : Attribute; }"),
                CSharpSyntaxTree.ParseText(generatedOptions),
            ],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        await Assert.That(result.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(diagnostic => diagnostic.ToString())).IsEmpty();
        stream.Position = 0;
        var loadContext = new AssemblyLoadContext("gcloud-upload-validation", isCollectible: true);
        try
        {
            var type = loadContext.LoadFromStream(stream).GetType($"ModularPipelines.Google.Options.{typeName}")!;
            await verify(type);
        }
        finally
        {
            loadContext.Unload();
        }
    }

    [Test]
    [Arguments("\n")]
    [Arguments("\r\n")]
    public async Task Gcloud_Flag_Sections_Preserve_Requiredness_And_Section_Boundaries(string newLine)
    {
        var helpText = """
            NAME
                gcloud example create - create an example
            POSITIONAL ARGUMENTS
                 INPUT
                    Input identifier.
            REQUIRED FLAGS
                 --name=NAME
                    The name to create.
            OPTIONAL FLAGS
                 Exactly one of these must be specified:
                   --file=FILE
                      The input file.
                   --directory=DIRECTORY
                      The input directory.
            FLAGS
                 --async
                    Run asynchronously.
            GCLOUD WIDE FLAGS
                 --project=PROJECT
            NOTES
                 --example-only=VALUE
            """.ReplaceLineEndings(newLine);
        var command = (await CreateGcloudScraper().Parse(["gcloud", "example", "create"], helpText))!;
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--name", "--file", "--directory", "--async"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--name").IsRequired).IsTrue();
        await Assert.That(command.Options.Where(option => option.SwitchName != "--name").All(option => !option.IsRequired)).IsTrue();
        var optionalChoice = command.RequiredAlternativeGroups.Single();
        await Assert.That(optionalChoice.IsRequired).IsFalse();
        await Assert.That(optionalChoice.IsMutuallyExclusive).IsTrue();
        await Assert.That(optionalChoice.PropertyNames).IsEquivalentTo(["File", "Directory"]);
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName)).IsEquivalentTo(["Input"]);
    }

    [Test]
    public async Task SharedParser_Recovers_Nested_Alternatives_And_Documentation()
    {
        const string section = """
              --mode=MODE
                 Select the operating mode.

                 Shorthand Example:
                   --example-only=literal

              At most one of these can be specified:
                --token=TOKEN
                   Authenticate with a token.

                Or at least one of these can be specified:
                  --profile=PROFILE
                     Select a saved profile.
                  --[no-]interactive
                     Enable or disable prompts.
            """;

        var group = TestArgumentGroupScraper.ParseGroups(section);
        var arguments = group.FlattenArguments().ToArray();

        await Assert.That(string.Join(" ", arguments.Select(argument => argument.SwitchName)))
            .IsEqualTo("--mode --token --profile --interactive");
        await Assert.That(group.Groups.Single().Kind)
            .IsEqualTo(CliArgumentGroupKind.AtMostOne);
        await Assert.That(group.Groups.Single().Groups.Single().Kind)
            .IsEqualTo(CliArgumentGroupKind.Alternative | CliArgumentGroupKind.AtLeastOne);
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--profile").Documentation)
            .Contains("At most one of these can be specified")
            .And.Contains("Or at least one of these can be specified")
            .And.Contains("Select a saved profile");
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--interactive").IsNegatable)
            .IsTrue();
    }

    [Test]
    public async Task SharedValidation_Rejects_Grouped_Declaration_Not_Emitted_As_Option()
    {
        const string helpText = """
            Fake help.

            FLAGS
              --parent=PARENT
                 Parent value.

              At most one of these can be specified:
                --nested=NESTED
                   Nested value.
            """;
        var scraper = new OmittingArgumentScraper(helpText);
        var commands = new List<CliCommandDefinition>();

        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }

        await Assert.That(commands).IsEmpty();
    }

    [Test]
    public async Task SharedParser_Preserves_Undocumented_Sibling_And_Final_Declarations()
    {
        const string section = """
              --quiet
              --verbosity=VERBOSITY
                 Override the default verbosity.
              --recursive
            """;

        var arguments = TestArgumentGroupScraper.ParseGroups(section)
            .FlattenArguments()
            .ToArray();

        await Assert.That(arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--quiet", "--verbosity", "--recursive"]);
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--quiet").Description)
            .IsNull();
        await Assert.That(arguments.Single(argument => argument.SwitchName == "--recursive").Description)
            .IsNull();
    }

    [Test]
    public async Task SharedParser_Scopes_Named_Flag_Sections()
    {
        const string section = """
            Customer-Managed Encryption Key (CMEK) Flags:
              The following flags apply to a single container.
              --container=CONTAINER
                 Select a container.

            Service Flags
              The following flags apply to the service.
              --allow-unauthenticated
                 Allow public access.
            """;

        var arguments = TestArgumentGroupScraper.ParseGroups(section)
            .FlattenArguments()
            .ToArray();
        var container = arguments.Single(argument => argument.SwitchName == "--container");
        var service = arguments.Single(argument => argument.SwitchName == "--allow-unauthenticated");

        using (Assert.Multiple())
        {
            await Assert.That(container.Documentation)
                .Contains("Customer-Managed Encryption Key (CMEK) Flags:")
                .And.DoesNotContain("Service Flags");
            await Assert.That(service.Documentation)
                .Contains("Service Flags")
                .And.DoesNotContain("Customer-Managed Encryption Key (CMEK) Flags:");
        }
    }

    [Test]
    public async Task Gcloud_Emits_Long_Options_With_Short_Aliases()
    {
        const string helpText = """
            NAME
                gcloud compute instances list - list instances

            SYNOPSIS
                gcloud compute instances list

            FLAGS
                 --zone=ZONE, -z ZONE
                    Zone to list.
                 --tag=TAG, -t TAG

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "list"],
            helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--zone", "--tag"]);
    }

    [Test]
    public async Task Gcloud_Does_Not_Inherit_Previous_Option_Documentation_Into_Sibling_Groups()
    {
        const string helpText = """
            NAME
                gcloud functions deploy - deploy a function

            SYNOPSIS
                gcloud functions deploy

            FLAGS
                 --security-level=SECURITY_LEVEL; default="secure-always"
                    SECURITY_LEVEL must be one of: secure-always, secure-optional.

                 At most one of these can be specified:

                   --clear-max-instances
                      Clear the maximum instances setting.

                   --max-instances=MAX_INSTANCES
                      Set the maximum number of instances.

                 At most one of these can be specified:

                   --clear-min-instances
                      Clear the minimum instances setting.

                   --min-instances=MIN_INSTANCES
                      Set the minimum number of instances.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "functions", "deploy"],
            helpText);
        var maxInstances = command!.Options.Single(option => option.SwitchName == "--max-instances");
        var minInstances = command.Options.Single(option => option.SwitchName == "--min-instances");

        await Assert.That(maxInstances.Description!).DoesNotContain("--security-level");
        await Assert.That(maxInstances.EnumDefinition).IsNull();
        await Assert.That(minInstances.Description!).DoesNotContain("--security-level");
        await Assert.That(minInstances.EnumDefinition).IsNull();
    }

    [Test]
    public async Task Gcloud_Measures_Declaration_Indentation_With_Tab_Stops()
    {
        // Declarations must use the same tab-aware column units as the parser's line
        // comparisons; a raw character count makes a tab-indented sibling look nested.
        var helpText = """
            NAME
                gcloud example deploy - deploy an example

            SYNOPSIS
                gcloud example deploy

            FLAGS
                 --mode=MODE
                    Select the operating mode.

                 At most one of these can be specified:

            {TAB}--token=TOKEN
            {TAB}   Authenticate with a token.

            {TAB}--profile=PROFILE
            {TAB}   Select a saved profile.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """.Replace("{TAB}", "\t", StringComparison.Ordinal);

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "deploy"], helpText);

        var root = command!.ArgumentGroups.Single();
        var nested = root.Groups.Single();
        using (Assert.Multiple())
        {
            await Assert.That(command.Options.Select(option => option.SwitchName))
                .IsEquivalentTo(["--mode", "--token", "--profile"]);
            await Assert.That(root.Arguments.Select(argument => argument.SwitchName))
                .IsEquivalentTo(["--mode"]);
            await Assert.That(nested.Kind).IsEqualTo(CliArgumentGroupKind.AtMostOne);
            await Assert.That(nested.Arguments.Select(argument => argument.SwitchName))
                .IsEquivalentTo(["--token", "--profile"]);
        }
    }

    [Test]
    public async Task Gcloud_Does_Not_Leak_Named_Flag_Sections()
    {
        const string helpText = """
            NAME
                gcloud run deploy - deploy a service

            SYNOPSIS
                gcloud run deploy

            FLAGS
                 Container Flags
                   The following flags apply to a single container.
                   --container=CONTAINER
                      Select a container.

                 Service Flags
                   The following flags apply to the service.
                   --allow-unauthenticated
                      Allow public access.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "run", "deploy"],
            helpText);
        var container = command!.Options.Single(option => option.SwitchName == "--container");
        var service = command.Options.Single(option => option.SwitchName == "--allow-unauthenticated");

        using (Assert.Multiple())
        {
            await Assert.That(container.Description)
                .Contains("Container Flags")
                .And.DoesNotContain("Service Flags");
            await Assert.That(service.Description)
                .Contains("Service Flags")
                .And.DoesNotContain("Container Flags");
        }
    }

    [Test]
    public async Task Gcloud_Parses_Spaced_Value_Hints_Without_Leaking_Documentation()
    {
        const string helpText = """
            NAME
                gcloud compute instances create-with-container - create an instance

            SYNOPSIS
                gcloud compute instances create-with-container

            FLAGS
                 --container-env=[KEY=VALUE, ...,...]
                    Declare environment variables. KEY can be repeated more than once.

                 --machine-type=MACHINE_TYPE
                    Specifies the machine type.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "create-with-container"],
            helpText);
        var containerEnvironment = command!.Options.Single(option =>
            option.SwitchName == "--container-env");
        var machineType = command.Options.Single(option =>
            option.SwitchName == "--machine-type");

        using (Assert.Multiple())
        {
            await Assert.That(containerEnvironment.Description)
                .Contains("Declare environment variables");
            await Assert.That(containerEnvironment.IsKeyValue).IsTrue();
            await Assert.That(containerEnvironment.CSharpType)
                .IsEqualTo("IReadOnlyList<KeyValue>?");
            await Assert.That(machineType.Description)
                .IsEqualTo("Specifies the machine type.");
            await Assert.That(machineType.AcceptsMultipleValues).IsFalse();
            await Assert.That(machineType.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gcloud_Emits_Both_Forms_Of_Negatable_Flags()
    {
        const string helpText = """
            NAME
                gcloud run deploy - deploy a service

            SYNOPSIS
                gcloud run deploy

            FLAGS
                 --[no-]allow-unauthenticated
                    Use --allow-unauthenticated to enable and
                    --no-allow-unauthenticated to disable.

                 --launch-browser
                    Enabled by default, use --no-launch-browser to disable.

                 --use
                    This does not control --no-use-orchestrator.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "run", "deploy"],
            helpText);

        await Assert.That(command!.Options.Select(option => option.SwitchName))
            .IsEquivalentTo([
                "--allow-unauthenticated",
                "--no-allow-unauthenticated",
                "--launch-browser",
                "--no-launch-browser",
                "--use",
            ]);
        await Assert.That(command.Options.Select(option => option.PropertyName))
            .IsEquivalentTo([
                "AllowUnauthenticated",
                "NoAllowUnauthenticated",
                "LaunchBrowser",
                "NoLaunchBrowser",
                "Use",
            ]);
    }

    [Test]
    public async Task Gcloud_Description_Negation_Of_Value_Option_Is_A_Flag()
    {
        const string helpText = """
            NAME
                gcloud compute disks update - update a disk

            SYNOPSIS
                gcloud compute disks update

            FLAGS
                 --boot-disk-size=SIZE
                    Set the boot disk size, or use --no-boot-disk-size to unset it.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "disks", "update"],
            helpText);
        var positive = command!.Options.Single(option => option.SwitchName == "--boot-disk-size");
        var negative = command.Options.Single(option => option.SwitchName == "--no-boot-disk-size");

        using (Assert.Multiple())
        {
            await Assert.That(positive.IsFlag).IsFalse();
            await Assert.That(negative.PropertyName).IsEqualTo("NoBootDiskSize");
            await Assert.That(negative.CSharpType).IsEqualTo("bool?");
            await Assert.That(negative.IsFlag).IsTrue();
            await Assert.That(negative.ValueSeparator).IsEqualTo(" ");
            await Assert.That(negative.IsNumeric).IsFalse();
        }
    }

    [Test]
    public async Task Gcloud_Numeric_Hints_Require_Whole_Tokens()
    {
        const string helpText = """
            NAME
                gcloud functions upgrade - upgrade a function

            SYNOPSIS
                gcloud functions upgrade

            FLAGS
                 --trigger-service-account=TRIGGER_SERVICE_ACCOUNT
                    IAM service-account email address.
                 --project=PROJECT_ID_OR_NUMBER
                    Project associated with the custom module.
                 --project-number=PROJECT_NUMBER
                    Numeric project number.
                 --retry-count=RETRY_COUNT
                    Number of retries.
                 --disk-size=DISK_SIZE
                    Disk size.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "functions", "upgrade"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--trigger-service-account").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--project").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--project-number").CSharpType)
                .IsEqualTo("int?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--retry-count").CSharpType)
                .IsEqualTo("int?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--disk-size").CSharpType)
                .IsEqualTo("int?");
        }
    }

    [Test]
    public async Task Gcloud_Ingestion_Service_Accounts_Are_Textual()
    {
        // Verbatim FLAGS excerpt from Google Cloud SDK 550.0.0:
        // gcloud pubsub topics update --help
        const string helpText = """
            NAME
                gcloud pubsub topics update - update a topic

            SYNOPSIS
                gcloud pubsub topics update

            FLAGS
                 --aws-msk-ingestion-service-account=AWS_MSK_INGESTION_SERVICE_ACCOUNT
                    Google Cloud service account to be used for Federated Identity
                    authentication with MSK.
                 --azure-event-hubs-ingestion-service-account=AZURE_EVENT_HUBS_INGESTION_SERVICE_ACCOUNT
                    Google Cloud service account to be used for Federated Identity
                    authentication with Azure Event Hubs.
                 --confluent-cloud-ingestion-service-account=CONFLUENT_CLOUD_INGESTION_SERVICE_ACCOUNT
                    Google Cloud service account to be used for Federated Identity
                    authentication with Confluent Cloud.
                 --kinesis-ingestion-service-account=KINESIS_INGESTION_SERVICE_ACCOUNT
                    Google Cloud service account to be used for Federated Identity
                    authentication with Kinesis.
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "pubsub", "topics", "update"],
            helpText);

        var serviceAccountOptions = command!.Options
            .Where(option => option.SwitchName.EndsWith(
                "-ingestion-service-account",
                StringComparison.Ordinal))
            .ToArray();
        using (Assert.Multiple())
        {
            await Assert.That(serviceAccountOptions).Count().IsEqualTo(4);
            await Assert.That(serviceAccountOptions.Select(option => option.CSharpType))
                .IsEquivalentTo(["string?", "string?", "string?", "string?"]);
        }
    }

    [Test]
    public async Task Gcloud_Service_Account_Stays_Scalar_When_Group_Description_Mentions_Repetition()
    {
        const string helpText = """
            NAME
                gcloud compute instances create-with-container - create an instance

            SYNOPSIS
                gcloud compute instances create-with-container INSTANCE_NAMES

            FLAGS
                 --service-account=SERVICE_ACCOUNT
                    Values for a sibling option may be repeated more than once. A service
                    account can be set using its email address.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "create-with-container"],
            helpText);
        var serviceAccount = command!.Options.Single(option =>
            option.SwitchName == "--service-account");

        using (Assert.Multiple())
        {
            await Assert.That(serviceAccount.CSharpType).IsEqualTo("string?");
            await Assert.That(serviceAccount.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    public async Task Gcloud_File_Hints_Take_Precedence_Over_Numeric_Tokens()
    {
        const string helpText = """
            NAME
                gcloud storage insights dataset-configs update - update a dataset config

            SYNOPSIS
                gcloud storage insights dataset-configs update

            FLAGS
                 --source-folders=FOLDER_NUMBERS,...
                    List of source folder IDs.
                 --source-folders-file=FOLDER_NUMBERS_FILE
                    CSV formatted file containing source folder IDs, one per line.
                 --source-projects=PROJECT_NUMBERS,...
                    List of source project numbers.
                 --source-projects-file=PROJECT_NUMBERS_FILE
                    CSV formatted file containing source project numbers, one per line.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "storage", "insights", "dataset-configs", "update"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--source-folders").CSharpType)
                .IsEqualTo("IEnumerable<int>?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-folders-file").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-projects").CSharpType)
                .IsEqualTo("IEnumerable<int>?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--source-projects-file").CSharpType)
                .IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gcloud_Structured_And_Categorical_Values_Remain_Textual()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --actions=ACTIONS
                    Shorthand Example: --actions=actionId=string. File Example: --actions=path_to_file.yaml.
                 --build-service-account=BUILD_SERVICE_ACCOUNT
                    Must be of the format projects/${PROJECT_ID}/serviceAccounts/${ACCOUNT_EMAIL_ADDRESS}.
                 --instance-size=INSTANCE_SIZE
                    INSTANCE_SIZE must be one of: extra-small Extra small instance size, maps to 0.1. small Small instance size, maps to 0.5.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);

        using (Assert.Multiple())
        {
            foreach (var option in command!.Options)
            {
                await Assert.That(option.CSharpType).IsEqualTo("string?");
            }
        }
    }

    [Test]
    public async Task Gcloud_Yaml_Examples_Remain_Structured()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --configuration=COUNT
                    YAML Example: count: 1

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);
        var configuration = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(configuration.CSharpType).IsEqualTo("string?");
            await Assert.That(configuration.IsNumeric).IsFalse();
            await Assert.That(configuration.EnumDefinition).IsNull();
        }
    }

    [Test]
    public async Task Gcloud_Bms_Update_Uses_Current_Structured_Value_Type()
    {
        const string helpText = """
            NAME
                gcloud bms nfs-shares update - update an NFS share

            SYNOPSIS
                gcloud bms nfs-shares update

            FLAGS
                 --update-labels=[KEY=VALUE,...]
                    List of label KEY=VALUE pairs to update.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "bms", "nfs-shares", "update"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.PropertyName).IsEqualTo("UpdateLabels");
            await Assert.That(option.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
        }
    }

    [Test]
    public async Task Gcloud_Structured_Values_Preserve_Repeatability_Without_Nested_Enums()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --add-allowed-client=[PROPERTY=VALUE,...]
                    This flag can be repeated to specify multiple allowed clients. mount-permissions The mount permissions. MOUNT_PERMISSIONS must be one of: READ_ONLY, READ_WRITE.
                 --enabled-tool=[accountConnector=ACCOUNTCONNECTOR],[config=CONFIG],[handle=HANDLE]
                    Shorthand Example: --enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string --enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string JSON Example: --enabled-tool='[{"accountConnector":"string"}]' File Example: --enabled-tool=path_to_file.(yaml|json)
                 --add-enabled-tool=[accountConnector=ACCOUNTCONNECTOR],[config=CONFIG],[handle=HANDLE]
                    Shorthand Example: --add-enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string --add-enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string JSON Example: --add-enabled-tool='[{"accountConnector":"string"}]' File Example: --add-enabled-tool=path_to_file.(yaml|json)
                 --remove-enabled-tool=[accountConnector=ACCOUNTCONNECTOR],[config=CONFIG],[handle=HANDLE]
                    Shorthand Example: --remove-enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string --remove-enabled-tool=accountConnector=string,config=[{key=string,value=string}],handle=string JSON Example: --remove-enabled-tool='[{"accountConnector":"string"}]' File Example: --remove-enabled-tool=path_to_file.(yaml|json)
                 --labels=[LABELS,...]
                    Labels as key value pairs. Shorthand Example: --labels=string=string JSON Example: --labels='{"string":"string"}' File Example: --labels=path_to_file.(yaml|json)

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);

        using (Assert.Multiple())
        {
            foreach (var switchName in new[]
                     {
                         "--add-allowed-client",
                         "--enabled-tool",
                         "--add-enabled-tool",
                         "--remove-enabled-tool",
                         "--labels",
                     })
            {
                var option = command!.Options.Single(candidate => candidate.SwitchName == switchName);
                await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
                await Assert.That(option.AcceptsMultipleValues).IsTrue();
                await Assert.That(option.EnumDefinition).IsNull();
            }
        }
    }

    [Test]
    public async Task Gcloud_Models_Declared_Value_Lists_As_Collections()
    {
        const string helpText = """
            NAME
                gcloud storage diagnose - diagnose storage performance

            SYNOPSIS
                gcloud storage diagnose

            FLAGS
                 --object-sizes=SIZES
                    List of object sizes to use for the tests. Sizes should be provided for each object specified using --object-count flag.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "storage", "diagnose"],
            helpText);
        var objectSizes = command!.Options.Single(option => option.SwitchName == "--object-sizes");

        await Assert.That(objectSizes.AcceptsMultipleValues).IsTrue();
        await Assert.That(objectSizes.CSharpType).IsEqualTo("IEnumerable<int>?");
    }

    [Test]
    public async Task Gcloud_Keeps_Identifiers_And_Composite_Values_Textual()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --billing-account=BILLING_ACCOUNT
                    Billing account of the resource.
                 --oauth-service-account-email=OAUTH_SERVICE_ACCOUNT_EMAIL
                    IAM service-account email address.
                 --lint-response-summary=COUNT
                    Shorthand Example: --lint-response-summary=count=int,severity=string. JSON Example: --lint-response-summary='[{"count": int}]'. File Example: --lint-response-summary=path_to_file.json.
                 --autoprovisioning-standard-rollout-policy=[BATCH_NODE_COUNT=...,...]
                    Standard rollout policy options for blue-green upgrade.
                 --max-accelerator=type=TYPE,count=COUNT
                    Sets the accelerator type and count.
                 --composite-application-parameters-service-account-map=SERVICE_ACCOUNT_MAP
                    Shorthand Example: --composite-application-parameters-service-account-map=string=string. JSON Example: --composite-application-parameters-service-account-map='{"string":"string"}'.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option =>
                option.SwitchName == "--billing-account").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--oauth-service-account-email").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--lint-response-summary").CSharpType)
                .IsEqualTo("string?");
            var rolloutPolicy = command.Options.Single(option =>
                option.SwitchName == "--autoprovisioning-standard-rollout-policy");
            await Assert.That(rolloutPolicy.CSharpType).IsEqualTo("string?");
            await Assert.That(rolloutPolicy.AcceptsMultipleValues).IsFalse();
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--max-accelerator").CSharpType)
                .IsEqualTo("string?");
            await Assert.That(command.Options.Single(option =>
                option.SwitchName == "--composite-application-parameters-service-account-map").CSharpType)
                .IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Gcloud_Models_Repeatable_Options_As_Collections()
    {
        const string helpText = """
            NAME
                gcloud asset search-all-resources - search all resources

            SYNOPSIS
                gcloud asset search-all-resources

            FLAGS
                 --order-by=FIELD
                    This flag can be repeated to provide a list of fields.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "asset", "search-all-resources"],
            helpText);
        var orderBy = command!.Options.Single(option => option.SwitchName == "--order-by");

        await Assert.That(orderBy.AcceptsMultipleValues).IsTrue();
        await Assert.That(orderBy.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Gcloud_Uses_Whole_Option_Block_For_Repeatability()
    {
        const string helpText = """
            NAME
                gcloud compute instances create - create an instance

            SYNOPSIS
                gcloud compute instances create

            FLAGS
                 --ipv6-public-ptr-domain=DOMAIN
                    Domain for the public PTR record.
                 This flag can be repeated to configure multiple domains.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "instances", "create"],
            helpText);
        var option = command!.Options.Single(candidate =>
            candidate.SwitchName == "--ipv6-public-ptr-domain");

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
    }

    [Test]
    public async Task Gcloud_Repeatable_Key_Value_Option_Keeps_Key_Value_Type()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --metadata=KEY=VALUE
                    This flag can be repeated to add metadata entries.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);
        var metadata = command!.Options.Single(option => option.SwitchName == "--metadata");

        await Assert.That(metadata.AcceptsMultipleValues).IsTrue();
        await Assert.That(metadata.CSharpType).IsEqualTo("IReadOnlyList<KeyValue>?");
    }

    [Test]
    public async Task Gcloud_External_Ipv6_Prefix_Length_Is_Known_Scalar()
    {
        var scraper = CreateGcloudScraper();

        await Assert.That(scraper.IsKnownScalar(
                ["compute", "instances", "create"],
                "--external-ipv6-prefix-length"))
            .IsTrue();
    }

    [Test]
    public async Task Gcloud_Prioritizes_Enum_Types_Over_Key_Value_Hints()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --labels=KEY=VALUE,...
                    Value must be one of: alpha, beta.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "update"],
            helpText);
        var labels = command!.Options.Single(option => option.SwitchName == "--labels");

        await Assert.That(labels.CSharpType).IsEqualTo("IEnumerable<GcloudLabels>?");
        await Assert.That(labels.AcceptsMultipleValues).IsTrue();
        await Assert.That(labels.EnumDefinition).IsNotNull();
    }

    [Test]
    public async Task Gcloud_Does_Not_Mask_Secret_Named_File_Path_Options()
    {
        const string helpText = """
            NAME
                gcloud example create - create an example

            SYNOPSIS
                gcloud example create

            FLAGS
                 --credential=CREDENTIAL
                    Path to the credential file.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "example", "create"],
            helpText);
        var credential = command!.Options.Single(option => option.SwitchName == "--credential");

        await Assert.That(credential.IsSecret).IsFalse();
    }

    [Test]
    public async Task GcloudAgentIdentityUpdate_Emits_Nested_Scope_Credential_And_Negatable_Flags()
    {
        const string helpText = """
            NAME
                gcloud agent-identity auth-providers update - update an auth provider

            SYNOPSIS
                gcloud agent-identity auth-providers update

            FLAGS
                 --request-id=REQUEST_ID
                    An optional request ID.

                 Update allowed_scopes.

                 At most one of these can be specified:
                   --allowed-scopes=[ALLOWED_SCOPES,...]
                      Set allowed_scopes to a new value.

                   Or at least one of these can be specified:
                     --add-allowed-scopes=[ADD_ALLOWED_SCOPES,...]
                        Add values to allowed_scopes.

                     At most one of these can be specified:
                       --clear-allowed-scopes
                          Clear allowed_scopes.
                       --remove-allowed-scopes=[REMOVE_ALLOWED_SCOPES,...]
                          Remove values from allowed_scopes.

                 AuthProvider type specific parameters.

                   --clear-auth-provider-type-params
                      Reset type parameters.

                   At most one of these can be specified:
                     --api-key=API_KEY
                        The API key for this auth provider.
                     --three-legged-oauth-client-secret=THREE_LEGGED_OAUTH_CLIENT_SECRET
                        The OAuth client secret.
                     --[no-]three-legged-oauth-enable-pkce
                        Enable or disable PKCE.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;
        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "agent-identity", "auth-providers", "update"],
            helpText);
        var switches = command!.Options.Select(option => option.SwitchName).ToArray();

        await Assert.That(switches).Contains("--allowed-scopes");
        await Assert.That(switches).Contains("--add-allowed-scopes");
        await Assert.That(switches).Contains("--clear-allowed-scopes");
        await Assert.That(switches).Contains("--remove-allowed-scopes");
        await Assert.That(switches).Contains("--api-key");
        await Assert.That(switches).Contains("--three-legged-oauth-client-secret");
        await Assert.That(switches).Contains("--three-legged-oauth-enable-pkce");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--request-id").Description!)
            .DoesNotContain("--allowed-scopes");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--three-legged-oauth-client-secret").IsSecret)
            .IsTrue();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--three-legged-oauth-enable-pkce").IsFlag)
            .IsTrue();
    }

    [Test]
    public async Task GcloudDeveloperConnectCreate_Emits_Nested_Resource_And_Secret_Flags()
    {
        const string helpText = """
            NAME
                gcloud developer-connect connections create - create a connection resource

            SYNOPSIS
                gcloud developer-connect connections create CONNECTION

            FLAGS
                 --validate-only
                    If set, validate the request without posting it.

                 The crypto key configuration. The arguments in this resource group
                 can specify its attributes.

                   --crypto-key-config-reference=CRYPTO_KEY_CONFIG_REFERENCE
                      ID of the cryptoKey.
                   --key-ring=KEY_RING
                      The keyRing ID.

                 Arguments for the connection config.

                 At most one of these can be specified:
                   Configuration for Bitbucket Cloud.

                     --bitbucket-cloud-config-authorizer-credential-user-token-secret-version=BITBUCKET_SECRET_VERSION
                        SecretManager version containing the user token.
                     --bitbucket-cloud-config-workspace=BITBUCKET_WORKSPACE
                        The workspace ID.

                   Configuration for an HTTP provider.

                     Arguments for the authentication.

                     At most one of these can be specified:
                       --http-config-bearer-token-authentication-secret-version=HTTP_BEARER_SECRET_VERSION
                          SecretManager version containing the bearer token.
                       --http-config-basic-authentication-password-secret-version=HTTP_PASSWORD_SECRET_VERSION
                          SecretManager version containing the password.

            GCLOUD WIDE FLAGS
                 --project=PROJECT_ID
            """;
        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "developer-connect", "connections", "create"],
            helpText);
        var switches = command!.Options.Select(option => option.SwitchName).ToArray();

        await Assert.That(switches).Contains("--crypto-key-config-reference");
        await Assert.That(switches).Contains("--key-ring");
        await Assert.That(switches)
            .Contains("--bitbucket-cloud-config-authorizer-credential-user-token-secret-version");
        await Assert.That(switches)
            .Contains("--http-config-bearer-token-authentication-secret-version");
        await Assert.That(switches)
            .Contains("--http-config-basic-authentication-password-secret-version");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--validate-only").Description!)
            .DoesNotContain("--crypto-key-config-reference");
        await Assert.That(command.Options
                .Single(option => option.SwitchName == "--http-config-basic-authentication-password-secret-version")
                .IsSecret)
            .IsTrue();
        await Assert.That(command.ArgumentGroups.Single().Groups)
            .Contains(group => group.Kind.HasFlag(CliArgumentGroupKind.Resource));
    }

    private static TestGcloudScraper CreateGcloudScraper() =>
        new(new EmptyExecutor());

    private sealed class TestGcloudScraper(ICliCommandExecutor executor)
        : GcloudCliScraper(
            executor,
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<GcloudCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);

        public bool IsKnownScalar(IReadOnlyList<string> commandParts, string switchName) =>
            ShouldTreatOptionAsScalar(commandParts, switchName);
    }

    private sealed class TestArgumentGroupScraper : CliScraperBase
    {
        private TestArgumentGroupScraper()
            : base(
                new EmptyExecutor(),
                new HelpTextCache(NullLogger<HelpTextCache>.Instance),
                NullLogger<TestArgumentGroupScraper>.Instance)
        {
        }

        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        public static CliArgumentGroup ParseGroups(string section) =>
            ParseArgumentGroups(section, ParseArgument);

        protected override IEnumerable<string> ExtractSubcommands(string helpText) => [];

        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string helpText,
            CancellationToken cancellationToken) =>
            Task.FromResult<CliCommandDefinition?>(null);

        private static CliArgumentDefinition? ParseArgument(string line)
        {
            var match = ArgumentPattern().Match(line);
            if (!match.Success)
            {
                return null;
            }

            var negatable = match.Groups["negatable"].Success;
            return new CliArgumentDefinition
            {
                SwitchName = negatable
                    ? $"--{match.Groups["negatableName"].Value}"
                    : match.Groups["long"].Value,
                ValueHint = match.Groups["value"].Value,
                IsNegatable = negatable,
                Indentation = GetIndentation(match.Groups["indent"].Value),
            };
        }
    }

    private sealed class OmittingArgumentScraper(string helpText) : TestArgumentGroupScraperBase(helpText)
    {
        protected override Task<CliCommandDefinition?> ParseCommandAsync(
            string[] commandPath,
            string commandHelpText,
            CancellationToken cancellationToken)
        {
            var group = ParseArgumentGroups(
                commandHelpText[(commandHelpText.IndexOf("FLAGS", StringComparison.Ordinal) + "FLAGS".Length)..],
                ParseNeutralArgument);
            var parent = group.FlattenArguments().Single(argument => argument.SwitchName == "--parent");

            return Task.FromResult<CliCommandDefinition?>(new CliCommandDefinition
            {
                FullCommand = "fake",
                CommandParts = [],
                ClassName = "FakeOptions",
                ParentClassName = "FakeOptions",
                ToolNamespacePrefix = "Fake",
                Options =
                [
                    new CliOptionDefinition
                    {
                        SwitchName = parent.SwitchName,
                        PropertyName = "Parent",
                        CSharpType = "string?",
                        Description = parent.Documentation,
                    },
                ],
                ArgumentGroups = [group],
            });
        }
    }

    private abstract class TestArgumentGroupScraperBase(string helpText) : CliScraperBase(
        new SingleHelpExecutor(helpText),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<TestArgumentGroupScraperBase>.Instance)
    {
        public override string ToolName => "fake";

        public override string NamespacePrefix => "Fake";

        public override string TargetNamespace => "ModularPipelines.Fake";

        public override string OutputDirectory => "src/ModularPipelines.Fake";

        protected override IEnumerable<string> ExtractSubcommands(string commandHelpText) => [];

        protected static CliArgumentDefinition? ParseNeutralArgument(string line)
        {
            var match = ArgumentPattern().Match(line);
            return !match.Success
                ? null
                : new CliArgumentDefinition
                {
                    SwitchName = match.Groups["long"].Value,
                    ValueHint = match.Groups["value"].Value,
                    Indentation = GetIndentation(match.Groups["indent"].Value),
                };
        }
    }

    private sealed class UploadHelpExecutor(string helpText) : EmptyExecutor
    {
        public override Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = arguments switch
                {
                    "--help" => "GROUPS\n     artifacts\n",
                    "artifacts --help" => "GROUPS\n     files\n",
                    "artifacts files --help" => "COMMANDS\n     upload\n",
                    "artifacts files upload --help" => helpText,
                    _ => throw new InvalidOperationException($"Unexpected help request: {arguments}"),
                },
                StandardError = string.Empty,
            });
    }

    private sealed class SingleHelpExecutor(string helpText) : EmptyExecutor
    {
        public override Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = helpText,
                StandardError = string.Empty,
            });
    }

    private class EmptyExecutor : ICliCommandExecutor
    {
        public virtual Task<CliCommandResult> ExecuteAsync(
            string command,
            string arguments,
            CancellationToken cancellationToken = default,
            string? workingDirectory = null) =>
            throw new InvalidOperationException("Execution was not expected.");

        public Task<bool> IsAvailableAsync(
            string command,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }

    [GeneratedRegex(
        @"^(?<indent>\s+)(?:(?<negatable>--\[no-\])(?<negatableName>[\w-]+)|(?<long>--[\w-]+))(?:=(?<value>\S+))?$")]
    private static partial Regex ArgumentPattern();
}
