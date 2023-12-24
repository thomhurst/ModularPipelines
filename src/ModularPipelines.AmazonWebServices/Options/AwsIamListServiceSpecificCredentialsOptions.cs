using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "list-service-specific-credentials")]
public record AwsIamListServiceSpecificCredentialsOptions : AwsOptions
{
    [CommandSwitch("--user-name")]
    public string? UserName { get; set; }

    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}