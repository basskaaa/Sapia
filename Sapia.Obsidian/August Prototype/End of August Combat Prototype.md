Premade character - fighter class level 2: [[Fighter Sheet]]

- [[ATTRIBUTES|Attributes]] & attribute scores
	- [[Dice Rolling]]
		- Only need implementation for Attribute Checks for prototype. These will be replaced with skill checks and instinct checks later, but are all thats needed for now.
	- Hp - Fighter has 18, ([[Fighter Hit Points]] systems not needed currently)
	- defence - Fighter has 14 
- Abilities system
	-  Stamina points - Fighter has 6
	- basic abilities - Fighter can choose 5 from 10 options
- Conditions system
	- Frightened condition reverses target movement direction
- Gear system
	- Ability to equip weapons and armour
	- Fighter has light armour and longsword
- Ability/attribute card UI
- Turn based system
	- Action types: main action and starter action
	- Top down movement
	- Basic enemy ai

![[sapiaShitCardTest.jpg]]


# Combat Script
Setup: Fighter ([[Fighter Sheet]]) approaches 2 giant rats (green and blue)

1. Set initiative rolls to determine turn order
2. Green turn
	1. Starter action uses 'Venom Drip' - adds 1d4 poison to next attack before the start of the next turn
	2. Moves within 1 unit of fighter
	3. Main action uses 'Rabid rodent bite' - 1d8 bleeding to fighter + 1d4 poison from 'Venom Drip'
	4. Turn ends. Actions rapidly progress. whole turn takes between 1-2s
3. Fighter turn
	1. Card menu opens (can be toggled with button press)
	2. Starter action uses 'Backstep' - step back 5ft without triggering attack of opportunity
	3. Card menu opacity drops to show action effect
	4. Main action uses 'Lunge attack' - deals additional damage die when target is 10ft away
	5. Moves 4 units away to climbable wall
	6. Free action uses 'Wistful Climb' - use remaining movement to climb without having to succeed strength check
	7. Turn ends
4. Blue turn 
	1. Moves to wall at Fighter. 
	2. Starter action - Must succeed on athletics check to climb
	3. Fails - takes 1d6 bashing damage from fall
	4. Main action uses 'Raging Hiss' - fighter makes wisdom saving throw or is frightened until the end of their next turn
	5. Reaction prompt - card menu appears with appropriate reaction 'Brave Front' - fighter gets advantage on next wisdom save.
	6. Fighter succeeds wisdom save.
	7. Turn ends