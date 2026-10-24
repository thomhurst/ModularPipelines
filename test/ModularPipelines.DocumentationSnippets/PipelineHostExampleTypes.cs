using ModularPipelines.Context;

namespace ModularPipelines.DocumentationSnippets.PipelineHostExample;

// Application-specific types supplied by the consumer of the pipeline-host example.
public class NuGetSettings;

public class PublishSettings;

public class LocalBuildModule : ExampleModule;

public class LocalTestModule : ExampleModule;

public class CIBuildModule : ExampleModule;

public class CITestModule : ExampleModule;

public class PublishModule : ExampleModule;

public class CleanupModule : ExampleModule;

public class ReportModule : ExampleModule;

public abstract class ExampleModule : Module
{
    protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
