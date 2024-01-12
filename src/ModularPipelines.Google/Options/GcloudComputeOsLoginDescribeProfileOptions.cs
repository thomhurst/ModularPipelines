using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-login", "describe-profile")]
public record GcloudComputeOsLoginDescribeProfileOptions : GcloudOptions;