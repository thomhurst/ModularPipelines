import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { runInNewContext } from 'node:vm';

const workflow = readFileSync(new URL('../workflows/dotnet.yml', import.meta.url), 'utf8');
const jobs = Object.fromEntries([...workflow.matchAll(/^  ([a-z0-9-]+):\r?\n([\s\S]*?)(?=^  [a-z0-9-]+:|$(?![\s\S]))/gm)]
  .map(match => [match[1], match[2]]));
const dependencies = name => (jobs[name].match(/^    needs: (.+)$/m)?.[1] ?? '').trim()
  .replace(/[\[\]]/g, '').split(',').map(value => value.trim()).filter(Boolean);
const expression = jobs.publish.match(/^    if: \$\{\{ (.*?) \}\}/m)?.[1];
assert.ok(expression, 'Publication must have an explicit job gate');

function canPublish(options = {}) {
  const { aggregate, pipeline, event, ref, publish, cancelled } = { aggregate: 'success', pipeline: 'success',
    event: 'workflow_dispatch', ref: 'refs/heads/main', publish: true, cancelled: false, ...options };
  const javascript = expression.replace(/(needs|inputs)\.([a-z-]+)/g, "$1['$2']");
  return runInNewContext(javascript, { needs: { 'required-pipeline': { result: aggregate }, pipeline: { result: pipeline } },
    github: { event_name: event, ref }, inputs: { 'publish-packages': publish }, cancelled: () => cancelled });
}

test('only a successful aggregate and package producer allow a release', () => {
  for (const aggregate of ['success', 'failure', 'cancelled', 'skipped', '']) {
    for (const pipeline of ['success', 'failure', 'cancelled', 'skipped', '']) {
      for (const cancelled of [true, false]) {
        assert.equal(canPublish({ aggregate, pipeline, cancelled }),
          aggregate === 'success' && pipeline === 'success' && !cancelled,
          `aggregate=${aggregate} producer=${pipeline} cancelled=${cancelled}`);
      }
    }
  }
});

test('PRs, pushes, validation dispatches and feature branches cannot publish', () => {
  for (const event of ['pull_request', 'push', 'workflow_dispatch']) {
    for (const ref of ['refs/heads/main', 'refs/heads/feature']) {
      for (const publish of [true, false, undefined]) {
        assert.equal(canPublish({ event, ref, publish }), event === 'workflow_dispatch' && ref === 'refs/heads/main' && publish === true);
      }
    }
  }
});

test('the publication graph waits for every validation without holding up workers', () => {
  assert.deepEqual(dependencies('publish'), ['required-pipeline', 'pipeline']);
  assert.deepEqual(dependencies('required-pipeline').sort(), ['fast-fail', 'pipeline', 'pipeline-workers',
    'cross-platform-build', 'generated-integration', 'changed-integration', 'artifact-storage', 'analyzers', 'trim-aot', 'cmd-tests'].sort());
  assert.deepEqual(dependencies('pipeline'), ['fast-fail', 'pipeline-host']);
  assert.deepEqual(dependencies('pipeline-workers'), ['fast-fail', 'pipeline-host']);
  // Reject a dependency cycle, including a publication dependency introduced into the aggregate.
  function visit(name, ancestors = []) {
    assert.ok(!ancestors.includes(name), `Dependency cycle: ${[...ancestors, name].join(' -> ')}`);
    for (const dependency of dependencies(name)) visit(dependency, [...ancestors, name]);
  }
  visit('publish');
});

test('the aggregate owns skip applicability and cancelled runs never publish', () => {
  assert.match(expression, /!cancelled\(\)/);
  assert.doesNotMatch(expression, /success\(\)/, 'Implicit success would suppress releases with intentional skipped ancestors');
  assert.match(jobs['required-pipeline'], /if: always\(\)/);
  assert.match(jobs['required-pipeline'], /Assert-RequiredPipelineContext.ps1/);
  assert.match(jobs['fast-fail'], /Test-RequiredPipelineContext.ps1/);
  assert.match(jobs['fast-fail'], /Test-PublishRelease.ps1/);
  assert.match(jobs['fast-fail'], /release-gate.test.mjs/);
});

test('only the gated job receives publishing credentials and identity permissions', () => {
  assert.match(jobs.publish, /environment: Production/);
  assert.match(jobs.publish, /id-token: write/);
  assert.match(jobs.publish, /NuGet\/login@/);
  assert.match(jobs.publish, /secrets.ADMIN_TOKEN \|\| github.token/);
  assert.match(jobs.publish, /Publish-Release.ps1/);
  assert.ok(jobs.publish.indexOf('-ValidateOnly') < jobs.publish.indexOf('NuGet/login@'));
  for (const [name, body] of Object.entries(jobs)) {
    if (name !== 'publish') assert.doesNotMatch(body, /NuGet\/login@|NUGET_API_KEY|secrets.ADMIN_TOKEN|id-token: write|Publish-Release.ps1/, name);
  }
  const program = readFileSync(new URL('../../src/ModularPipelines.Build/Program.cs', import.meta.url), 'utf8');
  assert.match(program, /else if \(!isRunningInCI && !builder.Environment.IsDevelopment\(\)\)\s*\{\s*builder.AddModule<UploadPackagesToNugetModule>\(\)\s*\.AddModule<CreateReleaseModule>\(\);/);
});

test('publication consumes the producer attempt and the exact rehearsed packages', () => {
  assert.match(jobs.pipeline, /release_artifact: release-packages-\$\{\{ github.run_attempt \}\}/);
  assert.match(jobs.pipeline, /name: release-packages-\$\{\{ github.run_attempt \}\}/);
  assert.match(jobs.pipeline, /path: \$\{\{ runner.temp \}\}\/nuget-publish-rehearsal\//);
  assert.match(jobs.pipeline, /if-no-files-found: error/);
  assert.match(jobs.publish, /name: \$\{\{ needs.pipeline.outputs.release_artifact \}\}/);
  assert.match(jobs.publish, /-Commit \$env:GITHUB_SHA/);
  assert.doesNotMatch(jobs.publish, /dotnet (?:build|pack|run)/);
});
