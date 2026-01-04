using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> DescribeReportCreation(AwsResourcegroupstaggingapiDescribeReportCreationOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiDescribeReportCreationOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetComplianceSummary(AwsResourcegroupstaggingapiGetComplianceSummaryOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetComplianceSummaryOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetResources(AwsResourcegroupstaggingapiGetResourcesOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetResourcesOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTagKeys(AwsResourcegroupstaggingapiGetTagKeysOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsResourcegroupstaggingapiGetTagKeysOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> GetTagValues(AwsResourcegroupstaggingapiGetTagValuesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> StartReportCreation(AwsResourcegroupstaggingapiStartReportCreationOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TagResources(AwsResourcegroupstaggingapiTagResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }

    public async Task<CommandResult> UntagResources(AwsResourcegroupstaggingapiUntagResourcesOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, cancellationToken);
    }
}