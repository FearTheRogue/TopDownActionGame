# 🌲 Cursed Forest — 2.5D Relic Survival Prototype

![Unity](https://img.shields.io/badge/Unity-6.3_LTS-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Game_Development-239120?logo=c-sharp)
![URP](https://img.shields.io/badge/Render_Pipeline-URP_2D-blueviolet)
![Status](https://img.shields.io/badge/Status-Active_Development-green)
![License](https://img.shields.io/badge/License-MIT-lightgrey)

A modular 2.5D action survival prototype built in Unity, exploring tension-driven relic combat within a cursed medieval forest.

This project focuses on:

- Clean, modular C# architecture  
- Ability-driven combat systems  
- Survival pacing over arcade chaos  
- Vertical-slice-first development  

> ⚠️ **Work in Progress**  
> Currently building and refining the first playable forest encounter slice.

---

## 🎮 Current Gameplay Systems

### 🧍 Player

- Smooth physics-based 2D movement  
- Mouse-aimed arm pivot with separated visuals (2.5D presentation)  
- Dash with invulnerability window  
- Root Burst defensive ability (knockback + stun + chip damage)  
- Health system with knockback response  
- Death and respawn loop  
- Hit flash + damage feedback  
- Combat state integration  

---

### 🔮 Relic Weapon System

ScriptableObject-driven modular weapon architecture.

#### Ember Rune (Semi-Auto Relic)

- Charge-based firing system  
- In-combat regeneration  
- Fire rate limiting  
- Controlled spread  
- Balanced hit-stun  
- Designed for deliberate pacing  

Weapons emphasize:

- Resource management  
- Positioning over spam  
- Survival rhythm  

---

### 👁️ Enemy Architecture

Component-based modular enemy system:

- `EnemyMovement`  
- `EnemyFacing`  
- Behaviour logic modules  
- `EnemyHealth`  
- `HitReaction`  
- Melee attack component  
- Knockback integration  

#### Implemented Enemies

- **Hollow Wanderer** — Teaches spacing and pressure  
- **Thicket Stalker** — Ambush predator with rush attack  

Enemies support:

- Custom stun durations  
- Knockback stacking  
- Modular behaviour composition  

---

### 🎥 Combat & Camera Systems

- Combat state manager  
- Smooth camera zoom transitions (explore ↔ combat)  
- Priority-based Cinemachine setup  
- Ability-triggered combat engagement  

---

### 🖥 UI Systems

- Player health bar with smoothing  
- Dash cooldown indicator  
- Root Burst cooldown indicator  
- Reusable ability cooldown framework  
- Modular UI cooldown architecture  

---

## 🌲 Design Direction

Cursed Forest is evolving into:

> A slow, tension-driven relic survival experience.

Core pillars:

- **Isolation** — Quiet spaces between danger  
- **Unease** — Ambush predators and limited resources  
- **Fragile Survival** — Abilities provide escape, not dominance  

**Core Loop:**

Explore → Tension → Encounter → Survive → Silence  

---

## 🛠 Tech Stack

- Unity 6.3 LTS  
- C#  
- URP 2D Renderer  
- Cinemachine  
- Component-driven modular architecture  
- ScriptableObjects for weapons and configuration  

---

## 🚧 In Active Development

- Forest vertical slice level design  
- Enemy encounter pacing  
- Relic charge balancing  
- Combat feel refinement  
- Ability feedback polish  

---

## 🔮 Planned Systems

- Additional relic types  
- Rune modifiers / upgrades  
- Enemy archetype expansion  
- Light AI pathing improvements  
- Environmental interaction systems  
- Expanded UI polish & feedback  

---

## 🧭 Development Philosophy

> Build the core loop first.  
> Make it feel good.  
> Expand only when it earns it.

The goal is cohesion over feature quantity.

---

## 📌 Current Milestone

First fully playable forest slice featuring:

- Charge-based relic combat  
- Dash + Root Burst abilities  
- Ambush enemy encounters  
- Combat state camera transitions  

---

## 👤 Author

Junior software & game developer focused on:

- Clean, maintainable systems  
- Modular architecture  
- Iterative design  
- Building complete playable experiences  
