import assert from 'node:assert/strict';
import childProcess from 'node:child_process';
import { appendFileSync } from 'node:fs';
import { syncBuiltinESMExports } from 'node:module';

// Only replace the GitHub process boundary. The publisher still runs its real
// command-line entry point, filesystem reads, extraction, validation, and rendering.
childProcess.spawnSync = (command, args, options) => {
  assert.equal(command, 'gh');
  assert.equal(options.shell, false);
  appendFileSync(process.env.REVIEW_TEST_GITHUB_CALLS, JSON.stringify({ args, input: options.input }) + '\n');
  return {
    status: 0,
    stdout: args[0] === 'pr'
      ? JSON.stringify({ state: 'OPEN', headRefOid: process.env.REVIEW_HEAD_SHA }) : '{}',
  };
};
syncBuiltinESMExports();
