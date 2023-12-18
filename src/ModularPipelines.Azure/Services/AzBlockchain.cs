using System.Diagnostics.CodeAnalysis;

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