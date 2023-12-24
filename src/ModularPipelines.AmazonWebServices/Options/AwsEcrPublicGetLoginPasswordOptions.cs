using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr-public", "get-login-password")]
public record AwsEcrPublicGetLoginPasswordOptions : AwsOptions;