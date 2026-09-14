using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Gcloud_Enum_Default_Headers_Do_Not_Contaminate_Sibling_Options()
    {
        // Unmodified Windows help from Google Cloud SDK 550.0.0. SDK 584.0.0
        // generation also exposes these enum defaults with embedded spaces.
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "storage-diagnose.txt"));
        var command = (await CreateGcloudScraper().Parse(["gcloud", "storage", "diagnose"], help))!;

        await Assert.That(command.Options.Select(option => option.SwitchName))
            .Contains("--download-type").And.Contains("--upload-type");
        var downloadType = command.Options.Single(option => option.SwitchName == "--download-type");
        var uploadType = command.Options.Single(option => option.SwitchName == "--upload-type");
        await Assert.That(downloadType.CSharpType).IsEqualTo("GcloudDownloadType?");
        await Assert.That(uploadType.CSharpType).IsEqualTo("GcloudUploadType?");
        var logsPath = command.Options.Single(option => option.SwitchName == "--logs-path");
        await Assert.That(logsPath.CSharpType).IsEqualTo("string?");
        await Assert.That(logsPath.Description)
            .IsEqualTo("If the diagnostic supports writing logs, write the logs to this file location.");
        var processCount = command.Options.Single(option => option.SwitchName == "--process-count");
        await Assert.That(processCount.CSharpType).IsEqualTo("int?");
        await Assert.That(processCount.Description)
            .IsEqualTo("Number of processes at max to use for each diagnostic test.");
        var threadCount = command.Options.Single(option => option.SwitchName == "--thread-count");
        await Assert.That(threadCount.CSharpType).IsEqualTo("int?");
        await Assert.That(threadCount.Description)
            .IsEqualTo("Number of threads at max to use for each diagnostic test.");
    }

    [Test]
    public async Task Gcloud_Scopes_Same_Indentation_Groups_From_Authoritative_Help()
    {
        // Unmodified Linux help from Google Cloud SDK 584.0.0. Plain group headings
        // have the same indentation as their flags, unlike named resource groups.
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "metastore-services-migrations-start.txt"));
        var command = (await CreateGcloudScraper().Parse(
            ["gcloud", "metastore", "services", "migrations", "start"], help))!;
        var root = command.ArgumentGroups.Single();

        await Assert.That(root.Description).IsNull();
        await Assert.That(root.Groups).Count().IsEqualTo(2);
        await Assert.That(root.Groups[0].Arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--hive-catalog", "--hive-databases"]);
        await Assert.That(root.Groups[1].Arguments.Select(argument => argument.SwitchName))
            .IsEquivalentTo(["--iceberg-catalog", "--iceberg-namespaces"]);
        await Assert.That(command.Options.Single(option => option.SwitchName == "--async").Description)
            .IsEqualTo("Return immediately, without waiting for the operation in progress to complete.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--hive-catalog").Description)
            .StartsWith("Configuration for migrating Hive tables to a BigLake Hive catalog.")
            .And.DoesNotContain("Iceberg");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--iceberg-catalog").Description)
            .StartsWith("Configuration for migrating Iceberg tables to a BigLake Iceberg REST catalog.")
            .And.DoesNotContain("Hive");
        var generated = (await new OptionsClassGenerator().GenerateAsync(new()
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        })).Single().Content;
        var propertyIndex = generated.IndexOf(" Async ", StringComparison.Ordinal);
        var documentationStart = generated.LastIndexOf("/// <summary>", propertyIndex, StringComparison.Ordinal);
        await Assert.That(generated[documentationStart..propertyIndex])
            .DoesNotContain("Hive").And.DoesNotContain("Iceberg");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Gcloud_Scopes_Plain_Sibling_Group_Descriptions(bool includeTopLevelOption)
    {
        var help = """
            NAME
                gcloud metastore services migrations start - start migration
            SYNOPSIS
                gcloud metastore services migrations start
            FLAGS
                 Configuration for Hive migration.
                   --hive-databases=DATABASES
                      Select Hive databases.

                 Configuration for Iceberg migration.
                   --iceberg-namespaces=NAMESPACES
                      Select Iceberg namespaces.
            """;
        if (includeTopLevelOption)
        {
            help += "\n     --async\n        Return without waiting for the operation to complete.\n";
        }

        var command = (await CreateGcloudScraper().Parse(["gcloud", "metastore", "services", "migrations", "start"], help))!;
        var root = command.ArgumentGroups.Single();
        await Assert.That(root.Description).IsNull();
        await Assert.That(root.Groups).Count().IsEqualTo(2);
        await Assert.That(root.Groups[0].Arguments.Single().SwitchName).IsEqualTo("--hive-databases");
        await Assert.That(root.Groups[1].Arguments.Single().SwitchName).IsEqualTo("--iceberg-namespaces");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--hive-databases").Description)
            .IsEqualTo("Configuration for Hive migration. Select Hive databases.");
        await Assert.That(command.Options.Single(option => option.SwitchName == "--iceberg-namespaces").Description)
            .IsEqualTo("Configuration for Iceberg migration. Select Iceberg namespaces.");
        if (includeTopLevelOption)
        {
            await Assert.That(root.Arguments.Single().SwitchName).IsEqualTo("--async");
            await Assert.That(command.Options.Single(option => option.SwitchName == "--async").Description)
                .IsEqualTo("Return without waiting for the operation to complete.");
        }
    }

    [Test]
    public async Task Gcloud_Nested_Context_Does_Not_Change_Unrelated_Option_Types()
    {
        const string help = """
            NAME
                gcloud compute backend-services create - create a backend service
            SYNOPSIS
                gcloud compute backend-services create
            FLAGS
                 Configure unhealthy host eviction.
                   --eviction-limit=LIMIT
                      Maximum number of unhealthy hosts.

                 Configure AWS S3 authentication.
                   --authentication-mode=MODE
                      Select authentication mode.

                   Supply secret credentials as a list of key=value pairs.
                     --credentials=CREDENTIALS
                        Specify credentials.

                 Configure mTLS.
                   --certificate=CERTIFICATE
                      Specify the certificate.

                 --network-endpoint-group=NETWORK_ENDPOINT_GROUP
                    Name of the network endpoint group.
            """;
        var command = (await CreateGcloudScraper().Parse(["gcloud", "compute", "backend-services", "create"], help))!;
        var root = command.ArgumentGroups.Single();
        await Assert.That(root.Description).IsNull();
        await Assert.That(root.Groups).Count().IsEqualTo(3);
        await Assert.That(root.Groups[1].Groups.Single().Arguments.Single().SwitchName).IsEqualTo("--credentials");
        var credentials = command.Options.Single(option => option.SwitchName == "--credentials");
        await Assert.That(credentials.Description).Contains("Configure AWS S3 authentication.")
            .And.Contains("Supply secret credentials").And.DoesNotContain("eviction").And.DoesNotContain("mTLS");
        var network = command.Options.Single(option => option.SwitchName == "--network-endpoint-group");
        await Assert.That(network.Description).IsEqualTo("Name of the network endpoint group.");
        await Assert.That(network.CSharpType).IsEqualTo("string?");
        await Assert.That(network.IsSecret).IsFalse();
        await Assert.That(network.AcceptsMultipleValues).IsFalse();
        var files = await new OptionsClassGenerator().GenerateAsync(new()
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        });
        var generated = files.Single().Content;
        var propertyIndex = generated.IndexOf(" NetworkEndpointGroup ", StringComparison.Ordinal);
        var documentationStart = generated.LastIndexOf("/// <summary>", propertyIndex, StringComparison.Ordinal);
        var propertyDocumentation = generated[documentationStart..propertyIndex];
        await Assert.That(propertyDocumentation).Contains("Name of the network endpoint group.")
            .And.DoesNotContain("authentication").And.DoesNotContain("mTLS").And.DoesNotContain("eviction");
    }
}
