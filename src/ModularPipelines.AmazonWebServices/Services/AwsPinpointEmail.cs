using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> CreateConfigurationSet(AwsPinpointEmailCreateConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateConfigurationSetEventDestination(AwsPinpointEmailCreateConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDedicatedIpPool(AwsPinpointEmailCreateDedicatedIpPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateDeliverabilityTestReport(AwsPinpointEmailCreateDeliverabilityTestReportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> CreateEmailIdentity(AwsPinpointEmailCreateEmailIdentityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationSet(AwsPinpointEmailDeleteConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteConfigurationSetEventDestination(AwsPinpointEmailDeleteConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteDedicatedIpPool(AwsPinpointEmailDeleteDedicatedIpPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteEmailIdentity(AwsPinpointEmailDeleteEmailIdentityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAccount(AwsPinpointEmailGetAccountOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetAccountOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetBlacklistReports(AwsPinpointEmailGetBlacklistReportsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfigurationSet(AwsPinpointEmailGetConfigurationSetOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetConfigurationSetEventDestinations(AwsPinpointEmailGetConfigurationSetEventDestinationsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDedicatedIp(AwsPinpointEmailGetDedicatedIpOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDedicatedIps(AwsPinpointEmailGetDedicatedIpsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetDedicatedIpsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDeliverabilityDashboardOptions(AwsPinpointEmailGetDeliverabilityDashboardOptionsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailGetDeliverabilityDashboardOptionsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDeliverabilityTestReport(AwsPinpointEmailGetDeliverabilityTestReportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDomainDeliverabilityCampaign(AwsPinpointEmailGetDomainDeliverabilityCampaignOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetDomainStatisticsReport(AwsPinpointEmailGetDomainStatisticsReportOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetEmailIdentity(AwsPinpointEmailGetEmailIdentityOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListConfigurationSets(AwsPinpointEmailListConfigurationSetsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListConfigurationSetsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDedicatedIpPools(AwsPinpointEmailListDedicatedIpPoolsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListDedicatedIpPoolsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDeliverabilityTestReports(AwsPinpointEmailListDeliverabilityTestReportsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListDeliverabilityTestReportsOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListDomainDeliverabilityCampaigns(AwsPinpointEmailListDomainDeliverabilityCampaignsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListEmailIdentities(AwsPinpointEmailListEmailIdentitiesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailListEmailIdentitiesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsPinpointEmailListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAccountDedicatedIpWarmupAttributes(AwsPinpointEmailPutAccountDedicatedIpWarmupAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutAccountDedicatedIpWarmupAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutAccountSendingAttributes(AwsPinpointEmailPutAccountSendingAttributesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutAccountSendingAttributesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationSetDeliveryOptions(AwsPinpointEmailPutConfigurationSetDeliveryOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationSetReputationOptions(AwsPinpointEmailPutConfigurationSetReputationOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationSetSendingOptions(AwsPinpointEmailPutConfigurationSetSendingOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutConfigurationSetTrackingOptions(AwsPinpointEmailPutConfigurationSetTrackingOptionsOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutDedicatedIpInPool(AwsPinpointEmailPutDedicatedIpInPoolOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutDedicatedIpWarmupAttributes(AwsPinpointEmailPutDedicatedIpWarmupAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutDeliverabilityDashboardOption(AwsPinpointEmailPutDeliverabilityDashboardOptionOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsPinpointEmailPutDeliverabilityDashboardOptionOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutEmailIdentityDkimAttributes(AwsPinpointEmailPutEmailIdentityDkimAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutEmailIdentityFeedbackAttributes(AwsPinpointEmailPutEmailIdentityFeedbackAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutEmailIdentityMailFromAttributes(AwsPinpointEmailPutEmailIdentityMailFromAttributesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SendEmail(AwsPinpointEmailSendEmailOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsPinpointEmailTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsPinpointEmailUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UpdateConfigurationSetEventDestination(AwsPinpointEmailUpdateConfigurationSetEventDestinationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}