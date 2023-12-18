using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm")]
public class AzVmImage
{
    public AzVmImage(
        AzVmImageTerms terms,
        ICommand internalCommand
    )
    {
        Terms = terms;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzVmImageTerms Terms { get; }

    public async Task<CommandResult> List(AzVmImageListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListOffers(AzVmImageListOffersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListPublishers(AzVmImageListPublishersOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzVmImageListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzVmImageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzVmImageShowOptions(), token);
    }
}