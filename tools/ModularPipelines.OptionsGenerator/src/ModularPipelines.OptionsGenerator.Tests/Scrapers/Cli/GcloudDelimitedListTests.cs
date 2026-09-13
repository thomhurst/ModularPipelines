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
}
