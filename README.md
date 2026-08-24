# GOAP-AI-Tool
<img width="1917" height="862" alt="Screenshot 2026-08-24 140528" src="https://github.com/user-attachments/assets/5e5c3126-71ef-4e18-8e95-2a850c3236af" />

This is an example project that showcases my GOAP AI Tool which is an AI behavior tool for Unity.


## To showcase the tool I’ve chosen a survival scenario with two different agent types:

The most important one is the Hermit, an agent whose main goal is to build a camp that includes a campfire, a monolith to have a place to calm down after being scared and to build a proper tent. 
## 🥷 The hermit character
<img width="306" height="553" alt="Screenshot 2026-05-24 210207" src="https://github.com/user-attachments/assets/6c3174ef-7929-45fa-8c9c-b672981afe89" />




The hermit also has some needs like hunger, thirst and fear (scared). To satisfy those the hermit needs to find a food source and then cook it at the campfire. 

The water source can be collected at a small pond which is next to the second agent, the Threat. 


## ⚔️ The threat character
<img width="714" height="519" alt="Screenshot 2026-05-24 210102" src="https://github.com/user-attachments/assets/9bf464ca-359c-44c0-a290-da5d8e9de9d2" />

The threat is very simple, it only has two goals, idle and moveToRandomPosition.

Its only purpose is to demonstrate how a plan can be changed at runtime when the current world state changes during execution.

## 🌗 The example project at night
<img width="1917" height="857" alt="Screenshot 2026-08-24 151416" src="https://github.com/user-attachments/assets/557e1481-b30e-4533-9185-7f679482d377" />

To further illustrate this re-planing there is another element in the project: 
The dayNightCycle. The default time is day and that won’t change unless the user is pressing the button Start Night:

After pressing this button, the light changes and a new goal gets available for the hermit: Sleep. 

<img width="485" height="377" alt="Screenshot 2026-08-24 152439" src="https://github.com/user-attachments/assets/8586768a-5ad3-4cf4-b981-92072900722f" />

This goal can be achieved in two ways, SleepAtImprovised camp and SleepAtTent. 
The first one is more expansive than the other and requires a wood resource that has not yet been stockpiled. The second one is less expansive but requires a proper tent. 

There are also two different resource types for construction, stone and wood. In this rather naive implementation there is no real inventory or something similar, all resources are saved in the world state for easy usage.

## ❗All the graphical content that I used was provided by several asset packages.

Used packages:

- POLY - Medieval Camp by ANIMPIC STUDIO
 
- FREE - Modular Character - Fantasy RPG Human Male by Blink 

- Mini Legion Grunt PBR HP Polyart by Dungeon Mason 

- Environment Pack: Free Forest Sample by Supercyan 

