using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudResourceArgumentTests
{
    [Test]
    [Arguments(" ")]
    [Arguments("=")]
    public async Task Required_Option_Alias_Preserves_The_Following_Resource_Operand(string aliasSeparator)
    {
        var help = $$"""
            NAME
                gcloud example create - create a resource
            SYNOPSIS
                gcloud example create --instance=INSTANCE, -i{{aliasSeparator}}INSTANCE RESOURCE
            POSITIONAL ARGUMENTS
                 RESOURCE
                    The resource name.
            REQUIRED FLAGS
                 --instance=INSTANCE, -i{{aliasSeparator}}INSTANCE
                    The instance name.
            """;
        var command = (await ScrapeFixture("example create", help)).Single();
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("Resource");
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsTrue();
        var option = command.Options.Single();
        await Assert.That(option.SwitchName).IsEqualTo("--instance");
        await Assert.That(option.IsRequired).IsTrue();
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
    }

    [Test]
    [Arguments("\"/\"")]
    [Arguments("\"a ] : | b\"")]
    [Arguments("'a ) : | b'")]
    [Arguments("<Mode.VALUE: 1>")]
    [Arguments("enabled")]
    public async Task Default_Annotations_Do_Not_Change_Optional_Group_Operands(string defaultValue)
    {
        var help = $$"""
            NAME
                gcloud example create - create a resource
            SYNOPSIS
                gcloud example create RESOURCE [--file=FILE : --directory=DIRECTORY; default={{defaultValue}}]
            POSITIONAL ARGUMENTS
                 RESOURCE
                    The resource name.
            FLAGS
                 --file=FILE
                    The input file.
                 --directory=DIRECTORY; default={{defaultValue}}
                    The input directory.
            """;
        var command = (await ScrapeFixture("example create", help)).Single();
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("Resource");
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--file", "--directory"]);
        await Assert.That(command.Options.Any(option => option.IsRequired)).IsFalse();
    }

    [Test]
    public async Task Positional_Section_Groups_Are_Not_Deferred_As_Option_Only_Metadata()
    {
        const string help = """
            SYNOPSIS
                gcloud example create RESOURCE ((--a=A --x=X):--b=B)
            POSITIONAL ARGUMENTS
                 At least one of these must be specified:
                   RESOURCE
                      The resource operand.
                   --a=A
                      The first option.
                   --x=X
                      The second option.
                   --b=B
                      The third option.
            """;
        await Assert.That(() => new TestScraper().Parse(["gcloud", "example", "create"], help))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("unsupported required option-only colon group");
    }

    [Test]
    [Arguments("gcloud-ai-custom-jobs-local-run.txt")]
    [Arguments("gcloud-ai-platform-local-train.txt")]
    [Arguments("gcloud-app-instances-scp.txt")]
    [Arguments("gcloud-artifacts-image-streaming-cache-create.txt")]
    [Arguments("gcloud-artifacts-image-streaming-cache-delete.txt")]
    [Arguments("gcloud-artifacts-image-streaming-cache-describe.txt")]
    [Arguments("gcloud-audit-manager-enrollments-add.txt")]
    [Arguments("gcloud-cloud-shell-scp.txt")]
    [Arguments("gcloud-components-install.txt")]
    [Arguments("gcloud-compute-connect-to-serial-port.txt")]
    [Arguments("gcloud-compute-copy-files.txt")]
    [Arguments("gcloud-compute-network-endpoint-groups-update.txt")]
    [Arguments("gcloud-compute-scp.txt")]
    [Arguments("gcloud-compute-ssh.txt")]
    [Arguments("gcloud-compute-tpus-queued-resources-scp.txt")]
    [Arguments("gcloud-compute-tpus-queued-resources-ssh.txt")]
    [Arguments("gcloud-compute-tpus-tpu-vm-scp.txt")]
    [Arguments("gcloud-compute-tpus-tpu-vm-ssh.txt")]
    [Arguments("gcloud-config-get.txt")]
    [Arguments("gcloud-config-list.txt")]
    [Arguments("gcloud-config-unset.txt")]
    [Arguments("gcloud-container-fleet-policycontroller-enable.txt")]
    [Arguments("gcloud-container-fleet-policycontroller-update.txt")]
    [Arguments("gcloud-container-hub-policycontroller-enable.txt")]
    [Arguments("gcloud-container-hub-policycontroller-update.txt")]
    [Arguments("gcloud-dataplex-metadata-jobs-create.txt")]
    [Arguments("gcloud-dataproc-batches-submit-spark.txt")]
    [Arguments("gcloud-dataproc-jobs-submit-flink.txt")]
    [Arguments("gcloud-dataproc-jobs-submit-hadoop.txt")]
    [Arguments("gcloud-dataproc-jobs-submit-spark.txt")]
    [Arguments("gcloud-dataproc-workflow-templates-add-job-hadoop.txt")]
    [Arguments("gcloud-dataproc-workflow-templates-add-job-spark.txt")]
    [Arguments("gcloud-dns-dns-keys-describe.txt")]
    [Arguments("gcloud-docker.txt")]
    [Arguments("gcloud-iam-service-accounts-keys-create.txt")]
    [Arguments("gcloud-iam-service-accounts-keys-delete.txt")]
    [Arguments("gcloud-iam-service-accounts-sign-blob.txt")]
    [Arguments("gcloud-iam-service-accounts-sign-jwt.txt")]
    [Arguments("gcloud-model-armor-floorsettings-update.txt")]
    [Arguments("gcloud-preview-compute-connect-to-serial-port.txt")]
    [Arguments("gcloud-preview-compute-copy-files.txt")]
    [Arguments("gcloud-preview-compute-network-endpoint-groups-update.txt")]
    [Arguments("gcloud-preview-compute-scp.txt")]
    [Arguments("gcloud-preview-compute-ssh.txt")]
    [Arguments("gcloud-preview-config-get.txt")]
    [Arguments("gcloud-preview-config-list.txt")]
    [Arguments("gcloud-preview-config-unset.txt")]
    [Arguments("gcloud-resource-manager-tags-keys-create.txt")]
    [Arguments("gcloud-resource-manager-tags-values-create.txt")]
    [Arguments("gcloud-services-api-keys-undelete.txt")]
    [Arguments("gcloud-spanner-operations-cancel.txt")]
    [Arguments("gcloud-spanner-operations-describe.txt")]
    [Arguments("gcloud-storage-buckets-relocate.txt")]
    [Arguments("gcloud-transfer-jobs-create.txt")]
    [Arguments("gcloud-transfer-jobs-update.txt")]
    [Arguments("gcloud-dns-record-sets-changes-list.txt")]
    [Arguments("gcloud-dns-record-sets-list.txt")]
    [Arguments("gcloud-dns-record-sets-transaction-abort.txt")]
    [Arguments("gcloud-dns-record-sets-transaction-describe.txt")]
    [Arguments("gcloud-dns-record-sets-transaction-execute.txt")]
    [Arguments("gcloud-dns-record-sets-transaction-start.txt")]
    [Arguments("gcloud-sql-backups-create.txt")]
    [Arguments("gcloud-sql-databases-list.txt")]
    [Arguments("gcloud-sql-operations-list.txt")]
    [Arguments("gcloud-sql-ssl-client-certs-list.txt")]
    [Arguments("gcloud-sql-ssl-entraid-certs-create.txt")]
    [Arguments("gcloud-sql-ssl-entraid-certs-list.txt")]
    [Arguments("gcloud-sql-ssl-entraid-certs-rollback.txt")]
    [Arguments("gcloud-sql-ssl-entraid-certs-rotate.txt")]
    [Arguments("gcloud-sql-ssl-server-ca-certs-create.txt")]
    [Arguments("gcloud-sql-ssl-server-ca-certs-list.txt")]
    [Arguments("gcloud-sql-ssl-server-ca-certs-rollback.txt")]
    [Arguments("gcloud-sql-ssl-server-ca-certs-rotate.txt")]
    [Arguments("gcloud-sql-ssl-server-certs-create.txt")]
    [Arguments("gcloud-sql-ssl-server-certs-list.txt")]
    [Arguments("gcloud-sql-ssl-server-certs-rollback.txt")]
    [Arguments("gcloud-sql-ssl-server-certs-rotate.txt")]
    [Arguments("gcloud-sql-ssl-certs-list.txt")]
    [Arguments("gcloud-sql-users-list.txt")]
    [Arguments("gcloud-builds-triggers-create-bitbucket-cloud.txt")]
    [Arguments("gcloud-builds-triggers-create-bitbucket-data-center.txt")]
    [Arguments("gcloud-builds-triggers-create-bitbucketserver.txt")]
    [Arguments("gcloud-builds-triggers-create-cloud-source-repositories.txt")]
    [Arguments("gcloud-builds-triggers-create-github.txt")]
    [Arguments("gcloud-builds-triggers-create-gitlab.txt")]
    [Arguments("gcloud-builds-triggers-create-manual.txt")]
    [Arguments("gcloud-builds-triggers-create-pubsub.txt")]
    [Arguments("gcloud-builds-triggers-create-webhook.txt")]
    [Arguments("gcloud-builds-triggers-update-bitbucket-cloud.txt")]
    [Arguments("gcloud-builds-triggers-update-bitbucket-data-center.txt")]
    [Arguments("gcloud-builds-triggers-update-bitbucketserver.txt")]
    [Arguments("gcloud-builds-triggers-update-cloud-source-repositories.txt")]
    [Arguments("gcloud-builds-triggers-update-github.txt")]
    [Arguments("gcloud-builds-triggers-update-gitlab.txt")]
    [Arguments("gcloud-builds-triggers-update-manual.txt")]
    [Arguments("gcloud-builds-triggers-update-pubsub.txt")]
    [Arguments("gcloud-builds-triggers-update-webhook.txt")]
    public async Task Captured_585_Commands_Retain_Coverage(string fixture)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0", fixture));
        var nameLine = help.Split('\n')[1].Trim();
        var separator = nameLine.IndexOf(" - ", StringComparison.Ordinal);
        await Assert.That(separator).IsGreaterThan(0);
        var commandPath = nameLine[..separator].Replace("gcloud ", "");
        await new TestScraper().Parse(["gcloud", .. commandPath.Split(' ')], help);
        var command = (await ScrapeFixture(commandPath, help)).Single();
        await Assert.That(command.FullCommand).IsEqualTo("gcloud " + commandPath);
    }

    [Test]
    public async Task Captured_Manual_Trigger_Retains_Configuration_And_Dockerfile_Constraints()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "585.0.0", "gcloud-builds-triggers-create-manual.txt"));
        var command = (await ScrapeFixture("builds triggers create manual", help)).Single();
        var trigger = command.RequiredAlternativeGroups.Single(group => group.IsMutuallyExclusive
            && group.PropertyNames.Contains("TriggerConfig"));
        await Assert.That(trigger.IsRequired).IsTrue();
        await Assert.That(trigger.Members.Single().PropertyName).IsEqualTo("TriggerConfig");
        var flags = trigger.Groups.Single().Groups.Single();
        var build = flags.Groups.Single(group => group.PropertyNames.Contains("BuildConfig"));
        await Assert.That(build.IsRequired).IsTrue();
        await Assert.That(build.IsMutuallyExclusive).IsTrue();
        await Assert.That(build.Members.Select(member => member.PropertyName))
            .IsEquivalentTo(["BuildConfig", "InlineConfig"]);
        var dockerfile = build.Groups.Single().Groups.Single();
        await Assert.That(dockerfile.PropertyNames)
            .IsEquivalentTo(["Dockerfile", "DockerfileDir", "DockerfileImage"]);
        await Assert.That(dockerfile.Members.Single(member => member.PropertyName == "Dockerfile").IsRequired).IsTrue();
        await Assert.That(command.PositionalArguments).IsEmpty();
    }

    [Test]
    public async Task Captured_Metadata_Job_Retains_Nested_Export_Import_Constraints()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "585.0.0", "gcloud-dataplex-metadata-jobs-create.txt"));
        var command = (await ScrapeFixture("dataplex metadata-jobs create", help)).Single();
        var operation = command.RequiredAlternativeGroups.Single(group => group.IsMutuallyExclusive
            && group.PropertyNames.Contains("ExportOutputPath"));
        await Assert.That(operation.IsRequired).IsTrue();
        await Assert.That(operation.Groups).Count().IsEqualTo(2);
        var export = operation.Groups.Single(group => group.PropertyNames.Contains("ExportOutputPath"));
        await Assert.That(export.IsChoice).IsFalse();
        await Assert.That(export.Members.Single(member => member.PropertyName == "ExportOutputPath").IsRequired).IsTrue();
        var boundary = export.Groups.Single();
        await Assert.That(boundary.IsRequired).IsTrue();
        await Assert.That(boundary.IsChoice).IsTrue();
        await Assert.That(boundary.IsMutuallyExclusive).IsFalse();
        await Assert.That(boundary.Members.Select(member => member.PropertyName))
            .IsEquivalentTo(["ExportAspectTypes", "ExportEntryTypes"]);
        var scope = boundary.Groups.Single();
        await Assert.That(scope.IsMutuallyExclusive).IsTrue();
        await Assert.That(scope.PropertyNames).IsEquivalentTo(["ExportEntryGroups", "ExportOrganizationLevel", "ExportProjects"]);
        await Assert.That(command.Options.Where(option => option.IsRequired).Select(option => option.PropertyName))
            .IsEquivalentTo(["Type"]);
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("MetadataJob");
    }

    [Test]
    [Arguments("gcloud-transfer-jobs-create.txt", "transfer jobs create")]
    [Arguments("gcloud-transfer-jobs-update.txt", "transfer jobs update")]
    public async Task Group_Condition_Prose_Does_Not_Turn_Glob_Into_A_Collection(string fixture, string commandPath)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "585.0.0", fixture));
        var command = (await ScrapeFixture(commandPath, help)).Single();
        var glob = command.Options.Single(option => option.SwitchName == "--match-glob");
        await Assert.That(glob.CSharpType).IsEqualTo("string?");
        await Assert.That(glob.AcceptsMultipleValues).IsFalse();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--include-prefixes").AcceptsMultipleValues).IsTrue();
    }

    [Test]
    public async Task Multiword_Option_Value_Does_Not_Consume_A_Real_Operand()
    {
        const string help = """
            NAME
                gcloud example create - create a resource
            SYNOPSIS
                gcloud example create --buckets=BUCKET
                    URI,[BUCKET URI,...] RESOURCE
            POSITIONAL ARGUMENTS
                 RESOURCE
                    The resource name.
            FLAGS
                 --buckets=BUCKET URI,[BUCKET URI,...]
                    The bucket list.
            """;
        var command = (await ScrapeFixture("example create", help)).Single();
        await Assert.That(command.PositionalArguments.Single().PropertyName).IsEqualTo("Resource");
        await Assert.That(command.PositionalArguments.Single().IsRequired).IsTrue();
        await Assert.That(command.Options.Single().AcceptsMultipleValues).IsTrue();
    }

    [Test]
    [Arguments("compute-ssh", "compute ssh", "UserInstance,SshArgs")]
    [Arguments("compute-scp", "compute scp", "UserInstanceSrc,UserInstanceDest")]
    [Arguments("iam-service-accounts-keys-create", "iam service-accounts keys create", "OutputFile")]
    [Arguments("ai-custom-jobs-local-run", "ai custom-jobs local-run", "Args")]
    [Arguments("resource-manager-tags-keys-create", "resource-manager tags keys create", "ShortName")]
    public async Task Captured_Compound_And_Grouped_Operands_Are_Preserved(string fixture, string path, string names)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", fixture + "-550.0.0.txt"));
        var command = (await ScrapeFixture(path, help)).Single();
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(names.Split(','), TUnit.Assertions.Enums.CollectionOrdering.Matching);
        foreach (var operand in command.PositionalArguments)
        {
            var forwarded = operand.PropertyName is "Args" or "SshArgs";
            await Assert.That(operand.IsRequired).IsEqualTo(!forwarded);
            await Assert.That(operand.PrependOptionTerminator).IsEqualTo(forwarded);
            await Assert.That(operand.IsVariadic).IsEqualTo(forwarded || operand.PropertyName == "UserInstanceSrc");
        }
    }

    [Test]
    public async Task Required_Group_Preserves_Parent_Constructor_Argument()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "resource-manager-tags-keys-create-550.0.0.txt"));
        var command = (await ScrapeFixture("resource-manager tags keys create", help)).Single();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--parent").IsRequired).IsTrue();
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains("string Parent");
        await Assert.That(generated).Contains("this.Parent = Parent;");
    }

    [Test]
    public async Task Group_Repeatability_Produces_Repeated_Structured_Options()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", "compute-network-endpoint-groups-update-550.0.0.txt"));
        var command = (await ScrapeFixture("compute network-endpoint-groups update", help)).Single();
        foreach (var name in new[] { "--add-endpoint", "--remove-endpoint" })
        {
            var option = command.Options.Single(option => option.SwitchName == name);
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.CollectionSeparator).IsNull();
        }
    }

    [Test]
    [Arguments("B")]
    [Arguments("[B]")]
    public async Task Incomplete_Synopsis_Does_Not_Guess_Operand_Order_Or_Requiredness(string middle)
    {
        var help = $$"""
            NAME
                gcloud example copy - copy resources
            SYNOPSIS
                gcloud example copy A C
            POSITIONAL ARGUMENTS
                 A
                    The first operand.
                 {{middle}}
                    The middle operand.
                 C
                    The last operand.
            """;
        var exception = await Assert.That(async () =>
            { await new TestScraper().Parse(["gcloud", "example", "copy"], help); })
            .Throws<InvalidOperationException>();
        await Assert.That(exception!.Message).Contains("synopsis omits declared positional operand 'B'");
    }

    [Test]
    public async Task Without_Synopsis_Explicit_Operand_Declarations_Retain_Order_And_Optionality()
    {
        const string help = """
            NAME
                gcloud example copy - copy resources
            POSITIONAL ARGUMENTS
                 A
                    The first operand.
                 [B]
                    The optional middle operand.
                 C
                    The last operand.
            """;
        var command = (await new TestScraper().Parse(["gcloud", "example", "copy"], help))!;
        await Assert.That(command.PositionalArguments.Select(argument => argument.PropertyName))
            .IsEquivalentTo(["A", "B", "C"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
        await Assert.That(command.PositionalArguments.Select(argument => argument.IsRequired))
            .IsEquivalentTo([true, false, true], TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments("kms-keyrings-delete", "kms keyrings delete", "Keyring", "--location")]
    [Arguments("metastore-services-migrations-describe", "metastore services migrations describe", "Migration", "--location,--service")]
    [Arguments("metastore-services-migrations-delete", "metastore services migrations delete", "Migration", "--location,--service,--async")]
    public async Task Resource_Groups_Preserve_Required_Operands_And_Optional_Selectors(
        string fixture, string path, string operandName, string switches)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", fixture + ".txt"));
        var command = (await ScrapeFixture(path, help)).Single();

        var operand = command.PositionalArguments.Single();
        await Assert.That(command.HasOperandTakingUsage).IsTrue();
        await Assert.That(command.UsageSynopsis).Contains($"gcloud {path}");
        await Assert.That(command.UsagePositionalArguments.Single().PropertyName).IsEqualTo(operandName);
        await Assert.That(operand.PropertyName).IsEqualTo(operandName);
        await Assert.That(operand.IsRequired).IsTrue();
        await Assert.That(operand.PositionIndex).IsEqualTo(0);
        await Assert.That(operand.Description).Contains("fully qualified identifier");
        await Assert.That(command.Options.Select(option => option.SwitchName)).IsEquivalentTo(switches.Split(','));
        await Assert.That(command.Options.All(option => !option.IsRequired)).IsTrue();
        await Assert.That(command.RequiredAlternativeGroups).IsEmpty();

        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        await Assert.That(generated).Contains($"string {operandName}");
        await Assert.That(generated).Contains("public string? Location");
        await Assert.That(generated).Contains("[CliArgument(0, Phase = CommandLinePhase.EarlyOperand, Required = true)]");
        await Assert.That(generated).Contains($"public string {operandName} {{ get; private init; }}");
        var services = await new SubDomainClassGenerator().GenerateAsync(tool);
        var service = string.Join(Environment.NewLine, services.Select(file => file.Content));
        await Assert.That(service).Contains($"{command.ClassName} options,");
        await Assert.That(service).DoesNotContain($"{command.ClassName}? options = null");
    }

    [Test]
    public async Task Resource_Operands_Retain_Synopsis_Order_And_Optional_Repetition()
    {
        const string help = """
            NAME
                gcloud example move - move resources
            SYNOPSIS
                gcloud example move SOURCE DESTINATION [EXTRA ...] [GCLOUD_WIDE_FLAG ...]
            POSITIONAL ARGUMENTS
                 Source resource - The source to move.
                   SOURCE
                      The source identifier.
                      EXAMPLE
                         This uppercase example is documentation, not an operand.
                   --source-location=LOCATION
                      The source location or a fully qualified source name.
                 Destination resource - The destination to use.
                   DESTINATION
                      The destination identifier.
                   --destination-location=LOCATION
                      The destination location or a fully qualified destination name.
                 [EXTRA ...]
                    Additional optional identifiers.
            GCLOUD WIDE FLAGS
                 --project=PROJECT
            """;
        var command = (await new TestScraper().Parse(["gcloud", "example", "move"], help))!;

        await Assert.That(string.Join(",", command.PositionalArguments.Select(argument => argument.PropertyName)))
            .IsEqualTo("Source,Destination,Extra");
        await Assert.That(string.Join(",", command.PositionalArguments.Select(argument => argument.PositionIndex)))
            .IsEqualTo("0,1,2");
        await Assert.That(command.PositionalArguments.Take(2).All(argument => argument.IsRequired)).IsTrue();
        var extra = command.PositionalArguments[2];
        await Assert.That(extra.IsRequired).IsFalse();
        await Assert.That(extra.IsVariadic).IsTrue();
        await Assert.That(extra.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(command.Options.Select(option => option.SwitchName))
            .IsEquivalentTo(["--source-location", "--destination-location"]);
    }

    [Test]
    public async Task Optional_Resource_Can_Use_Configuration_Without_An_Operand()
    {
        const string help = """
            NAME
                gcloud example describe - describe a resource
            SYNOPSIS
                gcloud example describe [RESOURCE : --location=LOCATION] [GCLOUD_WIDE_FLAG ...]
            POSITIONAL ARGUMENTS
                 Resource resource - The resource to describe.
                   RESOURCE
                      The resource identifier; otherwise use the configured default.
                   --location=LOCATION
                      The location; otherwise use the configured default or a fully qualified name.
            """;
        var command = (await new TestScraper().Parse(["gcloud", "example", "describe"], help))!;

        await Assert.That(command.PositionalArguments.Single().IsRequired).IsFalse();
        await Assert.That(command.PositionalArguments.Single().CSharpType).IsEqualTo("string?");
        await Assert.That(command.Options.Single().IsRequired).IsFalse();
        await Assert.That(command.RequiredAlternativeGroups).IsEmpty();
    }

    [Test]
    public async Task Shared_Synopsis_Preserves_Required_Option_Alternatives()
    {
        const string help = """
            SYNOPSIS
                gcloud example export (--destination=DESTINATION | --stdout) [GCLOUD_WIDE_FLAG ...]
            FLAGS
                 --destination=DESTINATION
                    Where to write the export.
                 --stdout
                    Write the export to standard output.
            """;
        var command = (await ScrapeFixture("example export", help)).Single();

        await Assert.That(command.RequiredAlternativeGroups.Single().PropertyNames)
            .IsEquivalentTo(["Destination", "Stdout"]);
    }

    [Test]
    public async Task Shared_Synopsis_Excludes_The_Gcloud_Wide_Flag_Placeholder()
    {
        const string help = """
            SYNOPSIS
                gcloud example list [GCLOUD_WIDE_FLAG ...]
            FLAGS
                 --filter=FILTER
                    Filter the returned resources.
            """;
        var command = (await ScrapeFixture("example list", help)).Single();

        await Assert.That(command.HasOperandTakingUsage).IsFalse();
        await Assert.That(command.UsagePositionalArguments).IsEmpty();
        await Assert.That(command.Options.Single().PropertyName).IsEqualTo("Filter");
    }

    [Test]
    public async Task Shared_Synopsis_Rejects_Commands_With_Unrepresented_Operands()
    {
        const string help = """
            SYNOPSIS
                gcloud example show RESOURCE [GCLOUD_WIDE_FLAG ...]
            FLAGS
                 --format=FORMAT
                    Format the resource details.
            """;

        await Assert.That(await ScrapeFixture("example show", help)).IsEmpty();
    }

    internal static async Task<List<CliCommandDefinition>> ScrapeFixture(string path, string help)
    {
        var scraper = new GcloudCliScraper(new FixtureExecutor(path.Split(' '), help),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance), NullLogger<GcloudCliScraper>.Instance);
        var commands = new List<CliCommandDefinition>();
        await foreach (var command in scraper.ScrapeAsync())
        {
            commands.Add(command);
        }
        return commands;
    }

    private sealed class TestScraper() : GcloudCliScraper(
        new UnusedExecutor(),
        new HelpTextCache(NullLogger<HelpTextCache>.Instance),
        NullLogger<GcloudCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] path, string help) =>
            ParseCommandAsync(path, help, CancellationToken.None);
    }

    private sealed class FixtureExecutor(string[] path, string help) : UnusedExecutor
    {
        public override Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null)
        {
            var prefix = arguments.Split(' ').TakeWhile(part => part != "--help").ToArray();
            if (!prefix.SequenceEqual(path.Take(prefix.Length)))
            {
                throw new InvalidOperationException($"Unexpected command: {arguments}");
            }
            return Task.FromResult(new CliCommandResult
            {
                ExitCode = 0,
                StandardOutput = prefix.Length == path.Length ? help : $"COMMANDS\n     {path[prefix.Length]}\n",
                StandardError = string.Empty,
            });
        }
    }

    private class UnusedExecutor : ICliCommandExecutor
    {
        public virtual Task<CliCommandResult> ExecuteAsync(string command, string arguments,
            CancellationToken cancellationToken = default, string? workingDirectory = null) =>
            throw new InvalidOperationException("Execution was not expected.");

        public Task<bool> IsAvailableAsync(string command, CancellationToken cancellationToken = default) =>
            Task.FromResult(true);
    }
}
