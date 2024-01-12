using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudTransfer
{
    public GcloudTransfer(
        GcloudTransferAgentPools agentPools,
        GcloudTransferAgents agents,
        GcloudTransferJobs jobs,
        GcloudTransferOperations operations,
        ICommand internalCommand
    )
    {
        AgentPools = agentPools;
        Agents = agents;
        Jobs = jobs;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudTransferAgentPools AgentPools { get; }

    public GcloudTransferAgents Agents { get; }

    public GcloudTransferJobs Jobs { get; }

    public GcloudTransferOperations Operations { get; }

    public async Task<CommandResult> Authorize(GcloudTransferAuthorizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudTransferAuthorizeOptions(), token);
    }
}