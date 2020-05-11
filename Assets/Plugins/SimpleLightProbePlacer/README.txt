
# Simple Light Probe Placer

Simple Light Probe Placer it is simple tool for Unity3d
and it help you easily place Light Probes in your scene.

It's provides two new components: Light Probe Volume and Light Probe Group Control.

 - Light Probe Volume
	it’s important part of Simple Light Probe Placer
	and can help you easily place your probes all over the scene.
	Provides simple volume (or bounding box) with density settings,
	which you can use for determine where probes should be.

	You can create new instance, just using create menu on top of
	Hierarchy tab or add new Light Probe Volume component to your
	GameObject using Add Component menu.

 - Light Probe Group Control
	Light Probe Group Control is second part of Simple Light Probe Placer and controls Light Probe Group,
	attached to the same GameObject. It will finds all Light Probe Volume and using them settings for creating
	Light Probe positions, for future scene baking.

	You can create new instance, just using create menu on top of
	Hierarchy tab or add new Light Probe Group Control component to your
	GameObject using Add Component menu.
	
Read more on GitHub documentation: https://github.com/AlexanderVorobyov/simple-light-probe-placer
	
- Alexander Vorobyov