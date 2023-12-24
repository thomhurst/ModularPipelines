using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "describe-folder-contents")]
public record AwsWorkdocsDescribeFolderContentsOptions(
[property: CommandSwitch("--folder-id")] string FolderId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--order")]
    public string? Order { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}