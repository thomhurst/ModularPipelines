using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzElastic
{
    public AzElastic(
        AzElasticMonitor monitor,
        ICommand internalCommand
    )
    {
        Monitor = monitor;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzElasticMonitor Monitor { get; }

    public async Task<CommandResult> GetOrganizationApiKey(AzElasticGetOrganizationApiKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzElasticGetOrganizationApiKeyOptions(), token);
    }
}