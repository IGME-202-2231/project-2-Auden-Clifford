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

Medium-sized and a little slower than the player, These enemies seek the player and attempt to collide with them. They will also flee from the player if their health is low and seek a healing item, if no healing item exists, they enter a frenzy. In all cases except frenzy, these agents will attempt to avoid colliding with other enemies.

### Fight

**Objective:** Seek the player and collide with them.

#### Steering Behaviors

 
- Behaviors
   - Seek the player
- Obstacles - Sawblade (a new map hazard; stationary, but deadly)
- Seperation - Other enemies
   
#### State Transistions

- initial state
- agent rotation speed is above 200rpm
   
### Heal

**Objective:** The Enemy will flee from the player and arrive at a healing item

#### Steering Behaviors

- Behaviors
   - arrive at health item
   - Flee the player
- Obstacles - Sawblade
- Seperation - Other enemies
   
#### State Transistions

- agent rotational velocity is below 100rpm

### Frenzy

**Objective:** ram the player, ignore all other stimuli

#### Steering Behaviors

- Behaviors
   - Seek the player
- Obstacles - none
- Seperation - none
   
#### State Transistions

- agent rotational velocity is below 200rpm and there are no available healing items

## Shooter Enemy

Large and tanky, these enemies will also approach the player's position, however, they will attempt to stay at range and fire a spray of bullets at the player

### Seek

**Objective:** Approach the player's position

#### Steering Behaviors

- Behaviors
   - Seek the player
- Obstacles - Sawblade
- Seperation - Other enemies
   
#### State Transistions

- initial state
- the player is outside the agent's range and agent's rotational velocity is above 300rpm
   
### Shoot

**Objective:** The agent will hang back from the player's position and fire bullets

#### Steering Behaviors

- Behaviors
   - flee from the player
- Obstacles - Sawblade
- Seperation other enemies
   
#### State Transistions

- player is within the agent's range and agent's rotational velocity is above 300rpm

### Frenzy

**Objective:** agent will shoot projectiles at a faster pace and attempt to ram the player

#### Steering Behaviors

- Behaviors
   - seek the player
- Obstacles - none
- Seperation - none
   
#### State Transistions

- agent's rotational velocity is below 300rpm

## Fast Enemy

Quick and dangerous, these enemies will ram themselves into the player at high speeds as if always in a frenzy.

### Fight

**Objective:** Ram the player

#### Steering Behaviors

- Behaviors
   - Persue the player
- Obstacles - Sawblade
- Seperation - Only other fast enemies
   
#### State Transistions

- initial state
- agent's rotational velocity is above 600rpm
   
### Frenzy

**Objective:** Ram the player at all costs (with a higher max speed)

#### Steering Behaviors

- Behaviors
   - Persue the player
- Obstacles - none
- Seperation - none
   
#### State Transistions

- agent's rotational velocity is below 600rpm


## Sources

- all graphics and sprites are custom made.

## Make it Your Own

- This simulation is also a SHMUP, it extends the previous project and allows the user to control a player character which fights against the agents
- added one extra agent and one extra state to two of the agents
- all of the assets are my own custom design

## Known Issues

- agents sometimes get stuck on obstacles.
- fast enemy's persuit sometimes leads it to sit is a spot directly in front of the player when the player is moving particularly fast

### Requirements not completed

- missing 2 steering force for agents to follow.
- MISSING FROM PLAN (but not required): I didn't end up implementing the healing items or healing state for the standard enemy, but the standard enemy still has the required number of states.

