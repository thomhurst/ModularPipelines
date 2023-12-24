using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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

    public async Task<CommandResult> AssociateServiceQuotaTemplate(AwsServiceQuotasAssociateServiceQuotaTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasAssociateServiceQuotaTemplateOptions(), token);
    }

    public async Task<CommandResult> DeleteServiceQuotaIncreaseRequestFromTemplate(AwsServiceQuotasDeleteServiceQuotaIncreaseRequestFromTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateServiceQuotaTemplate(AwsServiceQuotasDisassociateServiceQuotaTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasDisassociateServiceQuotaTemplateOptions(), token);
    }

    public async Task<CommandResult> GetAssociationForServiceQuotaTemplate(AwsServiceQuotasGetAssociationForServiceQuotaTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasGetAssociationForServiceQuotaTemplateOptions(), token);
    }

    public async Task<CommandResult> GetAwsDefaultServiceQuota(AwsServiceQuotasGetAwsDefaultServiceQuotaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRequestedServiceQuotaChange(AwsServiceQuotasGetRequestedServiceQuotaChangeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceQuota(AwsServiceQuotasGetServiceQuotaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetServiceQuotaIncreaseRequestFromTemplate(AwsServiceQuotasGetServiceQuotaIncreaseRequestFromTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAwsDefaultServiceQuotas(AwsServiceQuotasListAwsDefaultServiceQuotasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListRequestedServiceQuotaChangeHistory(AwsServiceQuotasListRequestedServiceQuotaChangeHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListRequestedServiceQuotaChangeHistoryOptions(), token);
    }

    public async Task<CommandResult> ListRequestedServiceQuotaChangeHistoryByQuota(AwsServiceQuotasListRequestedServiceQuotaChangeHistoryByQuotaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServiceQuotaIncreaseRequestsInTemplate(AwsServiceQuotasListServiceQuotaIncreaseRequestsInTemplateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListServiceQuotaIncreaseRequestsInTemplateOptions(), token);
    }

    public async Task<CommandResult> ListServiceQuotas(AwsServiceQuotasListServiceQuotasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListServices(AwsServiceQuotasListServicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsServiceQuotasListServicesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsServiceQuotasListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutServiceQuotaIncreaseRequestIntoTemplate(AwsServiceQuotasPutServiceQuotaIncreaseRequestIntoTemplateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RequestServiceQuotaIncrease(AwsServiceQuotasRequestServiceQuotaIncreaseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsServiceQuotasTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsServiceQuotasUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}