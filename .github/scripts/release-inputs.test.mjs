import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import test from 'node:test';
import { runInNewContext } from 'node:vm';

const workflow = readFileSync(new URL('../workflows/dotnet.yml', import.meta.url), 'utf8');
const expression = workflow.match(/Publish__IsAlpha:\s*\$\{\{\s*(.*?)\s*\}\}/)?.[1];
assert.ok(expression, 'The workflow must set Publish__IsAlpha');

test('manual releases default to stable', () => {
  assert.match(workflow, /is-alpha:\r?\n(?: {8}[^\r\n]*\r?\n)*? {8}default: false/);
});

// These boolean operators and property lookups share GitHub Actions and JavaScript semantics.
for (const { name, event, inputs, expected } of [
  { name: 'push without dispatch inputs stays alpha', event: 'push', inputs: {}, expected: true },
  { name: 'pull request without dispatch inputs stays alpha', event: 'pull_request', inputs: {}, expected: true },
  { name: 'validation dispatch with default inputs stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': false, 'is-alpha': false }, expected: true },
  { name: 'validation dispatch with explicit alpha stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': false, 'is-alpha': true }, expected: true },
  { name: 'publishing dispatch with default alpha input selects stable', event: 'workflow_dispatch', inputs: { 'publish-packages': true, 'is-alpha': false }, expected: false },
  { name: 'publishing dispatch with explicit alpha stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': true, 'is-alpha': true }, expected: true },
]) {
  test(name, () => {
    // Hyphenated GitHub Actions property names use bracket lookup in JavaScript.
    const javascript = expression.replace(/inputs\.([a-z-]+)/g, "inputs['$1']");
    assert.equal(runInNewContext(javascript, { github: { event_name: event }, inputs }), expected);
  });
}
