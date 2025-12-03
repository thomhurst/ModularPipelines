using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps")]
public class AzIotDpsEnrollment
{
    public AzIotDpsEnrollment(
        AzIotDpsEnrollmentRegistration registration,
        ICommand internalCommand
    )
    {
        Registration = registration;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzIotDpsEnrollmentRegistration Registration { get; }

    public async Task<CommandResult> Create(AzIotDpsEnrollmentCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzIotDpsEnrollmentDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzIotDpsEnrollmentListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotDpsEnrollmentListOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotDpsEnrollmentShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzIotDpsEnrollmentUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}