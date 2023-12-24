using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSes
{
    public AwsSes(
        AwsSesWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsSesWait Wait { get; }

    public async Task<CommandResult> CloneReceiptRuleSet(AwsSesCloneReceiptRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSet(AwsSesCreateConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsSesCreateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSetTrackingOptions(AwsSesCreateConfigurationSetTrackingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomVerificationEmailTemplate(AwsSesCreateCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReceiptFilter(AwsSesCreateReceiptFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReceiptRule(AwsSesCreateReceiptRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateReceiptRuleSet(AwsSesCreateReceiptRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTemplate(AwsSesCreateTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsSesDeleteConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsSesDeleteConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSetTrackingOptions(AwsSesDeleteConfigurationSetTrackingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomVerificationEmailTemplate(AwsSesDeleteCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIdentity(AwsSesDeleteIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIdentityPolicy(AwsSesDeleteIdentityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReceiptFilter(AwsSesDeleteReceiptFilterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReceiptRule(AwsSesDeleteReceiptRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReceiptRuleSet(AwsSesDeleteReceiptRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTemplate(AwsSesDeleteTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeActiveReceiptRuleSet(AwsSesDescribeActiveReceiptRuleSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesDescribeActiveReceiptRuleSetOptions(), token);
    }

    public async Task<CommandResult> DescribeConfigurationSet(AwsSesDescribeConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReceiptRule(AwsSesDescribeReceiptRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReceiptRuleSet(AwsSesDescribeReceiptRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccountSendingEnabled(AwsSesGetAccountSendingEnabledOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesGetAccountSendingEnabledOptions(), token);
    }

    public async Task<CommandResult> GetCustomVerificationEmailTemplate(AwsSesGetCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityDkimAttributes(AwsSesGetIdentityDkimAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityMailFromDomainAttributes(AwsSesGetIdentityMailFromDomainAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityNotificationAttributes(AwsSesGetIdentityNotificationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityPolicies(AwsSesGetIdentityPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetIdentityVerificationAttributes(AwsSesGetIdentityVerificationAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSendQuota(AwsSesGetSendQuotaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesGetSendQuotaOptions(), token);
    }

    public async Task<CommandResult> GetSendStatistics(AwsSesGetSendStatisticsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesGetSendStatisticsOptions(), token);
    }

    public async Task<CommandResult> GetTemplate(AwsSesGetTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConfigurationSets(AwsSesListConfigurationSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListConfigurationSetsOptions(), token);
    }

    public async Task<CommandResult> ListCustomVerificationEmailTemplates(AwsSesListCustomVerificationEmailTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListCustomVerificationEmailTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListIdentities(AwsSesListIdentitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListIdentitiesOptions(), token);
    }

    public async Task<CommandResult> ListIdentityPolicies(AwsSesListIdentityPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListReceiptFilters(AwsSesListReceiptFiltersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListReceiptFiltersOptions(), token);
    }

    public async Task<CommandResult> ListReceiptRuleSets(AwsSesListReceiptRuleSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListReceiptRuleSetsOptions(), token);
    }

    public async Task<CommandResult> ListTemplates(AwsSesListTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesListTemplatesOptions(), token);
    }

    public async Task<CommandResult> PutConfigurationSetDeliveryOptions(AwsSesPutConfigurationSetDeliveryOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutIdentityPolicy(AwsSesPutIdentityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ReorderReceiptRuleSet(AwsSesReorderReceiptRuleSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendBounce(AwsSesSendBounceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendBulkTemplatedEmail(AwsSesSendBulkTemplatedEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendCustomVerificationEmail(AwsSesSendCustomVerificationEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendEmail(AwsSesSendEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendRawEmail(AwsSesSendRawEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendTemplatedEmail(AwsSesSendTemplatedEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetActiveReceiptRuleSet(AwsSesSetActiveReceiptRuleSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesSetActiveReceiptRuleSetOptions(), token);
    }

    public async Task<CommandResult> SetIdentityDkimEnabled(AwsSesSetIdentityDkimEnabledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIdentityFeedbackForwardingEnabled(AwsSesSetIdentityFeedbackForwardingEnabledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIdentityHeadersInNotificationsEnabled(AwsSesSetIdentityHeadersInNotificationsEnabledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIdentityMailFromDomain(AwsSesSetIdentityMailFromDomainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetIdentityNotificationTopic(AwsSesSetIdentityNotificationTopicOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetReceiptRulePosition(AwsSesSetReceiptRulePositionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestRenderTemplate(AwsSesTestRenderTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccountSendingEnabled(AwsSesUpdateAccountSendingEnabledOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesUpdateAccountSendingEnabledOptions(), token);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsSesUpdateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationSetReputationMetricsEnabled(AwsSesUpdateConfigurationSetReputationMetricsEnabledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationSetSendingEnabled(AwsSesUpdateConfigurationSetSendingEnabledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationSetTrackingOptions(AwsSesUpdateConfigurationSetTrackingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomVerificationEmailTemplate(AwsSesUpdateCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReceiptRule(AwsSesUpdateReceiptRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateTemplate(AwsSesUpdateTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyDomainDkim(AwsSesVerifyDomainDkimOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyDomainIdentity(AwsSesVerifyDomainIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VerifyEmailIdentity(AwsSesVerifyEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}