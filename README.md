# Black Ops 4 Streamer Statistics

This program is intended for Streamers and content creators to be able to write their Black Ops 4 Stats to .txt files so they can then be displayed via OBS or other broadcasting software.

## Stats
Not all stats are currently being updated to the Call of Duty public API so not all stats are available within this program. I have done my best to provide Stats that seem to be stable (Treyarch no removing them from API) and also desired from a players perspective. 
Blackout Stats are still trickling in so there is not much there to use. As I have not played a whole of Zombies I'm not sure which stats are the best to include but have done my best. At this time I cannot find accurate weapon data other than the specialist weapons (not all of which are working).

If you see a stat being displayed on another Tracker or Stats website let me know as I should be able to add it in if the data is being pulled from the API.

### Refresh Times
In Terms of when data is updated, I have added a refresh system that polls the API every 60 seconds. When you turn the auto refresh system on it will gather initial or current data before starting the countdown timer.

From an API perspective I have done some testing, Blackout stats seem to updated 1-3 minutes after a match finishes. I would say MP matches are about the same. Zombies stats updated mid match but I have not figured out if there are specific triggers for this. Appears to be once every couple of rounds?


## Disclaimer
This product is not owned by or endorsed by Treyarch, Activision or Blizzard. It is also my first C# WPF project so excuse the mess ;)