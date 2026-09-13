using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
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
