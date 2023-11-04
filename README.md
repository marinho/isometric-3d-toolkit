# Isometric 3D Engine for Godot

## Overview

This library contains multiple utilities that can be used to build games for Godot 4.x using C# language.

It is mostly targeted to 3D games using Isometric and Orthographic camera, even though parts of it can be used for any type of game. It also contains utilities for Shaders and in the future will contain scripts fpr Blender as well. My goal is to release as much as possible of the code base I use to develop games with Godot, except for the final games themselves.

## License

As you can read in LICENSE file, this repository is licensed under CC BY 4.0, which means you can do whatever you want with this code, but please refer to my work by mentioning this repository.

## Contents

I will gradually add more information and content to this repository, as long as I manage to dettach parts of code of my games, shaders and artwork. That's because a lot of what I build got not a clear separation of concerns, as it's sometimes just a prototype meant to be quickly made. Regardless, I will continue moving this stuff into this repository and adding documentation as much as possible.

Be aware the code API may change drastically from a commit to another, as the code is still very fresh and I can learn new ways to do things I wasn't aware before. I have been using Godot since September 2023, which means a lot of what is here is made by a beginner in that engine.

## How to use it

That's how I do it:

1. Go to or create an `addons` folder in your Godot game project
2. In a terminal window, execute: `git submodule add https://github.com/marinho/isometric-3d-engine.git isometric_3d_engine`
3. Try using the C# scripts in your nodes (I will soon provide a folder "examples" with a playground level and some exemples)

## How to contribute

Please bear in mind this repository isn't aimed to become a well refined community-ready project, as I mostly have no time for such. Therefore, go ahead and fork the repository and make changes for your own need, and in case you see it can be helpful to share, feel free to create issues and pull requests, but don't expect much of my response 😉
