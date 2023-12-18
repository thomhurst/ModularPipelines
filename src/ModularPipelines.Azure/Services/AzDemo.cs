using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzDemo
{
    public AzDemo(
        AzDemoSecretStore secretStore,
        ICommand internalCommand
    )
    {
        SecretStore = secretStore;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDemoSecretStore SecretStore { get; }

    public async Task<CommandResult> ByoAccessToken(AzDemoByoAccessTokenOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Style(AzDemoStyleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDemoStyleOptions(), token);
    }
}