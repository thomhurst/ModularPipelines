using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsAppflow
{
    public AwsAppflow(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelFlowExecutions(AwsAppflowCancelFlowExecutionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateConnectorProfile(AwsAppflowCreateConnectorProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFlow(AwsAppflowCreateFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteConnectorProfile(AwsAppflowDeleteConnectorProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFlow(AwsAppflowDeleteFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnector(AwsAppflowDescribeConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnectorEntity(AwsAppflowDescribeConnectorEntityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeConnectorProfiles(AwsAppflowDescribeConnectorProfilesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowDescribeConnectorProfilesOptions(), token);
    }

    public async Task<CommandResult> DescribeConnectors(AwsAppflowDescribeConnectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowDescribeConnectorsOptions(), token);
    }

    public async Task<CommandResult> DescribeFlow(AwsAppflowDescribeFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeFlowExecutionRecords(AwsAppflowDescribeFlowExecutionRecordsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListConnectorEntities(AwsAppflowListConnectorEntitiesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowListConnectorEntitiesOptions(), token);
    }

    public async Task<CommandResult> ListConnectors(AwsAppflowListConnectorsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowListConnectorsOptions(), token);
    }

    public async Task<CommandResult> ListFlows(AwsAppflowListFlowsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowListFlowsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsAppflowListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterConnector(AwsAppflowRegisterConnectorOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowRegisterConnectorOptions(), token);
    }

    public async Task<CommandResult> ResetConnectorMetadataCache(AwsAppflowResetConnectorMetadataCacheOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsAppflowResetConnectorMetadataCacheOptions(), token);
    }

    public async Task<CommandResult> StartFlow(AwsAppflowStartFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopFlow(AwsAppflowStopFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsAppflowTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnregisterConnector(AwsAppflowUnregisterConnectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsAppflowUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectorProfile(AwsAppflowUpdateConnectorProfileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateConnectorRegistration(AwsAppflowUpdateConnectorRegistrationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFlow(AwsAppflowUpdateFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}