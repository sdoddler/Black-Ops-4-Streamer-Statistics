using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BO4_Statistics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// Can you query a JToken ina dictionary via .Path?
    /// If so then 1 Dictionary with Display name.
    ///     Otherwise 2 dictionaries, 1 with aDisplay for Comob and a path to data. And one with the tokens.. Second a list?
    /// Should be able to do first option but brain power fading.
    /// 

    /// When Starting Timer, check if SessionStart file exists for each Stat. If not Download Start Session Stats.
    /// Start Timer
    ///    Redownload ALL current stats, incase the session was started earlier. (Dont want to reuse OLD currentstats.json files.
    ///    

        /// Need list for each Player of Stats to get.

        /// On Load
        ///     > Check what was saved previously.
        ///     > Load Through Current\Session Start Stats for those.
        ///     > Download any that are missing
        ///     > Populate Treeview
        ///     > Populate Current Stats and playername with last added?
    
        /// Boolean - Session Started (Whether or not there is a current running session).
    public partial class MainWindow : Window
    {
        string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BO4_Statistics\\";

        //List<PlayerData> playerData = new List<PlayerData>();
        Dictionary<string, PlayerData> playerData = new Dictionary<string, PlayerData>();
        List<StatToSave> statsToSave = new List<StatToSave>();
        JObject currentStats, currentZombieStats, currentBOStats;
        IWebProxy proxy;
        HttpClientHandler clientHandler = new HttpClientHandler();
        //bool sessionStarted = false;
        bool autoRefresh = false;
        DateTime refreshCountdown;

        Settings settings = new Settings();


        public Dictionary<string, string> availableMPStats = new Dictionary<string, string>()
        {
            { "Total Kills", "data.mp.lifetime.all.kills"},
            { "Total EKIA", "data.mp.lifetime.all.ekia"},
            { "Total Shots", "data.mp.lifetime.all.totalShots"},
            { "Total Hits", "data.mp.lifetime.all.hits"},
            { "Total Misses", "data.mp.lifetime.all.misses"},
            { "Total Rank XP", "data.mp.lifetime.all.rankxp"},
            { "Featured in Best Play", "data.mp.lifetime.all.featuredInBestPlay"},
            { "Hatchet Kills", "data.mp.lifetime.all.statsHatchetKill"},
            { "First Kills", "data.mp.lifetime.all.statsFirstKill"},
            { "Kill With Pick Up (?)", "data.mp.lifetime.all.killWithPickup"},
            { "Melee (fists) Kills",  "data.mp.lifetime..all.statsKillEnemyWithFists"},
            { "Melee (gunbutt) Kills", "data.mp.lifetime.all.statsKillEnemyWithGunbutt"},
            { "One Shot, One Kills", "data.mp.lifetime.all.statsKillEnemyOneBullet"},
            { "AJAX - Shield Kills", "data.mp.lifetime.all.statsBattleShieldKill"},
            { "BATTERY - Cluster Kills", "data.mp.lifetime.all.statsClusterSemtexKill"},
            { "BATTERY - Cluster Sticks", "data.mp.lifetime.all.statsClusterSemtexStick"},
            { "BATTERY - WarMachine Kills", "data.mp.lifetime.all.statsWarMachineKill" },
            { "CRASH - Allies Tak5 Boosted", "data.mp.lifetime.all.statsTak5Boosted"},
            { "FIREBREAK - Purifier Kills", "data.mp.lifetime.all.statsPurifierKill"},
            { "FIREBREAK - Radiation Kills", "data.mp.lifetime.all.statsRadiationFieldKill"},
            { "NOMAD - MeshMine Kills", "data.mp.lifetime.all.statsTripwireIedKill"},
            { "PROPHET - Enemies Shocked", "data.mp.lifetime.all.statsSeekerShockMineParalyzedEnemy"},
            { "PROPHET - Tempest Kills", "data.mp.lifetime.all.statsTempestKill"},
            { "RUIN - Grav Slam Kills", "data.mp.lifetime.all.statsGravitySlamKill"},
            { "SERAPH - Annihilator ills", "data.mp.lifetime.all.statsAnnihilatorKill"},
            { "TORQUE - Wire Kills", "data.mp.lifetime.all.statsConcertinaWireKill"},
            { "RECON - Sensor Kills", "data.mp.lifetime.all.statsSensorDartKill"},
            { "RECON - Sensor Assists", "data.mp.lifetime.all.statsSensorDartAssist"},
            { "RECON - Vision Pulse Kills", "data.mp.lifetime.all.statsVisionPulseKill"},
            { "RECON - Vision Pulse Assists", "data.mp.lifetime.all.statsVisionPulseAssist"},
        };

        public Dictionary<string, string> availableZombieStats = new Dictionary<string, string>()
        {
            { "Total Rounds Survived", "data.mp.lifetime.all.totalRoundsSurvived"},
            { "Total Kills", "data.mp.lifetime.all.kills"},
            { "Melee Kills", "data.mp.lifetime.all.meleeKills"},
            { "Total Hits", "data.mp.lifetime.all.hits"},
            { "Total Misses", "data.mp.lifetime.all.misses"},
            { "Total Score", "data.mp.lifetime.all.score"},
            { "Total Rank XP", "data.mp.lifetime.all.rankxp"},
            { "Rounds With No Damage Taken(?)", "data.mp.lifetime.all.roundsNoDamage"},
            { "Rounds With No Downs", "data.mp.lifetime.all.roundsNoDowns"},
            { "Special Weapon Levels(?)", "data.mp.lifetime.all.specialWeaponLevels"},
            { "Tiger Kills", "data.mp.lifetime.all.tigersKilled"},
            { "Gladiator Kills", "data.mp.lifetime.all.gladiatorsKilled"},
            { "Total Headshots", "data.mp.lifetime.all.headshots"},
            { "Grenade Kills", "data.mp.lifetime.all.grenadeKills"},
        };

        public Dictionary<string, string> availableBOStats = new Dictionary<string, string>()
        {
            { "Total Wins", "data.mp.lifetime.all.wins"},
            { "Total Kills", "data.mp.lifetime.all.kills"},
            { "Total EKIA", "data.mp.lifetime.all.ekia"},
            { "Total Assists", "data.mp.lifetime.all.assists" },
            { "Vehicles Destroyed", "data.mp.lifetime.all.vehiclesDestroyed"},
            { "Player in Top 5", "data.mp.lifetime.all.top5PlacementPlayer"},
            { "Team in Top 5", "data.mp.lifetime.all.top5PlacementTeam"},
        };

        //  DispatcherTimer setup
        

        public MainWindow()
        {

            InitializeComponent();
            DataContext = this;
            Log("Starting Up..");
            //On Startup Create App Data Dir if not already there
            System.Diagnostics.Debug.WriteLine(appDataDir);
            if (!Directory.Exists(appDataDir))
            {
                Directory.CreateDirectory(appDataDir);

            }
            if (!Directory.Exists(appDataDir + "battle")) Directory.CreateDirectory(appDataDir + "battle");
            if (!Directory.Exists(appDataDir + "psn")) Directory.CreateDirectory(appDataDir + "psn");
            if (!Directory.Exists(appDataDir + "xbox")) Directory.CreateDirectory(appDataDir + "xbox");

            proxy = WebRequest.DefaultWebProxy;
            //System.Diagnostics.Debug.WriteLine(WebRequest.DefaultWebProxy.GetProxy(new Uri("http://www.google.com")));

            //System.Diagnostics.Debug.WriteLine(proxy.GetProxy(new Uri("http://www.google.com")));
            clientHandler.Proxy = proxy;
            clientHandler.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stat.ItemsSource = availableMPStats;
            Stat.DisplayMemberPath = "Key";
            Stat.SelectedIndex = 0;
            //Stat.IsEnabled = true;

            var bindingList = new BindingList<StatToSave>(statsToSave);
            var source = new BindingSource(bindingList, null);
            ListOfStats.ItemsSource = source;


            var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            Log("Ready to Stat it up bro..");
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!autoRefresh) return;

            TimeSpan timeTillRefresh = refreshCountdown - DateTime.Now;

            RefreshStatus.Text = $"ON({timeTillRefresh.Seconds.ToString()})"; // May as well still update just incase errors.
            if (timeTillRefresh.TotalSeconds <= 0)
            {// This works to check and Auto Update
                AutoRefreshStats();
            }
            
        }

        private async void GetStats_Click(object sender, RoutedEventArgs e)
        {
            GetStats.IsEnabled = false;
            string playerName = PlayerName_.Text;
            // playerName = playerName.Replace("#", "%23");
            playerName = Uri.EscapeDataString(playerName);
            string platform;
            switch (Platform.Text)
            {
                default:
                case "Battle.net":
                    platform = "battle";
                    break;
                case "PSN":
                    platform = "psn";
                    break;
                case "Xbox":
                    platform = "xbox";
                    break;
            }

            string url = $"https://my.callofduty.com/api/papi-client/crm/cod/v2/title/bo4/platform/{platform}/gamer/{playerName}/profile/";
            System.Diagnostics.Debug.WriteLine(url);
            string json = await GetStatsFromURL(url, playerName, platform);
            if (json == "Error Connecting")
            {

                GetStats.IsEnabled = true;
                return;
            }
            if (string.IsNullOrEmpty(json))
            {

                Log("Error getting Player Data, " + playerName + " likely does not exist on this platform");
                GetStats.IsEnabled = true;
                return;
            }

            string filename = platform + "\\" + playerName + "_currentStats.json";

            JObject jsonResult = JObject.Parse(json);
            string status = jsonResult["status"].ToString();
            System.Diagnostics.Debug.WriteLine("Status:" + status);
            if (status != "success")
            {
               Log("Error getting Player Data, " + playerName + " likely does not exist on this platform");
                GetStats.IsEnabled = true;
                return;
            }

            //Stat.IsEnabled = true;
            //  var results = jsonResult["data"]["mp"].Children<JToken>().ToList();
            
            if (!(PlayerName_.Items.Contains(PlayerName_.Text)))
            {
                PlayerName_.Items.Add(new ComboBoxItem().Content = PlayerName_.Text);
            }

            if (!playerData.ContainsKey(PlayerName_.Text))
            {
                playerData.Add(PlayerName_.Text, new PlayerData());
            }

            currentStats = jsonResult as JObject;
            playerData[PlayerName_.Text].CurrentStats.MPStats = currentStats;
            File.WriteAllText(System.IO.Path.Combine(appDataDir, filename), json);

            json = await GetStatsFromURL(url, playerName, platform, "zombies");
            currentZombieStats = JObject.Parse(json);

            if (currentZombieStats["status"].ToString() != "success")
            {
                Log("Error Loading Zombies Stats for " + playerName);
                currentZombieStats = null;
            }
            playerData[PlayerName_.Text].CurrentStats.ZMStats = currentZombieStats;

            json = await GetStatsFromURL(url, playerName, platform,"blackout");
            currentBOStats = JObject.Parse(json);

            if (currentBOStats["status"].ToString() != "success")
            {
                Log("Error Loading Blackout Stats for " + playerName);
                currentBOStats = null;
            }
            playerData[PlayerName_.Text].CurrentStats.BOStats = currentBOStats;
            Stat_SelectionChanged(null, null);
            //IDictionary<string, JToken> dict = currentStats;

            //currentStatsPath = GetKeysFromIDict(dict);

            //Stat.ItemsSource = currentStatsPath;
            //Stat.DisplayMemberPath = "Key";

            GetStats.IsEnabled = true;
        }

        private async void UpdateListStats()
        {
            // Download all stat pages first, Incase of any network or API errors, allowing for multiple attempts.

            Toggle_Auto_Refresh.IsEnabled = false;
            Dictionary<string, TypesRefreshed> playersRefreshed = new Dictionary<string, TypesRefreshed>();

            foreach (StatToSave stat in statsToSave)
            {
                if (playersRefreshed.ContainsKey(stat.PlayerName))// check if stat Type already refreshed.
                {
                    bool skipLoop = false;
                    switch (stat.StatType)
                    {
                        case "MP":
                            if (playersRefreshed[stat.PlayerName].MP)
                                skipLoop = true;
                            break;
                        case "Blackout":
                            if (playersRefreshed[stat.PlayerName].BO)
                                skipLoop = true;
                            break;
                        case "Zombies":
                            if (playersRefreshed[stat.PlayerName].ZM)
                                skipLoop = true;
                            break;
                    }
                    if (skipLoop) continue;
                }
                string playerName = Uri.EscapeDataString(stat.PlayerName);// Turn Display names into URI compatible.
                string platform;
                switch (stat.Platform) // Turn Platform into nice string.
                {
                    default:
                    case "Battle.net":
                        platform = "battle";
                        break;
                    case "PSN":
                        platform = "psn";
                        break;
                    case "Xbox":
                        platform = "xbox";
                        break;
                }

                string url = $"https://my.callofduty.com/api/papi-client/crm/cod/v2/title/bo4/platform/{platform}/gamer/{playerName}/profile/";
                string type = "";
                if (stat.StatType != "MP") type = stat.StatType.ToLower(); // No type required for MP Stats at the moment.


                string json = await GetStatsFromURL(url, playerName, platform, type); // Get Stats from API
                if (json == "Error Connecting") continue; // If HTTP Error, then Continue Loop (will try to get stats again.

                JObject jsonResult = JObject.Parse(json);
                string status = jsonResult["status"].ToString();
                if (status != "success") // If API Error then Log it and Continue loop.
                {
                    Log($"Error Loading {type} Stats for {playerName}");
                    continue;
                }

                if (!playerData.ContainsKey(stat.PlayerName))
                {//if there is no player data for current player, add it to Dictionary.
                    playerData.Add(stat.PlayerName, new PlayerData());
                    playerData[stat.PlayerName].Platform = platform;
                    playerData[stat.PlayerName].CurrentStats.RefreshTime = DateTime.Now; // Set Current Refresh Time.
                }
                

                string filename = $"{platform}\\{playerName}_current_{stat.StatType}.json"; // Set Saving File Name.

                File.WriteAllText(System.IO.Path.Combine(appDataDir, filename), json); // Save File for later loads.

                if (!playersRefreshed.ContainsKey(stat.PlayerName)) // If player is not in Players Refreshed, Add it.
                    playersRefreshed.Add(stat.PlayerName, new TypesRefreshed());

                switch (stat.StatType)
                {// Depending on Stat Type, update Refreshed Bool and Player Data. 
                    // If for any reason it's a session stat and the session stat data = null then load current data to session data.
                        // Should only occur when new stats are added after the session is started or there was no data when the session was restarted.
                    case "MP":
                        playersRefreshed[stat.PlayerName].MP = true;
                        playerData[stat.PlayerName].CurrentStats.MPStats = jsonResult;
                            if (playerData[stat.PlayerName].SessionStartStats.MPStats == null 
                            || playerData[stat.PlayerName].SessionStartStats.MPStats.ToString() == "null")
                            {
                                playerData[stat.PlayerName].SessionStartStats.MPStats =
                        playerData[stat.PlayerName].CurrentStats.MPStats = jsonResult;
                            
                        }
                        break;
                    case "Blackout":

                        playersRefreshed[stat.PlayerName].BO = true;

                        playerData[stat.PlayerName].CurrentStats.BOStats = jsonResult;
                            if (playerData[stat.PlayerName].SessionStartStats.BOStats == null 
                            || playerData[stat.PlayerName].SessionStartStats.BOStats.ToString() == "null")
                            {
                                playerData[stat.PlayerName].SessionStartStats.BOStats =
                        playerData[stat.PlayerName].CurrentStats.BOStats = jsonResult;
                            }
                        
                        break;
                    case "Zombies":

                        playersRefreshed[stat.PlayerName].ZM = true;

                        playerData[stat.PlayerName].CurrentStats.ZMStats = jsonResult;

                       
                            if (playerData[stat.PlayerName].SessionStartStats.ZMStats == null
                            || playerData[stat.PlayerName].SessionStartStats.ZMStats.ToString() == "null")
                            {
                                playerData[stat.PlayerName].SessionStartStats.ZMStats =
                        playerData[stat.PlayerName].CurrentStats.ZMStats = jsonResult;
                            }
                        
                        break;
                }
            }

            foreach (StatToSave stat in statsToSave)
            {
                if (playersRefreshed.ContainsKey(stat.PlayerName))
                {// If Stat Type not refreshed Skip Loop.
                    bool skipLoop = false;
                    switch (stat.StatType)
                    {
                        case "MP":
                            if (!playersRefreshed[stat.PlayerName].MP)
                                skipLoop = true;
                            break;
                        case "Blackout":
                            if (!playersRefreshed[stat.PlayerName].BO)
                                skipLoop = true;
                            break;
                        case "Zombies":
                            if (!playersRefreshed[stat.PlayerName].ZM)
                                skipLoop = true;
                            break;
                    }
                    if (skipLoop) continue;
                }//else grab data from PlayerData and save to FilePath.
                Console.WriteLine($"Writing {stat.DisplayName} with TextPrefix: {stat.TextPrefix} to {stat.FilePath} for Player: {stat.PlayerName}");
                if (playerData[stat.PlayerName].CurrentStats !=null)
                {
                    string statResult = "";
                    string currResult = "0";
                    string sessResult = "0";
                    switch (stat.StatType)
                    {
                        case "MP":
                            if (playerData[stat.PlayerName].CurrentStats.MPStats != null)
                                if (stat.SessionStat)
                                {
                                    if (playerData[stat.PlayerName].SessionStartStats.MPStats != null)
                                    {
                                        currResult = playerData[stat.PlayerName].CurrentStats.MPStats.SelectToken(stat.GetPath()).ToString();
                                        sessResult = playerData[stat.PlayerName].SessionStartStats.MPStats.SelectToken(stat.GetPath()).ToString();

                                        statResult = (float.Parse(currResult) - float.Parse(sessResult)).ToString("0.##");

                                        Console.WriteLine("MP-CurrData:" + currResult +"\nMP-SessionData:" + sessResult + "\nStatResult:"+statResult);
                                    }
                                }
                                else
                                {
                                    statResult = playerData[stat.PlayerName].CurrentStats.MPStats.SelectToken(stat.GetPath()).ToString();
                                }
                            break;
                        case "Blackout":
                            if (playerData[stat.PlayerName].CurrentStats.BOStats != null)
                                if (stat.SessionStat)
                                {
                                    if (playerData[stat.PlayerName].SessionStartStats.BOStats != null)
                                    {
                                        currResult = playerData[stat.PlayerName].CurrentStats.BOStats.SelectToken(stat.GetPath()).ToString();
                                        sessResult = playerData[stat.PlayerName].SessionStartStats.BOStats.SelectToken(stat.GetPath()).ToString();

                                        statResult = (float.Parse(currResult) - float.Parse(sessResult)).ToString("0.##");

                                        Console.WriteLine("BO-CurrData:" + currResult + "\nBO-SessionData:" + sessResult + "\nStatResult:" + statResult);
                                    }
                                }
                                else
                                {
                                    statResult = playerData[stat.PlayerName].CurrentStats.BOStats.SelectToken(stat.GetPath()).ToString();
                                }
                            break;
                        case "Zombies":
                            if (playerData[stat.PlayerName].CurrentStats.ZMStats != null)
                                if (stat.SessionStat)
                                {
                                    if (playerData[stat.PlayerName].SessionStartStats.ZMStats != null)
                                    {
                                        currResult = playerData[stat.PlayerName].CurrentStats.ZMStats.SelectToken(stat.GetPath()).ToString();
                                        sessResult = playerData[stat.PlayerName].SessionStartStats.ZMStats.SelectToken(stat.GetPath()).ToString();

                                        statResult = (float.Parse(currResult) - float.Parse(sessResult)).ToString("0.##");

                                        Console.WriteLine("ZM-CurrData:" + currResult + "\nZM-SessionData:" + sessResult + "\nStatResult:" + statResult);
                                    }
                                }
                                else
                                {
                                    statResult = playerData[stat.PlayerName].CurrentStats.ZMStats.SelectToken(stat.GetPath()).ToString();
                                }
                            break;
                                
                    }
                    if (statResult != "")
                    {
                        Console.WriteLine("Writing: " + stat.TextPrefix + statResult + " to " + stat.FilePath);
                        File.WriteAllText(stat.FilePath, stat.TextPrefix + statResult);
                    }
                }

            }


            Toggle_Auto_Refresh.IsEnabled = true;

        }

        private void SetPlayerSessionStats()
        {

        }

        private async Task<string> GetStatsFromURL(string url, string playerName, string platform, string type = "")
        {
            string json;

            string log = "Getting stats for " + playerName;
            if (type != "")
            {
                log += " Type:" + type;
                url += "?type=" + type;
            }

            Log(log);
            //LogBox.ScrollToEnd();

            try
            {
                var httpClient = new HttpClient(clientHandler);
                // json = await wc.DownloadStringTaskAsync(webUri);

                json = await httpClient.GetStringAsync(url);

            }
            catch (Exception ex)
            {

               Log("Cannot Connect to stat Servers:" +ex.Message);
                //System.Windows.MessageBox.Show("Cannot connect to Stats Servers");
                return "Error Connecting";
            }
            return json;
            // System.Diagnostics.Debug.WriteLine(json);
            // MessageBox.Text = json;
            //  string filename = platform + "\\" +playerName + "_" + DateTime.UtcNow.ToFileTimeUtc()+ ".json";




        }

        private void StatType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // var selectedItem = StatType
            var selected = ((ComboBoxItem)StatType.SelectedItem).Content as string;
            Console.WriteLine("StatTypeChanged: " + selected);
            switch (selected)
            {
                case "MP":
                    Stat.ItemsSource = availableMPStats;
                    Stat.SelectedIndex = 0;
                    break;
                case "Blackout":
                    Stat.ItemsSource = availableBOStats;
                    Stat.SelectedIndex = 0;

                    break;
                case "Zombies":
                    Stat.ItemsSource = availableZombieStats;
                    Stat.SelectedIndex = 0;

                    break;
            }
            if (currentStats != null) Stat_SelectionChanged(null, null);
        }

        public Dictionary<string, JToken> GetKeysFromIDict(IDictionary<string, JToken> dict, string prefix = "")
        {
            Dictionary<string, JToken> retDict = new Dictionary<string, JToken>();
            foreach (var item in dict)
            {
                if (!item.Value.HasValues) // THIS SEPERATES IT!!! WOOOO
                {
                    System.Diagnostics.Debug.WriteLine("PropertyName: " + item.Value.Path);//+prefix + item.Key);
                    retDict.Add(prefix + item.Key, item.Value);
                }
                else
                {

                    IDictionary<string, JToken> subDict = item.Value as JObject;
                    Dictionary<string, JToken> exDict = GetKeysFromIDict(subDict, prefix + item.Key + "|");
                    foreach (var j in exDict)
                    {

                        retDict.Add(j.Key, j.Value);
                    }
                }
            }
            return retDict;
        }

        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Start_Session_Click(object sender, RoutedEventArgs e)
        {
            // DIALOG BOX HERE.
            // Are you sure you want to restart The session? This will Override all current session data
            // Fix wording.
            if (settings.SessionStarted)
            {
                DialogResult dlg = System.Windows.Forms.MessageBox.Show("Are you sure you wish to restart the session? This will reset all session total stats to 0", "Restart Session", MessageBoxButtons.YesNo);
                if (dlg == System.Windows.Forms.DialogResult.No) return;
            }
            else
            {
                settings.SessionStarted = true;
            }
            // Save Current Stats to Session Start Stats in Player Data.
            for (int i = 0; i < playerData.Count;i++)
            {
                playerData.ElementAt(i).Value.SessionStartStats = playerData.ElementAt(i).Value.CurrentStats;
            }
        }        

        private void AddStat_Click(object sender, RoutedEventArgs e)
        {
            
           Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
            saveFile.FileName = PlayerName_.Text;
            saveFile.DefaultExt = ".txt";
            saveFile.Filter = "Text documents (.txt)|*.txt";

            string filename;
            bool replace = false;
            StatToSave replaceStat = null;

            if (saveFile.ShowDialog() == true)
            {
                filename = saveFile.FileName;
                replaceStat = statsToSave.Where(x => x.FilePath == filename).FirstOrDefault();
                
                if (replaceStat !=null)
                {
                    Log(filename+" already exists in list. Replacing previous Setting.");
                    
                        replace = true;
                    

                }

                
            } else
            {
                Log("Save location cancelled. Stat will not be added.");
                return;
            }
            // Add Stat to List
            StatToSave sts = new StatToSave();

            var selectedItem = (KeyValuePair<string, string>?)(Stat).SelectedItem;
            if (selectedItem == null)
            {
                Log("Error Finding Stat");
                return;
            }
            sts.DisplayName = selectedItem.Value.Key;

            sts.PlayerName = PlayerName_.Text;
            sts.Platform = ((ComboBoxItem)Platform.SelectedItem).Content as string;
            sts.StatType =  ((ComboBoxItem)StatType.SelectedItem).Content as string;
            sts.SetPath(selectedItem.Value.Value.ToString());
            if (((ComboBoxItem)StatMode.SelectedItem).Content as string == "Stat Total")
            {
                sts.SessionStat = false;
            } else
            {
                sts.SessionStat = true;
            }
            sts.FilePath = filename;
            sts.TextPrefix = TextPrefix.Text;


            if (!replace)
            {
                statsToSave.Add(sts);
            }else
            {
                if (replaceStat != null)
                    statsToSave[statsToSave.IndexOf(replaceStat)] = sts;
                else
                    Log("Cannot find stat to replace");
            }



            RefreshListOfStats();
        }

        private void RefreshListOfStats()
        {

            ListOfStats.ItemsSource = null;
            ListOfStats.ItemsSource = statsToSave;
        }

        private void ListOfStats_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
           if (e.Column.Header.ToString() == "Path")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }

        private void PlayerName__TextChanged(object sender, TextChangedEventArgs e)
        {
            string playerName = PlayerName_.Text;
            if (playerData.ContainsKey(playerName))
            {
                if (playerData[playerName].CurrentStats != null)
                {
                    currentStats = playerData[playerName].CurrentStats.MPStats;
                    currentBOStats = playerData[playerName].CurrentStats.BOStats;
                    currentZombieStats = playerData[playerName].CurrentStats.ZMStats;

                    if (currentStats != null) Stat_SelectionChanged(null, null);
                }
            } else
            {
                StatDisplay.Text = "Stat Preview";
            }
        }

        private void Remove_Selected_Click(object sender, RoutedEventArgs e)
        {
            if (statsToSave.Count == 0)
            {
                //Log("Debug:No Items selected to remove");
                return;
            }
           switch (ListOfStats.SelectedItems.Count)
            {
                case 0:
                    Log("Debug:No Items selected to remove");
                        break;
                case 1:
                    StatToSave sts = (StatToSave)ListOfStats.SelectedItems[0];
                    DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you wish to replace "+sts.DisplayName+ " for "+ sts.PlayerName +"\nSaving to " +sts.FilePath, "Remove Stat", MessageBoxButtons.YesNo);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (sts.SessionStat)
                        {

                        }
                        statsToSave.Remove(sts);
                    }
                    RefreshListOfStats();
                    break;
                default:
                    DialogResult dgResult = System.Windows.Forms.MessageBox.Show("More than one Stat selected, are you sure you wish to remove all of these?", "Remove Stats", MessageBoxButtons.YesNo);
                    if (dgResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        foreach (StatToSave stat in ListOfStats.SelectedItems)
                             statsToSave.Remove(stat);

                        RefreshListOfStats();
                    }
                    break;
            }
        }

        private void Toggle_Auto_Refresh_Click(object sender, RoutedEventArgs e)
        {
            autoRefresh = !autoRefresh;

            if (autoRefresh)
            {
                AutoRefreshStats();
                RefreshStatus.Background = Brushes.ForestGreen;
            } else
            {
                RefreshStatus.Text = "OFF";

                RefreshStatus.Background = Brushes.Red;

            }
        }

        private void AutoRefreshStats()
        {
            Log("Refreshing Current Stat List..");
            UpdateListStats();
            refreshCountdown = DateTime.Now.AddSeconds(60);
            
        }

        private void PlayerName__SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PlayerName__TextChanged(null, null);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // On Exit

            //Save 
            _Json.WriteToJsonFile(appDataDir + "stats.json",statsToSave);
            List<string> items = new List<string>();
            foreach(var item in PlayerName_.Items)
            {
                items.Add(item.ToString());
            }
            _Json.WriteToJsonFile(appDataDir + "players.json", items);
            _Json.WriteToJsonFile(appDataDir + "playerData.json", playerData);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // On Startup

            //Load
            if (File.Exists(appDataDir + "players.json"))
            {
                List<string> items = _Json.ReadFromJsonFile<List<string>>(appDataDir + "players.json");
                foreach (string item in items)
                {
                    PlayerName_.Items.Add(item);
                }
            }
            if (File.Exists(appDataDir + "stats.json"))
            {
                statsToSave = _Json.ReadFromJsonFile<List<StatToSave>>(appDataDir + "stats.json");
            }
            if(File.Exists(appDataDir + "playerData.json"))
            {
                playerData = _Json.ReadFromJsonFile<Dictionary<string, PlayerData>>(appDataDir + "playerData.json");            
            }
            RefreshListOfStats();
            Stat_SelectionChanged(null, null);
        }        

        private void Stat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            var selectedItem = (KeyValuePair<string, string>?)(Stat).SelectedItem;
            if (selectedItem == null) return;
            string key = selectedItem.Value.Key;
            string currentStat = null;
            System.Diagnostics.Debug.WriteLine(key);
            switch (((ComboBoxItem)StatType.SelectedItem).Content as string)
            {
                case "MP":
                    if (currentStats == null)
                    {
                        StatDisplay.Text = "Stat Preview";
                        return;
                    }
                    currentStat = currentStats.SelectToken(selectedItem.Value.Value).ToString();

                    break;
                case "Blackout":
                    if (currentBOStats == null)
                    {
                        StatDisplay.Text = "Stat Preview";
                        return;
                    }
                    currentStat = currentBOStats.SelectToken(selectedItem.Value.Value).ToString();
                    break;
                case "Zombies":
                    if (currentZombieStats == null)
                    {
                        StatDisplay.Text = "Stat Preview";
                        return;
                    }
                    currentStat = currentZombieStats.SelectToken(selectedItem.Value.Value).ToString();
                    break;
            }

            if (currentStat != null)
            {
                StatDisplay.Text = currentStat;//currentStats.SelectToken(currentStatsPath[key].Path).ToString();
            }
            else
            {
                StatDisplay.Text = "Stat not found";
            }
        }

        public void Log(string message)
        {
            LogBox.AppendText("\n["+ DateTime.Now.ToString("HH:mm")+"]"+ message);
            LogBox.ScrollToEnd();
        }
    }

   
}

public class PlayerData
{
    public string Platform { get; set; }
    public string PlayerName { get; set; }
    public PlayerStats CurrentStats { get; set; } = new PlayerStats();
    public PlayerStats SessionStartStats { get; set; } = new PlayerStats();
   
}

public class PlayerStats
{
    public JObject MPStats { get; set; } = null;
    public JObject BOStats { get; set; } = null;
    public JObject ZMStats { get; set; } = null;
    public DateTime RefreshTime { get; set; }
}

[Serializable]
public class StatToSave {

    public string DisplayName { get; set; }
    public string PlayerName { get; set; }
    public string Platform { get; set; }
    public string Path { get; set; }
    public bool SessionStat { get; set; } // If not SessionStat then use Current Stat (Total) Else do Current - Session Start
    public string StatType { get; set; }
    public string TextPrefix { get; set; }
    public string FilePath { get; set; }

    public void SetPath(string path)
    {
        Path = path;
    }
        public string GetPath()
        {
            return Path;
        }
}

public enum StatType_Enum // Not used.
{
    MP,
    Blackout,
    Zombies,
    //Weapon?
}

public class TypesRefreshed
{
    public bool MP { get; set; } = false;
    public bool BO { get; set; } = false;
    public bool ZM { get; set; } = false;
}

public class Settings
{
   public bool SessionStarted { get; set; } = false;
}

    public class _Json
    {     
    /// <summary>
    /// Writes the given object instance to a Json file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
    /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
    {
        TextWriter writer = null;
        try
        {
            var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
            writer = new StreamWriter(filePath, append);
            writer.Write(contentsToWriteToFile);
        }
        finally
        {
            if (writer != null)
                writer.Close();
        }
    }

    /// <summary>
    /// Reads an object instance from an Json file.
    /// <para>Object type must have a parameterless constructor.</para>
    /// </summary>
    /// <typeparam name="T">The type of object to read from the file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the Json file.</returns>
    public static T ReadFromJsonFile<T>(string filePath) where T : new()
    {
        TextReader reader = null;
        try
        {
            reader = new StreamReader(filePath);
            var fileContents = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(fileContents);
        }
        finally
        {
            if (reader != null)
                reader.Close();
        }
    }
}

//var selectedItem = (KeyValuePair<string, string>?)(sender as ComboBox).SelectedItem;
//string key = selectedItem.Value.Key;
//System.Diagnostics.Debug.WriteLine(key);
//          JToken currentStats.SelectToken(selectedItem.Value.Value);
//            if (currentStatsPath.ContainsKey(key))
//            {
//                StatDisplay.Text = selectedItem.Value.Value.ToString() ;//currentStats.SelectToken(currentStatsPath[key].Path).ToString();
//                    }
//            else
//            {
//                StatDisplay.Text = "Stat not found";
//            }

//HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create("https://my.callofduty.com/api/papi-client/crm/cod/v2/title/bo4/platform/battle/gamer/doddler%236282/profile/");
//IWebProxy proxy2 = myWebRequest.Proxy;
//myWebRequest.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
//if (proxy2 != null)
//{
//    Console.WriteLine("Proxy: {0}", proxy2.GetProxy(myWebRequest.RequestUri));
//}
//else
//{
//    Console.WriteLine("Proxy is null; no proxy will be used");
//}

//HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
//Console.WriteLine(myWebResponse.StatusCode + myWebResponse.StatusDescription);


//foreach (var item in dict)
//{

//    if (!item.Value.HasValues) // THIS SEPERATES IT!!! WOOOO
//    {
//        System.Diagnostics.Debug.WriteLine("PropertyName: " + item.Key);
//    }
//    else
//    {
//        IDictionary<string, JToken> subDict = item.Value as JObject;
//        foreach (var j in subDict)
//        {

//            System.Diagnostics.Debug.WriteLine("PropertyName: " + item.Key +"|"+j.Key);
//        }
//    }
//        //Stat.Items.Add(item.);

//}