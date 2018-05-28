# Bullet Hell Fish

The Bullet Hell Fish is a program for Windows that generates different streams of random input in one of the opend windowed.

## Getting Started

Downlaod the project and compile it with Visual Studio (or whatever you prefer)

### Setting it up

The program allows to:
1. Choose the inputs allowed through external csv file. The format is defined in standard files in folder Input Sheets.
2. Choose the source through external csv file. Optional.
3. Define custom behaviors. Optional. Some custom behaviors are found in the Behaviors folder.

### BHF scripting language

To define behavior, place a ".bhf" in a folder and select it from the program. the scripting language allows:
1. Selection
2. Iteration
3. Image recognition
4. Input managment

´´´
int coins
if "continue_screen" similar in 285 245 458 56
	lock input
		press START
		coins = coins + 1
		wait 500 ms
	unlock input
´´´

Declaration of variables:
[int / integer / bool / yesno / string / phrase] [name of variable]
´´´
int coins
bool test = false
string name = "Mark"
´´´

Selection:
if [condition]

´´´
if test
	[content]
´´´

Lock:
It locks the input until "unlock input" called

Wait:
´´´
wait 500 ms
wait 3 seconds
´´´

Press:
´´´
press BUTTON_NAME
´´´

Image comparison:
the keyword is "similar"

[source image] [percentage] similar in [rectangle coordinates]

´´´
"continue_screen" similar in 285 245 458 56
"winning_screen" 90% similar in 285 245 458 56
´´´

