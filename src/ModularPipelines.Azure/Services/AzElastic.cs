using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

