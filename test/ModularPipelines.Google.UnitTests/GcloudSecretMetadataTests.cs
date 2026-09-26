using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Extensions;
using ModularPipelines.Google.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Google.UnitTests;

public class GcloudSecretMetadataTests
{
    [Test]
    [Arguments("text")]
    [Arguments("json")]
    public async Task Log_Masking_Preserves_Credential_Formats_And_Redacts_Credential_Headers(string format)
    {
        await using var pipeline = await TestPipelineBuilder.Create().AddModule<TrueModule>().BuildAsync();
        var options = new GcloudIamWorkforcePoolsCreateCredConfigOptions("credentials.json", "project", "pool/provider")
        {
            CredentialSourceUrl = "https://example.com/credentials",
            CredentialSourceType = format,
            CredentialSourceHeaders = "Authorization=private-credential",
        };
        var obfuscator = pipeline.Services.GetRequiredService<ISecretObfuscator>();

        var output = obfuscator.Obfuscate($"{format} output includes Authorization=private-credential", options);

        await Assert.That(output).IsEqualTo($"{format} output includes {new SecretMaskingOptions().MaskValue}");
    }

    [Test]
    [Arguments(typeof(GcloudIamWorkforcePoolsCreateCredConfigOptions), nameof(GcloudIamWorkforcePoolsCreateCredConfigOptions.CredentialSourceType), false)]
    [Arguments(typeof(GcloudIamWorkforcePoolsCreateCredConfigOptions), nameof(GcloudIamWorkforcePoolsCreateCredConfigOptions.SubjectTokenType), false)]
    [Arguments(typeof(GcloudIamWorkforcePoolsCreateCredConfigOptions), nameof(GcloudIamWorkforcePoolsCreateCredConfigOptions.CredentialSourceUrl), false)]
    [Arguments(typeof(GcloudIamWorkforcePoolsCreateCredConfigOptions), nameof(GcloudIamWorkforcePoolsCreateCredConfigOptions.CredentialSourceHeaders), true)]
    [Arguments(typeof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions), nameof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions.CredentialSourceType), false)]
    [Arguments(typeof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions), nameof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions.SubjectTokenType), false)]
    [Arguments(typeof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions), nameof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions.CredentialSourceUrl), false)]
    [Arguments(typeof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions), nameof(GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions.CredentialSourceHeaders), true)]
    [Arguments(typeof(GcloudBiglakeIcebergCatalogsCreateOptions), nameof(GcloudBiglakeIcebergCatalogsCreateOptions.CredentialMode), false)]
    [Arguments(typeof(GcloudSchedulerJobsCreateHttpOptions), nameof(GcloudSchedulerJobsCreateHttpOptions.OauthTokenScope), false)]
    [Arguments(typeof(GcloudSchedulerJobsCreateHttpOptions), nameof(GcloudSchedulerJobsCreateHttpOptions.OidcTokenAudience), false)]
    [Arguments(typeof(GcloudTasksQueuesCreateOptions), nameof(GcloudTasksQueuesCreateOptions.HttpOauthTokenScopeOverride), false)]
    [Arguments(typeof(GcloudTasksQueuesCreateOptions), nameof(GcloudTasksQueuesCreateOptions.HttpOidcTokenAudienceOverride), false)]
    [Arguments(typeof(GcloudContainerAiProfilesListOptions), nameof(GcloudContainerAiProfilesListOptions.TargetCostPerMillionInputTokens), false)]
    [Arguments(typeof(GcloudContainerAiProfilesListOptions), nameof(GcloudContainerAiProfilesListOptions.TargetCostPerMillionOutputTokens), false)]
    [Arguments(typeof(GcloudFunctionsDeployOptions), nameof(GcloudFunctionsDeployOptions.SetSecrets), false)]
    [Arguments(typeof(GcloudFunctionsDeployOptions), nameof(GcloudFunctionsDeployOptions.UpdateSecrets), false)]
    [Arguments(typeof(GcloudFunctionsDeployOptions), nameof(GcloudFunctionsDeployOptions.RemoveSecrets), false)]
    [Arguments(typeof(GcloudRunDeployOptions), nameof(GcloudRunDeployOptions.SetSecrets), false)]
    [Arguments(typeof(GcloudRunDeployOptions), nameof(GcloudRunDeployOptions.UpdateSecrets), false)]
    [Arguments(typeof(GcloudRunDeployOptions), nameof(GcloudRunDeployOptions.RemoveSecrets), false)]
    [Arguments(typeof(GcloudAgentIdentityAuthProvidersCreateOptions), nameof(GcloudAgentIdentityAuthProvidersCreateOptions.ThreeLeggedOauthTokenUrl), false)]
    [Arguments(typeof(GcloudAgentIdentityAuthProvidersCreateOptions), nameof(GcloudAgentIdentityAuthProvidersCreateOptions.TwoLeggedOauthTokenUrl), false)]
    [Arguments(typeof(GcloudAgentIdentityAuthProvidersCreateOptions), nameof(GcloudAgentIdentityAuthProvidersCreateOptions.ThreeLeggedOauthClientSecret), true)]
    [Arguments(typeof(GcloudAgentIdentityAuthProvidersCreateOptions), nameof(GcloudAgentIdentityAuthProvidersCreateOptions.TwoLeggedOauthClientSecret), true)]
    [Arguments(typeof(GcloudContainerAwsClustersCreateOptions), nameof(GcloudContainerAwsClustersCreateOptions.ProxySecretArn), false)]
    [Arguments(typeof(GcloudSqlBackupsRestoreOptions), nameof(GcloudSqlBackupsRestoreOptions.ActiveDirectorySecretManagerKey), false)]
    [Arguments(typeof(GcloudSqlInstancesCreateOptions), nameof(GcloudSqlInstancesCreateOptions.PasswordPolicyComplexity), false)]
    [Arguments(typeof(GcloudSqlInstancesCreateOptions), nameof(GcloudSqlInstancesCreateOptions.PasswordPolicyPasswordChangeInterval), false)]
    [Arguments(typeof(GcloudSqlInstancesCreateOptions), nameof(GcloudSqlInstancesCreateOptions.RootPassword), true)]
    [Arguments(typeof(GcloudApihubPluginsInstancesCreateOptions), nameof(GcloudApihubPluginsInstancesCreateOptions.ApiKeyConfigSecretVersion), false)]
    [Arguments(typeof(GcloudApihubPluginsInstancesCreateOptions), nameof(GcloudApihubPluginsInstancesCreateOptions.Oauth2ClientCredentialsConfigSecretVersion), false)]
    [Arguments(typeof(GcloudApihubPluginsInstancesCreateOptions), nameof(GcloudApihubPluginsInstancesCreateOptions.UserPasswordConfigSecretVersion), false)]
    [Arguments(typeof(GcloudSecretsVersionsAccessOptions), nameof(GcloudSecretsVersionsAccessOptions.Secret), false)]
    [Arguments(typeof(GcloudBuildsTriggersCreateWebhookOptions), nameof(GcloudBuildsTriggersCreateWebhookOptions.Secret), false)]
    [Arguments(typeof(GcloudManagedKafkaConnectClustersCreateOptions), nameof(GcloudManagedKafkaConnectClustersCreateOptions.Secret), false)]
    [Arguments(typeof(GcloudDeveloperConnectConnectionsCreateOptions), nameof(GcloudDeveloperConnectConnectionsCreateOptions.Secret), false)]
    [Arguments(typeof(GcloudOracleDatabaseGoldengateConnectionsCreateOptions), nameof(GcloudOracleDatabaseGoldengateConnectionsCreateOptions.Secret), false)]
    public async Task Credential_Values_Are_Masked_While_Metadata_And_References_Remain_Visible(
        Type optionsType, string propertyName, bool secret)
    {
        var property = optionsType.GetProperty(propertyName)!;
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>() is not null).IsEqualTo(secret);
    }
}
