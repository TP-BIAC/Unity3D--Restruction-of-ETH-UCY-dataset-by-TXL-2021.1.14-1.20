# Unity3D--restruction-of-ETH-UCY-dataset-by-TXL 2021.1.14-2021.1.20
# Function：
1. eth.cs : It's main function is to create the character and control the movement of the character according to the ETH-eth dataset.
2. hotel.cs : Based on the ETH-hotel dataset.
3. zara01.cs : Based on the UCY-zara01 dataset.
4. zara02.cs : Based on the UCY-zara02 dataset.
5. univ_students001.cs : Based on the UCY-univ-studengts001 dataset.
6. Program.cs ：It is uesd to extract the data from pixel_pos.csv to the DataTable which will be invoked in main scripts.
7. Track_exercise.cs : Only for exercise  - It will generate a number of random track points and make the character move according to these points.
# General idea
1. Extracting the data of each frame and Manipulating the data between frames.
2. Creating an array of structures. Each character created is an element of the structure and has some attributes which are used to distinguish each other.
# Process
1. Five scenes were set up respectively.
2. Creating characters and controling their turning and movement according to the data set.
3. Deleting a character when it stops moving for more than 5 frames.
# Defect
1. The scene does not exactly match the trajectory points
2. There is no precise control over when to delete a character.
3. The character rotates and moves stiffly.
4  Code is not standardized.
