using ModularPipelines.OptionsGenerator;
using ModularPipelines.OptionsGenerator.TypeDetection;

if (UnixProcessGroupLauncher.IsInvocation(args))
{
    return await UnixProcessGroupLauncher.RunAsync(args);
}

return await OptionsGeneratorCommand.RunAsync(args);
