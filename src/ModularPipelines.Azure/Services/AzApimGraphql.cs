using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim")]
public class AzApimGraphql
{
    public AzApimGraphql(
        AzApimGraphqlResolver resolver
    )
    {
        Resolver = resolver;
    }

    public AzApimGraphqlResolver Resolver { get; }
}