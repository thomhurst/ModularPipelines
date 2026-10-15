import { spawn } from 'node:child_process';
import path from 'node:path';
import { pathToFileURL } from 'node:url';
import { startDiagnostics } from './diagnostics.mjs';

// JavaScript actions receive the runtime token needed by @actions/artifact.
// Keep it in this process tree rather than exporting it to subsequent steps.
export function startPipeline(sourceEnvironment, startProcess = spawn) {
  const environment = { ...sourceEnvironment };
  // version-tag.yml owns tags for pushes to main.
  if (environment.GITHUB_EVENT_NAME === 'push') {
    delete environment.GitHub__StandardToken;
  }
  // Preserve the native compiler's GC defaults on macOS.
  if (environment.RUNNER_OS === 'macOS') {
    delete environment.DOTNET_gcServer;
    delete environment.COMPlus_gcServer;
  }
  return startProcess('dotnet', [path.join(environment.RUNNER_TEMP, 'pipeline-host', 'ModularPipelines.Build.dll')], {
    cwd: path.join(environment.GITHUB_WORKSPACE, 'src', 'ModularPipelines.Build'),
    env: environment,
    stdio: 'inherit',
  });
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  if (process.env.MODULARPIPELINES_ARTIFACT_CHECK === 'true') {
    try {
      const { validate } = await import('./validate.mjs');
      await validate();
    } catch {
      console.error('Concurrent GitHub artifact validation failed.');
      process.exitCode = 1;
    }
  } else {
    const child = startPipeline(process.env);
    startDiagnostics(process.env, child);
    child.on('error', () => {
      console.error('Unable to start the pipeline host.');
      process.exitCode = 1;
    });
    child.on('exit', code => { process.exitCode = code ?? 1; });
    for (const signal of ['SIGINT', 'SIGTERM']) {
      process.once(signal, () => child.kill(signal));
    }
  }
}
