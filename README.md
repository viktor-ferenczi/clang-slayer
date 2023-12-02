# Clang Slayer

The Clang Slager mod "defuses" clang drives, weapons or almost any contraption which imposes clang.

It works both in single and multiplayer. In case of multiplayer it works only server side 
where the physics simulation runs. Works in both creative and survival game modes.

[Clang Slayer mod on the Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=2918228306)

## Theory of operation

### Step by step
1. The mod tracks all mechanical connection bases.
2. For each mechanical base it compares the intended head position and orientation to the actual one.
3. Mechanical strain (clang forces) are measured based on the differece in position or orientation.
4. The mod temporarily overrides the parameters of the affected mechanical bases to "defuse" clang.

### Explanation
What it does is measuring how much a mechanical top part is off-positioned compared to the current settings 
and position read from its base part, which is proportional to the mechanical stress in the physics engine. 
Then it tries to "tune out" the excess stress by correcting the settings of the mechanical bases, while
temporarily overriding the player's settings. It has to be done carefully to keep the expected functionality 
working. Mechanical connections should still be able to bare the expected load as long as they don't clang out.

## Test world

TBD

# Commands
TBD

# Tunable parameters
TBD

# Known issues
- This mod may interfere with some programmable block scripts controlling mechanical base blocks.

# Troubleshooting

TBD

# Support / Help

SE Mods Discord: https://discord.gg/PYPFPGf3Ca
