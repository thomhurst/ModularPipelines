using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dls")]
public class AzDlsAccount
{
    public AzDlsAccount(
        AzDlsAccountFirewall firewall,
        AzDlsAccountNetworkRule networkRule,
        AzDlsAccountTrustedProvider trustedProvider,
        ICommand internalCommand
    )
    {
        Firewall = firewall;
        NetworkRule = networkRule;
        TrustedProvider = trustedProvider;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDlsAccountFirewall Firewall { get; }

    public AzDlsAccountNetworkRule NetworkRule { get; }

    public AzDlsAccountTrustedProvider TrustedProvider { get; }

    public async Task<CommandResult> Create(AzDlsAccountCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDlsAccountDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlsAccountDeleteOptions(), token);
    }

    public async Task<CommandResult> EnableKeyVault(AzDlsAccountEnableKeyVaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlsAccountEnableKeyVaultOptions(), token);
    }

    public async Task<CommandResult> List(AzDlsAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlsAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDlsAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlsAccountShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDlsAccountUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlsAccountUpdateOptions(), token);
    }
}