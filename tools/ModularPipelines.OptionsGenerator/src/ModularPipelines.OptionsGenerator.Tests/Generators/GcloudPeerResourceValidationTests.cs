using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Gcloud_Mysql_Tls_Is_Optional_And_Requires_A_Complete_Certificate_Bundle()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("datastream connection-profiles create");
        var group = Descendants(command.RequiredAlternativeGroups)
            .Single(group => group.Members.Any(member => member.PropertyName == "MysqlHostname"));
        const string connection = "MysqlHostname,MysqlPort,MysqlUsername,MysqlPassword";
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            (connection, true),
            ("MysqlPort,MysqlUsername,MysqlPassword", false),
            (connection + ",CaCertificate", false),
            (connection + ",CaCertificate,ClientCertificate,ClientKey", true),
        ]);
    }

    [Test]
    public async Task Gcloud_Postgresql_Client_Certificates_Are_Optional_Within_Tls()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("datastream connection-profiles create");
        var group = Descendants(command.RequiredAlternativeGroups)
            .Single(group => group.Members.Any(member => member.PropertyName == "PostgresqlHostname"));
        const string connection = "PostgresqlDatabase,PostgresqlHostname,PostgresqlPort,PostgresqlUsername,PostgresqlPassword";
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            (connection, true),
            (connection + ",PostgresqlCaCertificate", true),
            (connection + ",PostgresqlCaCertificate,PostgresqlServerCertificateHostname", true),
            (connection + ",PostgresqlClientCertificate,PostgresqlClientKey", false),
            (connection + ",PostgresqlCaCertificate,PostgresqlClientCertificate", false),
            (connection + ",PostgresqlCaCertificate,PostgresqlClientCertificate,PostgresqlClientKey", true),
        ]);
    }

    [Test]
    public async Task Gcloud_Kerberos_Choice_Keeps_Its_Resource_Bundle_Optional()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("dataproc clusters create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("KerberosConfigFile"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("KerberosConfigFile", true),
            ("EnableKerberos", true),
            ("EnableKerberos,KerberosRootPrincipalPasswordUri", true),
            ("EnableKerberos,KerberosKmsKey", true),
            ("EnableKerberos,KerberosKmsKey,KerberosKmsKeyKeyring", true),
            ("KerberosKmsKeyKeyring", false),
            ("EnableKerberos,KerberosConfigFile", false),
            ("KerberosKmsKey,KerberosConfigFile", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Notebook_Image_Branches_Retain_Resource_Selectors()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("notebooks instances create");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("Environment"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("Environment", true),
            ("Environment,EnvironmentLocation", true),
            ("EnvironmentLocation", false),
            ("ContainerRepository", true),
            ("ContainerRepository,ContainerTag", true),
            ("ContainerTag", false),
            ("VmImageProject,VmImageFamily", true),
            ("VmImageProject,VmImageName", true),
            ("VmImageFamily,VmImageName", false),
            ("Environment,ContainerRepository", false),
            ("Environment,VmImageFamily", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Authentication_Branches_Remain_Independent()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("apihub plugins instances create");
        await Assert.That(command.Options.Single(option => option.PropertyName == "UserPasswordConfigUsername").Description)
            .DoesNotContain("Oauth");
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("AuthConfigType"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("AuthConfigType", true),
            ("AuthConfigType,UserPasswordConfigSecretVersion,UserPasswordConfigUsername", true),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId,Oauth2ClientCredentialsConfigSecretVersion", true),
            ("AuthConfigType,ApiKeyConfigHttpElementLocation,ApiKeyConfigName,ApiKeyConfigSecretVersion", true),
            ("UserPasswordConfigSecretVersion,UserPasswordConfigUsername", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigSecretVersion", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId", false),
            ("AuthConfigType,UserPasswordConfigUsername", false),
            ("AuthConfigType,Oauth2ClientCredentialsConfigId,Oauth2ClientCredentialsConfigSecretVersion,UserPasswordConfigSecretVersion,UserPasswordConfigUsername", false),
        ]);
    }

    [Test]
    public async Task Gcloud_Required_Database_Bundle_Retains_Resource_Members()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("oracle-database db-systems create");
        var group = Descendants(command.RequiredAlternativeGroups)
            .Single(group => group.Members.Any(member => member.PropertyName == "DbHomeVersion"));
        await ValidateCapturedGroup(command, group,
        [
            ("", true),
            ("DbHomeVersion", false),
            ("DbHomeVersion,DatabaseAdminPassword", true),
            ("DbHomeVersion,DatabaseAdminPasswordSecretVersion", true),
            ("DbHomeVersion,DatabaseDbName", true),
            ("DatabaseAdminPasswordSecretVersion", false),
        ]);
    }

    private static IEnumerable<CliRequiredAlternativeGroup> Descendants(IEnumerable<CliRequiredAlternativeGroup> groups) =>
        groups.SelectMany(group => new[] { group }.Concat(Descendants(group.Groups)));

    private static async Task ValidateCapturedGroup(CliCommandDefinition command, CliRequiredAlternativeGroup group,
        (string Properties, bool Valid)[] cases)
    {
        var options = command.Options.Where(option => group.PropertyNames.Contains(option.PropertyName)).ToList();
        var generated = await Generate(options, alternativeGroups: [group]);
        const string secretAttribute = "namespace ModularPipelines.Secrets { public sealed class SecretValueAttribute : System.Attribute; }";
        var optionsType = Compile(generated, secretAttribute).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        foreach (var (properties, valid) in cases)
        {
            var instance = Activator.CreateInstance(optionsType)!;
            foreach (var name in properties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var property = optionsType.GetProperty(name)!;
                object value = "value";
                if (property.PropertyType == typeof(bool?))
                {
                    value = true;
                }
                else if (property.PropertyType == typeof(int?))
                {
                    value = 1;
                }
                else if (property.PropertyType == typeof(IEnumerable<string>))
                {
                    value = new[] { "value" };
                }
                else if (property.PropertyType == typeof(IReadOnlyList<ModularPipelines.Models.KeyValue>))
                {
                    value = new[] { new ModularPipelines.Models.KeyValue("key", "value", "=") };
                }
                property.SetValue(instance, value);
            }

            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true))
                .IsEqualTo(valid).Because($"Selected {properties}: {string.Join("; ", errors)}");
        }
    }
}
