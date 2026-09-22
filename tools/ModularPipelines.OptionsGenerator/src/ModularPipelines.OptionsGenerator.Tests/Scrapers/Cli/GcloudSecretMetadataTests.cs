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
