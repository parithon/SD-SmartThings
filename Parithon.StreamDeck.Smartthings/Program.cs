using Parithon.StreamDeck.SDK;

var sdclient = new StreamDeckClientBuilder(args)
    .Build();

sdclient.Execute();
