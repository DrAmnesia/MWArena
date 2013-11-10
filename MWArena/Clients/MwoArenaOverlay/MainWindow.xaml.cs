using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MatchLogger;
using Microsoft.Win32;
using MWA.Integration;
using MWA.Models;


namespace GW2Stuff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<String, EventItem> eventDictionary;
        private List<EventItem> eventList;
        private ListCollectionView eventView;
        private bool gw2Focus = false;
        //private String language = "en";
        private OptionsWindow optionsWindow;
        private TaskQueue taskQueue;
        private DispatcherTimer timerFocus;
        private DispatcherTimer timerRefresh;
        private DispatcherTimer timerTick;
        private int worldId = 0;
        private List<WorldInformation> worlds;
        public static WebApiIntegrationConnector mwApiConn;
        public static ExeIntegrationConnector lwConn;
        public static WebApiIntegrationConnector buildApiConn;
        public static String UserName = String.Empty;
        public MainWindow()
        {

            SetBrowserFeatureControl();




            InitializeComponent();
            mwApiConn = new WebApiIntegrationConnector("MWApi");

            buildApiConn = new WebApiIntegrationConnector();
            lwConn = new ExeIntegrationConnector("Logwarrior.exe");
            //lwConn = new LWConn("MatchCompletedPublishingTestForm.exe");

            //MatchLogger.MatchCompletedPublisher.OnMatchCompleted+= HandleMatch;
            if (tbPilotName.Text != "Your Pilot Name")
            {
                //MatchLogger.MatchLogger.SetPlayerName(tbPilotName.Text);
                UserName = tbPilotName.Text;
                tbSystemMessages.Text = String.Format("MWApi:{0}", MatchLogger.MatchLogger.GetApiUrl());
                mwApiConn = new WebApiIntegrationConnector("MWApi", new Uri(MatchLogger.MatchLogger.GetApiUrl()));
            }
            else
                ForcePilotNameEntry();

            OverlaySettings.loadSettings();
            btnLWConn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            btnLWConn.BorderThickness = new Thickness(0, 0, 0, 0);
            btnLWConn.Background = System.Windows.Media.Brushes.Transparent;
            btnApiConn.BorderBrush = System.Windows.Media.Brushes.Transparent;
            btnApiConn.BorderThickness = new Thickness(0, 0, 0, 0);
            btnApiConn.Background = System.Windows.Media.Brushes.Transparent;

            if (Properties.Settings.Default.dailyCompletionList == null)
            {
                Properties.Settings.Default.dailyCompletionList = new System.Collections.Specialized.StringCollection();
            }

            btnBuildConn_Click("", new RoutedEventArgs());
            eventDictionary = new Dictionary<String, EventItem>();
            eventList = new List<EventItem>();
            eventView = new ListCollectionView(eventList);
            taskQueue = new TaskQueue();

            eventView.CustomSort = (IComparer)(new EventItemComparer());

            taskQueue.taskQueueItemComplete += taskQueue_taskQueueItemComplete;

            timerFocus = new DispatcherTimer();
            timerFocus.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timerRefresh = new DispatcherTimer();
            timerRefresh.Interval = new TimeSpan(0, 0, 0, 30, 0);
            timerTick = new DispatcherTimer();
            timerTick.Interval = new TimeSpan(0, 0, 0, 1, 0);

            timerFocus.Tick += timerFocus_Elapsed;
            timerRefresh.Tick += timerRefresh_Elapsed;
            timerTick.Tick += timerTick_Elapsed;

            timerFocus.Start();
            timerRefresh.Start();
            timerTick.Start();

            menuItem_filters_Association.IsChecked = Properties.Settings.Default.filterAssociation;
            menuItem_filters_ShowBuilds.IsChecked = Properties.Settings.Default.filterShowBuilds;


            if (Properties.Settings.Default.positionWidth > 0)
            {
                Width = Properties.Settings.Default.positionWidth;
                Height = Properties.Settings.Default.positionHeight;
                Left = Properties.Settings.Default.positionLeft;
                Top = Properties.Settings.Default.positionTop;
            }

            getStartupInformation();

        }


        /*private void HandleMatch(object sender, MatchCompletedEventArgs mce)
        {
          // handle Logged Match events  
            tbSystemMessages.Text = "New Match Published";
        }*/
        private void Window_Closed_1(object sender, EventArgs e) { Properties.Settings.Default.Save(); }

        // Every second, tick the timers
        private void timerTick_Elapsed(object sender, EventArgs e)
        {
            foreach (EventItem eventItem in eventView)
            {
                eventItem.tick();
            }
            limitVisibleEvents();
        }

        // Every 30 seconds, pull new timer data from the server
        private void timerRefresh_Elapsed(object sender, EventArgs e) { updateTimers(); }

        // Check for whether GW2 is (probably) in focus
        private void timerFocus_Elapsed(object sender, EventArgs e)
        {
            bool gameMode = false;

            // Holding down Ctrl will also allow you to move the window
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                gameMode = false;
            }
            else
            {
                IntPtr hWnd = NativeMethods.GetForegroundWindow();
                StringBuilder windowTitle = new StringBuilder(250);
                if (NativeMethods.GetWindowText(hWnd, windowTitle, 250) > 0)
                {
                    if (windowTitle.ToString() == "Guild Wars 2") gameMode = true;
                }
            }

            if (gameMode)
            {
                if (!gw2Focus)
                {
                    gw2Focus = true;
                    resizer.Visibility = Visibility.Hidden;
                    if (OverlaySettings.hideTitleInGame)
                    {
                        titleBar.Visibility = Visibility.Hidden;
                    }
                    if (OverlaySettings.clickThroughInGame)
                    {
                        setClickThrough(true);
                    }
                }
            }
            else
            {
                if (gw2Focus)
                {
                    gw2Focus = false;
                    resizer.Visibility = Visibility.Visible;
                    titleBar.Visibility = Visibility.Visible;
                    setClickThrough(false);
                }
            }
        }

        // Update world ID and refresh display
        private void setWorldId(int worldId)
        {
            if (eventContainer.ItemsSource == null)
            {
                eventContainer.Items.Clear();
                eventContainer.ItemsSource = eventView;
            }

            this.worldId = worldId;
            eventDictionary.Clear();
            eventList.Clear();
            ((EventItemComparer)eventView.CustomSort).updateCurrentTimestamp();
            eventView.Refresh();
            updateTimers();
        }

        /* Window interaction */

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = System.Windows.WindowState.Minimized;
            }
            else
            {
                this.DragMove();
            }
        }

        private void resizer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            NativeMethods.SendMessage(handle, NativeMethods.WM_SYSCOMMAND,
                                      (IntPtr)(NativeMethods.RESIZE_BASE + NativeMethods.ResizeDirection.BottomRight),
                                      IntPtr.Zero);
        }

        private void eventContainer_SizeChanged(object sender, SizeChangedEventArgs e) { limitVisibleEvents(); }

        // Use this horrid method of limiting the number of items in the list
        // to those that fit fully.
        private void limitVisibleEvents()
        {
            double cumulativeHeight = 0;
            double maxHeight = eventContainer.ActualHeight;

            bool filterCompleted = Properties.Settings.Default.filterAssociation;
            bool filterWindow = Properties.Settings.Default.filterShowBuilds;
            bool filterInactive = Properties.Settings.Default.filterEventInactive;
            bool filterBoss = Properties.Settings.Default.filterEventBoss;
            bool filterTemple = Properties.Settings.Default.filterEventTemple;

            foreach (EventItem eventItem in eventView)
            {
                if (
                    (!filterCompleted && eventItem.completed) ||
                    (!filterWindow && (eventItem.displayMode == "window")) ||
                    (!filterInactive && (eventItem.displayMode == "inactive")) ||
                    (!filterBoss && (eventItem.eventInfo.eventType == "boss")) ||
                    (!filterTemple && (eventItem.eventInfo.eventType == "temple"))
                    )
                {
                    eventItem.Visibility = Visibility.Collapsed;
                }
                else
                {
                    eventItem.Visibility = Visibility.Visible;
                    cumulativeHeight += eventItem.ActualHeight;
                    if (cumulativeHeight >= maxHeight)
                    {
                        eventItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        // Toggle the mouse click-through state
        private void setClickThrough(bool state)
        {
            IntPtr handle = new WindowInteropHelper(this).Handle;
            int style = NativeMethods.GetWindowLong(handle, NativeMethods.GWL_EXSTYLE);
            style = state ? (style | NativeMethods.WS_EX_TRANSPARENT) : (style & ~NativeMethods.WS_EX_TRANSPARENT);
            NativeMethods.SetWindowLong(handle, NativeMethods.GWL_EXSTYLE, style);
        }

        /* Menu */

        // Close down
        private void menuItem_close_Click(object sender, RoutedEventArgs e) { this.Close(); }

        // Change world
        private void menuItem_world_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem)
            {
                MenuItem menuItem = (MenuItem)sender;
                if (menuItem.Tag is WorldInformation)
                {
                    setWorldId(((WorldInformation)(menuItem.Tag)).worldId);
                }
            }
        }

        private void menuItem_options_Click(object sender, RoutedEventArgs e)
        {
            if (optionsWindow == null)
            {
                optionsWindow = new OptionsWindow();
                optionsWindow.Owner = this;

                List<WorldInformation> sortedWorlds = new List<WorldInformation>(worlds);
                sortedWorlds.Sort();
                optionsWindow.defaultWorld.ItemsSource = sortedWorlds;
                optionsWindow.refresh();
            }
            optionsWindow.Show();
        }

        private void menuItem_profile_UserName_Click(object sender, RoutedEventArgs e)
        {
            ForcePilotNameEntry();

        }

        private void ForcePilotNameEntry()
        {
            tbPilotName.IsReadOnly = false;
            tbPilotName.Background = Brushes.WhiteSmoke;
            SettingsMenu.IsOpen = false;
            tbPilotName.Text = "";
            tbPilotName.ToolTip = "Press Enter to Save.";
            tbPilotName.Focus();
        }
        private void menuItem_tests_MCE_Click(object sender, RoutedEventArgs e)
        {
            MatchLogger.MatchLogger.LoggedMatch lm = new MatchLogger.MatchLogger.LoggedMatch();
            lm.AssociationName = "TEST";
            lm.PublishingUserName = "DrAmnesia";
            lm.EnemyMatchStats = new List<MatchStat>();
            lm.FriendlyMatchStats = new List<MatchStat>();
            // MatchLogger.MatchCompletedPublisher.Publish(lm);

        }
        private void menuItem_filters_Association_Click(object sender, RoutedEventArgs e) { Properties.Settings.Default.filterAssociation = menuItem_filters_Association.IsChecked; }

        private void menuItem_filters_ShowBuilds_Click(object sender, RoutedEventArgs e) { Properties.Settings.Default.filterShowBuilds = menuItem_filters_ShowBuilds.IsChecked; }
        /*
              private void menuItem_profile_Faction_Click(object sender, RoutedEventArgs e) { Properties.Settings.Default.filterShowBuilds = menuItem_filters_window.IsChecked; }

              private void menuItem_profile_Company_Click(object sender, RoutedEventArgs e) { Properties.Settings.Default.filterEventInactive = menuItem_filters_inactive.IsChecked; }
       */
        private void menuItem_saveLocation_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.positionWidth = Width;
            Properties.Settings.Default.positionHeight = Height;
            Properties.Settings.Default.positionLeft = Left;
            Properties.Settings.Default.positionTop = Top;
        }

        /* Timer updates */

        // Trigger a timer update request
        private void updateTimers()
        {
            if (worldId == 0) return;
            taskQueue.run(worldId, async_updateTimers);
        }

        // Grab timer data
        private Object async_updateTimers(Object worldId)
        {
            HttpWebRequest httpRequest =
                (HttpWebRequest)
                WebRequest.Create("http://" + OverlaySettings.dataServer + "/events-request/" + worldId);
            httpRequest.Method = "GET";
            httpRequest.Timeout = 10000;
            httpRequest.Proxy = null;

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(EventsRequestResponse));
                Object deserialized = serializer.ReadObject(response.GetResponseStream());

                if (deserialized is EventsRequestResponse)
                {
                    String serverDateString = response.GetResponseHeader("Date");
                    if (serverDateString != null)
                    {
                        DateTime serverDate = DateTime.Parse(serverDateString);
                        ((EventsRequestResponse)deserialized).currentTime = GW2StuffAPI.dateToUnix(serverDate);
                    }
                }

                return deserialized;
            }
            else
            {
                return null;
            }
        }

        /* Other essential data */

        private void getStartupInformation() { taskQueue.run(null, async_getStartupInformation); }

        // Grab startup data
        private Object async_getStartupInformation(Object ignored)
        {
            HttpWebRequest httpRequest =
                (HttpWebRequest)WebRequest.Create("http://" + OverlaySettings.dataServer + "/overlay-startup");
            httpRequest.Method = "GET";
            httpRequest.Timeout = 10000;
            httpRequest.Proxy = null;

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(StartupInformation));
                return serializer.ReadObject(response.GetResponseStream());
            }
            else
            {
                return null;
            }
        }

        /* Remote result handling */

        // Handle server responses
        private void taskQueue_taskQueueItemComplete(Object source, TaskQueueItemCompletionArgs args)
        {
            if (args.func == async_updateTimers)
            {
                // Ignore errors when fetching timer data, connections can drop
                if (args.exception != null) return;
                if (args.result == null) return;

                EventsRequestResponse response = (EventsRequestResponse)args.result;

                GW2StuffAPI.setCurrentTime(response.currentTime);

                if (response.dayStart > Properties.Settings.Default.dailyCompletionTime)
                {
                    Properties.Settings.Default.dailyCompletionTime = response.dayStart;
                    Properties.Settings.Default.dailyCompletionList.Clear();
                }

                foreach (EventInformation eventInfo in response.events)
                {
                    EventItem eventItem;
                    if (eventDictionary.TryGetValue(eventInfo.id, out eventItem))
                    {
                        eventItem.eventInfo = eventInfo;
                    }
                    else
                    {
                        eventItem = new EventItem();
                        eventItem.eventInfo = eventInfo;
                        eventDictionary.Add(eventInfo.id, eventItem);
                        eventList.Add(eventItem);
                    }

                    eventItem.completed = Properties.Settings.Default.dailyCompletionList.Contains(eventInfo.id);
                }

                ((EventItemComparer)eventView.CustomSort).updateCurrentTimestamp();
                eventView.Refresh();
                eventContainer.UpdateLayout();
                limitVisibleEvents();
            }
            else if (args.func == async_getStartupInformation)
            {
                if (args.exception != null)
                {
                    this.Hide();
                    MessageBox.Show("Cannot retrieve startup information from the server. The program will now close.",
                                    "GW2Stuff Overlay");
                    this.Close();
                    return;
                }

                StartupInformation response = (StartupInformation)args.result;

                worlds = response.worlds;
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                int currentVersion = (version.Major * 1000000) + (version.Minor * 1000) + version.Revision;

                if (currentVersion < response.minVersion)
                {
                    this.Hide();
                    MessageBox.Show("A required update for the overlay is now available: gw2stuff.com/overlay",
                                    "GW2Stuff Overlay");
                    this.Close();
                    return;
                }

                menuItem_options.IsEnabled = true;
                menuItem_world.IsEnabled = true;
                menuItem_world_eu.Items.Clear();
                menuItem_world_us.Items.Clear();
                foreach (WorldInformation worldInformation in worlds)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = worldInformation.name;
                    menuItem.Tag = worldInformation;
                    menuItem.Click += menuItem_world_Click;

                    switch (worldInformation.region)
                    {
                        case "eu":
                            menuItem_world_eu.Items.Add(menuItem);
                            break;
                        case "us":
                            menuItem_world_us.Items.Add(menuItem);
                            break;
                    }
                }

                if (OverlaySettings.defaultWorldId != 0)
                {
                    setWorldId(OverlaySettings.defaultWorldId);
                }
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) { this.MainBrowser.Navigate(new Uri("http://mwoarenaapidemo.azurewebsites.net/duels.html")); }



        private void SetBrowserFeatureControl()
        {
            // http://msdn.microsoft.com/en-us/library/ee330720(v=vs.85).aspx

            // FeatureControl settings are per-process
            var fileName = System.IO.Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);

            // make the control is not running inside Visual Studio Designer
            if (String.Compare(fileName, "devenv.exe", true) == 0 || String.Compare(fileName, "XDesProc.exe", true) == 0)
                return;

            SetBrowserFeatureControlKey("FEATURE_BROWSER_EMULATION", fileName, GetBrowserEmulationMode());
            // Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode.
            SetBrowserFeatureControlKey("FEATURE_AJAX_CONNECTIONEVENTS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ENABLE_CLIPCHILDREN_OPTIMIZATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_MANAGE_SCRIPT_CIRCULAR_REFS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_DOMSTORAGE ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_GPU_RENDERING ", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_IVIEWOBJECTDRAW_DMLT9_WITH_GDI  ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_NINPUT_LEGACYMODE", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_LEGACY_COMPRESSION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_LOCALMACHINE_LOCKDOWN", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_OBJECT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_BLOCK_LMZ_SCRIPT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_DISABLE_NAVIGATION_SOUNDS", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SCRIPTURL_MITIGATION", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_SPELLCHECKING", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_STATUS_BAR_THROTTLING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_TABBED_BROWSING", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_VALIDATE_NAVIGATE_URL", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_DOCUMENT_ZOOM", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_POPUPMANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBOC_MOVESIZECHILD", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_ADDON_MANAGEMENT", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_WEBSOCKET", fileName, 1);
            SetBrowserFeatureControlKey("FEATURE_WINDOW_RESTRICTIONS ", fileName, 0);
            SetBrowserFeatureControlKey("FEATURE_XMLHTTP", fileName, 1);
        }

        private void SetBrowserFeatureControlKey(string feature, string appName, uint value)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(
                String.Concat(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\", feature),
                RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                key.SetValue(appName, (UInt32)value, RegistryValueKind.DWord);
            }
        }

        private UInt32 GetBrowserEmulationMode()
        {
            int browserVersion = 7;
            using (var ieKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer",
                                                                RegistryKeyPermissionCheck.ReadSubTree,
                                                                System.Security.AccessControl.RegistryRights.QueryValues)
                )
            {
                var version = ieKey.GetValue("svcVersion");
                if (null == version)
                {
                    version = ieKey.GetValue("Version");
                    if (null == version)
                        throw new ApplicationException("Microsoft Internet Explorer is required!");
                }
                int.TryParse(version.ToString().Split('.')[0], out browserVersion);
            }

            UInt32 mode = 10000;
            // Internet Explorer 10. Webpages containing standards-based !DOCTYPE directives are displayed in IE10 Standards mode. Default value for Internet Explorer 10.
            switch (browserVersion)
            {
                case 7:
                    mode = 7000;
                    // Webpages containing standards-based !DOCTYPE directives are displayed in IE7 Standards mode. Default value for applications hosting the WebBrowser Control.
                    break;
                case 8:
                    mode = 8000;
                    // Webpages containing standards-based !DOCTYPE directives are displayed in IE8 mode. Default value for Internet Explorer 8
                    break;
                case 9:
                    mode = 9000;
                    // Internet Explorer 9. Webpages containing standards-based !DOCTYPE directives are displayed in IE9 mode. Default value for Internet Explorer 9.
                    break;
                default:
                    // use IE10 mode by default
                    break;
            }


            return mode;
        }




        private void btnApiConn_MouseEnter(object sender, MouseEventArgs e)
        {
            imgMWApiConn.Opacity = 1;
            //  btnApiConn.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;
        }

        private void btnLWConn_MouseEnter(object sender, MouseEventArgs e)
        {
            imgLWConn.Opacity = 1;
            // btnLWConn.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;

        }
        private void btnSettings_MouseEnter(object sender, MouseEventArgs e)
        {
            imgClose.Opacity = 1;
            // btnLWConn.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;

        }



        private void btnBuildConn_MouseLeave(object sender, MouseEventArgs e)
        {

            imgBuildConn.Opacity = .4;
            //  tbLWStatus.Background = lwConn.IsConnected ? System.Windows.Media.Brushes.DodgerBlue : System.Windows.Media.Brushes.DimGray;
        }
        private void btnBuildConn_MouseEnter(object sender, MouseEventArgs e)
        {
            imgBuildConn.Opacity = 1;
            // btnLWConn.BorderBrush = System.Windows.Media.Brushes.DodgerBlue;

        }



        private void btnSettings_MouseLeave(object sender, MouseEventArgs e)
        {

            imgClose.Opacity = .5;
            //  tbLWStatus.Background = lwConn.IsConnected ? System.Windows.Media.Brushes.DodgerBlue : System.Windows.Media.Brushes.DimGray;
        }

        private void btnLWConn_MouseLeave(object sender, MouseEventArgs e)
        {

            imgLWConn.Opacity = (lwConn.IsConnected) ? 1 : .4;
            //  tbLWStatus.Background = lwConn.IsConnected ? System.Windows.Media.Brushes.DodgerBlue : System.Windows.Media.Brushes.DimGray;
        }

        private void btnApiConn_MouseLeave(object sender, MouseEventArgs e)
        {
            imgMWApiConn.Opacity = (mwApiConn.IsConnected) ? 1 : .4;
        }

        private void btnLWConn_Click(object sender, RoutedEventArgs e)
        {
            InfoBlock.Text =
   "This will launch mechwarrior with match logging enabled.";
            lwConn.ConnectionState = (lwConn.IsConnected) ? ConnState.DISCONNECTED : ConnState.CONNECTED;
            if (lwConn.IsConnected)
                lwConn.Connect();
            else
                lwConn.Disconnect();

            tbLWStatus.Fill = lwConn.IsConnected ? System.Windows.Media.Brushes.LimeGreen : System.Windows.Media.Brushes.DimGray;
            imgLWConn.Opacity = (lwConn.IsConnected) ? 1 : .4;
            btnLWConn.BorderThickness = new System.Windows.Thickness(0, 0, 0, 0);

            MwaMainDataGrid.Focus();
        }

        private void btnApiConn_Click(object sender, RoutedEventArgs e)
        {
            
              
            if (!mwApiConn.IsConnected)
            {
                // ToDO: put in ConnectionState Handler
                mwApiConn.ConnectionState = ConnState.CONNECTING;
                tbMWApiStatus.Fill = System.Windows.Media.Brushes.Yellow;
                tbMWApiStatus.Fill = mwApiConn.IsConnected ? System.Windows.Media.Brushes.LimeGreen : System.Windows.Media.Brushes.DimGray;
                imgMWApiConn.Opacity = (mwApiConn.IsConnected) ? 1 : .4;
                btnApiConn.BorderThickness = new System.Windows.Thickness(0, 0, 0, 0);
                MwaMainDataGrid.Focus();
          

                InfoBlock.Text =
                    " The data is a detail view of your recent matches.";


                mwApiConn.ConnectAndRefreshEvery(60);
            }
            else
            {
                mwApiConn.IsActive = false;
            }

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsMenu.PlacementTarget = this;
            SettingsMenu.IsOpen = true;
        }

        private void tbPilotName_LostFocus(object sender, RoutedEventArgs e)
        {
            tbPilotName.IsReadOnly = true;
            tbPilotName.Background = Brushes.Transparent;
            Properties.Settings.Default.UserName = tbPilotName.Text;
            UserName = tbPilotName.Text;
            //MatchLogger.MatchLogger.SetPlayerName(UserName);
            tbSystemMessages.Text = String.Format("MWApi:{0}", MatchLogger.MatchLogger.GetApiUrl());
            mwApiConn = new WebApiIntegrationConnector("MWApi", new Uri(MatchLogger.MatchLogger.GetApiUrl()));
        }

        private void btnBuildConn_Click(object sender, RoutedEventArgs e)
        {
            buildApiConn.ConnectionState = (buildApiConn.IsConnected) ? ConnState.DISCONNECTED : ConnState.CONNECTED;
            tbBuildStatus.Fill = buildApiConn.IsConnected ? System.Windows.Media.Brushes.LimeGreen : System.Windows.Media.Brushes.DimGray;
            imgBuildConn.Opacity = (buildApiConn.IsConnected) ? 1 : .4;
            btnBuildConn.BorderThickness = new System.Windows.Thickness(0, 0, 0, 0);
            MwaMainDataGrid.Focus();
            if (buildApiConn.IsConnected)
            {
                InfoBlock.Text =
                    " The data above aggregates all matches published to the MWApis. This is not just your data. Coming Soon 'You vs The World' view!";


                buildApiConn.ConnectionState = ConnState.CONNECTING;
                tbBuildStatus.Fill = System.Windows.Media.Brushes.Yellow;
                GetVariantData();
            }
        }
        private async void GetMatchData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = mwApiConn.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            try
            {
                string endp = String.Format("mwoamatchmetric/{0}", tbPilotName.Text);
                response = await client.GetAsync(endp);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var matches = await response.Content.ReadAsAsync<IEnumerable<MwoAMatchMetric>>();
                MwaMainDataGrid.ItemsSource = matches.Select(o => new { o.level, o.matchType, o.mech, o.kills, o.damage });

            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // This exception indicates a problem deserializing the request body.
                InfoBlock.Text = "ERROR:" + jEx.Message;
                mwApiConn.ConnectionState = ConnState.ERROR;
            }
            catch (HttpRequestException ex)
            {
                InfoBlock.Text = "ERROR:" + ex.Message;
                mwApiConn.ConnectionState = ConnState.ERROR;
            }
            finally
            {
                mwApiConn.ConnectionState = ConnState.CONNECTED;
                tbMWApiStatus.Fill = (mwApiConn.IsConnected) ? System.Windows.Media.Brushes.LimeGreen : (mwApiConn.ConnectionState == ConnState.ERROR) ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DimGray; ;

            }

        }

        private async void GetVariantData()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = mwApiConn.ApiUrl;
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            try
            {

                string endp = "VariantAssocMetric";
                response = await client.GetAsync(endp);
                response.EnsureSuccessStatusCode(); // Throw on error code.
                var mechs = await response.Content.ReadAsAsync<IEnumerable<vwVariantAssocMetric>>();
                MwaMainDataGrid.ItemsSource = mechs.Select(o => new { Mech = o.BaseVariantName, Drops = o.matches, o.WinPerc, K = o.kills, D = o.deaths, o.DmgPM });

            }
            catch (Newtonsoft.Json.JsonException jEx)
            {
                // This exception indicates a problem deserializing the request body.
                InfoBlock.Text = "ERROR:" + jEx.Message;
                buildApiConn.ConnectionState = ConnState.ERROR;
            }
            catch (HttpRequestException ex)
            {
                InfoBlock.Text = "ERROR:" + ex.Message;
                buildApiConn.ConnectionState = ConnState.ERROR;
            }
            finally
            {
                buildApiConn.ConnectionState = ConnState.CONNECTED;
                tbBuildStatus.Fill = (buildApiConn.IsConnected) ? System.Windows.Media.Brushes.LimeGreen : (mwApiConn.ConnectionState == ConnState.ERROR) ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.DimGray; ;

            }

        }

    }

  

    public class WebApiIntegrationConnector : IntegrationConnector
    {
        private bool debug = false;
        private Uri apiUrl;

           public WebApiIntegrationConnector()
            : base("")
        {

        }

           public WebApiIntegrationConnector(string name)
            : base(name)
        {

        }
        public WebApiIntegrationConnector(string name,Uri ApiUrl):base(name,ApiUrl.ToString())
        {
    
        }
        public Uri ApiUrl
        {
            get { return apiUrl; }
            set
            {
                if (value != this.apiUrl)
                {
                    this.apiUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }

    }


    public class ExeIntegrationConnector : MWA.Integration.IntegrationConnector
    {
        private bool debug = false;


        public ExeIntegrationConnector()
            : base("")
        {

        }

        public ExeIntegrationConnector(string name)
            : base(name)
        {

        }
        public ExeIntegrationConnector(string name, string connectorSource)
            : base(name,connectorSource)
        {

        }


        public bool IsProcessRunning()
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running.
                //Remember, if you have the process running more than once, 
                //say IE open 4 times the loop thr way it is now will close all 4,
                //if you want it to just close the first one it finds
                //then add a return; after the Kill
                if (clsProcess.ProcessName.Contains(this.Name))
                {
                    //if the process is found to be running then we
                    //return a true
                    return true;
                }
            }
            //otherwise we return a false
            return false;
        }


        public override void Connect()
        {

            if (!IsProcessRunning())
            {
                //AppDomain currentDomain = AppDomain.CurrentDomain;
                //currentDomain.ExecuteAssembly("logwarrior.exe");


                ProcessStartInfo stinfo = new ProcessStartInfo();

                // Assign file name

                stinfo.FileName = this.Name;

                // start the process without creating new window default is false

                stinfo.CreateNoWindow = false;

                // true to use the shell when starting the process; otherwise, the process is created directly from the executable file

                stinfo.UseShellExecute = true;



                // Creating Process class object to start process

                Process myProcess = Process.Start(stinfo);

                // start the process

                myProcess.Start();
            }
        }

        public override void Disconnect()
        {    //Shut it down
            InputManager.Current.ProcessInput(
                new KeyEventArgs(Keyboard.PrimaryDevice,
                Keyboard.PrimaryDevice.ActiveSource,
                0, Key.F12)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                }
            );
        }
    }

    

   




}
