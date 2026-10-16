using ModularPipelines.Attributes;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Dependencies;

public class TagFilterTests : TestBase
{
    [ModuleTag("fast")]
    [ModuleCategory("build")]
    private class TaggedModule : SimpleTestModule<string>
    {
        protected override string Result => "tagged";
    }

    private class FluentModule : SimpleTestModule<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithTags("fast", "remote");

        protected override string Result => "fluent";
    }

    private class UntaggedModule : SimpleTestModule<string>
    {
        protected override string Result => "untagged";
    }

    [ModuleTag("fast")]
    [ModularPipelines.DependsOn<UntaggedModule>]
    private class RequiredDependent : SimpleTestModule<string>
    {
        protected override string Result => throw new InvalidOperationException("Filtered dependency must cascade skip.");
    }

    [Test]
    public async Task IncludesAnyMatchingTag_FromAttributesAndFluentConfiguration()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<FluentModule>()
            .AddModule<UntaggedModule>()
            .RunOnlyTags("missing", "FAST")
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<TaggedModule>().Single()).ValueOrDefault).IsEqualTo("tagged");
        await Assert.That((await summary.Modules.OfType<FluentModule>().Single()).ValueOrDefault).IsEqualTo("fluent");
        await Assert.That((await summary.Modules.OfType<UntaggedModule>().Single()).SkipDecisionOrDefault).IsNotNull();
    }

    [Test]
    public async Task ExclusionWinsWhenModuleHasIncludedAndIgnoredTags()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<FluentModule>()
            .RunOnlyTags("fast")
            .IgnoreTags("REMOTE")
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<TaggedModule>().Single()).ValueOrDefault).IsEqualTo("tagged");
        await Assert.That((await summary.Modules.OfType<FluentModule>().Single()).SkipDecisionOrDefault!.Reason)
            .Contains("tag");
    }

    [Test]
    public async Task IgnoreTagsLeavesUntaggedModulesRunnable()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<UntaggedModule>()
            .IgnoreTags("FAST")
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<TaggedModule>().Single()).SkipDecisionOrDefault).IsNotNull();
        await Assert.That((await summary.Modules.OfType<UntaggedModule>().Single()).ValueOrDefault).IsEqualTo("untagged");
    }

    [Test]
    public async Task EmptyFiltersDoNotRestrictExecution()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<UntaggedModule>()
            .RunOnlyTags()
            .IgnoreTags()
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<TaggedModule>().Single()).ValueOrDefault).IsEqualTo("tagged");
        await Assert.That((await summary.Modules.OfType<UntaggedModule>().Single()).ValueOrDefault).IsEqualTo("untagged");
    }

    [Test]
    public async Task CategoryAndTagFiltersBothApply()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<FluentModule>()
            .RunOnlyTags("fast")
            .ConfigureOptions(options => options with { RunOnlyCategories = ["build"] })
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<TaggedModule>().Single()).ValueOrDefault).IsEqualTo("tagged");
        await Assert.That((await summary.Modules.OfType<FluentModule>().Single()).SkipDecisionOrDefault).IsNotNull();
    }

    [Test]
    public async Task FilteredRequiredDependencyCascadeSkipsDependent()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<UntaggedModule>()
            .AddModule<RequiredDependent>()
            .RunOnlyTags("fast")
            .RunAsync();

        await Assert.That((await summary.Modules.OfType<RequiredDependent>().Single()).SkipDecisionOrDefault!.Reason)
            .Contains(nameof(UntaggedModule));
    }

    [Test]
    public async Task PlanUsesSameAttributeAndFluentTagFilters()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<TaggedModule>()
            .AddModule<FluentModule>()
            .AddModule<UntaggedModule>()
            .RunOnlyTags("FAST")
            .IgnoreTags("REMOTE");
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(TaggedModule)].ShouldSkip).IsFalse();
        await Assert.That(modules[typeof(FluentModule)].ShouldSkip).IsTrue();
        await Assert.That(modules[typeof(UntaggedModule)].ShouldSkip).IsTrue();
    }

    [Test]
    public async Task TagFiltersAreDefensivelyCopied()
    {
        var included = new List<string> { "fast" };
        var ignored = new List<string> { "remote" };
        var options = new PipelineOptions { RunOnlyTags = included, IgnoreTags = ignored };
        included.Clear();
        ignored.Clear();

        await Assert.That(options.RunOnlyTags).IsEquivalentTo(["fast"]);
        await Assert.That(options.IgnoreTags).IsEquivalentTo(["remote"]);
        await Assert.That(((ICollection<string>) options.RunOnlyTags!).IsReadOnly).IsTrue();
        await Assert.That(((ICollection<string>) options.IgnoreTags!).IsReadOnly).IsTrue();
    }
}
