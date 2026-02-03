# playerMechs
This is a simple 2D player movement script to attach to the player to allow movement, jumping, dashing, double jumping, and a sword slash in Unity game Engine.
attach this script to the player GameObject which has a Rigidbody2D and Animator component.
make sure to set up the Animator with appropriate parameters and animations.
also, create a sword hitbox GameObject as a child of the player and assign it to the swordHitbox variable.
same thing for the groundCheck GameObject to detect ground collisions.
adjust the movement, dash, jump, and slash settings in the inspector as needed.
ensure the ground GameObjects are tagged as "Ground" for proper ground detection.
this script also includes a simple health system where the player can take damage and die.
customize the TakeDamage and Die methods to fit your game's requirements.
I learned from this simple code how physics based bodies work in a 2D game and how to make them interact with the world around them through implementing multiple fun mechanics.
Future updates: 
-better organize the script 
-make it more specific instead of handling multiple mechanics at the same time
-improve the double jump mechanic by adding a tiny timer between the initial jump and the double jump
-make the platforming more forgiving by adding a buffer for each jump(so you don't need to perfectly time each jump the moment you land)
