using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudDelimitedListTests
{
    [Test]
    public async Task MigrationListsRenderAsOneCommaSeparatedValuePerOption()
    {
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions
        {
            HiveDatabases = ["db1", "db2"],
            IcebergNamespaces = ["ns1", "ns2"],
        });

        await AssertArguments(arguments,
        [
            "--hive-databases=db1,db2",
            "--iceberg-namespaces=ns1,ns2",
        ]);
    }

    [Test]
    public async Task MigrationListsPreserveAnExplicitEscapedListValue()
    {
        // gcloud topic escaping defines ^DELIM^ for values containing commas.
        // Pass the complete escaped list as one entry so its delimiter stays intact.
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions
        {
            HiveDatabases = ["^:^db,one:db,two"],
            IcebergNamespaces = ["^|^ns,one|ns,two"],
        });

        await AssertArguments(arguments,
        [
            "--hive-databases=^:^db,one:db,two",
            "--iceberg-namespaces=^|^ns,one|ns,two",
        ]);
    }

    [Test]
    public async Task EmptyMigrationListsOmitTheirOptions()
    {
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions
        {
            HiveDatabases = [],
            IcebergNamespaces = [],
        });

        await AssertArguments(arguments, []);
    }
}
