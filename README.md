# Unity3D--ucy/zara01--2020.1.7-1.14
# Function：
1. Track.cs : It's main function is to create the character and control the movement of the character according to the data set.
2. Program.cs ：It is uesd to extract the data from pixel_pos.csv to the DataTable which will be invoked in Track.cs.
3. Track_exercise.cs : Only for exercise  - It will generate a number of random track points and make the character move according to these points.
# General idea
1. Extracting the data of each frame and Manipulating the data between frames.
2. Creating an array of structures. Each character created is an element of the structure and has some attributes which are used to distinguish each other.
# Process
1. The scene is set up.
2. Creating characters and controling their turning and movement according to the data set.
3. Deleting a character when it stops moving for more than 5 frames.
# Defect
1. The scene does not exactly match the trajectory points
2. There is no precise control over when to delete a character.
3. The character rotates and moves stiffly.
4  Code is not standardized.
