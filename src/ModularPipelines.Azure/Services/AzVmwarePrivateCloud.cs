using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware")]
public class AzVmwarePrivateCloud
{
    public AzVmwarePrivateCloud(
        AzVmwarePrivateCloudIdentity identity,
        AzVmwarePrivateCloudIdentitySource identitySource,
        ICommand internalCommand
    )
    {
        Identity = identity;
        IdentitySource = identitySource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmwarePrivateCloudIdentity Identity { get; }

    public AzVmwarePrivateCloudIdentitySource IdentitySource { get; }

    public async Task<CommandResult> AddCmkEncryption(AzVmwarePrivateCloudAddCmkEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AddIdentitySource(AzVmwarePrivateCloudAddIdentitySourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzVmwarePrivateCloudCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzVmwarePrivateCloudDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteCmkEncryption(AzVmwarePrivateCloudDeleteCmkEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DeleteIdentitySource(AzVmwarePrivateCloudDeleteIdentitySourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DisableCmkEncryption(AzVmwarePrivateCloudDisableCmkEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> EnableCmkEncryption(AzVmwarePrivateCloudEnableCmkEncryptionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzVmwarePrivateCloudListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAdminCredentials(AzVmwarePrivateCloudListAdminCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RotateNsxtPassword(AzVmwarePrivateCloudRotateNsxtPasswordOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePrivateCloudRotateNsxtPasswordOptions(), token);
    }

    public async Task<CommandResult> RotateVcenterPassword(AzVmwarePrivateCloudRotateVcenterPasswordOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePrivateCloudRotateVcenterPasswordOptions(), token);
    }

    public async Task<CommandResult> Show(AzVmwarePrivateCloudShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePrivateCloudShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzVmwarePrivateCloudUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePrivateCloudUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzVmwarePrivateCloudWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmwarePrivateCloudWaitOptions(), token);
    }
}