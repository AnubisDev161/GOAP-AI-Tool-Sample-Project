# GOAP-AI-Tool

This is an example project that showcases my GOAP AI Tool which is an AI behavior tool for Unity.

It includes a test scene with two characters: The hermit, and the Threat:

## To showcase the tool I’ve chosen a survival scenario with two different agent types:
 
The most important one is the Hermit, an agent whose main goal is to build a camp that includes a campfire, a monolith to have a place to calm down after being scared and a proper tent. 

The hermit also has some needs like hunger, thirst and fear (scared). To satisfy those the hermit needs to find a food source and then cook it at the campfire. 

The water source can be collected at a small pond which is next to the second agent, the Threat. 


## The  hermit character

The threat is very simple, it only has two goals, idle and moveToRandomPosition.

Its only purpose is to demonstrate how a plan can be changed at runtime when the current world state changes during execution.


## The threat character


To further illustrate this re-planing there is another element in the project: 
The dayNightCycle. The default time is day and that won’t change unless the user is pressing the button start Night:
 
## The example project at night

After pressing this button, the light changes and a new goal gets available for the hermit: Sleep. This goal can be achieved in two ways, SleepAtImprovised camp and SleepAtTent. 



The first one is more expansive than the other and requires a wood resource that has not yet been stockpiled. The second one is less expansive but requires a proper tent. 

There are also two different resource types for construction, stone and wood. In this rather naive implementation there is no real inventory or something similar, all resources are saved in the world state for easy usage. 
