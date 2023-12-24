using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("m2", "update-application")]
public record AwsM2UpdateApplicationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--current-application-version")] int CurrentApplicationVersion
) : AwsOptions
{
    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}