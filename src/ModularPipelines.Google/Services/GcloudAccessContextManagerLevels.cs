using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager")]
public class GcloudAccessContextManagerLevels
{
    public GcloudAccessContextManagerLevels(
        GcloudAccessContextManagerLevelsConditions conditions,
        ICommand internalCommand
    )
    {
        Conditions = conditions;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAccessContextManagerLevelsConditions Conditions { get; }

    public async Task<CommandResult> Create(GcloudAccessContextManagerLevelsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudAccessContextManagerLevelsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAccessContextManagerLevelsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAccessContextManagerLevelsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAccessContextManagerLevelsListOptions(), token);
    }

    public async Task<CommandResult> ReplaceAll(GcloudAccessContextManagerLevelsReplaceAllOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudAccessContextManagerLevelsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}