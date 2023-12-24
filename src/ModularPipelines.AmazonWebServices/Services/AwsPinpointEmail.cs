using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsPinpointEmail
{
    public AwsPinpointEmail(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointEmailCreateConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsPinpointEmailCreateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDedicatedIpPool(AwsPinpointEmailCreateDedicatedIpPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDeliverabilityTestReport(AwsPinpointEmailCreateDeliverabilityTestReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateEmailIdentity(AwsPinpointEmailCreateEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointEmailDeleteConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsPinpointEmailDeleteConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDedicatedIpPool(AwsPinpointEmailDeleteDedicatedIpPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteEmailIdentity(AwsPinpointEmailDeleteEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetAccount(AwsPinpointEmailGetAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetAccountOptions(), token);
    }

    public async Task<CommandResult> GetBlacklistReports(AwsPinpointEmailGetBlacklistReportsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationSet(AwsPinpointEmailGetConfigurationSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConfigurationSetEventDestinations(AwsPinpointEmailGetConfigurationSetEventDestinationsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDedicatedIp(AwsPinpointEmailGetDedicatedIpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDedicatedIps(AwsPinpointEmailGetDedicatedIpsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetDedicatedIpsOptions(), token);
    }

    public async Task<CommandResult> GetDeliverabilityDashboardOptions(AwsPinpointEmailGetDeliverabilityDashboardOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetDeliverabilityDashboardOptionsOptions(), token);
    }

    public async Task<CommandResult> GetDeliverabilityTestReport(AwsPinpointEmailGetDeliverabilityTestReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomainDeliverabilityCampaign(AwsPinpointEmailGetDomainDeliverabilityCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDomainStatisticsReport(AwsPinpointEmailGetDomainStatisticsReportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEmailIdentity(AwsPinpointEmailGetEmailIdentityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConfigurationSets(AwsPinpointEmailListConfigurationSetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListConfigurationSetsOptions(), token);
    }

    public async Task<CommandResult> ListDedicatedIpPools(AwsPinpointEmailListDedicatedIpPoolsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListDedicatedIpPoolsOptions(), token);
    }

    public async Task<CommandResult> ListDeliverabilityTestReports(AwsPinpointEmailListDeliverabilityTestReportsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListDeliverabilityTestReportsOptions(), token);
    }

    public async Task<CommandResult> ListDomainDeliverabilityCampaigns(AwsPinpointEmailListDomainDeliverabilityCampaignsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListEmailIdentities(AwsPinpointEmailListEmailIdentitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListEmailIdentitiesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsPinpointEmailListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutAccountDedicatedIpWarmupAttributes(AwsPinpointEmailPutAccountDedicatedIpWarmupAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutAccountDedicatedIpWarmupAttributesOptions(), token);
    }

    public async Task<CommandResult> PutAccountSendingAttributes(AwsPinpointEmailPutAccountSendingAttributesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutAccountSendingAttributesOptions(), token);
    }

    public async Task<CommandResult> PutConfigurationSetDeliveryOptions(AwsPinpointEmailPutConfigurationSetDeliveryOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetReputationOptions(AwsPinpointEmailPutConfigurationSetReputationOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetSendingOptions(AwsPinpointEmailPutConfigurationSetSendingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutConfigurationSetTrackingOptions(AwsPinpointEmailPutConfigurationSetTrackingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDedicatedIpInPool(AwsPinpointEmailPutDedicatedIpInPoolOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDedicatedIpWarmupAttributes(AwsPinpointEmailPutDedicatedIpWarmupAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutDeliverabilityDashboardOption(AwsPinpointEmailPutDeliverabilityDashboardOptionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutDeliverabilityDashboardOptionOptions(), token);
    }

    public async Task<CommandResult> PutEmailIdentityDkimAttributes(AwsPinpointEmailPutEmailIdentityDkimAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityFeedbackAttributes(AwsPinpointEmailPutEmailIdentityFeedbackAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEmailIdentityMailFromAttributes(AwsPinpointEmailPutEmailIdentityMailFromAttributesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SendEmail(AwsPinpointEmailSendEmailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsPinpointEmailTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsPinpointEmailUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsPinpointEmailUpdateConfigurationSetEventDestinationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}