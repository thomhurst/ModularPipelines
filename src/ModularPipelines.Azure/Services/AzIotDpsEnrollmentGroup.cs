using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

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

    public async Task<CommandResult> List(AzIotDpsEnrollmentGroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsEnrollmentGroupListOptions(), token);
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