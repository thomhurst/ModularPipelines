using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "policycontroller", "content")]
public class GcloudContainerFleetPolicycontrollerContentTemplates
{
    public GcloudContainerFleetPolicycontrollerContentTemplates(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(GcloudContainerFleetPolicycontrollerContentTemplatesDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerContentTemplatesDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerFleetPolicycontrollerContentTemplatesEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerFleetPolicycontrollerContentTemplatesEnableOptions(), token);
    }
}