using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsMedialive
{
    public AwsMedialive(
        AwsMedialiveWait wait,
        ICommand internalCommand
    )
    {
        Wait = wait;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AwsMedialiveWait Wait { get; }

    public async Task<CommandResult> AcceptInputDeviceTransfer(AwsMedialiveAcceptInputDeviceTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchDelete(AwsMedialiveBatchDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveBatchDeleteOptions(), token);
    }

    public async Task<CommandResult> BatchStart(AwsMedialiveBatchStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveBatchStartOptions(), token);
    }

    public async Task<CommandResult> BatchStop(AwsMedialiveBatchStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveBatchStopOptions(), token);
    }

    public async Task<CommandResult> BatchUpdateSchedule(AwsMedialiveBatchUpdateScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CancelInputDeviceTransfer(AwsMedialiveCancelInputDeviceTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ClaimDevice(AwsMedialiveClaimDeviceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveClaimDeviceOptions(), token);
    }

    public async Task<CommandResult> CreateChannel(AwsMedialiveCreateChannelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveCreateChannelOptions(), token);
    }

    public async Task<CommandResult> CreateInput(AwsMedialiveCreateInputOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveCreateInputOptions(), token);
    }

    public async Task<CommandResult> CreateInputSecurityGroup(AwsMedialiveCreateInputSecurityGroupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveCreateInputSecurityGroupOptions(), token);
    }

    public async Task<CommandResult> CreateMultiplex(AwsMedialiveCreateMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateMultiplexProgram(AwsMedialiveCreateMultiplexProgramOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreatePartnerInput(AwsMedialiveCreatePartnerInputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateTags(AwsMedialiveCreateTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteChannel(AwsMedialiveDeleteChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInput(AwsMedialiveDeleteInputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteInputSecurityGroup(AwsMedialiveDeleteInputSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMultiplex(AwsMedialiveDeleteMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteMultiplexProgram(AwsMedialiveDeleteMultiplexProgramOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteReservation(AwsMedialiveDeleteReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSchedule(AwsMedialiveDeleteScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteTags(AwsMedialiveDeleteTagsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeAccountConfiguration(AwsMedialiveDescribeAccountConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveDescribeAccountConfigurationOptions(), token);
    }

    public async Task<CommandResult> DescribeChannel(AwsMedialiveDescribeChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInput(AwsMedialiveDescribeInputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInputDevice(AwsMedialiveDescribeInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInputDeviceThumbnail(AwsMedialiveDescribeInputDeviceThumbnailOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeInputSecurityGroup(AwsMedialiveDescribeInputSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMultiplex(AwsMedialiveDescribeMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeMultiplexProgram(AwsMedialiveDescribeMultiplexProgramOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeOffering(AwsMedialiveDescribeOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeReservation(AwsMedialiveDescribeReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeSchedule(AwsMedialiveDescribeScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DescribeThumbnails(AwsMedialiveDescribeThumbnailsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListChannels(AwsMedialiveListChannelsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListChannelsOptions(), token);
    }

    public async Task<CommandResult> ListInputDeviceTransfers(AwsMedialiveListInputDeviceTransfersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListInputDevices(AwsMedialiveListInputDevicesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListInputDevicesOptions(), token);
    }

    public async Task<CommandResult> ListInputSecurityGroups(AwsMedialiveListInputSecurityGroupsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListInputSecurityGroupsOptions(), token);
    }

    public async Task<CommandResult> ListInputs(AwsMedialiveListInputsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListInputsOptions(), token);
    }

    public async Task<CommandResult> ListMultiplexPrograms(AwsMedialiveListMultiplexProgramsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListMultiplexes(AwsMedialiveListMultiplexesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListMultiplexesOptions(), token);
    }

    public async Task<CommandResult> ListOfferings(AwsMedialiveListOfferingsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListOfferingsOptions(), token);
    }

    public async Task<CommandResult> ListReservations(AwsMedialiveListReservationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveListReservationsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsMedialiveListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PurchaseOffering(AwsMedialivePurchaseOfferingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RebootInputDevice(AwsMedialiveRebootInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RejectInputDeviceTransfer(AwsMedialiveRejectInputDeviceTransferOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartChannel(AwsMedialiveStartChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInputDevice(AwsMedialiveStartInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartInputDeviceMaintenanceWindow(AwsMedialiveStartInputDeviceMaintenanceWindowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartMultiplex(AwsMedialiveStartMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopChannel(AwsMedialiveStopChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopInputDevice(AwsMedialiveStopInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StopMultiplex(AwsMedialiveStopMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TransferInputDevice(AwsMedialiveTransferInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateAccountConfiguration(AwsMedialiveUpdateAccountConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsMedialiveUpdateAccountConfigurationOptions(), token);
    }

    public async Task<CommandResult> UpdateChannel(AwsMedialiveUpdateChannelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateChannelClass(AwsMedialiveUpdateChannelClassOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInput(AwsMedialiveUpdateInputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInputDevice(AwsMedialiveUpdateInputDeviceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateInputSecurityGroup(AwsMedialiveUpdateInputSecurityGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMultiplex(AwsMedialiveUpdateMultiplexOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateMultiplexProgram(AwsMedialiveUpdateMultiplexProgramOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateReservation(AwsMedialiveUpdateReservationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}