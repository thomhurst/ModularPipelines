using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-login", "describe-profile")]
public record GcloudComputeOsLoginDescribeProfileOptions : GcloudOptions;