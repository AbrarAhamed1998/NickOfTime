# NickOfTime
A game about a worn out, run of the mill, protagonist that is tired of the predictable villains that he comes across.

This is currently under development. Some features mentioned here may have been reworked or may still be in progress.

## About
This game is a 2D platformer/shooter style game that I initially created for GameOff22 game jam but eventually decided to work on it a little more as I fell in love with the idea.
The mechanics are simple. You pick up any gun that you find and start shooting away at anything you come across. The quirks are that the guns behave in a very cliche fashion.

I've chosen the pixel art route for this game as I've always had a soft spot for the retro feel of pixel art in games. This was also to test my 2D art/animation skills which I still suck at but have slightly improved thanks to this game.

## Objective
Our main character "The Protaganist" has been captured by evil forces. Fortunately, he is assisted by a certain "Romantic Interest" behind enemy lines and must fight through some minions to reach the person responsible for his capture.

The game consists of several floors the player must traverse, to reach the top floor where the boss resides.

## Mechanics 
The main mechanics of the game can be broken up into movement and combat.

### Movement
The player can move left or right and has a jetpack that assists their movement and allows them to fly for brief periods of time.
The jetpack allows to build momentum which, after a certain threshold can damage enemies on collision. A part of this damage is also taken by the player depending on the intensity of the impact.

### Combat 
The player can pick up a variety of weapons along their journey. Each of these weapons have a unique quirk of thier own.

#### The Unlimited Pistol
![image](https://github.com/AbrarAhamed1998/NickOfTime/assets/51904325/a4e0e25c-71e0-4f6a-a7ea-a4ac7ad4277f)
<img src=https://github.com/AbrarAhamed1998/NickOfTime/assets/51904325/a4e0e25c-71e0-4f6a-a7ea-a4ac7ad4277f width=100 height=100>
A pistol that simply does not run out of ammo and never has to be reloaded, none has questioned this gross oversight in the making of this weapon, especially the developer of this game. This stands as a reference to all the action movies where a protaganist's weapon never has to be reloaded.

#### The Ocelot Revolver
This revolver is an homage to western classics and is very accurate. It also has the ability to ricochet off surfaces and may damage the wielder as well.
The revolver also seems to be possesed by a demon that loves to play russian roulette. When the demon is active the gun empties itself down to only one bullet and the player must cycle through the gun to fire the one bullet that does bonus damage.

#### The One-Liner Rocket Launcher 
The One Liner Rocket is a weapon that does a ton of damage, but requires its wielder to utter a cheesy one-liner from 80s action movies, before firing a rocket.This delay can mean the difference between life and death, coupled with the rarity of finding this weapon and the ammo for it, makes this weapon difficult to use.

## Systems
Here are some of the systems that I've built for the game that I'd like to showcase:

### Movement
The movement of the game is built upon a mix of the subclass sanbox pattern and state pattern. I found this to be appropriate when I started out but now I realize that I need to refactor these classes to use the component pattern as well.
1. The `CharacterBase` class acts as parent sandbox class that provides protected virtual methods that the subclass can implement. 
3. The `CharacterStateBase` class also acts in a similar way to provide methods to any child state classes that arise.
4. The `CharacterBaseConfigSO` class is the base Data class that provides the base variables needed by `CharacterBase`.

#### CharacterBase : 
Highlight some of the main template functions here for movement
The `CharacterBase` class acts as a sort of parent class that provides the base functionality for any character in the game.
Highlight usage of the base by enemy and player.

#### Character State BAse:
Highlight the use of the state pattern
How characters implement these states differently while still allowing for some base functionality.

#### CharacterBaseConfig:
Highlight some of the variables being used and reused between different types of characters

### AI
How the A* uses the state system
The different AI States go here

### Combat 
- WeaponBase classes template
- USe of the base class for the different weapons and their extension.

### Pooling system
- Pooling system logic and ease of access
- Usage
- Pros
- Cons
- Improvements that could be made

### Dialog System
- Simplicity and ease of use to create content

