# Cursed Forest — 2.5D Atmospheric Action Prototype

An in-development Unity project exploring **atmospheric 2.5D action survival gameplay** set within a mysterious, post-collapse medieval forest.

This repository showcases my growth as a **junior software and game developer**, focusing on:

- clean, modular C# architecture  
- readable and scalable gameplay systems  
- intentional game design rather than isolated prototypes  

> ⚠️ **Work in progress:**  
> The project is currently focused on building a **playable vertical slice** rather than full game content.

---

## 🎮 Current Gameplay Features

### Player
- Smooth 2D movement with mouse-aimed rotation  
- Modular shooting system with support for multiple weapon types  
- Health and damage pipeline using an `IDamageable` interface  

### Enemies
- Reusable **modular enemy architecture**:
  - Movement  
  - Facing  
  - Behaviour/state logic  
  - Health  
  - Melee attack component  

- Implemented enemy types:
  - **Hollow Wanderer** – slow melee enemy teaching combat rhythm  
  - **Thicket Stalker** – hidden ambush enemy creating exploration tension  

### Combat Loop
- Functional **damage, death, and spacing mechanics**  
- Designed around **moderate survival tension** rather than arcade chaos  
- Built to support an **atmospheric exploration → encounter → silence rhythm**

---

## 🌲 Design Direction

The long-term vision is a **short, atmospheric 2.5D action survival experience** where a lone wanderer explores cursed forest ruins and uncovers relic weapons from a lost civilisation.

### Key pillars
- **Isolation** – quiet environmental storytelling  
- **Unease** – tension even when no enemies are present  
- **Fragile survival** – mistakes matter, but gameplay remains fair  

Development is currently focused on creating a **3–5 minute vertical slice** to validate:

- combat feel  
- enemy pacing  
- ambush tension  
- environmental mood  

---

## 🛠 Tech Stack

- **Unity 6.3 LTS**
- **C#**
- **2D sprites in a 2.5D perspective**
- Modular, component-driven architecture

---

## 🚧 In Progress

- Forest vertical-slice level blockout  
- Encounter pacing and enemy placement  
- Relic weapon progression prototype  
- Basic environmental atmosphere and tension flow  

---

## 🔮 Planned Systems (Future)

- Additional enemy behaviours and variants  
- Relic abilities and weapon discovery progression  
- Encounter/spawn direction system  
- UI, feedback, and polish pass  
- Short multi-stage playable experience  

---

## 📂 Purpose of This Repository

This project serves as a **portfolio piece demonstrating**:

- practical Unity architecture  
- iterative gameplay design  
- ability to move from prototype → playable slice → polished experience  

---

## 🧭 Development Philosophy

Rather than building large unfinished systems, this project follows a **vertical-slice-first approach**:

> Build something **small, playable, and atmospheric**,  
> then expand safely.

This keeps scope realistic while ensuring steady, visible progress.

---

## 📌 Status

**Active development**  
Current milestone: **First playable forest slice with functional enemy encounters**

---

## 👤 Author

Developed by a junior software & game developer focused on:

- clean code  
- maintainable systems  
- thoughtful gameplay design  
- finishing real, playable projects
