using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsArcZonalShift
{
    public AwsArcZonalShift(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelZonalShift(AwsArcZonalShiftCancelZonalShiftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePracticeRunConfiguration(AwsArcZonalShiftCreatePracticeRunConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeletePracticeRunConfiguration(AwsArcZonalShiftDeletePracticeRunConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetManagedResource(AwsArcZonalShiftGetManagedResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAutoshifts(AwsArcZonalShiftListAutoshiftsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsArcZonalShiftListAutoshiftsOptions(), token);
    }

    public async Task<CommandResult> ListManagedResources(AwsArcZonalShiftListManagedResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsArcZonalShiftListManagedResourcesOptions(), token);
    }

    public async Task<CommandResult> ListZonalShifts(AwsArcZonalShiftListZonalShiftsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsArcZonalShiftListZonalShiftsOptions(), token);
    }

    public async Task<CommandResult> StartZonalShift(AwsArcZonalShiftStartZonalShiftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdatePracticeRunConfiguration(AwsArcZonalShiftUpdatePracticeRunConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateZonalAutoshiftConfiguration(AwsArcZonalShiftUpdateZonalAutoshiftConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateZonalShift(AwsArcZonalShiftUpdateZonalShiftOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}