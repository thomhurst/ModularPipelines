using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsServiceQuotas
{
    public AwsServiceQuotas(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateServiceQuotaTemplate(AwsServiceQuotasAssociateServiceQuotaTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasAssociateServiceQuotaTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DeleteServiceQuotaIncreaseRequestFromTemplate(AwsServiceQuotasDeleteServiceQuotaIncreaseRequestFromTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DisassociateServiceQuotaTemplate(AwsServiceQuotasDisassociateServiceQuotaTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasDisassociateServiceQuotaTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAssociationForServiceQuotaTemplate(AwsServiceQuotasGetAssociationForServiceQuotaTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasGetAssociationForServiceQuotaTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetAwsDefaultServiceQuota(AwsServiceQuotasGetAwsDefaultServiceQuotaOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetRequestedServiceQuotaChange(AwsServiceQuotasGetRequestedServiceQuotaChangeOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServiceQuota(AwsServiceQuotasGetServiceQuotaOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetServiceQuotaIncreaseRequestFromTemplate(AwsServiceQuotasGetServiceQuotaIncreaseRequestFromTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListAwsDefaultServiceQuotas(AwsServiceQuotasListAwsDefaultServiceQuotasOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRequestedServiceQuotaChangeHistory(AwsServiceQuotasListRequestedServiceQuotaChangeHistoryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListRequestedServiceQuotaChangeHistoryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListRequestedServiceQuotaChangeHistoryByQuota(AwsServiceQuotasListRequestedServiceQuotaChangeHistoryByQuotaOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServiceQuotaIncreaseRequestsInTemplate(AwsServiceQuotasListServiceQuotaIncreaseRequestsInTemplateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListServiceQuotaIncreaseRequestsInTemplateOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServiceQuotas(AwsServiceQuotasListServiceQuotasOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListServices(AwsServiceQuotasListServicesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListServicesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ListTagsForResource(AwsServiceQuotasListTagsForResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> PutServiceQuotaIncreaseRequestIntoTemplate(AwsServiceQuotasPutServiceQuotaIncreaseRequestIntoTemplateOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> RequestServiceQuotaIncrease(AwsServiceQuotasRequestServiceQuotaIncreaseOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResource(AwsServiceQuotasTagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResource(AwsServiceQuotasUntagResourceOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}