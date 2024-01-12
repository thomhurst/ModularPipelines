using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "policycontroller", "content")]
public class GcloudContainerHubPolicycontrollerContentTemplates
{
    public GcloudContainerHubPolicycontrollerContentTemplates(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Disable(GcloudContainerHubPolicycontrollerContentTemplatesDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerContentTemplatesDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(GcloudContainerHubPolicycontrollerContentTemplatesEnableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudContainerHubPolicycontrollerContentTemplatesEnableOptions(), token);
    }
}