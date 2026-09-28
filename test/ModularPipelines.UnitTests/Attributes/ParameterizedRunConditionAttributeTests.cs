using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class ParameterizedRunConditionAttributeTests
{
    [RunIfValue("expected")]
    private sealed class ParameterizedModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class RunIfValueAttribute(string expectedValue) : RunConditionAttribute(ConditionIntent.Run)
    {
        public string ExpectedValue { get; } = expectedValue;

        public override string ConditionNames => $"RunIfValue({ExpectedValue})";

        public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(ExpectedValue == "expected");
    }

    private sealed class AlwaysTrue : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class AlwaysFalse : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(false);
    }

    [Test]
    public async Task CustomAttribute_CarriesConstructorState()
    {
        var attribute = typeof(ParameterizedModule).GetCustomAttribute<RunIfValueAttribute>();

        using (Assert.Multiple())
        {
            await Assert.That(attribute).IsNotNull();
            await Assert.That(attribute!.ExpectedValue).IsEqualTo("expected");
            await Assert.That(attribute.Intent).IsEqualTo(ConditionIntent.Run);
            await Assert.That(attribute.GroupKey).IsNull();
            await Assert.That(await attribute.EvaluateAsync(Mock.Of<IPipelineContext>(), CancellationToken.None)).IsTrue();
        }
    }

    [Test]
    public async Task GenericAttribute_HonorsCancellation()
    {
        var attribute = new RunIfAttribute<AlwaysTrue>();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.That(() => attribute.EvaluateAsync(
                Mock.Of<IPipelineContext>(),
                cancellationTokenSource.Token))
            .Throws<OperationCanceledException>();
    }

    [Test]
    public async Task EnvironmentVariableAttributes_SupportSetValueAndUnsetChecks()
    {
        var variables = new Mock<IEnvironmentVariablesContext>();
        variables.Setup(x => x.Get("CI", EnvironmentVariableTarget.Process))
            .Returns("true");
        variables.Setup(x => x.Get("MISSING", EnvironmentVariableTarget.Process))
            .Returns((string?) null);
        var context = CreateContext(variables.Object);

        using (Assert.Multiple())
        {
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI").EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI", "true").EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI", "false").EvaluateAsync(context, CancellationToken.None)).IsFalse();
            await Assert.That(await new SkipIfEnvironmentVariableAttribute("CI", "true").EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableUnsetAttribute("MISSING").EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new SkipIfEnvironmentVariableUnsetAttribute("CI").EvaluateAsync(context, CancellationToken.None)).IsFalse();
        }
    }

    [Test]
    public async Task SkipIf_GenericAttributes_SupportThreeAndFourConditions()
    {
        var context = Mock.Of<IPipelineContext>();

        using (Assert.Multiple())
        {
            await Assert.That(await new SkipIfAttribute<AlwaysFalse, AlwaysFalse, AlwaysTrue>()
                .EvaluateAsync(context, CancellationToken.None)).IsTrue();
            await Assert.That(await new SkipIfAttribute<AlwaysFalse, AlwaysFalse, AlwaysFalse, AlwaysTrue>()
                .EvaluateAsync(context, CancellationToken.None)).IsTrue();
        }
    }

    private static IPipelineContext CreateContext(IEnvironmentVariablesContext variables)
    {
        var environment = Mock.Of<IEnvironmentContext>(x => x.Variables == variables);
        return Mock.Of<IPipelineContext>(x => x.Environment == environment);
    }
}
