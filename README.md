# CityPlanner
Group assignment for Computer Graphics course


# User guideline

## 1. Opening the project in Unity
By far the easiest method of getting this to run is to download zip/repository:
https://github.com/revosw/LandmarkVisibility

Then open Unity Hub, and press Open > Add project from disk. Navigate to where you downloaded the folder, and select the folder that contains Assets and Assembly-CSProj.

## 2. Controls
Left mouse click to place light intensity bar + calculate sky exposure

WASD to rotate camera, ↑←↓→ to translate camera


## 3. Using the features of the city planner
After opening this project, you can begin to use it by clicking the start icon and entering the game scene.

To show the sky exposure of one position, you can just click any position of the scene and the number will be shown in the button on the bottom right. The number can range from 0 to 100 in which 0 means the position can not get any sunlight while 100 means there is no shelter at all.

To calculate landmark visibility, users need to first click the Visibility index button and then the visibility index will show on the same button.

To show the shadows in the city, click the heatmap button, and then the tool will go into the shadow calculation function. The heatmap visualizes this parameter with different colors. A redder area means it is in shadow for a longer time during the daytime while black means there is no shadowed time.

To use the CFH pattern, one first has to activate day/night cycle and wait for 2 in-game days. After that, you can input three digit patterns inside the "Pattern" input field.

If the show barchart button is clicked, users can then click any position. And this tool will show how the light intensity at that position. While the day/night circle function starts, the bar chart will vary according to the light intensity change of the time.