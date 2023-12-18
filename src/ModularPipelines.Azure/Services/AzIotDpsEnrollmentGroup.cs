using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps")]
public class AzIotDpsEnrollmentGroup
{
    public AzIotDpsEnrollmentGroup(
        AzIotDpsEnrollmentGroupRegistration registration,
        ICommand internalCommand
    )
    {
        Registration = registration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDpsEnrollmentGroupRegistration Registration { get; }

    public async Task<CommandResult> ComputeDeviceKey(AzIotDpsEnrollmentGroupComputeDeviceKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzIotDpsEnrollmentGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotDpsEnrollmentGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotDpsEnrollmentGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzIotDpsEnrollmentGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotDpsEnrollmentGroupUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}