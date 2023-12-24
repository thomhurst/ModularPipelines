using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "get-access-grants-location")]
public record AwsS3controlGetAccessGrantsLocationOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--access-grants-location-id")] string AccessGrantsLocationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}