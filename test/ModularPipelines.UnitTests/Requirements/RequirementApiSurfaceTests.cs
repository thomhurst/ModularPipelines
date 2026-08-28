using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Requirements;
using Moq;

namespace ModularPipelines.UnitTests.Requirements;

public class RequirementApiSurfaceTests
{
    [Test]
    public async Task Evaluator_Surface_Uses_Conventional_Name_And_Cancellation()
    {
        var method = typeof(IPipelineRequirement).GetMethod("EvaluateAsync");
        var parameters = method!.GetParameters();

        using (Assert.Multiple())
        {
            await Assert.That(typeof(IPipelineRequirement).GetMethod("MustAsync")).IsNull();
            await Assert.That(typeof(PipelineRequirement).GetMethod(
                    "Evaluate",
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.NonPublic))
                .IsNull();
            await Assert.That(parameters).Count().IsEqualTo(2);
            await Assert.That(parameters[0].ParameterType).IsEqualTo(typeof(IPipelineContext));
            await Assert.That(parameters[1].ParameterType).IsEqualTo(typeof(CancellationToken));
            await Assert.That(method.ReturnType).IsEqualTo(typeof(Task<RequirementDecision>));
        }
    }

    [Test]
    public async Task Factory_Surface_Replaces_Duplicate_Requirement_Types()
    {
        var assembly = typeof(Require).Assembly;
        var factoryNames = typeof(Require)
            .GetMethods()
            .Select(static method => method.Name)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(factoryNames).Contains("Windows");
            await Assert.That(factoryNames).Contains("Linux");
            await Assert.That(factoryNames).Contains("MacOS");
            await Assert.That(factoryNames).Contains("WindowsAdmin");
            await Assert.That(factoryNames).Contains("Ci");
            await Assert.That(factoryNames).DoesNotContain("CIEnvironment");
            await Assert.That(assembly.GetType("ModularPipelines.Requirements.WindowsRequirement")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Requirements.LinuxRequirement")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Requirements.MacOSRequirement")).IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Requirements.WindowsAdminRequirement")).IsNull();
        }
    }

    [Test]
    public async Task Exception_Surface_Uses_Requirement_Not_Met_Name()
    {
        var assembly = typeof(RequirementNotMetException).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(typeof(RequirementNotMetException).IsSubclassOf(typeof(PipelineException))).IsTrue();
            await Assert.That(assembly.GetType("ModularPipelines.Exceptions.FailedRequirementsException")).IsNull();
        }
    }

    [Test]
    public async Task Async_Delegate_Receives_Evaluation_Token()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var expectedToken = cancellationTokenSource.Token;
        CancellationToken observedToken = default;
        var requirement = Require.ThatAsync(
            (_, cancellationToken) =>
            {
                observedToken = cancellationToken;
                return Task.FromResult(true);
            },
            "Should not fail");

        var decision = await requirement.EvaluateAsync(
            Mock.Of<IPipelineContext>(),
            expectedToken);

        using (Assert.Multiple())
        {
            await Assert.That(observedToken).IsEqualTo(expectedToken);
            await Assert.That(decision.IsSatisfied).IsTrue();
        }
    }

    [Test]
    public async Task Cancelled_Evaluation_Does_Not_Run_Delegate()
    {
        var evaluated = false;
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        var requirement = Require.That(
            _ =>
            {
                evaluated = true;
                return true;
            },
            "Should not run");

        await Assert.That(async () =>
            {
                await requirement.EvaluateAsync(
                    Mock.Of<IPipelineContext>(),
                    cancellationTokenSource.Token);
            })
            .Throws<OperationCanceledException>();
        await Assert.That(evaluated).IsFalse();
    }
}
