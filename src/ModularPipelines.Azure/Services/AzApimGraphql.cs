using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim")]
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