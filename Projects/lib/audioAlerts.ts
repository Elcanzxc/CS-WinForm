// Pure Web Audio API Sound Generator for Clinical Alarms & Hospital Chimes (Zero external audio files required)

type AlertCode = "CODE_BLUE" | "CODE_RED" | "CODE_YELLOW" | "CHIME_CALL";

let audioCtx: AudioContext | null = null;
let activeOscillators: OscillatorNode[] = [];
let activeGains: GainNode[] = [];
let intervalTimer: ReturnType<typeof setInterval> | null = null;
let currentActiveAlert: AlertCode | null = null;
let masterVolume = 0.65;
let isMuted = false;

export function getAudioContext(): AudioContext | null {
  if (typeof window === "undefined") return null;

  if (!audioCtx) {
    const AudioContextClass =
      window.AudioContext ||
      (window as unknown as { webkitAudioContext: typeof AudioContext }).webkitAudioContext;
    if (AudioContextClass) {
      audioCtx = new AudioContextClass();
    }
  }

  if (audioCtx && audioCtx.state === "suspended") {
    audioCtx.resume().catch(() => {});
  }

  return audioCtx;
}

export function setMasterVolume(vol: number) {
  masterVolume = Math.max(0, Math.min(1, vol));
}

export function getMasterVolume(): number {
  return masterVolume;
}

export function setMuted(muted: boolean) {
  isMuted = muted;
  if (muted) {
    stopCurrentAlarm();
  }
}

export function getMuted(): boolean {
  return isMuted;
}

export function getActiveAlarmCode(): AlertCode | null {
  return currentActiveAlert;
}

export function stopCurrentAlarm() {
  if (intervalTimer) {
    clearInterval(intervalTimer);
    intervalTimer = null;
  }

  activeOscillators.forEach((osc) => {
    try {
      osc.stop();
      osc.disconnect();
    } catch {}
  });
  activeOscillators = [];

  activeGains.forEach((gain) => {
    try {
      gain.disconnect();
    } catch {}
  });
  activeGains = [];

  currentActiveAlert = null;
}

/**
 * CODE BLUE: Reanimasiya / Klinik ölüm təhlükəsi
 * High-pitch urgent pulsating buzzer: 1000 Hz, rapid pulses.
 */
function startCodeBlueSiren(ctx: AudioContext) {
  stopCurrentAlarm();
  currentActiveAlert = "CODE_BLUE";

  const pulse = () => {
    if (isMuted) return;
    const now = ctx.currentTime;
    const osc = ctx.createOscillator();
    const gain = ctx.createGain();

    osc.type = "sawtooth";
    osc.frequency.setValueAtTime(1000, now);

    gain.gain.setValueAtTime(masterVolume * 0.85, now);
    gain.gain.exponentialRampToValueAtTime(0.001, now + 0.12);

    osc.connect(gain);
    gain.connect(ctx.destination);

    osc.start(now);
    osc.stop(now + 0.12);
  };

  pulse();
  intervalTimer = setInterval(pulse, 180);
}

/**
 * CODE RED: Kəskin Təcili Vəziyyət (İnfarct, İnsult, Politravma)
 * Alternating dual-tone European medical siren: 880 Hz / 440 Hz
 */
function startCodeRedSiren(ctx: AudioContext) {
  stopCurrentAlarm();
  currentActiveAlert = "CODE_RED";

  let isHigh = true;
  const osc = ctx.createOscillator();
  const gain = ctx.createGain();

  osc.type = "sine";
  osc.frequency.setValueAtTime(880, ctx.currentTime);

  gain.gain.setValueAtTime(masterVolume * 0.75, ctx.currentTime);

  osc.connect(gain);
  gain.connect(ctx.destination);
  osc.start();

  activeOscillators.push(osc);
  activeGains.push(gain);

  intervalTimer = setInterval(() => {
    if (isMuted) {
      gain.gain.setValueAtTime(0, ctx.currentTime);
      return;
    }
    const freq = isHigh ? 440 : 880;
    osc.frequency.setTargetAtTime(freq, ctx.currentTime, 0.05);
    gain.gain.setTargetAtTime(masterVolume * 0.75, ctx.currentTime, 0.02);
    isHigh = !isHigh;
  }, 400);
}

/**
 * CODE YELLOW: Xəbərdarlıq siqnalı
 * Gentle two-tone warning chime: 523.25 Hz (C5) -> 659.25 Hz (E5)
 */
function playCodeYellowChime(ctx: AudioContext) {
  stopCurrentAlarm();
  currentActiveAlert = "CODE_YELLOW";

  if (isMuted) return;

  const now = ctx.currentTime;

  // Tone 1
  const osc1 = ctx.createOscillator();
  const gain1 = ctx.createGain();
  osc1.type = "sine";
  osc1.frequency.setValueAtTime(523.25, now);
  gain1.gain.setValueAtTime(masterVolume * 0.6, now);
  gain1.gain.exponentialRampToValueAtTime(0.001, now + 0.35);
  osc1.connect(gain1);
  gain1.connect(ctx.destination);
  osc1.start(now);
  osc1.stop(now + 0.35);

  // Tone 2
  const osc2 = ctx.createOscillator();
  const gain2 = ctx.createGain();
  osc2.type = "sine";
  osc2.frequency.setValueAtTime(659.25, now + 0.18);
  gain2.gain.setValueAtTime(masterVolume * 0.65, now + 0.18);
  gain2.gain.exponentialRampToValueAtTime(0.001, now + 0.7);
  osc2.connect(gain2);
  gain2.connect(ctx.destination);
  osc2.start(now + 0.18);
  osc2.stop(now + 0.7);

  setTimeout(() => {
    if (currentActiveAlert === "CODE_YELLOW") {
      currentActiveAlert = null;
    }
  }, 800);
}

/**
 * CHIME_CALL: Xəstəxana daxili növbəti pasiyenti çağırma qonqu (Hospital Call Bell Ding-Dong)
 * Harmonic chime: 587.33 Hz (D5) -> 880.00 Hz (A5) with smooth acoustic decay
 */
export function playHospitalCallChime() {
  const ctx = getAudioContext();
  if (!ctx || isMuted) return;

  const now = ctx.currentTime;

  // First Bell Note (D5)
  const osc1 = ctx.createOscillator();
  const gain1 = ctx.createGain();
  osc1.type = "sine";
  osc1.frequency.setValueAtTime(587.33, now);
  gain1.gain.setValueAtTime(masterVolume * 0.7, now);
  gain1.gain.exponentialRampToValueAtTime(0.0001, now + 0.8);
  osc1.connect(gain1);
  gain1.connect(ctx.destination);
  osc1.start(now);
  osc1.stop(now + 0.85);

  // Second Higher Bell Note (A5)
  const osc2 = ctx.createOscillator();
  const gain2 = ctx.createGain();
  osc2.type = "sine";
  osc2.frequency.setValueAtTime(880.0, now + 0.28);
  gain2.gain.setValueAtTime(masterVolume * 0.8, now + 0.28);
  gain2.gain.exponentialRampToValueAtTime(0.0001, now + 1.4);
  osc2.connect(gain2);
  gain2.connect(ctx.destination);
  osc2.start(now + 0.28);
  osc2.stop(now + 1.45);
}

export function playEmergencyAlert(code: AlertCode) {
  const ctx = getAudioContext();
  if (!ctx) return;

  if (code === "CODE_BLUE") {
    startCodeBlueSiren(ctx);
  } else if (code === "CODE_RED") {
    startCodeRedSiren(ctx);
  } else if (code === "CODE_YELLOW") {
    playCodeYellowChime(ctx);
  } else if (code === "CHIME_CALL") {
    playHospitalCallChime();
  }
}
