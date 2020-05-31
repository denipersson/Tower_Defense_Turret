# Tower_Defense_Turret
These two scripts are all that is needed to create a functional turret/tower, most suited for a tower defense game

FOR UNITY

HOW TO USE:

Add the Tower.cs script to your turret game object.

The tower mesh needs to be divided in two parts: the stationary part and the moving part.
The moving part is the one you assign as the "top" part in the inspector.

You must also make an empty game object as a child of the turret, this game object needs a sphere collider (with the isTrigger checkbox checked), as well as the TowerViewSphere.cs script (this is what the turret uses to aim at and shoot enemies marked with the tag "enemy"). 

You then assign this as the "view Distance Sphere" in the inspector of the turret (in the Tower.cs script). 

After that you can assign the projectile(ammo) prefabs in the inspector. These prefabs need the Ammo.cs script. The Ammo.cs script needs to be modified down by the OnHitTriggerEnter method, if you want the projectiles to be able to deal damage.

Lastly, assign the ammo origin point (the transform from which the ammo that the turret uses emerges), this can be an empty game object, but it has to be a child of the top part of the mesh in order to function correctly.

You now have a working turret!

Feel free to use for any purpose as long as you give me credit!
