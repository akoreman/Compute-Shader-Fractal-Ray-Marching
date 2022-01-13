# Compute Shader Ray Marching
Work In Progress. Working on a program to use ray marching to render in real-time 3D fractals composed of (conformally) transformed primitives. Implemented using HLSL compute shaders through Unity3D. Inspired by http://blog.hvidtfeldts.net/index.php/2011/06/distance-estimated-3d-fractals-part-i/ .

**Currently Implemented:**
- Unlit rendering of fractals composed of translated primitives.
- Surface glow implemented by visualising how close non-hitting rays get to the object.

**To do:**
- Lit shading by normal vector approximation.
- Visual effects, e.g. ambient occlusion.
- Directional shadows.

# Images

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Spheres.PNG" width="400">  

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/Tetra.PNG" width="400">  

<img src="https://raw.github.com/akoreman/WIP-Compute-Shader-Ray-Marching/main/images/MengerSponge.PNG" width="400">  
