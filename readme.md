# General

This read me is for the scripts used to save scores. The scores for the active run are stored in a scriptable object (ScoreStructScriptable). Once a run is over that score is evaluated against the score on disk to determine if any of the scores are a new record. If a new record is found the player is asked for their initials and the score is saved to disk.

## Scripts

### Required
 - **SceneManager** - the brains of the operation, this is what is actually saving and loading the  - scores to disk
   - NOTE: Unity plays weirdly with JSON files at times. You may need to google your specific  - unity version to figure out how to get it to play nice. Look at JSON helper for this also.
 - **ScoreStructScriptable** - This is where you designate the type of data you are saving. You  - will probably want to modify this first and then fix everything that breaks.
 - **FinalScoreDisplay** - This displays the highest recorded scores of each type
 - **ScoreDisplay** - This displays the current score that is in the ScoreStructScriptable. This  - can be used to track the current score in game (refresh on update) or be used to track a  - score that was just earned
 - **ScoreInitialSaver** - This determines if the player has achieved a high score and allows them  - to enter their initials if they have.
   - NOTE: Rarely (twice over a period of 6 hours) players are visually unable to submit their  - initals, causing a soft lock. On the backend the initials have been saved and the game can  - be safely restarted to resolve the issue
 - **JsonHelper** - This is needed to make everything play nice. As was said in SceneManager, Unity has added JSon support to some versions but not others.

### Extra

RecordScoreDisplay - this is providing information if one of the scores that was just recorded is a high score.
SceneLoader - I'm not sure why this is in this folder?

