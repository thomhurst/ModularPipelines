using System.Reflection;
using System.Runtime.InteropServices;
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

    private sealed class RunIfValueAttribute(string expectedValue) : RunIfAllAttribute
    {
        public string ExpectedValue { get; } = expectedValue;

        public override string ConditionNames => $"RunIfValue({ExpectedValue})";

        public override Task<bool> EvaluateAsync(IPipelineContext context) =>
            Task.FromResult(ExpectedValue == "expected");
    }

    private sealed class AlwaysTrue : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    private sealed class AlwaysFalse : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    [Test]
    public async Task CustomAttribute_CarriesConstructorState()
    {
        var attribute = typeof(ParameterizedModule).GetCustomAttribute<RunIfValueAttribute>();

        using (Assert.Multiple())
        {
            await Assert.That(attribute).IsNotNull();
            await Assert.That(attribute!.ExpectedValue).IsEqualTo("expected");
            await Assert.That(attribute.Logic).IsEqualTo(ConditionLogic.All);
            await Assert.That(await attribute.EvaluateAsync(Mock.Of<IPipelineContext>())).IsTrue();
        }
    }

    [Test]
    public async Task CustomAttribute_DefaultCancellationOverload_HonorsCancellation()
    {
        var attribute = new RunIfValueAttribute("expected");
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
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI").EvaluateAsync(context)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI", "true").EvaluateAsync(context)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableAttribute("CI", "false").EvaluateAsync(context)).IsFalse();
            await Assert.That(await new SkipIfEnvironmentVariableAttribute("CI", "true").EvaluateAsync(context)).IsTrue();
            await Assert.That(await new RunIfEnvironmentVariableUnsetAttribute("MISSING").EvaluateAsync(context)).IsTrue();
            await Assert.That(await new SkipIfEnvironmentVariableUnsetAttribute("CI").EvaluateAsync(context)).IsFalse();
        }
    }

    [Test]
    public async Task OperatingSystemAttributes_AcceptParameterizedAlternatives()
    {
        var environment = Mock.Of<IEnvironmentContext>(x => x.OperatingSystem == OSPlatform.Linux);
        var context = Mock.Of<IPipelineContext>(x => x.Environment == environment);
        var runCondition = new RunIfOperatingSystemAttribute(
            OperatingSystemIdentifier.Windows,
            OperatingSystemIdentifier.Linux);

        using (Assert.Multiple())
        {
            await Assert.That(await runCondition.EvaluateAsync(context)).IsTrue();
            await Assert.That(await new SkipIfOperatingSystemAttribute(OperatingSystemIdentifier.MacOS)
                .EvaluateAsync(context)).IsFalse();
            await Assert.That(OperatingSystemConditions.GetTargets(runCondition))
                .IsEquivalentTo(["operating-system:windows|linux"]);
        }
    }

    [Test]
    public async Task OperatingSystemAttributes_RequireDefinedSelections()
    {
        await Assert.That(() => new RunIfOperatingSystemAttribute())
            .Throws<ArgumentException>();
        await Assert.That(() => new RunIfOperatingSystemAttribute((OperatingSystemIdentifier) 999))
            .Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task SkipIf_GenericAttributes_SupportThreeAndFourConditions()
    {
        var context = Mock.Of<IPipelineContext>();

        using (Assert.Multiple())
        {
            await Assert.That(await new SkipIfAttribute<AlwaysFalse, AlwaysFalse, AlwaysTrue>()
                .EvaluateAsync(context)).IsTrue();
            await Assert.That(await new SkipIfAttribute<AlwaysFalse, AlwaysFalse, AlwaysFalse, AlwaysTrue>()
                .EvaluateAsync(context)).IsTrue();
        }
    }

    private static IPipelineContext CreateContext(IEnvironmentVariablesContext variables)
    {
        var environment = Mock.Of<IEnvironmentContext>(x => x.Variables == variables);
        return Mock.Of<IPipelineContext>(x => x.Environment == environment);
    }
}
