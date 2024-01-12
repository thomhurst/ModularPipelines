using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute")]
public class GcloudComputeOsLogin
{
    public GcloudComputeOsLogin(
        GcloudComputeOsLoginSshKeys sshKeys,
        ICommand internalCommand
    )
    {
        SshKeys = sshKeys;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudComputeOsLoginSshKeys SshKeys { get; }

    public async Task<CommandResult> DescribeProfile(GcloudComputeOsLoginDescribeProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeOsLoginDescribeProfileOptions(), token);
    }

    public async Task<CommandResult> RemoveProfile(GcloudComputeOsLoginRemoveProfileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudComputeOsLoginRemoveProfileOptions(), token);
    }
}