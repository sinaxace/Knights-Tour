# Knights-Tour
This windows forms program solves the Knight's Tour challenge which is to go to as many available squares on the chessboard as possible without being placed on the same square twice.

## Non-intelligent version using random moves
The simple version of the Knight's Tour challenge is to just run a few trials at random and count how many squares the knight was able to be placed on until it can't move anymore. 

## Heuristic version with accessibility matrix
This algorithm is able to spot access levels of the chessboard using numbers from 2 to 8. For example: access level 2 would be the corner squares of the chessboard because they are the hardest to move on and access level 8 are the middle squares since the Knight piece can be placed in all of it's potential moves. The program would go for the level 2 corners first in order to be able to cover more squares.
