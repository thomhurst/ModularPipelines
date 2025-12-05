using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dns")]
public class GcloudDnsResponsePolicies
{
    public GcloudDnsResponsePolicies(
        GcloudDnsResponsePoliciesRules rules,
        ICommand internalCommand
    )
    {
        Rules = rules;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDnsResponsePoliciesRules Rules { get; }

    public async Task<CommandResult> Create(GcloudDnsResponsePoliciesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudDnsResponsePoliciesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDnsResponsePoliciesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDnsResponsePoliciesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDnsResponsePoliciesListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudDnsResponsePoliciesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}