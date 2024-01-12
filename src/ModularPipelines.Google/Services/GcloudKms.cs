using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudKms
{
    public GcloudKms(
        GcloudKmsEkmConfig ekmConfig,
        GcloudKmsEkmConnections ekmConnections,
        GcloudKmsImportJobs importJobs,
        GcloudKmsInventory inventory,
        GcloudKmsKeyrings keyrings,
        GcloudKmsKeys keys,
        GcloudKmsLocations locations,
        ICommand internalCommand
    )
    {
        EkmConfig = ekmConfig;
        EkmConnections = ekmConnections;
        ImportJobs = importJobs;
        Inventory = inventory;
        Keyrings = keyrings;
        Keys = keys;
        Locations = locations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudKmsEkmConfig EkmConfig { get; }

    public GcloudKmsEkmConnections EkmConnections { get; }

    public GcloudKmsImportJobs ImportJobs { get; }

    public GcloudKmsInventory Inventory { get; }

    public GcloudKmsKeyrings Keyrings { get; }

    public GcloudKmsKeys Keys { get; }

    public GcloudKmsLocations Locations { get; }

    public async Task<CommandResult> AsymmetricDecrypt(GcloudKmsAsymmetricDecryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AsymmetricSign(GcloudKmsAsymmetricSignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Decrypt(GcloudKmsDecryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Encrypt(GcloudKmsEncryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MacSign(GcloudKmsMacSignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MacVerify(GcloudKmsMacVerifyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RawDecrypt(GcloudKmsRawDecryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RawEncrypt(GcloudKmsRawEncryptOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}