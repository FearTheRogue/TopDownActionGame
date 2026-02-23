# 🌲 Cursed Forest — 2.5D Atmospheric Action Prototype

![Unity](https://img.shields.io/badge/Unity-6.3_LTS-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Game_Development-239120?logo=c-sharp)
![URP](https://img.shields.io/badge/Render_Pipeline-URP_2D-blueviolet)
![Status](https://img.shields.io/badge/Status-Active_Development-green)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

An in-development Unity project exploring atmospheric 2.5D action survival gameplay set within a mysterious, post-collapse medieval forest.

This repository showcases my growth as a junior software and game developer, focusing on:

- Clean, modular C# architecture  
- Readable and scalable gameplay systems  
- Intentional game design rather than isolated prototypes  

> ⚠️ **Work in Progress**  
> Currently focused on building a playable vertical slice.

---

## 🎮 Current Gameplay Features

### 🧍 Player

- Smooth 2D movement with mouse-aimed rotation  
- Separated visuals / arm pivot system for 2.5D presentation  
- ScriptableObject-driven modular weapon system  
- Pistol / Automatic / Burst weapons  
- Health + knockback system  
- Hit-stun and hit feedback  
- Death and respawn loop  

---

### 👁️ Enemies

Modular architecture:

- `EnemyMovement`
- `EnemyFacing`
- Behaviour/state logic
- `EnemyHealth`
- Melee attack component
- Hit-stun + knockback reaction

Implemented enemy types:

- **Hollow Wanderer** — Teaches combat spacing  
- **Thicket Stalker** — Hidden ambush predator  

---

### ⚔️ Combat Systems

- Damage pipeline using `IDamageable`
- Knockback + hit-stun mechanics
- Combat state system
- Smooth camera transitions between exploration and combat
- Designed for tension-based pacing

---

## 🌲 Design Direction

A short atmospheric 2.5D action survival experience focused on:

- **Isolation**
- **Unease**
- **Fragile Survival**

Current focus: 3–5 minute vertical slice validating:

- Combat feel  
- Enemy pacing  
- Ambush tension  
- Environmental mood  

---

## 🛠 Tech Stack

- Unity 6.3 LTS  
- C#  
- URP 2D Renderer  
- 2D sprites in 2.5D perspective  
- Component-driven modular architecture  

---

## 🚧 In Progress

- Forest vertical slice blockout  
- Encounter pacing  
- Relic weapon progression prototype  
- UI feedback polish  

---

## 🔮 Planned Systems

- Additional enemy variants  
- Relic ability system  
- Encounter direction system  
- Expanded UI polish  
- Multi-stage playable slice  

---

## 🧭 Development Philosophy

> Build something small, playable, and atmospheric — then expand safely.

Vertical-slice-first development keeps scope controlled and progress visible.

---

## 📌 Status

**Active development**  
Current milestone: First playable forest slice

---

## 👤 Author

Junior software & game developer focused on:

- Clean code  
- Maintainable systems  
- Thoughtful gameplay design  
- Finishing real, playable projects  
