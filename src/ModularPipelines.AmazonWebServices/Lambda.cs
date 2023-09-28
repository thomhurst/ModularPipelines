using System.Diagnostics.CodeAnalysis;
using Amazon.Lambda;

namespace ModularPipelines.AmazonWebServices;

[ExcludeFromCodeCoverage]
public class Lambda
{
    public AmazonLambdaClient Client { get; }

    public Lambda(AmazonLambdaClient client)
    {
        Client = client;
    }
}