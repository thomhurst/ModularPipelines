import assert from 'node:assert/strict';
import { EventEmitter } from 'node:events';
import test from 'node:test';
import { captureDiagnostics, forwardOutput, startDiagnostics } from './diagnostics.mjs';

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
  assert.match(snapshot, /^::group::Pipeline resource sample .+\n/);
  assert.match(snapshot, /\n::endgroup::$/);
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

test('samples wait for a line start outside the pipeline log groups', () => {
  const source = new EventEmitter();
  const written = [];
  const sink = { write: chunk => { written.push(chunk.toString()); return true; } };
  const write = forwardOutput(source, sink);
  const emit = text => source.emit('data', Buffer.from(text));

  write('idle');
  emit('::group::Module\nline one\n');
  write('stale');
  write('inside group');
  emit('line two\n::endgr');
  emit('oup::\npartial');
  write('inside line');
  emit(' line\n');
  assert.deepEqual(written, [
    'idle\n', '::group::Module\nline one\n', 'line two\n::endgr', 'oup::\npartial',
    ' line\n', 'inside line\n',
  ]);
});

test('forwarding pauses the pipeline output until the sink drains', () => {
  const source = Object.assign(new EventEmitter(), {
    paused: false, pause() { this.paused = true; }, resume() { this.paused = false; },
  });
  const sink = Object.assign(new EventEmitter(), { write: () => false });
  forwardOutput(source, sink);
  source.emit('data', Buffer.from('output\n'));
  assert.equal(source.paused, true);
  sink.emit('drain');
  assert.equal(source.paused, false);
});

for (const exitBeforeOutput of [false, true]) {
  test(`stdout cleanup waits for backpressure and resets on progress (exit first: ${exitBeforeOutput})`, t => {
    t.mock.timers.enable({ apis: ['setTimeout'] });
    const source = Object.assign(new EventEmitter(), {
      destroyed: false, pause() {}, resume() {}, destroy() { this.destroyed = true; },
    });
    const child = Object.assign(new EventEmitter(), { stdout: source });
    const sink = Object.assign(new EventEmitter(), { write: () => false });
    startDiagnostics(master, child, {
      stdout: sink, capture: async () => 'sample',
      schedule: () => ({ unref() {} }), cancel() {},
    });
    if (exitBeforeOutput) child.emit('exit');
    source.emit('data', Buffer.from('trailing output\n'));
    if (!exitBeforeOutput) child.emit('exit');
    t.mock.timers.tick(6000);
    assert.equal(source.destroyed, false, 'Backpressure must suspend the cleanup deadline');
    sink.write = () => true;
    sink.emit('drain');
    t.mock.timers.tick(4000);
    source.emit('data', Buffer.from('final output\n'));
    t.mock.timers.tick(4000);
    assert.equal(source.destroyed, false, 'Forwarding progress must reset the idle deadline');
    t.mock.timers.tick(1000);
    assert.equal(source.destroyed, true, 'An idle pipe held open by a descendant must still close');
  });
}

test('stdout completion cancels the post-exit cleanup deadline', t => {
  t.mock.timers.enable({ apis: ['setTimeout'] });
  const source = Object.assign(new EventEmitter(), {
    destroy() { assert.fail('Completed stdout must not be destroyed by a stale timer'); },
  });
  const child = Object.assign(new EventEmitter(), { stdout: source });
  startDiagnostics(master, child, {
    stdout: { write: () => true }, capture: async () => 'sample',
    schedule: () => ({ unref() {} }), cancel() {},
  });
  child.emit('exit');
  source.emit('end');
  t.mock.timers.tick(6000);
});

for (const pauses of [false, true]) {
  test(`post-exit forwarding has a total budget excluding backpressure (pause: ${pauses})`, t => {
    t.mock.timers.enable({ apis: ['setTimeout', 'Date'] });
    const source = Object.assign(new EventEmitter(), {
      destroyed: false, pause() {}, resume() {}, destroy() { this.destroyed = true; },
    });
    const child = new EventEmitter();
    const sink = Object.assign(new EventEmitter(), { write: () => true });
    forwardOutput(source, sink, child);
    child.emit('exit');
    for (let elapsed = 0; elapsed < 28_000; elapsed += 4000) {
      t.mock.timers.tick(4000);
      source.emit('data', Buffer.from('descendant output\n'));
    }
    if (pauses) {
      sink.write = () => false;
      source.emit('data', Buffer.from('buffered output\n'));
      t.mock.timers.tick(60_000);
      assert.equal(source.destroyed, false, 'The total budget must suspend during backpressure');
      sink.write = () => true;
      sink.emit('drain');
    }
    t.mock.timers.tick(1999);
    assert.equal(source.destroyed, false);
    t.mock.timers.tick(1);
    assert.equal(source.destroyed, true, 'Continuously writing descendants must not extend the total budget');
  });
}
