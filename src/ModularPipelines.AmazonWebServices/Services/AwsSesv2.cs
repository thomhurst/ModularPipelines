using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsSesv2
{
    public AwsSesv2(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BatchGetMetricData(AwsSesv2BatchGetMetricDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelExportJob(AwsSesv2CancelExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSet(AwsSesv2CreateConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsSesv2CreateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContact(AwsSesv2CreateContactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateContactList(AwsSesv2CreateContactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCustomVerificationEmailTemplate(AwsSesv2CreateCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDedicatedIpPool(AwsSesv2CreateDedicatedIpPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeliverabilityTestReport(AwsSesv2CreateDeliverabilityTestReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEmailIdentity(AwsSesv2CreateEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEmailIdentityPolicy(AwsSesv2CreateEmailIdentityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEmailTemplate(AwsSesv2CreateEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateExportJob(AwsSesv2CreateExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateImportJob(AwsSesv2CreateImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsSesv2DeleteConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsSesv2DeleteConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContact(AwsSesv2DeleteContactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteContactList(AwsSesv2DeleteContactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCustomVerificationEmailTemplate(AwsSesv2DeleteCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDedicatedIpPool(AwsSesv2DeleteDedicatedIpPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEmailIdentity(AwsSesv2DeleteEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEmailIdentityPolicy(AwsSesv2DeleteEmailIdentityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEmailTemplate(AwsSesv2DeleteEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSuppressedDestination(AwsSesv2DeleteSuppressedDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccount(AwsSesv2GetAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2GetAccountOptions(), token);
    }

    public async Task<CommandResult> GetBlacklistReports(AwsSesv2GetBlacklistReportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationSet(AwsSesv2GetConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationSetEventDestinations(AwsSesv2GetConfigurationSetEventDestinationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContact(AwsSesv2GetContactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetContactList(AwsSesv2GetContactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCustomVerificationEmailTemplate(AwsSesv2GetCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDedicatedIp(AwsSesv2GetDedicatedIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDedicatedIpPool(AwsSesv2GetDedicatedIpPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDedicatedIps(AwsSesv2GetDedicatedIpsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2GetDedicatedIpsOptions(), token);
    }

    public async Task<CommandResult> GetDeliverabilityDashboardOptions(AwsSesv2GetDeliverabilityDashboardOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2GetDeliverabilityDashboardOptionsOptions(), token);
    }

    public async Task<CommandResult> GetDeliverabilityTestReport(AwsSesv2GetDeliverabilityTestReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomainDeliverabilityCampaign(AwsSesv2GetDomainDeliverabilityCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomainStatisticsReport(AwsSesv2GetDomainStatisticsReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEmailIdentity(AwsSesv2GetEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEmailIdentityPolicies(AwsSesv2GetEmailIdentityPoliciesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEmailTemplate(AwsSesv2GetEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetExportJob(AwsSesv2GetExportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetImportJob(AwsSesv2GetImportJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMessageInsights(AwsSesv2GetMessageInsightsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSuppressedDestination(AwsSesv2GetSuppressedDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConfigurationSets(AwsSesv2ListConfigurationSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListConfigurationSetsOptions(), token);
    }

    public async Task<CommandResult> ListContactLists(AwsSesv2ListContactListsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListContactListsOptions(), token);
    }

    public async Task<CommandResult> ListContacts(AwsSesv2ListContactsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCustomVerificationEmailTemplates(AwsSesv2ListCustomVerificationEmailTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListCustomVerificationEmailTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListDedicatedIpPools(AwsSesv2ListDedicatedIpPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListDedicatedIpPoolsOptions(), token);
    }

    public async Task<CommandResult> ListDeliverabilityTestReports(AwsSesv2ListDeliverabilityTestReportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListDeliverabilityTestReportsOptions(), token);
    }

    public async Task<CommandResult> ListDomainDeliverabilityCampaigns(AwsSesv2ListDomainDeliverabilityCampaignsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEmailIdentities(AwsSesv2ListEmailIdentitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListEmailIdentitiesOptions(), token);
    }

    public async Task<CommandResult> ListEmailTemplates(AwsSesv2ListEmailTemplatesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListEmailTemplatesOptions(), token);
    }

    public async Task<CommandResult> ListExportJobs(AwsSesv2ListExportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListExportJobsOptions(), token);
    }

    public async Task<CommandResult> ListImportJobs(AwsSesv2ListImportJobsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListImportJobsOptions(), token);
    }

    public async Task<CommandResult> ListRecommendations(AwsSesv2ListRecommendationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListRecommendationsOptions(), token);
    }

    public async Task<CommandResult> ListSuppressedDestinations(AwsSesv2ListSuppressedDestinationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2ListSuppressedDestinationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsSesv2ListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAccountDedicatedIpWarmupAttributes(AwsSesv2PutAccountDedicatedIpWarmupAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2PutAccountDedicatedIpWarmupAttributesOptions(), token);
    }

    public async Task<CommandResult> PutAccountDetails(AwsSesv2PutAccountDetailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAccountSendingAttributes(AwsSesv2PutAccountSendingAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2PutAccountSendingAttributesOptions(), token);
    }

    public async Task<CommandResult> PutAccountSuppressionAttributes(AwsSesv2PutAccountSuppressionAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2PutAccountSuppressionAttributesOptions(), token);
    }

    public async Task<CommandResult> PutAccountVdmAttributes(AwsSesv2PutAccountVdmAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetDeliveryOptions(AwsSesv2PutConfigurationSetDeliveryOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetReputationOptions(AwsSesv2PutConfigurationSetReputationOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetSendingOptions(AwsSesv2PutConfigurationSetSendingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetSuppressionOptions(AwsSesv2PutConfigurationSetSuppressionOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetTrackingOptions(AwsSesv2PutConfigurationSetTrackingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetVdmOptions(AwsSesv2PutConfigurationSetVdmOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDedicatedIpInPool(AwsSesv2PutDedicatedIpInPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDedicatedIpPoolScalingAttributes(AwsSesv2PutDedicatedIpPoolScalingAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDedicatedIpWarmupAttributes(AwsSesv2PutDedicatedIpWarmupAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliverabilityDashboardOption(AwsSesv2PutDeliverabilityDashboardOptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsSesv2PutDeliverabilityDashboardOptionOptions(), token);
    }

    public async Task<CommandResult> PutEmailIdentityConfigurationSetAttributes(AwsSesv2PutEmailIdentityConfigurationSetAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityDkimAttributes(AwsSesv2PutEmailIdentityDkimAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityDkimSigningAttributes(AwsSesv2PutEmailIdentityDkimSigningAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityFeedbackAttributes(AwsSesv2PutEmailIdentityFeedbackAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityMailFromAttributes(AwsSesv2PutEmailIdentityMailFromAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSuppressedDestination(AwsSesv2PutSuppressedDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendBulkEmail(AwsSesv2SendBulkEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendCustomVerificationEmail(AwsSesv2SendCustomVerificationEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendEmail(AwsSesv2SendEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsSesv2TagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestRenderEmailTemplate(AwsSesv2TestRenderEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsSesv2UntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsSesv2UpdateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContact(AwsSesv2UpdateContactOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateContactList(AwsSesv2UpdateContactListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCustomVerificationEmailTemplate(AwsSesv2UpdateCustomVerificationEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEmailIdentityPolicy(AwsSesv2UpdateEmailIdentityPolicyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateEmailTemplate(AwsSesv2UpdateEmailTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}