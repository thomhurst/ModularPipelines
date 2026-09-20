using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Google.Enums;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudRegenerationTests
{
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
