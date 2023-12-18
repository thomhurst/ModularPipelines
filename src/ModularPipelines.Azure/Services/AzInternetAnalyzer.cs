using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzInternetAnalyzer
{
    public AzInternetAnalyzer(
        AzInternetAnalyzerPreconfiguredEndpoint preconfiguredEndpoint,
        AzInternetAnalyzerProfile profile,
        AzInternetAnalyzerTest test,
        ICommand internalCommand
    )
    {
        PreconfiguredEndpoint = preconfiguredEndpoint;
        Profile = profile;
        Test = test;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzInternetAnalyzerPreconfiguredEndpoint PreconfiguredEndpoint { get; }

    public AzInternetAnalyzerProfile Profile { get; }

    public AzInternetAnalyzerTest Test { get; }

    public async Task<CommandResult> ShowScorecard(AzInternetAnalyzerShowScorecardOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowTimeseries(AzInternetAnalyzerShowTimeseriesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}