using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudDelimitedListTests
{
    [Test]
    public async Task DnsRecordSetsRenderAsRepeatedOptions()
    {
        var arguments = BuildArguments(new GcloudDnsResponsePoliciesRulesUpdateOptions("rule")
        {
            LocalData =
            [
                "name=zone.com.,type=A,ttl=21600,rrdata=1.2.3.4",
                "name=www.zone.com.,type=CNAME,ttl=21600,rrdata=1.2.3.4|5.6.7.8",
            ],
        });

        await AssertArguments(arguments,
        [
            "rule",
            "--local-data=name=zone.com.,type=A,ttl=21600,rrdata=1.2.3.4",
            "--local-data=name=www.zone.com.,type=CNAME,ttl=21600,rrdata=1.2.3.4|5.6.7.8",
        ]);
    }

    [Test]
    public async Task MigrationListsRenderAsOneCommaSeparatedValuePerOption()
    {
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions("service")
        {
            HiveDatabases = ["db1", "db2"],
            IcebergNamespaces = ["ns1", "ns2"],
        });

        await AssertArguments(arguments,
        [
            "service",
            "--hive-databases=db1,db2",
            "--iceberg-namespaces=ns1,ns2",
        ]);
    }

    [Test]
    public async Task MigrationListsPreserveAnExplicitEscapedListValue()
    {
        // gcloud topic escaping defines ^DELIM^ for values containing commas.
        // Pass the complete escaped list as one entry so its delimiter stays intact.
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions("service")
        {
            HiveDatabases = ["^:^db,one:db,two"],
            IcebergNamespaces = ["^|^ns,one|ns,two"],
        });

        await AssertArguments(arguments,
        [
            "service",
            "--hive-databases=^:^db,one:db,two",
            "--iceberg-namespaces=^|^ns,one|ns,two",
        ]);
    }

    [Test]
    public async Task EmptyMigrationListsOmitTheirOptions()
    {
        var arguments = BuildArguments(new GcloudMetastoreServicesMigrationsStartOptions("service")
        {
            HiveDatabases = [],
            IcebergNamespaces = [],
        });

        await AssertArguments(arguments, ["service"]);
    }
}
