# 🌲 Cursed Forest — 2.5D Atmospheric Action Prototype

An in-development Unity project exploring atmospheric 2.5D action survival gameplay set within a mysterious, post-collapse medieval forest.

This repository showcases my growth as a junior software and game developer, focusing on:

- Clean, modular C# architecture  
- Readable and scalable gameplay systems  
- Intentional game design rather than isolated prototypes  

> ⚠️ **Work in Progress**  
> The project is currently focused on building a playable vertical slice rather than full game content.

---

## 🎮 Current Gameplay Features

### 🧍 Player

- Smooth 2D movement with mouse-aimed rotation  
- Separated visuals / arm pivot system for 2.5D presentation  
- Modular shooting system (ScriptableObject-driven weapons)  
- Support for multiple weapon types (pistol / automatic / burst)  
- Health + knockback system  
- Hit feedback (flash + impulse)  
- Death and respawn loop  

---

### 👁️ Enemies

Reusable modular enemy architecture:

- `EnemyMovement`
- `EnemyFacing`
- Behaviour / state components
- `EnemyHealth`
- Melee attack component
- Hit-stun + knockback reaction system

Implemented enemy types:

- **Hollow Wanderer**  
  Slow melee enemy that teaches combat spacing and rhythm.

- **Thicket Stalker**  
  Hidden ambush predator that lunges from concealment and pressures exploration.

---

### ⚔️ Combat Loop

- Functional damage, knockback, and hit-stun mechanics  
- Combat state system driving camera transitions  
- Smooth camera zoom between exploration and combat  
- Designed around moderate survival tension rather than arcade chaos  
- Structured to support an *exploration → encounter → silence* rhythm  

---

## 🌲 Design Direction

The long-term vision is a short, atmospheric 2.5D action survival experience where a lone wanderer explores cursed forest ruins and uncovers relic weapons from a lost civilisation.

### Core Pillars

- **Isolation** — quiet environmental storytelling  
- **Unease** — tension even when no enemies are present  
- **Fragile Survival** — mistakes matter, but gameplay remains fair  

Development is currently focused on creating a **3–5 minute vertical slice** to validate:

- Combat feel  
- Enemy pacing  
- Ambush tension  
- Environmental mood  

---

## 🛠 Tech Stack

- Unity 6.3 LTS  
- C#  
- URP (2D Renderer)  
- 2D sprites presented in a 2.5D perspective  
- Modular, component-driven architecture  

---

## 🚧 Currently In Development

- Forest vertical-slice level blockout  
- Encounter pacing and enemy placement  
- Relic weapon progression prototype  
- Environmental atmosphere and tension flow  
- UI feedback polish (health, hit reactions, etc.)  

---

## 🔮 Planned Systems

- Additional enemy behaviours and variants  
- Relic abilities and weapon discovery progression  
- Encounter direction / spawn pacing system  
- Expanded UI and combat feedback polish  
- Short multi-stage playable experience  

---

## 📂 Purpose of This Repository

This project serves as a portfolio piece demonstrating:

- Practical Unity architecture  
- Iterative gameplay design  
- Component-driven system design  
- Ability to move from prototype → playable slice → polished experience  

---

## 🧭 Development Philosophy

Rather than building large unfinished systems, this project follows a **vertical-slice-first approach**:

> Build something small, playable, and atmospheric —  
> then expand safely.

This keeps scope realistic while ensuring steady, visible progress.

---

## 📌 Status

**Active development**  
Current milestone: **First playable forest slice with functional enemy encounters**

---

## 👤 Author

Developed by a junior software & game developer focused on:

- Clean code  
- Maintainable systems  
- Thoughtful gameplay design  
- Finishing real, playable projects  
