using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
public class AwsTnb
{
    public AwsTnb(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> CancelSolNetworkOperation(AwsTnbCancelSolNetworkOperationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSolFunctionPackage(AwsTnbCreateSolFunctionPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbCreateSolFunctionPackageOptions(), token);
    }

    public async Task<CommandResult> CreateSolNetworkInstance(AwsTnbCreateSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateSolNetworkPackage(AwsTnbCreateSolNetworkPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbCreateSolNetworkPackageOptions(), token);
    }

    public async Task<CommandResult> DeleteSolFunctionPackage(AwsTnbDeleteSolFunctionPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSolNetworkInstance(AwsTnbDeleteSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteSolNetworkPackage(AwsTnbDeleteSolNetworkPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolFunctionInstance(AwsTnbGetSolFunctionInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolFunctionPackage(AwsTnbGetSolFunctionPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolFunctionPackageContent(AwsTnbGetSolFunctionPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolFunctionPackageDescriptor(AwsTnbGetSolFunctionPackageDescriptorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolNetworkInstance(AwsTnbGetSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolNetworkOperation(AwsTnbGetSolNetworkOperationOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolNetworkPackage(AwsTnbGetSolNetworkPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolNetworkPackageContent(AwsTnbGetSolNetworkPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetSolNetworkPackageDescriptor(AwsTnbGetSolNetworkPackageDescriptorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InstantiateSolNetworkInstance(AwsTnbInstantiateSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSolFunctionInstances(AwsTnbListSolFunctionInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbListSolFunctionInstancesOptions(), token);
    }

    public async Task<CommandResult> ListSolFunctionPackages(AwsTnbListSolFunctionPackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbListSolFunctionPackagesOptions(), token);
    }

    public async Task<CommandResult> ListSolNetworkInstances(AwsTnbListSolNetworkInstancesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbListSolNetworkInstancesOptions(), token);
    }

    public async Task<CommandResult> ListSolNetworkOperations(AwsTnbListSolNetworkOperationsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbListSolNetworkOperationsOptions(), token);
    }

    public async Task<CommandResult> ListSolNetworkPackages(AwsTnbListSolNetworkPackagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsTnbListSolNetworkPackagesOptions(), token);
    }

    public async Task<CommandResult> ListTagsForResource(AwsTnbListTagsForResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSolFunctionPackageContent(AwsTnbPutSolFunctionPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PutSolNetworkPackageContent(AwsTnbPutSolNetworkPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TagResource(AwsTnbTagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TerminateSolNetworkInstance(AwsTnbTerminateSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UntagResource(AwsTnbUntagResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSolFunctionPackage(AwsTnbUpdateSolFunctionPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSolNetworkInstance(AwsTnbUpdateSolNetworkInstanceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UpdateSolNetworkPackage(AwsTnbUpdateSolNetworkPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateSolFunctionPackageContent(AwsTnbValidateSolFunctionPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ValidateSolNetworkPackageContent(AwsTnbValidateSolNetworkPackageContentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}