"""Sound effect recipes for Flying Rush. Each function returns a rendered Track."""

from synth import Track, saw, square, triangle


def pulse(duty):
    return lambda p: square(p, duty)


def rifle():
    """The assault rifle: a short dry crack over a low thump, light enough to repeat ten times a second."""
    t = Track(0.22)
    t.add_noise(0, 0.07, 0.55, smoothing=0.25, decay_power=2.6, seed=1)
    t.add_tone(0, 0.05, 180, triangle, 0.5, freq_end=70, attack=0.001, release=0.04)
    t.add_tone(0, 0.02, 1400, pulse(0.3), 0.08, freq_end=600, release=0.01)
    return t


def shotgun():
    """The shotgun: a wide boom with a long gritty tail."""
    t = Track(0.6)
    t.add_noise(0, 0.32, 0.7, smoothing=0.45, decay_power=2.0, seed=2)
    t.add_tone(0, 0.16, 120, triangle, 0.7, freq_end=40, attack=0.001, release=0.1)
    t.add_noise(0.02, 0.1, 0.3, smoothing=0.05, decay_power=2.5, seed=3)
    return t


def sniper():
    """The sniper rifle: a sharp crack that rings out down the cage."""
    t = Track(1.0)
    t.add_noise(0, 0.05, 0.7, smoothing=0.05, decay_power=2.4, seed=4)
    t.add_tone(0, 0.12, 220, triangle, 0.6, freq_end=55, attack=0.001, release=0.1)
    t.add_noise(0.04, 0.8, 0.18, smoothing=0.85, decay_power=1.6, seed=5)
    return t


def rocket():
    """A rocket leaves the tube: a whump and a rising hiss."""
    t = Track(0.6)
    t.add_tone(0, 0.12, 90, triangle, 0.6, freq_end=50, attack=0.001, release=0.08)
    t.add_noise(0.02, 0.45, 0.35, smoothing=0.5, decay_power=1.0, seed=6)
    t.add_tone(0.02, 0.4, 240, pulse(0.5), 0.07, freq_end=900, release=0.05)
    return t


def explosion():
    """A rocket explodes: a deep blast and a rumbling tail."""
    t = Track(1.4)
    t.add_tone(0, 0.5, 80, triangle, 0.9, freq_end=28, attack=0.001, release=0.4)
    t.add_noise(0, 1.1, 0.7, smoothing=0.82, decay_power=1.8, seed=7)
    t.add_noise(0, 0.15, 0.4, smoothing=0.15, decay_power=2.5, seed=8)
    return t


def railgun():
    """The railgun: an electric charge snapping into a falling zap."""
    t = Track(0.9)
    t.add_tone(0, 0.06, 300, saw, 0.2, freq_end=1600, attack=0.001, release=0.01)
    t.add_tone(0.05, 0.55, 2200, pulse(0.5), 0.18, freq_end=180, release=0.15, vibrato=0.04)
    t.add_tone(0.05, 0.45, 1100, saw, 0.12, freq_end=90, release=0.15)
    t.add_noise(0.05, 0.2, 0.3, smoothing=0.1, decay_power=2.0, seed=9)
    return t


def reload():
    """A magazine out and in: two metallic clacks."""
    t = Track(0.45)
    for at, pitch, seed in ((0.0, 1500, 10), (0.22, 1900, 11)):
        t.add_noise(at, 0.04, 0.35, smoothing=0.1, decay_power=3.0, seed=seed)
        t.add_tone(at, 0.03, pitch, pulse(0.25), 0.12, freq_end=pitch * 0.7, release=0.02)
    return t


def empty():
    """Dry fire: a thin click."""
    t = Track(0.1)
    t.add_noise(0, 0.02, 0.3, smoothing=0.0, decay_power=3.0, seed=12)
    t.add_tone(0, 0.015, 2400, pulse(0.25), 0.1, release=0.01)
    return t


def switch():
    """Swapping weapons: a soft double clack."""
    t = Track(0.25)
    t.add_noise(0, 0.03, 0.25, smoothing=0.3, decay_power=3.0, seed=13)
    t.add_noise(0.08, 0.03, 0.2, smoothing=0.3, decay_power=3.0, seed=14)
    t.add_tone(0.08, 0.05, 700, triangle, 0.15, freq_end=900, release=0.02)
    return t


def pickup():
    """A power weapon picked up: a bright rising chime."""
    t = Track(0.6)
    for i, freq in enumerate((523.25, 659.25, 783.99, 1046.5)):
        t.add_tone(i * 0.06, 0.18, freq, pulse(0.25), 0.16, release=0.1)
    return t


def jump():
    """A jump from the ground: a quick boot push and a whoosh."""
    t = Track(0.3)
    t.add_noise(0, 0.18, 0.25, smoothing=0.75, decay_power=1.4, seed=15)
    t.add_tone(0, 0.1, 160, triangle, 0.25, freq_end=320, release=0.05)
    return t


def air_jump():
    """An air jump: the thruster pack puffs; the game raises its pitch with every jump used."""
    t = Track(0.35)
    t.add_noise(0, 0.22, 0.3, smoothing=0.6, decay_power=1.2, seed=16)
    t.add_tone(0, 0.16, 320, pulse(0.25), 0.1, freq_end=760, release=0.06)
    return t


def land():
    """Landing hard: a heavy thud."""
    t = Track(0.35)
    t.add_tone(0, 0.1, 110, triangle, 0.7, freq_end=45, attack=0.001, release=0.08)
    t.add_noise(0, 0.08, 0.3, smoothing=0.6, decay_power=2.0, seed=17)
    return t


def hit():
    """Hit marker: a short high tick."""
    t = Track(0.12)
    t.add_tone(0, 0.03, 1800, pulse(0.5), 0.25, release=0.03)
    return t


def headshot():
    """Headshot: a ringing ping."""
    t = Track(0.4)
    t.add_tone(0, 0.05, 2600, triangle, 0.4, release=0.3, vibrato=0.01)
    t.add_tone(0, 0.04, 1300, pulse(0.5), 0.15, release=0.1)
    return t


def kill():
    """Kill confirmed: two quick rising notes."""
    t = Track(0.4)
    t.add_tone(0, 0.07, 880, pulse(0.25), 0.22, release=0.04)
    t.add_tone(0.08, 0.14, 1318.5, pulse(0.25), 0.22, release=0.1)
    return t


def hurt():
    """Taking damage: a dull low hit."""
    t = Track(0.3)
    t.add_tone(0, 0.08, 140, square, 0.25, freq_end=70, release=0.08)
    t.add_noise(0, 0.08, 0.25, smoothing=0.7, decay_power=2.0, seed=18)
    return t


def death():
    """A fighter goes down: a crunchy burst and a falling tone."""
    t = Track(0.9)
    t.add_noise(0, 0.3, 0.5, smoothing=0.4, decay_power=1.8, seed=19)
    t.add_tone(0, 0.6, 500, saw, 0.2, freq_end=60, release=0.15)
    return t


def countdown():
    """The countdown: a clean beep."""
    t = Track(0.3)
    t.add_tone(0, 0.12, 880, pulse(0.5), 0.3, release=0.06)
    return t


def go():
    """Fight: a higher, longer beep with a fifth on top."""
    t = Track(0.7)
    t.add_tone(0, 0.4, 1318.5, pulse(0.5), 0.25, release=0.15)
    t.add_tone(0, 0.4, 1975.5, pulse(0.25), 0.12, release=0.15)
    return t


def streak():
    """A kill streak banner: a short fanfare."""
    t = Track(0.9)
    for i, (freq, length) in enumerate(((659.25, 0.1), (783.99, 0.1), (987.77, 0.1), (1318.5, 0.35))):
        t.add_tone(i * 0.11, length, freq, pulse(0.25), 0.22, release=0.08)
    return t


def win():
    """Victory: a rising major arpeggio that holds."""
    t = Track(1.8)
    notes = (523.25, 659.25, 783.99, 1046.5, 1318.5)
    for i, freq in enumerate(notes):
        t.add_tone(i * 0.12, 0.9 - i * 0.1, freq, pulse(0.25), 0.18, release=0.3)
    t.add_tone(0.6, 0.9, 261.63, triangle, 0.3, release=0.3)
    return t


def lose():
    """Defeat: a slow falling minor line."""
    t = Track(1.8)
    for i, freq in enumerate((659.25, 587.33, 523.25, 440.0)):
        t.add_tone(i * 0.25, 0.3, freq, triangle, 0.3, release=0.15, vibrato=0.01)
    t.add_tone(1.0, 0.6, 220.0, square, 0.12, release=0.2)
    return t


def move():
    """Menu cursor: a soft tick."""
    t = Track(0.08)
    t.add_tone(0, 0.025, 1100, pulse(0.25), 0.18, release=0.02)
    return t


def select():
    """Menu choice: two quick notes."""
    t = Track(0.2)
    t.add_tone(0, 0.04, 880, pulse(0.25), 0.2, release=0.02)
    t.add_tone(0.05, 0.06, 1318.5, pulse(0.25), 0.2, release=0.04)
    return t


def pause():
    """Pause: a low blip."""
    t = Track(0.2)
    t.add_tone(0, 0.08, 330, triangle, 0.35, freq_end=220, release=0.05)
    return t


EFFECTS = {
    "rifle.ogg": rifle,
    "shotgun.ogg": shotgun,
    "sniper.ogg": sniper,
    "rocket.ogg": rocket,
    "explosion.ogg": explosion,
    "railgun.ogg": railgun,
    "reload.ogg": reload,
    "empty.ogg": empty,
    "switch.ogg": switch,
    "pickup.ogg": pickup,
    "jump.ogg": jump,
    "air_jump.ogg": air_jump,
    "land.ogg": land,
    "hit.ogg": hit,
    "headshot.ogg": headshot,
    "kill.ogg": kill,
    "hurt.ogg": hurt,
    "death.ogg": death,
    "countdown.ogg": countdown,
    "go.ogg": go,
    "streak.ogg": streak,
    "win.ogg": win,
    "lose.ogg": lose,
    "move.ogg": move,
    "select.ogg": select,
    "pause.ogg": pause,
}

# Normalise every effect so their loudness matches.
for _name, _recipe in list(EFFECTS.items()):
    def _normalised(recipe=_recipe):
        track = recipe()
        track.normalize(0.85)
        return track
    EFFECTS[_name] = _normalised
