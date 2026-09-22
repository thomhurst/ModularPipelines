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
    [Arguments("GoogleBigQueryConnectionPropertiesServiceAccountKeyFile")]
    [Arguments("GoogleCloudStorageConnectionPropertiesServiceAccountKeyFile")]
    [Arguments("GooglePubsubConnectionPropertiesServiceAccountKeyFile")]
    [Arguments("IcebergConnectionPropertiesGoogleCloudStorageServiceAccountKeyFile")]
    [Arguments("OciObjectStorageConnectionPropertiesPrivateKeyFile")]
    [Arguments("OracleAiDataPlatformConnectionPropertiesPrivateKeyFile")]
    [Arguments("OracleNosqlConnectionPropertiesPrivateKeyFile")]
    [Arguments("SnowflakeConnectionPropertiesPrivateKeyFile")]
    public async Task Embedded_Credential_File_Contents_Are_Marked_As_Secrets(string propertyName)
    {
        var property = typeof(GcloudOracleDatabaseGoldengateConnectionsCreateOptions).GetProperty(propertyName)!;
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
    }

    [Test]
    [Arguments("none", true)]
    [Arguments("ca-only", false)]
    [Arguments("complete", true)]
    public async Task Mysql_Tls_Is_Optional_And_Requires_All_Certificates(string certificates, bool valid)
    {
        var options = new GcloudDatastreamConnectionProfilesCreateOptions("display", "MYSQL", "profile")
        {
            MysqlHostname = "database",
            MysqlPort = 3306,
            MysqlUsername = "user",
            MysqlPassword = "password",
            CaCertificate = certificates != "none" ? "ca" : null,
            ClientCertificate = certificates == "complete" ? "certificate" : null,
            ClientKey = certificates == "complete" ? "key" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", true)]
    [Arguments("ca-only", true)]
    [Arguments("missing-key", false)]
    [Arguments("complete", true)]
    public async Task Postgresql_Client_Certificates_Are_Optional_Together(string certificates, bool valid)
    {
        var options = new GcloudDatastreamConnectionProfilesCreateOptions("display", "POSTGRESQL", "profile")
        {
            PostgresqlDatabase = "database",
            PostgresqlHostname = "host",
            PostgresqlPort = 5432,
            PostgresqlUsername = "user",
            PostgresqlPassword = "password",
            PostgresqlCaCertificate = certificates != "none" ? "ca" : null,
            PostgresqlClientCertificate = certificates is "missing-key" or "complete" ? "certificate" : null,
            PostgresqlClientKey = certificates == "complete" ? "key" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("known", true)]
    [Arguments("oid", true)]
    [Arguments("both", true)]
    [Arguments("known-and-drop", false)]
    [Arguments("all", true)]
    [Arguments("all-and-known", false)]
    public async Task Certificate_Extension_Choices_Are_Independently_Optional(string selection, bool valid)
    {
        var options = new GcloudPrivatecaTemplatesUpdateOptions("template")
        {
            CopyKnownExtensions = selection is "known" or "both" or "known-and-drop" or "all-and-known"
                ? [GcloudPrivatecaTemplatesUpdateCopyKnownExtensions.BaseKeyUsage] : null,
            CopyExtensionsByOid = selection is "oid" or "both" ? ["1.1"] : null,
            DropKnownExtensions = selection == "known-and-drop" ? true : null,
            CopyAllRequestedExtensions = selection is "all" or "all-and-known" ? true : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("bitbucket", true)]
    [Arguments("bitbucket", false)]
    [Arguments("gitlab", true)]
    [Arguments("gitlab", false)]
    public async Task Provider_Host_Settings_Do_Not_Replace_Authorizer_Secrets(string provider, bool includeSecret)
    {
        var options = new GcloudDeveloperConnectConnectionsCreateOptions("connection")
        {
            BitbucketDataCenterConfigHostUri = provider == "bitbucket" ? "https://example.com" : null,
            BitbucketDataCenterConfigAuthorizerCredentialUserTokenSecretVersion = provider == "bitbucket" && includeSecret ? "authorizer" : null,
            BitbucketDataCenterConfigReadAuthorizerCredentialUserTokenSecretVersion = provider == "bitbucket" ? "reader" : null,
            BitbucketDataCenterConfigWebhookSecretVersion = provider == "bitbucket" ? "webhook" : null,
            GitlabEnterpriseConfigHostUri = provider == "gitlab" ? "https://example.com" : null,
            GitlabEnterpriseConfigAuthorizerCredentialUserTokenSecretVersion = provider == "gitlab" && includeSecret ? "authorizer" : null,
            GitlabEnterpriseConfigReadAuthorizerCredentialUserTokenSecretVersion = provider == "gitlab" ? "reader" : null,
            GitlabEnterpriseConfigWebhookSecretVersion = provider == "gitlab" ? "webhook" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(includeSecret);
    }

    [Test]
    [Arguments("enabled", true)]
    [Arguments("key", true)]
    [Arguments("keyring-only", false)]
    [Arguments("file", true)]
    [Arguments("mixed", false)]
    public async Task Kerberos_Resource_Requirements_Stay_Within_Their_Bundle(string selection, bool valid)
    {
        var options = new GcloudDataprocClustersCreateOptions("cluster")
        {
            EnableKerberos = selection is "enabled" or "key" or "mixed" ? true : null,
            KerberosKmsKey = selection == "key" ? "key" : null,
            KerberosKmsKeyKeyring = selection is "key" or "keyring-only" ? "ring" : null,
            KerberosConfigFile = selection is "file" or "mixed" ? "kerberos.yaml" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("environment", true)]
    [Arguments("location-only", false)]
    [Arguments("container", true)]
    [Arguments("tag-only", false)]
    [Arguments("family", true)]
    [Arguments("name", true)]
    [Arguments("both-images", false)]
    [Arguments("mixed", false)]
    public async Task Notebook_Image_Branches_Keep_Their_Resource_Selectors(string selection, bool valid)
    {
        var options = new GcloudNotebooksInstancesCreateOptions("instance")
        {
            Environment = selection is "environment" or "mixed" ? "environment" : null,
            EnvironmentLocation = selection is "environment" or "location-only" ? "us-central1" : null,
            ContainerRepository = selection is "container" or "mixed" ? "image" : null,
            ContainerTag = selection is "container" or "tag-only" ? "tag" : null,
            VmImageProject = selection is "family" or "name" or "both-images" ? "project" : null,
            VmImageFamily = selection is "family" or "both-images" ? "family" : null,
            VmImageName = selection is "name" or "both-images" ? "name" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments(typeof(GcloudApihubPluginsInstancesCreateOptions))]
    [Arguments(typeof(GcloudApihubPluginsInstancesUpdateOptions))]
    public async Task Api_Key_Http_Location_Does_Not_Register_Common_Words_As_Secrets(Type optionsType)
    {
        var property = optionsType
            .GetProperty(nameof(GcloudApihubPluginsInstancesCreateOptions.ApiKeyConfigHttpElementLocation))!;
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>()).IsNull();
    }

    [Test]
    [Arguments("inline", true)]
    [Arguments("secret", true)]
    [Arguments("none", false)]
    public async Task Oracle_Database_Bundle_Accepts_A_Password_Secret_Resource(string password, bool valid)
    {
        var options = new GcloudOracleDatabaseDbSystemsCreateOptions("display", "subnet", "system")
        {
            PropertiesComputeCount = 2,
            PropertiesDatabaseEdition = "edition",
            PropertiesInitialDataStorageSizeGb = 100,
            PropertiesLicenseModel = "license",
            PropertiesShape = "shape",
            PropertiesSshPublicKeys = ["ssh-key"],
            DbHomeVersion = "23",
            DatabaseAdminPassword = password == "inline" ? "password" : null,
            DatabaseAdminPasswordSecretVersion = password == "secret" ? "projects/project/secrets/password/versions/1" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", true)]
    [Arguments("password", true)]
    [Arguments("oauth", true)]
    [Arguments("both", false)]
    [Arguments("missing-secret", false)]
    public async Task Api_Hub_Authentication_Branches_Validate_Independently(string selection, bool valid)
    {
        var options = new GcloudApihubPluginsInstancesCreateOptions(["action"], "display", "instance")
        {
            AuthConfigType = selection == "none" ? null : "type",
            UserPasswordConfigUsername = selection is "password" or "both" ? "username" : null,
            UserPasswordConfigSecretVersion = selection is "password" or "both" ? "secret-version" : null,
            Oauth2ClientCredentialsConfigId = selection is "oauth" or "both" or "missing-secret" ? "client" : null,
            Oauth2ClientCredentialsConfigSecretVersion = selection is "oauth" or "both" ? "secret-version" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

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
    [Arguments("http", true)]
    [Arguments("http-directory", true)]
    [Arguments("http-directory-only", false)]
    [Arguments("http-bearer-directory", true)]
    public async Task Provider_Requirements_Apply_Only_To_The_Selected_Branch(string provider, bool valid)
    {
        var options = new GcloudDeveloperConnectConnectionsCreateOptions("connection")
        {
            SecureSourceManagerInstanceConfig = provider is "secure-source-manager" or "multiple"
                ? "projects/project/locations/us-central1/instances/source" : null,
            GithubConfigApp = provider is "github" or "multiple" ? "FIREBASE" : null,
            GithubConfigAppInstallationId = provider == "incomplete-github" ? "123" : null,
            HttpConfigHostUri = provider is "http" or "http-directory" or "http-bearer-directory" ? "https://example.com" : null,
            HttpConfigServiceDirectory = provider is "http-directory" or "http-directory-only" or "http-bearer-directory" ? "directory" : null,
            HttpConfigBearerTokenAuthenticationSecretVersion = provider == "http-bearer-directory" ? "secret-version" : null,
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
