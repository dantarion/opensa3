![enter image description here](http://i36.tinypic.com/vxhmkg.png)
#What is this?
Project SA, or Project Smash Attacks, is a moveset editor for the Wii fighting game Super Smash Brothers Brawl.  After a while, I wrote a open source version called OpenSA. One rewrite later, rewrites later, and I have decided to start this google code project on a final take on the application, with a goal of stepping away from the Brawl modding community when it is completed.

# Pieces
*  OpenSA3Lib, a C# library for modifying the files
* Tabuu, a editing application based on OpenSA3Lib

This will allow someone else to take over this project in my absence.

# Goal
**The goal of this project is to create a side tool to the wonderful BrawlBox created by Kryal**
While BrawlBox does an excellent job of editing 90% of Brawl files, there are a few filetypes that BrawlBox doesn't edit, or doesn't visualize well, etc. OpenSA's main focus are the moveset files that contain the information for each characters moves.
# Plan
## Milestone 1
* Create a moveset viewer that can view everything that OpenSA2 can
* Code the project so that editing support will be there from the beginning
* Code the project to be able to keep track of unknown areas in the files
* Emedded IronPython Console for running batch operations and queries on files.
 
## Milestone 2
* Create tools and scripts with the goal of being about to account for every byte in the moveset files
* Add in luxury features such as model+animation+hitbox display

## Milestone 3
* 100% Documentation of the moveset files
* A .pac file rebuilder
* A IronPython based PSA language for writing movesets


# Notes
The above is a tentative plan that can change at any time. If you have a feature request or a question, you can create an Issue on the Issues page, or you can PM Dantarion on SmashBoards, AIM, or on #projectm on irc.gamesurge.net
