using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp")]
public class AzContainerappIngress
{
    public AzContainerappIngress(
        AzContainerappIngressAccessRestriction accessRestriction,
        AzContainerappIngressCors cors,
        AzContainerappIngressStickySessions stickySessions,
        AzContainerappIngressTraffic traffic,
        ICommand internalCommand
    )
    {
        AccessRestriction = accessRestriction;
        Cors = cors;
        StickySessions = stickySessions;
        Traffic = traffic;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappIngressAccessRestriction AccessRestriction { get; }

    public AzContainerappIngressCors Cors { get; }

    public AzContainerappIngressStickySessions StickySessions { get; }

    public AzContainerappIngressTraffic Traffic { get; }

    public async Task<CommandResult> Disable(AzContainerappIngressDisableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappIngressDisableOptions(), token);
    }

    public async Task<CommandResult> Enable(AzContainerappIngressEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzContainerappIngressShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappIngressShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzContainerappIngressUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappIngressUpdateOptions(), token);
    }
}