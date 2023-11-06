# Project SpinnerScorge_v1.1

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Auden Clifford
-   Section: 02

## Simulation Design

My simulation will be an extention of my Project 1 SHMUP project where the enemies are updated to be autonomous agents with several states to control behavior.

### Controls

-   Movement
    -   Up: W
    -   Down: S
    -   Left: A
    -   Right: D
-   Fire: SPACEBAR

## Standard Enemy

These enemies seek the player and attempt to collide with them. They will also flee from the player if their health is low and seek a healing item, if no healing item exists, they enter a frenzy. In all cases except frenzy, these agents will attempt to avoid colliding with other enemies.

This enemy is medium sized and a little slower than the player, however they are the most plentiful of the enemies and can easily overwhelm the player.

### Fight

**Objective:** Seek the player and collide with them.

#### Steering Behaviors

 
- Behaviors
   - Seek() the player
   - Flee() obsticles & other enemies
- Obstacles - Sawblade (a new map hazard; stationary, but deadly)
- Seperation - Other enemies
   
#### State Transistions

This is this agent's initial state, it can be transitioned into when the 
rotational velocity of this enemy is above 1/3 it's starting value.
   
### Heal

**Objective:** The Enemy will flee from the player and seek out a healing item

#### Steering Behaviors

- _List all behaviors used by this state_
   - Seek() health item
   - Flee() the player, obsticles, other enemies
- Obstacles - Sawblade
- Seperation - Other enemies
   
#### State Transistions

- _List all the ways this agent can transition to this state_

### _State 3 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## _Agent 2 Name_

_A brief explanation of this agent._

### _State 1 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_
   
### _State 2 Name_

**Objective:** _A brief explanation of this state's objective._

#### Steering Behaviors

- _List all behaviors used by this state_
- Obstacles - _List all obstacle types this state avoids_
- Seperation - _List all agents this state seperates from_
   
#### State Transistions

- _List all the ways this agent can transition to this state_

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_

## Make it Your Own

- _List out what you added to your game to make it different for you_
- _If you will add more agents or states make sure to list here and add it to the documention above_
- _If you will add your own assets make sure to list it here and add it to the Sources section

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

