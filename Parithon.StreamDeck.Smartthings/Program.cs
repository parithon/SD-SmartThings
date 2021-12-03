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
//sdclient.DidReceiveGlobalSettings += async (s, e) =>
//{
//    if (e.Payload.Settings && !string.IsNullOrEmpty(e.Payload.Settings.AuthToken))
//    {
//        stclient.SetAuthToken(e.Payload.Settings.AuthToken);
//        await stclient.RefreshDevicesAsync();
//    }
//};

sdclient.Execute();
