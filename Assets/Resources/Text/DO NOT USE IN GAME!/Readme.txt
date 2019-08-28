For each textscript file, use the following formats for each line:

* required fields
- optional fields

---------------------------------------------------

TEXT LINE:
Changes the text/appearance of the Dialogue Box and the animation of the character model following an action (mouse click, camera change, etc.).

TEXT|{Character name}|{Dialogue text}|{Character animation}

* TEXT: indicates that the current line read is a text-line action.
* {Character name}: the name of the character will appear on the header of the Dialogue Box in the scene.
* {Line of dialogue}: the line of dialogue in which the character will speak will appear in the main canvas of the Dialogue Box in the scene.
- {Character animation}: the character model will mimic the animation from the animation's given filename [e.g. giving "idle_00" will call the animation from the "m01@idle_00.fbx" file in the "Assets/TaichiCharacterPack/Resources/Taichi/Animations Legacy" folder].

---------------------------------------------------

CAMERA LINE:
Changes the position and rotation of the camera following a mouse click.

CAMERA|{position dX},{position dY},{position dZ}|{rotation dX},{rotation dY},{rotation dZ}

* CAMERA: indicates that the current line read is a camera change action.
* {position dX},{position dY},{position dZ}: indicates how many units the camera must move in X, Y, and Z coordinates [e.g. "1,2,-3" would move the camera one unit in the +X direction (right), two units in the +Y direction (up), and three units in the -Z direction (back)].
* {rotation dX},{rotation dY},{rotation dZ}: indicates how many degrees the camera must rotate in X, Y, and Z coordinates [e.g. "10,-90,180" would rotate the camera 10 degrees around the X axis (down), -90 degrees around the Y axis (left), and 180 degrees around the Z axis (upside-down)].

---------------------------------------------------

INPUT LINE:
Triggers an input prompt to display with given options.

INPUT|{option text}={start at line}={end at line}={jump to line}|{option text}={start at line}={end at line}={jump to line}|{option text}={start at line}={end at line}={jump to line}|{option text}={start at line}={end at line}={jump to line}

* INPUT: indicates that the current line read is an input prompt action.
* For each of the fours options:
  {option text}: The text which displays for the given option
  {start at line}: When the option is selected, this is the line number which the code jumps to which triggers a response to the selected option.
  {end at line}: This is the line number in which the response to the option ends.
  {jump to line}: This is the line number in which the regular events of the game shall continue.
  
---------------------------------------------------

SCORE LINE:
Adds/subtracts the score from a certain character after an event is triggered.

SCORE|{character name}|{score increment}

*SCORE: indicates that the current line read is a score-increment action
*{character name}: the name of the character in whose route the score must be added to/decremented from.
*{score increment}: The number of points given/taken away from the character's route when an event is triggered.

