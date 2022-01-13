# Compute Shader Ray Marching
From scratch implemented render engine to use ray marching to render, in real-time, 3D fractals composed of (conformally) transformed primitives. Implemented using HLSL compute shaders through Unity3D. Unity is used to render the final image to screen and to handle user input. Inspired by http://blog.hvidtfeldts.net/index.php/2011/06/distance-estimated-3d-fractals-part-i/ .

**Currently Implemented:**
- Unlit rendering of fractals composed of translated primitives.
- Directional shadows by retracing from the point where a camera ray hits the geometry to the light source.
- Surface glow implemented by visualising how close non-hitting rays get to the object.
- Coarse ambient occlusion by using the number of marching steps as an analogue for the complexity of the geometry.

**To do:**
- Lit shading by normal vector approximation.
- Soft shadows.

# Images


<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Shadows.PNG" width="400">  

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Shadows2.PNG" width="400"> 

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Diffuse.PNG" width="400">

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Spheres.PNG" width="400">  

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Tetra.PNG" width="400">  

