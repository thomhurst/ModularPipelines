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
    [Arguments("VALUES", "At most one of these can be specified: Comma-separated list of values.", ",")]
    [Arguments("VALUES", "Exactly one of these must be specified: Comma-separated list of values.", ",")]
    [Arguments("VALUE", "Example output: Comma-separated list of values.", null)]
    [Arguments("VALUES", "The comma-separated list of values.", ",")]
    [Arguments("TAGS", "A single tag or a comma-delimited list of tags.", ",")]
    [Arguments("VALUES", "Accepts a single IP address or a comma-separated list of addresses.", ",")]
    [Arguments("LOGS", "Filter entries from a particular set of logs. Must be a comma-separated list of log names.", ",")]
    [Arguments("HEADERS", "This field can only be specified if logging is enabled. Contains a comma-separated list of HTTP request headers.", ",")]
    [Arguments("VALUES", "The flag can be a comma-separated list of updatable fields.", ",")]
    [Arguments("VALUE", "A single tag controls the comma-delimited list in --other.", null)]
    [Arguments("VALUE", "The --other flag can be a comma-separated list of values.", null)]
    [Arguments("VALUE", "The result contains a comma-separated list of values.", null)]
    [Arguments("KEY=VALUE", "Includes arbitrary headers in storage API calls. Accepts a comma separated list of key=value pairs.", ",")]
    [Arguments("VALUES", "Selects values. Specifies a comma-delimited list of values.", ",")]
    [Arguments("VALUES", "This option accepts a comma-separated list of values.", ",")]
    [Arguments("VALUES", "Selects values. (BETA) The flag specifies a comma-separated list of values.", ",")]
    [Arguments("VALUES", "Selects values. A comma-separated list of values.", ",")]
    [Arguments("VALUE", "Selects a value. The --other flag accepts a comma-separated list of values.", null)]
    [Arguments("VALUE", "Selects a value. When --other accepts a comma-separated list, use this value.", null)]
    [Arguments("VALUE", "Selects a value. This option specifies the limit for a comma-separated list.", null)]
    [Arguments("VALUES", "(DEPRECATED) A comma-separated list of values.", ",")]
    [Arguments("VALUES", "(BETA) A comma-separated list of values.", ",")]
    [Arguments("VALUES", "(ALPHA) A comma-delimited list of values.", ",")]
    [Arguments("SIZE", "(DEPRECATED) When using a comma-separated list in --worker, set the batch size.", null)]
    [Arguments("[VALUE,...]", "Can be repeated.", null)]
    [Arguments("[VALUE,...]", "Maybe specified multiple times.", null)]
    [Arguments("KEY_VERSION,[KEY_VERSION,...]", "A Cloud KMS asymmetric signing cryptoKeyVersion that will be used to verify service account tokens. Maybe specified multiple times.", null)]
    [Arguments("[VALUE,...]", "(DEPRECATED) Maybe specified multiple times.", null)]
    [Arguments("[VALUE,...]", "This flag maybe supplied more than once.", null)]
    [Arguments("[VALUE,...]", "MAYBE REPEATED.", null)]
    [Arguments("[VALUE,...]", "Values to include. The --other flag maybe specified multiple times.", ",")]
    [Arguments("[VALUE,...]", "Repeatable.", null)]
    [Arguments("[VALUE,...]", "Repeat or comma-separate for multiple values.", null)]
    [Arguments("[VALUE,...]", "May be provided one or more times.", null)]
    [Arguments("[VALUE,...]", "Specify multiple times.", null)]
    [Arguments("[VALUE,...]", "Multiples are supported by passing --values multiple times.", null)]
    [Arguments("[VALUE,...]", "(BETA) May be supplied more than once.", null)]
    [Arguments("[VALUE,...]", "Repeat to add more.", null)]
    [Arguments("[VALUE,...]", "Values to include. Can be repeated.", null)]
    [Arguments("[VALUE,...]", "Values to include. This flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "(DEPRECATED) This flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "(ALPHA) This option may be specified multiple times.", null)]
    [Arguments("[VALUE,...]", "(BETA) This is a repeated argument.", null)]
    [Arguments("[VALUE,...]", "(DEPRECATED) (BETA) This flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "(deprecated) This flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "Values to include. (BETA) The option is repeatable.", null)]
    [Arguments("[VALUE,...]", "(DEPRECATED) The --other flag can be repeated.", ",")]
    [Arguments("[VALUE,...]", "(BETA) Values to include alongside a repeated argument.", ",")]
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
    [Arguments("[KEY=VALUE,...]", "A comma-separated list of KEY=VALUE pairs.", ",")]
    [Arguments("KEY=VALUE", "A comma-separated list of KEY=VALUE pairs.", ",")]
    [Arguments("[KEY=VALUE,...]", "Attribute mappings.", ",")]
    [Arguments("[KEY=VALUE,...]", "This flag can be repeated.", null)]
    [Arguments("[KEY=VALUE,...]", "Adds a volume to the Cloud Run resource. To add more than one volume, specify this flag multiple times.", null)]
    [Arguments("[KEY=VALUE,...]", "Specify this flag multiple times.", null)]
    [Arguments("[KEY=VALUE,...]", "Supply the option more than once.", null)]
    [Arguments("[VALUE,...]", "To add more items, use this argument multiple times.", null)]
    [Arguments("[VALUE,...]", "Values to include. Specify the --other flag multiple times.", ",")]
    [Arguments("[VALUE,...]", "Values to include. To add more items, specify --other multiple times.", ",")]
    [Arguments("[name=NAME,config=CONFIG]", "A comma-separated list of fields.", null)]
    [Arguments("VALUE", "Namespaces to ignore, separated by commas if multiple are supplied.", ",")]
    [Arguments("VALUE", "One or more rule files (separated by commas if multiple).", ",")]
    [Arguments("VALUE", "Path to a file containing values separated by commas.", null)]
    [Arguments("VALUE", "Values for --other, separated by commas if multiple are supplied.", null)]
    [Arguments("VALUE", "Path of a JSON/YAML file. Multiple scopes can be specified, separated by commas.", null)]
    [Arguments("VALUE", "Configuration document. Nested fields accept names (separated by commas).", null)]
    [Arguments("VALUE", "Values to include. This option accepts names, separated by commas.", ",")]
    [Arguments("VALUE", "Values, seperated by commas if multiple are supplied.", ",")]
    [Arguments("VALUE", "A string of user-to-service-account mappings. Mappings are separated by commas.", ",")]
    [Arguments("VALUE", "(BETA) A string of labels. Labels are separated by commas.", ",")]
    [Arguments("VALUE", "(DEPRECATED) (ALPHA) The string of labels. Labels are separated by commas.", ",")]
    [Arguments("VALUE", "(BETA) Path to a file containing labels. Labels are separated by commas.", null)]
    [Arguments("VALUE", "A list of databases to migrate. Provide databases as a comma separated list.", ",")]
    [Arguments("VALUE", "(ALPHA) Supply names as a comma-separated list.", ",")]
    [Arguments("VALUE", "Provide values for --other as a comma separated list.", null)]
    [Arguments("VALUE", "A string of labels. Mappings are separated by commas.", null)]
    [Arguments("VALUE", "Path to a file containing mappings. Mappings are separated by commas.", null)]
    [Arguments("[VALUE,...]", "Specify the --values flag multiple times.", null)]
    [Arguments("[VALUE,...]", "The --values flag can be repeated.", null)]
    [Arguments("[VALUE,...]", "Specify the --values-other flag multiple times.", ",")]
    [Arguments("FLAG=VALUE,[FLAG=VALUE,...]", "Set pool flags.", ",")]
    [Arguments("[FLAG=VALUE,[FLAG=VALUE,...]]", "Set pool flags.", ",")]
    [Arguments("[FLAG=VALUE,[FLAG=VALUE,...]", "Set pool flags.", null)]
    [Arguments("FLAG=VALUE,[FLAG=VALUE,...]]", "Set pool flags.", null)]
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
    public async Task Gcloud_Config_File_Contents_Do_Not_Change_File_Option_Shape()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "container-clusters-create-550.0.0.txt"));
        var command = await CreateGcloudScraper().Parse(["gcloud", "container", "clusters", "create"], helpText);
        var option = command!.Options.Single(option => option.SwitchName == "--autoprovisioning-config-file");

        await Assert.That(option.CSharpType).IsEqualTo("string?");
        await Assert.That(option.AcceptsMultipleValues).IsFalse();
        await Assert.That(option.CollectionSeparator).IsNull();

        var scopes = command.Options.Single(option => option.SwitchName == "--autoprovisioning-scopes");
        await Assert.That(scopes.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(scopes.CollectionSeparator).IsEqualTo(",");

        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await Assert.That(generated).Contains("public string? AutoprovisioningConfigFile");
        await Assert.That(generated).Contains("[CliOption(\"--autoprovisioning-config-file\", Format = OptionFormat.EqualsSeparated)]");
    }

    [Test]
    [Arguments("Configure a comma-separated list of resources.")]
    [Arguments("One of these flags can be repeated: --resource.")]
    public async Task Gcloud_Containing_Group_Does_Not_Change_Sibling_Collection_Shapes(string groupDescription)
    {
        var helpText = $"""
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 {groupDescription}
                   --resource=RESOURCE
                      This flag can be repeated.
                   --label=LABEL
                      The label to assign.
            """;

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var option = command!.Options.Single(option => option.SwitchName == "--resource");

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CollectionSeparator).IsNull();
        var sibling = command.Options.Single(option => option.SwitchName == "--label");
        await Assert.That(sibling.AcceptsMultipleValues).IsFalse();
        await Assert.That(sibling.CSharpType).IsEqualTo("string?");
        await Assert.That(sibling.CollectionSeparator).IsNull();
    }

    [Test]
    [Arguments("dns-response-policies-rules-update", "dns response-policies rules update", "--local-data", null)]
    [Arguments("artifacts-docker-images-scan", "artifacts docker images scan", "--additional-package-types", ",")]
    [Arguments("compute-url-maps-invalidate-cdn-cache", "compute url-maps invalidate-cdn-cache", "--tags", ",")]
    [Arguments("app-logs-read", "app logs read", "--logs", ",")]
    [Arguments("artifacts-docker-upgrade-migrate", "artifacts docker upgrade migrate", "--projects", ",")]
    [Arguments("sql-instances-patch", "sql instances patch", "--connection-pool-flags", ",")]
    [Arguments("container-hub-policycontroller-enable", "container hub policycontroller enable", "--exemptable-namespaces", ",")]
    [Arguments("container-hub-policycontroller-enable", "container hub policycontroller enable", "--monitoring", ",")]
    [Arguments("compute-instances-create", "compute instances create", "--local-ssd", null, "550")]
    [Arguments("dataproc-clusters-create", "dataproc clusters create", "--secure-multi-tenancy-user-mapping", ",")]
    [Arguments("database-migration-migration-jobs-promote", "database-migration migration-jobs promote", "--databases-filter", ",")]
    public async Task Gcloud_Captured_Help_Preserves_Collection_Boundaries(
        string fixture, string commandPath, string switchName, string? separator, string version = "550.0.0")
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", $"{fixture}-{version}.txt"));
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
    [Arguments("iam-workforce-pools-providers-scim-tenants-update", "iam workforce-pools providers scim-tenants update", "--claim-mapping", "IReadOnlyList<KeyValue>?", true)]
    [Arguments("storage-folders-list", "storage folders list", "--additional-headers", "IEnumerable<string>?", false)]
    public async Task Gcloud_Captured_Comma_Lists_Join_Entries(
        string fixture, string commandPath, string switchName, string expectedType, bool isKeyValue)
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", $"{fixture}-550.0.0.txt"));
        var command = await CreateGcloudScraper().Parse(
            ["gcloud", .. commandPath.Split(' ')], helpText);
        var mapping = command!.Options.Single(option => option.SwitchName == switchName);
        await Assert.That(mapping.IsKeyValue).IsEqualTo(isKeyValue);
        await Assert.That(mapping.AcceptsMultipleValues).IsTrue();
        await Assert.That(mapping.CSharpType).IsEqualTo(expectedType);
        await Assert.That(mapping.CollectionSeparator).IsEqualTo(",");
        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;
        await Assert.That(generated).Contains($"[CliOption(\"{switchName}\", Format = OptionFormat.EqualsSeparated, CollectionSeparator = \",\")]");
    }

    [Test]
    public async Task Gcloud_Delimited_Enum_Allows_Multiple_Choices()
    {
        var helpText = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "Gcloud", "storage-diagnose.txt"));
        var command = await CreateGcloudScraper().Parse(["gcloud", "storage", "diagnose"], helpText);
        var option = command!.Options.Single(option => option.SwitchName == "--test-type");

        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CollectionSeparator).IsEqualTo(",");
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<GcloudTestType>?");
        await Assert.That(option.EnumDefinition!.Values.Select(value => value.CliValue))
            .IsEquivalentTo(["DIRECT_CONNECTIVITY", "DOWNLOAD_THROUGHPUT", "LATENCY", "UPLOAD_THROUGHPUT"]);

        var generated = (await new OptionsClassGenerator().GenerateAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        })).Single().Content;

        await Assert.That(generated).Contains("public IEnumerable<GcloudTestType>? TestType");
        await Assert.That(generated).Contains("CliOption(\"--test-type\", Format = OptionFormat.EqualsSeparated, CollectionSeparator = \",\")");
    }

    [Test]
    public async Task Gcloud_List_Hints_Respect_Repetition_In_The_Whole_Option_Block()
    {
        const string helpText = """
            NAME
                gcloud example update - update an example
            SYNOPSIS
                gcloud example update
            FLAGS
                 --values=[VALUE,...]
                    Values to include.
                 This flag can be repeated to configure multiple values.

                 --other=[VALUE,...]
                    Other values to include.
            """;
        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var repeated = command!.Options.Single(option => option.SwitchName == "--values");
        await Assert.That(repeated.AcceptsMultipleValues).IsTrue();
        await Assert.That(repeated.CollectionSeparator).IsNull();
        await Assert.That(command.Options.Single(option => option.SwitchName == "--other").CollectionSeparator).IsEqualTo(",");
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
        await Assert.That(negative.Description).IsEqualTo("Negates --values. Values to include. Use --no-values to disable selection.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--values").Description)
            .Contains("Collection entries are joined with commas");
    }

    [Test]
    [Arguments("Maybe specified multiple times.")]
    [Arguments("Specify this flag multiple times.")]
    [Arguments("This is a repeated argument.")]
    public async Task Gcloud_Local_Repetition_Grammar_Preserves_Value_Collections(string description)
    {
        var helpText = $"""
            NAME
                gcloud example update - update an example

            SYNOPSIS
                gcloud example update

            FLAGS
                 --values=VALUE
                    {description}
            """;

        var command = await CreateGcloudScraper().Parse(["gcloud", "example", "update"], helpText);
        var option = command!.Options.Single();
        await Assert.That(option.AcceptsMultipleValues).IsTrue();
        await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
        await Assert.That(option.CollectionSeparator).IsNull();
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
