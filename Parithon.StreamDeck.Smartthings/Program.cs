using Parithon.StreamDeck.SDK;
using Parithon.StreamDeck.Smartthings;

#if DEBUG
System.Diagnostics.Debugger.Launch();
#endif

var stclient = new SmartthingsClient();

var sdclient = new StreamDeckClientBuilder(args)
    .AddSingleton(stclient)
    .Build();

sdclient.Connected += (s, e) =>
{
    sdclient.GetGlobalSettingsAsync(sdclient.UUID);
};
sdclient.Disconnected += (s, e) => {
    stclient.CancelPendingRequests();
};
sdclient.DidReceiveGlobalSettings += async (s, e) =>
{
    string authToken = e.Payload.Settings.authToken;
    stclient.SetAuthToken(authToken);
    await stclient.RefreshDevicesAsync();
    await stclient.RefreshScenesAsync();
};

sdclient.Execute();
