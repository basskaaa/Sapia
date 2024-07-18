Premade character - fighter class level 2: [[Evad (L2 Fighter)]]

- Attribute system
	- Attribute scores and dice rolling
	- Hp
	- defence
- Abilities system
	- Stamina points
	- 10 basic abilities
- Proto-Gear system
	- Ability to equip weapons and armour
- Ability/attribute card UI
- Turn based system
	- Action types: main action/starter action
	- Top down movement
	- Basic enemy ai (can use simplified player stats for enemy)

![[sapiaShitCardTest.jpg]]


# Combat Script
Setup: Fighter approaches 2 giant rats (green and blue)

1. Roll [[Initiative]] instinct - based off of dex stat
	1. Fighter rolls 10
	2. Green rolls 16
	3. Blue rolls 3
2. Green turn
	1. Starter action uses 'Venom Drip' - adds 1d4 poison to next attack before the start of the next turn
	2. Moves within 5ft of fighter
	3. Main action uses 'Rabid rodent bite' - 1d8 bleeding to fighter + 1d4 poison from 'Venom Drip'
	4. Turn ends. Actions rapidly progress. whole turn takes between 1-2s
3. Fighter turn
	1. Card menu opens (can be toggled with button press)
	2. Starter action uses 'Backstep' - step back 5ft without triggering attack of opportunity
	3. Card menu opacity drops to show action effect
	4. Main action uses 'Lunge attack' - deals additional damage die when target is 10ft away
	5. Moves 20ft away to climbable wall
	6. Free action uses 'Wistful Climb' - use remaining movement to climb without having to succeed athletics check
	7. Turn ends
4. Blue turn 
	1. Moves to wall at Fighter. 
	2. Starter action - Must succeed on athletics check to climb
	3. Fails - takes 1d6 bashing damage from fall
	4. Main action uses 'Raging Hiss' - fighter makes will saving throw or is frightened until the end of their next turn
	5. Reaction prompt - card menu appears with appropriate reaction 'Brave Front' - fighter gets advantage on next will save.
	6. Fighter succeeds will save.
	7. Turn ends