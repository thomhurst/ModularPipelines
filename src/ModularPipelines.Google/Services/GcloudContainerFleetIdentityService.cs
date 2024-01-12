using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet")]
public class GcloudContainerFleetIdentityService
{
    public GcloudContainerFleetIdentityService(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Apply(GcloudContainerFleetIdentityServiceApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudContainerFleetIdentityServiceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIdentityServiceDeleteOptions(), token);
    }

    public async Task<CommandResult> Describe(GcloudContainerFleetIdentityServiceDescribeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIdentityServiceDescribeOptions(), token);
    }

    public async Task<CommandResult> Disable(GcloudContainerFleetIdentityServiceDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIdentityServiceDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetIdentityServiceEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetIdentityServiceEnableOptions(), token);
    }
}