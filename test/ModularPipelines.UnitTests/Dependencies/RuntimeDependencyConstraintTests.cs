using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class RuntimeDependencyConstraintTests
{
    [Test]
    [Arguments("sequential", false)]
    [Arguments("sequential", true)]
    [Arguments("key", false)]
    [Arguments("key", true)]
    [Arguments("limiter", false)]
    [Arguments("limiter", true)]
    [Arguments("hint", false)]
    [Arguments("hint", true)]
    [Arguments("global", false)]
    [Arguments("global", true)]
    public async Task Undeclared_Constrained_Wait_Fails_Without_Timing_Out(string constraint, bool optionalLookup)
    {
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await CreatePipeline(constraint)
            .AddModule(new Reader(constraint, optionalLookup, declareDependency: false, awaitDependency: true))
            .AddModule(new Target(constraint))
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout));

        var collision = exception!.InnerException as DependencyCollisionException;
        await Assert.That(collision).IsNotNull();
        await Assert.That(collision!.Message).Contains(nameof(Reader));
        await Assert.That(collision.Message).Contains(nameof(Target));
        await Assert.That(collision.Message).Contains("active but blocked on a runtime wait");
        await Assert.That(collision.Message).Contains("DependsOn");
    }

    [Test]
    [Arguments("sequential")]
    [Arguments("key")]
    [Arguments("limiter")]
    [Arguments("hint")]
    [Arguments("global")]
    public async Task Declared_Constrained_Wait_Succeeds(string constraint)
    {
        await Assert.That(async () => await CreatePipeline(constraint)
            .AddModule(new Reader(constraint, optionalLookup: false, declareDependency: true, awaitDependency: true))
            .AddModule(new Target(constraint))
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout)).ThrowsNothing();
    }

    [Test]
    public async Task Constrained_Lookup_Without_Awaiting_Succeeds()
    {
        await Assert.That(async () => await CreatePipeline()
            .AddModule(new Reader("sequential", optionalLookup: false, declareDependency: false, awaitDependency: false))
            .AddModule(new Target("sequential"))
            .RunAsync()
            .WaitAsync(TestHostSettings.DefaultTestTimeout)).ThrowsNothing();
    }

    private static PipelineBuilder CreatePipeline(string? constraint = null) => TestPipelineBuilder.Create()
        .ConfigureOptions(options => options with
        {
            DefaultModuleTimeout = TimeSpan.FromSeconds(10),
            Concurrency = options.Concurrency with
            {
                MaxParallelism = constraint == "global" ? 1 : 4,
                MaxCpuIntensiveModules = 1,
                Limits = new Dictionary<string, int> { ["shared"] = 1 },
            },
        });

    private static void ConfigureConstraint(ModuleConfigurationBuilder module, string constraint)
    {
        switch (constraint)
        {
            case "sequential":
                module.WithNotInParallel();
                break;
            case "key":
                module.WithNotInParallel("shared");
                break;
            case "limiter":
                module.WithParallelLimit("shared");
                break;
            case "hint":
                module.WithExecutionHint(ExecutionHint.CpuBound);
                break;
            case "global":
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(constraint));
        }
    }

    private sealed class Reader(string constraint, bool optionalLookup, bool declareDependency, bool awaitDependency) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module)
        {
            ConfigureConstraint(module, constraint);
            module.WithPriority(ModulePriority.Critical);
            if (declareDependency)
            {
                module.DependsOn<Target>();
            }
        }

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            // Keep the lookup in a helper, where a missing dependency can escape static analysis.
            var dependency = Lookup(context);
            if (awaitDependency)
            {
                await dependency;
            }

            return true;
        }

        private Target Lookup(IModuleContext context) => optionalLookup
            ? context.GetModuleIfRegistered<Target>()!
            : context.GetModule<Target>();
    }

    private sealed class Target(string constraint) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => ConfigureConstraint(module, constraint);

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
