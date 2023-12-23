using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "list-profiles")]
public record AwsWellarchitectedListProfilesOptions : AwsOptions
{
    [CommandSwitch("--profile-name-prefix")]
    public string? ProfileNamePrefix { get; set; }

    [CommandSwitch("--profile-owner-type")]
    public string? ProfileOwnerType { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}