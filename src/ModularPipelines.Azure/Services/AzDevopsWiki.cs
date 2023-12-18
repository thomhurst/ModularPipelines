using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops")]
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

    public async Task<CommandResult> Create(AzDevopsWikiCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDevopsWikiDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDevopsWikiListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDevopsWikiShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

