# BeatBrawl

**A guitar-learning game that listens to your playing and helps you improve.**

BeatBrawl is an experimental guitar-learning project designed to make learning guitar more interactive through real-time audio detection and gameplay.

The goal is to build a system that listens to a guitar through a microphone, identifies the notes being played, and eventually connects them to the guitar fretboard and interactive learning challenges.

## Features

- **Microphone Input** — Captures audio through the microphone.
- **Volume Detection** — Measures input levels to detect when a guitar is being played.
- **Pitch Detection** — Analyses audio to estimate the frequency of a played note.
- **Note Recognition** — Converts detected frequencies into musical note names.
- **Guitar Fretboard Mapping** — Developing a system to identify notes across strings and frets.

## Tech Stack

- Unity 6
- C#
- Visual Studio
- Microphone-based audio processing

## Current Status

The project is in early development.

Current work focuses on audio input, pitch detection, note recognition, and building the foundation for guitar fretboard recognition.

## Roadmap

- [x] Microphone input and volume monitoring
- [x] Pitch detection foundation
- [x] Musical note identification foundation
- [ ] Improve tuner accuracy and stability
- [ ] Map detected notes to guitar strings and frets
- [ ] Build interactive guitar-learning challenges
- [ ] Develop gameplay and progress tracking

## Getting Started

1. Clone this repository.
2. Open Unity Hub.
3. Select **Add project from disk**.
4. Choose the cloned project folder.
5. Open it with a compatible Unity 6 editor.

Allow Unity to import the project assets and packages before running it.

## Project Goal

BeatBrawl aims to make guitar practice more engaging by combining audio processing, music theory, and interactive gameplay.

This is a work in progress, with new features being developed and tested incrementally.

## Architecture

BeatBrawl is built around microphone-based guitar input. The microphone volume monitor checks the input level, while the pitch detector analyses the audio to estimate the played note. Detected pitch is then converted into a musical note name, which can be used by the fretboard system and future gameplay mechanics.

**Audio flow:** Microphone Input → Volume Monitoring → Pitch Detection → Note Identification → Fretboard Mapping → Gameplay

The project is under active development. The gameplay integration and timing-based combat system are planned features.

## Planned Gameplay Concept

BeatBrawl aims to turn guitar practice into a fighting game. Players will control a character by playing guitar notes and rhythms, with timing and accuracy influencing attacks and combat performance. This concept is still in development.


## Author

**Samarth Naik** — [@Sammybrimzz](https://github.com/Sammybrimzz)

---

*Built as a learning project exploring game development, audio processing, and guitar technology.*
