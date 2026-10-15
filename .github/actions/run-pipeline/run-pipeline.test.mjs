import assert from 'node:assert/strict';
import path from 'node:path';
import test from 'node:test';
import { startPipeline } from './run-pipeline.mjs';

for (const event of ['push', 'pull_request']) {
  for (const platform of ['Linux', 'Windows', 'macOS']) {
    test(`pipeline environment on ${event}/${platform} retains artifact authentication and publishing restrictions`, () => {
      const environment = {
        GITHUB_EVENT_NAME: event, RUNNER_OS: platform,
        RUNNER_TEMP: '/runner/temp', GITHUB_WORKSPACE: '/runner/workspace',
        GitHub__StandardToken: 'tag-token', DOTNET_gcServer: '0', COMPlus_gcServer: '0',
        ACTIONS_RUNTIME_TOKEN: 'runtime-token', ACTIONS_RESULTS_URL: 'results-url',
      };
      startPipeline(environment, (command, args, options) => {
        assert.equal(command, 'dotnet');
        assert.deepEqual(args, [path.join(environment.RUNNER_TEMP, 'pipeline-host', 'ModularPipelines.Build.dll')]);
        assert.equal(options.cwd, path.join(environment.GITHUB_WORKSPACE, 'src', 'ModularPipelines.Build'));
        assert.equal(options.env.GitHub__StandardToken, event === 'push' ? undefined : 'tag-token');
        assert.equal(options.env.DOTNET_gcServer, platform === 'macOS' ? undefined : '0');
        assert.equal(options.env.COMPlus_gcServer, platform === 'macOS' ? undefined : '0');
        assert.equal(options.env.ACTIONS_RUNTIME_TOKEN, 'runtime-token');
        assert.equal(options.env.ACTIONS_RESULTS_URL, 'results-url');
        assert.equal(options.stdio, 'inherit');
      });
      assert.equal(environment.GitHub__StandardToken, 'tag-token');
    });
  }
}
