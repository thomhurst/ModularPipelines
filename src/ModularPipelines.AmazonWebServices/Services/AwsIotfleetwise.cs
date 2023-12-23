using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsIotfleetwise
{
    public AwsIotfleetwise(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> AssociateVehicleFleet(AwsIotfleetwiseAssociateVehicleFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchCreateVehicle(AwsIotfleetwiseBatchCreateVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BatchUpdateVehicle(AwsIotfleetwiseBatchUpdateVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateCampaign(AwsIotfleetwiseCreateCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateDecoderManifest(AwsIotfleetwiseCreateDecoderManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateFleet(AwsIotfleetwiseCreateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateModelManifest(AwsIotfleetwiseCreateModelManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSignalCatalog(AwsIotfleetwiseCreateSignalCatalogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateVehicle(AwsIotfleetwiseCreateVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCampaign(AwsIotfleetwiseDeleteCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteDecoderManifest(AwsIotfleetwiseDeleteDecoderManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteFleet(AwsIotfleetwiseDeleteFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteModelManifest(AwsIotfleetwiseDeleteModelManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSignalCatalog(AwsIotfleetwiseDeleteSignalCatalogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteVehicle(AwsIotfleetwiseDeleteVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisassociateVehicleFleet(AwsIotfleetwiseDisassociateVehicleFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetCampaign(AwsIotfleetwiseGetCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetDecoderManifest(AwsIotfleetwiseGetDecoderManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetEncryptionConfiguration(AwsIotfleetwiseGetEncryptionConfigurationOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseGetEncryptionConfigurationOptions(), token);
    }

    public async Task<CommandResult> GetFleet(AwsIotfleetwiseGetFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetLoggingOptions(AwsIotfleetwiseGetLoggingOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseGetLoggingOptionsOptions(), token);
    }

    public async Task<CommandResult> GetModelManifest(AwsIotfleetwiseGetModelManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegisterAccountStatus(AwsIotfleetwiseGetRegisterAccountStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseGetRegisterAccountStatusOptions(), token);
    }

    public async Task<CommandResult> GetSignalCatalog(AwsIotfleetwiseGetSignalCatalogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVehicle(AwsIotfleetwiseGetVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetVehicleStatus(AwsIotfleetwiseGetVehicleStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportDecoderManifest(AwsIotfleetwiseImportDecoderManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportSignalCatalog(AwsIotfleetwiseImportSignalCatalogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListCampaigns(AwsIotfleetwiseListCampaignsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListCampaignsOptions(), token);
    }

    public async Task<CommandResult> ListDecoderManifestNetworkInterfaces(AwsIotfleetwiseListDecoderManifestNetworkInterfacesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDecoderManifestSignals(AwsIotfleetwiseListDecoderManifestSignalsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListDecoderManifests(AwsIotfleetwiseListDecoderManifestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListDecoderManifestsOptions(), token);
    }

    public async Task<CommandResult> ListFleets(AwsIotfleetwiseListFleetsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListFleetsOptions(), token);
    }

    public async Task<CommandResult> ListFleetsForVehicle(AwsIotfleetwiseListFleetsForVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListModelManifestNodes(AwsIotfleetwiseListModelManifestNodesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListModelManifests(AwsIotfleetwiseListModelManifestsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListModelManifestsOptions(), token);
    }

    public async Task<CommandResult> ListSignalCatalogNodes(AwsIotfleetwiseListSignalCatalogNodesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSignalCatalogs(AwsIotfleetwiseListSignalCatalogsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListSignalCatalogsOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsIotfleetwiseListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListVehicles(AwsIotfleetwiseListVehiclesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseListVehiclesOptions(), token);
    }

    public async Task<CommandResult> ListVehiclesInFleet(AwsIotfleetwiseListVehiclesInFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutEncryptionConfiguration(AwsIotfleetwisePutEncryptionConfigurationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutLoggingOptions(AwsIotfleetwisePutLoggingOptionsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegisterAccount(AwsIotfleetwiseRegisterAccountOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsIotfleetwiseRegisterAccountOptions(), token);
    }

    public async Task<CommandResult> TagResource(AwsIotfleetwiseTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsIotfleetwiseUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateCampaign(AwsIotfleetwiseUpdateCampaignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateDecoderManifest(AwsIotfleetwiseUpdateDecoderManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateFleet(AwsIotfleetwiseUpdateFleetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateModelManifest(AwsIotfleetwiseUpdateModelManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSignalCatalog(AwsIotfleetwiseUpdateSignalCatalogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateVehicle(AwsIotfleetwiseUpdateVehicleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}