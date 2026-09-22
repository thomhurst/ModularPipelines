using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public class GcloudSecretMetadataTests
{
    [Test]
    [Arguments("iam workforce-pools create-cred-config", "CredentialSourceType,SubjectTokenType,CredentialSourceUrl", "CredentialSourceHeaders")]
    [Arguments("iam workload-identity-pools create-cred-config", "CredentialSourceType,SubjectTokenType,CredentialSourceUrl", "CredentialSourceHeaders")]
    [Arguments("biglake iceberg catalogs create", "CredentialMode", "")]
    [Arguments("scheduler jobs create http", "OauthTokenScope,OidcTokenAudience", "")]
    [Arguments("tasks queues create", "HttpOauthTokenScopeOverride,HttpOidcTokenAudienceOverride", "")]
    [Arguments("container ai profiles list", "TargetCostPerMillionInputTokens,TargetCostPerMillionOutputTokens", "")]
    [Arguments("functions deploy", "SetSecrets,UpdateSecrets,RemoveSecrets", "")]
    [Arguments("run deploy", "SetSecrets,UpdateSecrets,RemoveSecrets", "")]
    [Arguments("agent-identity auth-providers create", "ThreeLeggedOauthTokenUrl,TwoLeggedOauthTokenUrl", "ThreeLeggedOauthClientSecret,TwoLeggedOauthClientSecret")]
    [Arguments("container aws clusters create", "ProxySecretArn", "")]
    [Arguments("sql backups restore", "ActiveDirectorySecretManagerKey", "")]
    [Arguments("sql instances create", "PasswordPolicyComplexity,PasswordPolicyPasswordChangeInterval", "RootPassword")]
    [Arguments("apihub plugins instances create", "ApiKeyConfigSecretVersion,Oauth2ClientCredentialsConfigSecretVersion,UserPasswordConfigSecretVersion", "")]
    [Arguments("secrets versions access", "Secret", "")]
    [Arguments("builds triggers create webhook", "Secret", "")]
    [Arguments("managed-kafka connect-clusters create", "Secret", "")]
    [Arguments("developer-connect connections create", "Secret", "")]
    public async Task Captured_Metadata_And_References_Are_Visible_While_Credentials_Stay_Secret(
        string commandName, string metadataNames, string secretNames)
    {
        var command = await GcloudCapturedSemanticsTests.Scrape(commandName);
        await AssertClassificationsBeforeAndAfterEnhancement(command, metadataNames, secretNames);
    }

    [Test]
    [Arguments("password", "PASSWORD", "Password", "The password value to send for authentication.")]
    [Arguments("password", "SECRET_VALUE_REF", "Password", "The password value to send for authentication.")]
    [Arguments("password", "KEY=VALUE", "Password", "Values should be in the form SECRET_NAME:SECRET_VERSION. The password value to send.")]
    [Arguments("password", "PASSWORD", "Password", "Password for authentication.")]
    [Arguments("token", "TOKEN", "Token", "The token contents to send for authentication.")]
    [Arguments("token", "TOKEN", "Token", "Token for authentication.")]
    [Arguments("service-account-key-file", "FILE", "ServiceAccountKeyFile", "The base64 encoded content of the service account key file.")]
    public async Task Inherited_Reference_Documentation_Does_Not_Unmask_Literal_Credentials(
        string switchName, string valueHint, string propertyName, string description)
    {
        var commands = await GcloudResourceArgumentTests.ScrapeFixture("example authenticate", $$"""
            NAME
                gcloud example authenticate - authenticate with a credential
            SYNOPSIS
                gcloud example authenticate [--secret-bindings=[KEY=VALUE,...] | --{{switchName}}={{valueHint}}]
            FLAGS
                At most one of these can be specified:

                  Secret bindings values should be in the form SECRET_NAME:SECRET_VERSION.

                    --secret-bindings=[KEY=VALUE,...]
                        The bindings to use.

                    --{{switchName}}={{valueHint}}
                        {{description}}
            """);
        var command = commands.Single();
        await Assert.That(command.Options.Single(option => option.PropertyName == propertyName).Description!)
            .Contains("SECRET_NAME:SECRET_VERSION");
        await AssertClassificationsBeforeAndAfterEnhancement(command, "SecretBindings", propertyName);
    }

    private static async Task AssertClassificationsBeforeAndAfterEnhancement(
        CliCommandDefinition command, string metadataNames, string secretNames)
    {
        await AssertClassifications(command, metadataNames, false);
        await AssertClassifications(command, secretNames, true);

        var enhancer = new OptionTypeEnhancer(
            new OptionTypeDetectorPipeline([], NullLogger<OptionTypeDetectorPipeline>.Instance),
            NullLogger<OptionTypeEnhancer>.Instance);
        var tool = await enhancer.EnhanceAsync(new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "src/ModularPipelines.Google",
            Commands = [command],
        });
        await AssertClassifications(tool.Commands.Single(), metadataNames, false);
        await AssertClassifications(tool.Commands.Single(), secretNames, true);
    }

    [Test]
    public async Task Positional_Credentials_Use_Their_Own_Prose_For_Secret_Classification()
    {
        var commands = await GcloudResourceArgumentTests.ScrapeFixture("example authenticate", """
            NAME
                gcloud example authenticate - authenticate with a credential
            SYNOPSIS
                gcloud example authenticate CREDENTIAL
            POSITIONAL ARGUMENTS
                Credential resource - Authentication settings. To set the project attribute:
                provide the project argument with a fully specified name.

                    CREDENTIAL
                        The credential value to send for authentication.
            """);
        await Assert.That(commands.Single().PositionalArguments.Single().IsSecret).IsTrue();
    }

    private static async Task AssertClassifications(CliCommandDefinition command, string names, bool secret)
    {
        foreach (var name in names.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            await Assert.That(command.Options.Single(option => option.PropertyName == name).IsSecret)
                .IsEqualTo(secret).Because($"{command.FullCommand}: {name}");
        }
    }
}
