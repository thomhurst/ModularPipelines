using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzLab
{
    public AzLab(
        AzLabArmTemplate armTemplate,
        AzLabArtifact artifact,
        AzLabArtifactSource artifactSource,
        AzLabCustomImage customImage,
        AzLabEnvironment environment,
        AzLabFormula formula,
        AzLabGalleryImage galleryImage,
        AzLabSecret secret,
        AzLabVm vm,
        AzLabVnet vnet,
        ICommand internalCommand
    )
    {
        ArmTemplate = armTemplate;
        Artifact = artifact;
        ArtifactSource = artifactSource;
        CustomImage = customImage;
        Environment = environment;
        Formula = formula;
        GalleryImage = galleryImage;
        Secret = secret;
        Vm = vm;
        Vnet = vnet;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLabArmTemplate ArmTemplate { get; }

    public AzLabArtifact Artifact { get; }

    public AzLabArtifactSource ArtifactSource { get; }

    public AzLabCustomImage CustomImage { get; }

    public AzLabEnvironment Environment { get; }

    public AzLabFormula Formula { get; }

    public AzLabGalleryImage GalleryImage { get; }

    public AzLabSecret Secret { get; }

    public AzLabVm Vm { get; }

    public AzLabVnet Vnet { get; }

    public async Task<CommandResult> Delete(AzLabDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Get(AzLabGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}