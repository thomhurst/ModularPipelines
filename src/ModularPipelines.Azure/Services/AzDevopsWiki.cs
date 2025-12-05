using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("devops")]
public class AzDevopsWiki
{
    public AzDevopsWiki(
        AzDevopsWikiPage page,
        ICommand internalCommand
    )
    {
        Page = page;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDevopsWikiPage Page { get; }

    public async Task<CommandResult> Create(AzDevopsWikiCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsWikiCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzDevopsWikiDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevopsWikiListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDevopsWikiListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDevopsWikiShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}