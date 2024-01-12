using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudDomains
{
    public GcloudDomains(
        GcloudDomainsRegistrations registrations,
        ICommand internalCommand
    )
    {
        Registrations = registrations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudDomainsRegistrations Registrations { get; }

    public async Task<CommandResult> ListUserVerified(GcloudDomainsListUserVerifiedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudDomainsListUserVerifiedOptions(), token);
    }

    public async Task<CommandResult> Verify(GcloudDomainsVerifyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}