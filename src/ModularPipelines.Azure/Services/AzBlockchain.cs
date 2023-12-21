using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzBlockchain
{
    public AzBlockchain(
        AzBlockchainConsortium consortium,
        AzBlockchainMember member,
        AzBlockchainTransactionNode transactionNode
    )
    {
        Consortium = consortium;
        Member = member;
        TransactionNode = transactionNode;
    }

    public AzBlockchainConsortium Consortium { get; }

    public AzBlockchainMember Member { get; }

    public AzBlockchainTransactionNode TransactionNode { get; }
}