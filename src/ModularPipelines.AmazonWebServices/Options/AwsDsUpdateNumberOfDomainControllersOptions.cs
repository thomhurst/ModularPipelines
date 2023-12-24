using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-number-of-domain-controllers")]
public record AwsDsUpdateNumberOfDomainControllersOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--desired-number")] int DesiredNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}