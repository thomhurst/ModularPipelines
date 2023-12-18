using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Options;

namespace ModularPipelines.Terraform.Options;

[ExcludeFromCodeCoverage]
public record TerraformOptions() : CommandLineToolOptions("terraform")
{
}