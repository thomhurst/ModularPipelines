using System.Collections.Concurrent;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Requirements;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class RequirementCheckerTests
{
    [Test]
    public async Task Requirements_Are_Checked_In_Ascending_Order()
    {
        var executionOrder = new ConcurrentQueue<int>();
        IPipelineRequirement[] requirements =
        [
            new RecordingRequirement(20, executionOrder),
            new RecordingRequirement(-10, executionOrder),
            new RecordingRequirement(0, executionOrder),
        ];
        var contextProvider = new Mock<IPipelineContextProvider>();
        contextProvider
            .Setup(provider => provider.GetModuleContext())
            .Returns(Mock.Of<IPipelineContext>());
        var checker = new RequirementChecker(requirements, contextProvider.Object);

        await checker.CheckRequirementsAsync(CancellationToken.None);

        var actualOrder = executionOrder.ToArray();
        await Assert.That(actualOrder.Length).IsEqualTo(3);
        await Assert.That(actualOrder[0]).IsEqualTo(-10);
        await Assert.That(actualOrder[1]).IsEqualTo(0);
        await Assert.That(actualOrder[2]).IsEqualTo(20);
    }

    private sealed class RecordingRequirement(
        int order,
        ConcurrentQueue<int> executionOrder) : IPipelineRequirement
    {
        public int Order => order;

        public Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            executionOrder.Enqueue(order);
            return Task.FromResult(RequirementDecision.Passed);
        }
    }
}
