using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Gcloud_Migration_Lists_Use_Comma_Separated_Collection_Metadata()
    {
        // The option descriptions reproduce the migration help reported in #4797.
        const string helpText = """
            NAME
                gcloud metastore services migrations start - start a migration

            SYNOPSIS
                gcloud metastore services migrations start

            FLAGS
                 Configuration for migrating Hive tables to a BigLake Hive catalog.
                   --hive-databases=HIVE_DATABASES
                      Comma-separated list of databases to migrate to the Hive catalog.
                      Defaults to * (migrate all databases).

                 Configuration for migrating Iceberg tables to a BigLake Iceberg REST catalog.
                   --iceberg-namespaces=ICEBERG_NAMESPACES
                      Comma-separated list of namespaces to migrate to the Iceberg REST catalog.
                      Defaults to * (migrate all namespaces).
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "metastore", "services", "migrations", "start"], helpText);
        foreach (var name in new[] { "--hive-databases", "--iceberg-namespaces" })
        {
            var option = command!.Options.Single(option => option.SwitchName == name);
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.CollectionSeparator).IsEqualTo(",");
        }

        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command!],
        })).Single().Content;

        await Assert.That(generated).Contains("CliOption(\"--hive-databases\", Format = OptionFormat.EqualsSeparated, CollectionSeparator = \",\")");
        await Assert.That(generated).Contains("CliOption(\"--iceberg-namespaces\", Format = OptionFormat.EqualsSeparated, CollectionSeparator = \",\")");
        await Assert.That(generated).Contains("supply one pre-escaped list value using gcloud topic escaping");
    }

    [Test]
    [Arguments("VALUE[,VALUE,...]", "Values to include.", ",")]
    [Arguments("[VALUE,...]", "Values to include.", ",")]
    [Arguments("VALUES", "A comma-delimited list of values.", ",")]
    [Arguments("VALUES", "A comma separated list of values.", ",")]
    [Arguments("VALUES", "The comma-separated list of values.", ",")]
    [Arguments("VALUES", "(DEPRECATED) A comma-separated list of values.", ",")]
    [Arguments("VALUES", "(BETA) A comma-separated list of values.", ",")]
    [Arguments("VALUES", "(ALPHA) A comma-delimited list of values.", ",")]
    [Arguments("SIZE", "(DEPRECATED) When using a comma-separated list in --worker, set the batch size.", null)]
    [Arguments("[VALUE,...]", "Values to include. This flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "This is a repeated argument that can be specified multiple times.", null)]
    [Arguments("[VALUE,...]", "This option may be specified multiple times.", null)]
    [Arguments("[VALUE,...]", "Values to include. This option accepts multiple values.", ",")]
    [Arguments("[VALUE,...]", "Values to include. The --other flag can be repeated.", ",")]
    [Arguments("[VALUE,...]", "Values to include alongside a repeated argument.", ",")]
    [Arguments("[VALUE,...]", "Values to include. The option is repeatable.", null)]
    [Arguments("SIZE", "When using a comma-separated list in --worker, set the batch size.", null)]
    [Arguments("VALUE", "The current value controls a comma-separated list in --other.", null)]
    [Arguments("VALUE", "This flag can be repeated.", null)]
    [Arguments("VALUE", "Matches values that contain comma-separated text.", null)]
    [Arguments("[name=NAME,config=CONFIG]", "This flag can be repeated.", null)]
    [Arguments("KEY=VALUE", "This flag can be repeated.", null)]
    public async Task Gcloud_Distinguishes_Delimited_Lists_From_Repeated_Options(
        string hint, string description, string? separator)
    {
        var helpText = $"""
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --values={hint}
                    {description}
            """;

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);

        await Assert.That(command!.Options.Single().CollectionSeparator).IsEqualTo(separator);
    }

    [Test]
    public async Task Gcloud_Containing_Group_Does_Not_Make_A_Repeated_Option_Delimited()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 Configure a comma-separated list of resources.
                   --resource=RESOURCE
                      This flag can be repeated.
            """;

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var option = command!.Options.Single();

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CollectionSeparator).IsNull();
    }

    [Test]
    [Arguments("dns-response-policies-rules-update", "dns response-policies rules update", "--local-data", null)]
    [Arguments("artifacts-docker-images-scan", "artifacts docker images scan", "--additional-package-types", ",")]
    public async Task Gcloud_Captured_Help_Preserves_Collection_Boundaries(
        string fixture, string commandPath, string switchName, string? separator)
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", $"{fixture}-550.0.0.txt"));
        var command = await CreateGcloudScraper().Parse(["gcloud", .. commandPath.Split(' ')], helpText);
        var option = command!.Options.Single(option => option.SwitchName == switchName);

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.CollectionSeparator).IsEqualTo(separator);

        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;

        var attribute = separator is null
            ? $"[CliOption(\"{switchName}\", Format = OptionFormat.EqualsSeparated)]"
            : $"[CliOption(\"{switchName}\", Format = OptionFormat.EqualsSeparated, CollectionSeparator = \",\")]";
        await Assert.That(generated).Contains(attribute);
    }

    [Test]
    public async Task Gcloud_Negated_Flag_Does_Not_Inherit_List_Serialization()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --values=[VALUE,...]
                    Values to include. Use --no-values to disable selection.
            """;

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var negative = command!.Options.Single(option => option.SwitchName == "--no-values");

        await Assert.That(negative.IsFlag).IsTrue();
        await Assert.That(negative.AcceptsMultipleValues).IsFalse();
        await Assert.That(negative.CollectionSeparator).IsNull();
    }

    [Test]
    [Arguments("scp")]
    [Arguments("ssh")]
    public async Task Gcloud_Tpu_Batch_Size_Does_Not_Inherit_The_Worker_List(string commandName)
    {
        var helpText = $"""
            NAME
                gcloud compute tpus queued-resources {commandName} - connect to workers

            SYNOPSIS
                gcloud compute tpus queued-resources {commandName}

            FLAGS
                 --batch-size=BATCH_SIZE
                    Batch size for simultaneous command execution on the client's side.
                    When using a comma-separated list (e.g. '1,4,6') or a range (e.g. '1-3')
                    or ``all`` keyword in --worker flag, it executes the command concurrently
                    in groups of the batch size.
                 --worker=WORKER
                    TPU worker to connect to.
            """;

        var command = await CreateGcloudScraper().Parse(
            ["gcloud", "compute", "tpus", "queued-resources", commandName], helpText);
        var batchSize = command!.Options.Single(option => option.SwitchName == "--batch-size");

        await Assert.That(batchSize.CSharpType).IsEqualTo("int?");
        await Assert.That(batchSize.AcceptsMultipleValues).IsFalse();
        await Assert.That(batchSize.CollectionSeparator).IsNull();
    }
}
