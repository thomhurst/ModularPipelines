using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsResourcegroupstaggingapi
{
    public AwsResourcegroupstaggingapi(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DescribeReportCreation(AwsResourcegroupstaggingapiDescribeReportCreationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiDescribeReportCreationOptions(), token);
    }

    public async Task<CommandResult> GetComplianceSummary(AwsResourcegroupstaggingapiGetComplianceSummaryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetComplianceSummaryOptions(), token);
    }

    public async Task<CommandResult> GetResources(AwsResourcegroupstaggingapiGetResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetResourcesOptions(), token);
    }

    public async Task<CommandResult> GetTagKeys(AwsResourcegroupstaggingapiGetTagKeysOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetTagKeysOptions(), token);
    }

    public async Task<CommandResult> GetTagValues(AwsResourcegroupstaggingapiGetTagValuesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartReportCreation(AwsResourcegroupstaggingapiStartReportCreationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResources(AwsResourcegroupstaggingapiTagResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResources(AwsResourcegroupstaggingapiUntagResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}