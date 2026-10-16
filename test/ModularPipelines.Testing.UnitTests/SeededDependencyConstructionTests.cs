using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing.UnitTests;

public class SeededDependencyConstructionTests
{
    [Test]
    [Arguments("success")]
    [Arguments("failure")]
    [Arguments("skipped")]
    public async Task MissingDependencyServiceExplainsHowToRegisterIt(string outcome)
    {
        var builder = ModuleTester.For<Consumer, string>();
        switch (outcome)
        {
            case "success":
                builder.WithDependencyResult<ServiceDependency, string>("seeded");
                break;
            case "failure":
                builder.WithDependencyFailure<ServiceDependency, string>(new Exception("seeded failure"));
                break;
            case "skipped":
                builder.WithSkippedDependency<ServiceDependency, string>("seeded skip");
                break;
        }

        var exception = await Assert.That(async () => { await builder.ExecuteAsync(); }).Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).Contains(nameof(ServiceDependency));
        await Assert.That(exception.Message).Contains(nameof(DependencyService));
        await Assert.That(exception.Message).Contains("WithService");
        await Assert.That(exception.InnerException).IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.Message).Contains(exception.InnerException!.Message);
    }

    [Test]
    public async Task MissingTransitiveServiceExplainsHowToRegisterIt()
    {
        var exception = await Assert.That(async () =>
        {
            await ModuleTester.For<TransitiveConsumer, string>()
                .WithDependencyResult<TransitiveDependency, string>("seeded")
                .ExecuteAsync();
        }).Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).Contains(nameof(ServiceDependency));
        await Assert.That(exception.Message).Contains(nameof(DependencyService));
        await Assert.That(exception.Message).Contains("WithService");
    }

    [Test]
    public async Task RegisteringDependencyServiceAllowsSeededResultConsumption()
    {
        var run = await ModuleTester.For<Consumer, string>()
            .WithDependencyResult<ServiceDependency, string>("seeded")
            .WithService(new DependencyService())
            .ExecuteAsync();

        await Assert.That(run.Value).IsEqualTo("seeded");
    }

    [Test]
    public async Task UnrelatedConstructionFailureIsPreserved()
    {
        var exception = await Assert.That(async () =>
        {
            await ModuleTester.For<BrokenConsumer, string>()
                .WithDependencyResult<ServiceDependency, string>("seeded")
                .ExecuteAsync();
        }).Throws<InvalidOperationException>();

        await Assert.That(exception!.Message).IsEqualTo("constructor failed");
        await Assert.That(exception.InnerException).IsNull();
    }

    public sealed class DependencyService;

    public sealed class ServiceDependency : Module<string>
    {
        public ServiceDependency(DependencyService service)
        {
            ArgumentNullException.ThrowIfNull(service);
        }

        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Seeded dependency must not execute.");
    }

    [ModularPipelines.DependsOn<ServiceDependency>]
    public sealed class TransitiveDependency : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Seeded dependency must not execute.");
    }

    [ModularPipelines.DependsOn<ServiceDependency>]
    public sealed class Consumer : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => (await context.GetModule<ServiceDependency>()).Value;
    }

    [ModularPipelines.DependsOn<TransitiveDependency>]
    public sealed class TransitiveConsumer : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("Construction must fail before execution.");
    }

    public sealed class BrokenConsumer : Module<string>
    {
        public BrokenConsumer() => throw new InvalidOperationException("constructor failed");

        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult("unreachable");
    }
}
