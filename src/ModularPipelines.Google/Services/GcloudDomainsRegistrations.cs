using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("domains")]
public class GcloudDomainsRegistrations
{
    public GcloudDomainsRegistrations(
        GcloudDomainsRegistrationsAuthorizationCode authorizationCode,
        GcloudDomainsRegistrationsConfigure configure,
        GcloudDomainsRegistrationsOperations operations,
        ICommand internalCommand
    )
    {
        AuthorizationCode = authorizationCode;
        Configure = configure;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDomainsRegistrationsAuthorizationCode AuthorizationCode { get; }

    public GcloudDomainsRegistrationsConfigure Configure { get; }

    public GcloudDomainsRegistrationsOperations Operations { get; }

    public async Task<CommandResult> Delete(GcloudDomainsRegistrationsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudDomainsRegistrationsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetRegisterParameters(GcloudDomainsRegistrationsGetRegisterParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetTransferParameters(GcloudDomainsRegistrationsGetTransferParametersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudDomainsRegistrationsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDomainsRegistrationsListOptions(), token);
    }

    public async Task<CommandResult> ListImportableDomains(GcloudDomainsRegistrationsListImportableDomainsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDomainsRegistrationsListImportableDomainsOptions(), token);
    }

    public async Task<CommandResult> Register(GcloudDomainsRegistrationsRegisterOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SearchDomains(GcloudDomainsRegistrationsSearchDomainsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudDomainsRegistrationsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}