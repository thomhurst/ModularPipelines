using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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