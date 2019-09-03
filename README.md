# NarrowingWalls

Affordance perception in AR environment. Designed for Microsoft HoloLens.
This project is created using Unity 2018.2.21f1. Any Unity 2018.2.x should work fine with this project.

## Overview

![Overview](/Pictures/overview.png?raw=true "Overview")

This project is designed for assessing human affordance perception abilities in AR through Microsoft HoloLens. Project consists of two large 15m \* 30m \* 0.25m walls with a gap in between that changes width under voice commands, interactive buttons to change and display user's shoulder width and eye height, and floating text to indicate experiment status and data. This project implements a full experimental protocol that will be explained in [Details](#Details).

## Details

### Setup

Subject's shoulder width (distance from his/her left shoulder to right shoulder) and eye height (distance from his/her eyes to the ground) should be measured and recorded. These records shall later be used in the experiment.

When starting the program, HoloLens should be facing the area where the subject is supposed to be facing. Camera height is set to 1.6m by default, but can be changed to subject's eye height through the interactive buttons. This sets the initial tracking position of the device and allows for a smooth visual experience. There should also be a marked position on the ground where the subject stays at during Phase 1 of the experiment.

### Experimental Protocol

![Procedure](/Pictures/NarrowingWallsProcedure.png?raw=true "Procedure")

Above is the summarizing flowchart of the entire experimental protocol. Subject should set up the shoulder width and camera height before making any further into the experiment. The experiment is divided into two phases, with Phase 1 being the `Perception` phase and Phase 2 being the `Practice` phase.

- For the `Perception` phase, subject stays at a fixed location and adjusts the gap between the walls until he/she believes the gap is just wide enough for him/her to pass without allowing any object wider than his/her shoulders. Subject can either choose `Ascending`, where the gap is narrow and walls are moving away from each other, or `Descending`, where the gap is wide and walls are closing in to each other. Either case, subject can adjust gap width by saying `Wider` or `Narrower`. When he/she is finished, saying `Finish` will end the phase, record current gap width and display it on a floating plate right behind the subject.

- For the `Practice` phase, subject is required to walk through the gap 20 times. When ready, saying `Next` will start trial 1. For each trial, subject should walk through the gap, stop, turn around, and say next to get to the next trial. Floating text initially to the left of the subject will indicate what trial he/she is on. Subject should not move until trial number is incremented. Subject should try his/her best not to run into the walls during each trial. When collided, a beeping sound will indicate which side of the wall the subject ran into, and for the next trial the gap expands; otherwise, the gap shrinks for the next trial. At trial 20, saying `Next` will end the phase, record the minimal gap width for the 20 trials and display it on the same floating plate for Phase 1.

### Text UI

The floating text to the left of the subject indicates:

- Current phase of this experiment, namely `Perception` or `Practice`.

- Context for that phase. If under `Perception`, the context would be either `Ascending` by default or `Decending` if set otherwise. If under `Practice`, the context would be either "Enter Shoulder Width..." before the trials, "Trial #\[1-20\]" during the trials, or "Finished/Reset. Enter Shoulder Width..." when 20 trials are finished.

The floating plate to the back of the subject indicates gap width as a result for each phase. Notice that the plate is not visible during the `Practice` unless subject completes 20 trials.

### Interactive Buttons

There are two sets of interactive buttons in the scene that can be accessed at any time during Phase 1 and at the beginning or end of Phase 2. The two sets of buttons look similar and can only be differentiated by their titles.

- The `Shoulder Width` button set adjusts subject's shoulder width (default 0.4m) in increments or decrements of 0.01m to be interpreted by the program.
- The `Camera Height` button set adjusts subject's eye height in the program (default 1.6m) also in increments or decrements of 0.01m in real time.

It is suggested that these parameters to be set before Phase 1 and never to be changed throughout the whole session.

### Voice Commands

Subject can navigate through the experiment by saying certain keywords. Supported keywords in this program are:

- **Reset Shoulder Width**: reset subject shoulder width to default (0.4m)

- **Reset Camera Height**: reset subject eye height to default (1.6m)

- **Perception**: set experiment to `Perception` phase, set mode to `Ascending`

- **Practice**: set experiment to `Practice` phase, set trial number to uninitialized

- **Ascending**: under `Perception` phase, gap between walls start at 0.3m wide and is meant to be adjusted wider

- **Descending**: under `Perception` phase, gap between walls start at 0.7m wide and is meant to be adjusted narrower

- **Wider**: under `Perception` phase, increase gap width by 0.015m

- **Narrower**: under `Perception` phase, decrease gap width by 0.020m

- **Finish**: under `Perception` phase, end phase and display final gap width on the floating plate at the back

- **Next**: under `Practice` phase, start the 20 trials, move on to the next trial, or finish the trials and display minimal gap width on the floating plate at the back

- **Reset**: reset phase. Under `Perception`, this will set the program to `Ascending` with default initial gap width;
under `Practice`, this will reset the trial number to uninitialized.

## Notes

- When building a `.sln` solution, please make sure the "Development Build" option is unchecked and build to a separate folder. Unity will create a file called `UnityOverwrite.txt` under that folder. If different, delete its contents that do not start with `#` and write `overwrite-all` instead. This makes sure Unity rebuilds all solution files and reduces chances of error.

- When deploying, make sure the settings are `Release`, `x86` and `Device` if HoloLens is connected to the computer, or `Remote Machine` if connected via wireless network.

- When program is running, try not to turn your head around too fast. It will likely mess up HoloLens's spatial tracking and cause some objects to be out of place.

## Credits

This project is made under supervision of Professor Robert E. Bodenheimer at Vanderbilt University EECS Department, in collaboration with Sarah Creem-Regeher, Jeanine Stefanucci and Holly Gagnon at the University of Utah Department of Psychology. Special thanks to Lauren E. Buck at Vanderbilt EECS Department for development advices.

## References

- Franchak, J. M., & Somoano, F. A. (2018). Rate of recalibration to changing affordances for squeezing through doorways reveals the role of feedback. Experimental brain research, 236(6), 1699-1711.

- Rosales, C. S., Pointon, G., Adams, H., Stefanucci, J., Creem-Regehr, S., Thompson, W. B., & Bodenheimer, B. (2019, March). Distance Judgments to On-and Off-Ground Objects in Augmented Reality. In 2019 IEEE Conference on Virtual Reality and 3D User Interfaces (VR) (pp. 237-243). IEEE.
