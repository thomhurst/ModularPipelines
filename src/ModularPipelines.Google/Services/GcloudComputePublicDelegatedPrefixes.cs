using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputePublicDelegatedPrefixes
{
    public GcloudComputePublicDelegatedPrefixes(
        GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixes delegatedSubPrefixes,
        ICommand internalCommand
    )
    {
        DelegatedSubPrefixes = delegatedSubPrefixes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputePublicDelegatedPrefixesDelegatedSubPrefixes DelegatedSubPrefixes { get; }

    public async Task<CommandResult> Create(GcloudComputePublicDelegatedPrefixesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudComputePublicDelegatedPrefixesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudComputePublicDelegatedPrefixesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudComputePublicDelegatedPrefixesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputePublicDelegatedPrefixesListOptions(), token);
    }
}