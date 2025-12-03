using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network")]
public class AzNetworkPerimeter
{
    public AzNetworkPerimeter(
        AzNetworkPerimeterAssociation association,
        AzNetworkPerimeterLink link,
        AzNetworkPerimeterLinkReference linkReference,
        AzNetworkPerimeterOnboardedResources onboardedResources,
        AzNetworkPerimeterProfile profile,
        ICommand internalCommand
    )
    {
        Association = association;
        Link = link;
        LinkReference = linkReference;
        OnboardedResources = onboardedResources;
        Profile = profile;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkPerimeterAssociation Association { get; }

    public AzNetworkPerimeterLink Link { get; }

    public AzNetworkPerimeterLinkReference LinkReference { get; }

    public AzNetworkPerimeterOnboardedResources OnboardedResources { get; }

    public AzNetworkPerimeterProfile Profile { get; }

    public async Task<CommandResult> Create(AzNetworkPerimeterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPerimeterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPerimeterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkPerimeterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPerimeterShowOptions(), token);
    }
}