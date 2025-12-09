using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack")]
public class AzPaloAltoCloudngfwLocalRulestackCertificate
{
    public AzPaloAltoCloudngfwLocalRulestackCertificate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzPaloAltoCloudngfwLocalRulestackCertificateCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzPaloAltoCloudngfwLocalRulestackCertificateDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackCertificateDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzPaloAltoCloudngfwLocalRulestackCertificateListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzPaloAltoCloudngfwLocalRulestackCertificateShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackCertificateShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzPaloAltoCloudngfwLocalRulestackCertificateWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPaloAltoCloudngfwLocalRulestackCertificateWaitOptions(), token);
    }
}