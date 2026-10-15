import assert from 'node:assert/strict';
import { EventEmitter } from 'node:events';
import test from 'node:test';
import { captureDiagnostics, startDiagnostics } from './diagnostics.mjs';

const master = { RUNNER_OS: 'Linux', MODULARPIPELINES_INSTANCE_INDEX: '0' };
const settle = () => new Promise(resolve => setImmediate(resolve));

test('resource snapshot bounds process output and selects memory counters', async () => {
  const snapshot = await captureDiagnostics(async (command, args, options) => {
    assert.equal(command, 'ps');
    assert.deepEqual(args, ['-eo', 'pid,ppid,comm,pcpu,rss,stat,wchan:24', '--sort=-rss']);
    assert.equal(options.timeout, 5000);
    assert.equal(options.maxBuffer, 1024 * 1024);
    return { stdout: ['PID PPID COMMAND %CPU RSS STAT WCHAN',
      ...Array.from({ length: 30 }, (_, index) => `${index} process`)].join('\n') };
  }, async (file, encoding) => {
    assert.equal(file, '/proc/meminfo');
    assert.equal(encoding, 'utf8');
    return 'MemTotal: 100 kB\nMemAvailable: 50 kB\nSwapTotal: 20 kB\nSwapFree: 10 kB\nDirty: 1 kB\nWriteback: 0 kB\nOther: 42 kB\n';
  });
  assert.match(snapshot, /MemAvailable: 50 kB/);
  assert.match(snapshot, /SwapFree: 10 kB/);
  assert.match(snapshot, /19 process/);
  assert.doesNotMatch(snapshot, /20 process|Other:/);
});

test('workers and other platforms never schedule diagnostics', () => {
  for (const environment of [
    { ...master, RUNNER_OS: 'Windows' }, { ...master, RUNNER_OS: 'macOS' },
    { ...master, MODULARPIPELINES_INSTANCE_INDEX: '1' }, {},
  ]) {
    startDiagnostics(environment, new EventEmitter(), {
      schedule: () => assert.fail('Unexpected diagnostics timer'),
    });
  }
});

for (const event of ['exit', 'error']) {
  test(`diagnostics stop on child ${event} and discard pending snapshots`, async () => {
    const child = new EventEmitter();
    const lines = [];
    let complete;
    let tick;
    let captures = 0;
    let canceled = 0;
    const timer = { unref() {} };
    startDiagnostics(master, child, {
      capture: () => { captures++; return new Promise(resolve => { complete = resolve; }); },
      write: line => lines.push(line),
      schedule: (callback, milliseconds) => {
        assert.equal(milliseconds, 60_000);
        tick = callback;
        return timer;
      },
      cancel: value => { assert.equal(value, timer); canceled++; },
    });
    await tick();
    assert.equal(captures, 1, 'Slow snapshots must not overlap');
    complete('first sample');
    await settle();
    assert.deepEqual(lines, ['first sample']);
    const pending = tick();
    child.emit(event);
    assert.equal(canceled, 1);
    complete('late sample');
    await pending;
    await tick();
    assert.equal(captures, 2);
    assert.deepEqual(lines, ['first sample']);
  });
}

test('diagnostic failures stay generic and do not prevent later snapshots', async () => {
  const child = new EventEmitter();
  const lines = [];
  let tick;
  let fail = true;
  startDiagnostics(master, child, {
    capture: async () => { if (fail) throw new Error('private diagnostic detail'); return 'recovered'; },
    write: line => lines.push(line),
    schedule: callback => { tick = callback; return { unref() {} }; },
    cancel() {},
  });
  await settle();
  fail = false;
  await tick();
  child.emit('exit');
  assert.deepEqual(lines, ['Pipeline resource sample unavailable.', 'recovered']);
});
