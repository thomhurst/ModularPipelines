using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci")]
public class AzStackHciCluster
{
    public AzStackHciCluster(
        AzStackHciClusterIdentity identity,
        ICommand internalCommand
    )
    {
        Identity = identity;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStackHciClusterIdentity Identity { get; }

    public async Task<CommandResult> Create(AzStackHciClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateIdentity(AzStackHciClusterCreateIdentityOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterCreateIdentityOptions(), token);
    }

    public async Task<CommandResult> Delete(AzStackHciClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterDeleteOptions(), token);
    }

    public async Task<CommandResult> ExtendSoftwareAssuranceBenefit(AzStackHciClusterExtendSoftwareAssuranceBenefitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterExtendSoftwareAssuranceBenefitOptions(), token);
    }

    public async Task<CommandResult> List(AzStackHciClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStackHciClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStackHciClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStackHciClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStackHciClusterWaitOptions(), token);
    }
}