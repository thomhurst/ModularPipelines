using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("proton", "delete-repository")]
public record AwsProtonDeleteRepositoryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider")] string Provider
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}