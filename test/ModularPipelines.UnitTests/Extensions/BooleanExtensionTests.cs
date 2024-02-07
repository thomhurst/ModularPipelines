﻿using ModularPipelines.Extensions;

namespace ModularPipelines.UnitTests.Extensions;

public class BooleanExtensionTests
{
    [Test]
    public async Task True()
    {
        var trueSkipDecision = true.AsSkipDecisionIfTrue("My reason");
        
        await Assert.Multiple(() =>
        {
            Assert.That(trueSkipDecision.ShouldSkip).Is.True();
            Assert.That(trueSkipDecision.Reason).Is.EqualTo("My reason");
        });
    }

    [Test]
    public async Task False()
    {
        var falseSkipDecision = false.AsSkipDecisionIfTrue("My reason");

        await Assert.Multiple(() =>
        {
            Assert.That(falseSkipDecision.ShouldSkip).Is.False();
            Assert.That(falseSkipDecision.Reason).Is.Null();
        });
    }
}