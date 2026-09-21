using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ModularPipelines.Google.Enums;
using ModularPipelines.Google.Options;
using ModularPipelines.Secrets;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudRegenerationTests
{
    [Test]
    [Arguments("none", true)]
    [Arguments("schema", false)]
    [Arguments("encoding", false)]
    [Arguments("complete", true)]
    public async Task Topic_Schema_Settings_Are_Optional_But_Validate_Together(string selection, bool valid)
    {
        var options = new GcloudPubsubTopicsCreateOptions(["topic"])
        {
            Schema = selection is "schema" or "complete" ? "schema" : null,
            MessageEncoding = selection is "encoding" or "complete"
                ? GcloudPubsubTopicsCreateMessageEncoding.Json : null,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments(true, true, true)]
    [Arguments(false, true, false)]
    [Arguments(true, false, false)]
    public async Task Iceberg_Connection_Requires_Its_Catalog_And_Storage_Bundles(
        bool catalog, bool storage, bool valid)
    {
        var options = new GcloudOracleDatabaseGoldengateConnectionsCreateOptions("ICEBERG", "connection", "connection")
        {
            IcebergConnectionPropertiesTechnologyType = "ICEBERG",
            IcebergConnectionPropertiesCatalogType = catalog ? "hadoop" : null,
            IcebergConnectionPropertiesStorageType = storage ? "google-cloud-storage" : null,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", true)]
    [Arguments("region", true)]
    [Arguments("autoscale", true)]
    [Arguments("key", true)]
    [Arguments("keyring", false)]
    [Arguments("application", false)]
    [Arguments("tenant", false)]
    [Arguments("entra", true)]
    [Arguments("region-key", true)]
    [Arguments("region-zone", false)]
    public async Task Sql_Configuration_Groups_Validate_Independently(string selection, bool valid)
    {
        var options = new GcloudSqlInstancesCreateOptions("instance")
        {
            Region = selection is "region" or "region-key" or "region-zone" ? "us-central1" : null,
            Zone = selection == "region-zone" ? "us-central1-a" : null,
            AutoScaleMaxNodeCount = selection == "autoscale" ? 2 : null,
            DiskEncryptionKey = selection is "key" or "region-key"
                ? "projects/project/locations/global/keyRings/ring/cryptoKeys/key" : null,
            DiskEncryptionKeyKeyring = selection == "keyring" ? "ring" : null,
            EntraIdApplicationId = selection is "application" or "entra" ? "application" : null,
            EntraIdTenantId = selection is "tenant" or "entra" ? "tenant" : null,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    public async Task Created_Token_Auth_User_Name_Is_Not_Secret()
    {
        var property = typeof(GcloudMemorystoreInstancesCreateTokenAuthUserOptions)
            .GetProperty(nameof(GcloudMemorystoreInstancesCreateTokenAuthUserOptions.TokenAuthUser))!;
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>()).IsNull();

        await AssertArguments(BuildArguments(new GcloudMemorystoreInstancesCreateTokenAuthUserOptions("user-id", "instance")),
            ["instance", "--token-auth-user=user-id"]);
    }

    [Test]
    public async Task Editions_Retain_Command_Specific_Values()
    {
        await AssertArguments(BuildArguments(new GcloudFirestoreDatabasesCreateOptions("us-central1")
        {
            Edition = GcloudFirestoreDatabasesCreateEdition.Standard,
        }), ["--location=us-central1", "--edition=standard"]);

        await AssertArguments(BuildArguments(new GcloudSqlInstancesCreateOptions("instance")
        {
            Edition = GcloudSqlInstancesCreateEdition.EnterprisePlus,
        }), ["instance", "--edition=enterprise-plus"]);
    }

    [Test]
    [Arguments("none", true)]
    [Arguments("secure-source-manager", true)]
    [Arguments("github", true)]
    [Arguments("incomplete-github", false)]
    [Arguments("multiple", false)]
    public async Task Provider_Requirements_Apply_Only_To_The_Selected_Branch(string provider, bool valid)
    {
        var options = new GcloudDeveloperConnectConnectionsCreateOptions("connection")
        {
            SecureSourceManagerInstanceConfig = provider is "secure-source-manager" or "multiple"
                ? "projects/project/locations/us-central1/instances/source" : null,
            GithubConfigApp = provider is "github" or "multiple" ? "FIREBASE" : null,
            GithubConfigAppInstallationId = provider == "incomplete-github" ? "123" : null,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    public async Task Password_Secret_Version_Renders_The_Resource_Value()
    {
        const string secretVersion = "projects/project/secrets/password/versions/1";
        var arguments = BuildArguments(new GcloudOracleDatabaseGoldengateConnectionsCreateOptions(
            "mysql", "connection", "connection")
        {
            MysqlConnectionPropertiesPasswordSecretVersion = secretVersion,
        });

        await Assert.That(arguments).Contains($"--mysql-connection-properties-password-secret-version={secretVersion}");
    }

    [Test]
    public async Task Inline_Private_Key_Is_Secret_But_Resource_Identifiers_Are_Not()
    {
        var privateKey = typeof(GcloudDatabaseMigrationConnectionProfilesCreateMysqlOptions)
            .GetProperty(nameof(GcloudDatabaseMigrationConnectionProfilesCreateMysqlOptions.PrivateKey))!;
        await Assert.That(privateKey.GetCustomAttribute<SecretValueAttribute>()).IsNotNull();

        var resourceType = typeof(GcloudMemorystoreInstancesTokenAuthUsersAuthTokensDescribeOptions);
        foreach (var name in new[] { "AuthToken", "TokenAuthUser" })
        {
            await Assert.That(resourceType.GetProperty(name)!.GetCustomAttribute<SecretValueAttribute>()).IsNull();
        }

        await AssertArguments(BuildArguments(new GcloudMemorystoreInstancesTokenAuthUsersAuthTokensDescribeOptions("token-id")
        {
            TokenAuthUser = "user-id",
        }), ["token-id", "--token-auth-user=user-id"]);
    }
}
