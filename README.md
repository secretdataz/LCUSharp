# LCUSharp 
League of Legends client integration library for .NET

LCUSharp is written as my experiment with C# async/await concept. The API is currently tentative and may change anytime.
Steps for reading LCU's token and port is taken from @Pupix's [lcu-connector](https://github.com/Pupix/lcu-connector) library for NodeJS.


For the LCU API documentation check out [Rift explorer](https://github.com/Pupix/rift-explorer)

## Usage
```cs
private void Form1_Load(object sender, EventArgs e)
{
	AfterLoad();
}

private async void AfterLoad()
{
	// Connect to LCU and let LCUSharp finds all details about LCU (Requires admin privilege)
	League = await LeagueClient.Connect();
	
	// Connect to LCU by providing path.
	// League = await LeagueClient.Connect(@"C:/Riot/League/");
	
	// will be executed after LCUSharp obtains connection to LCU.
	this.label1.Text = "Ready";
	
	// do something with LCU
	// ...
	// ...
	
	League.LeagueClosed += () =>
	{
		// This will be executed after League exits
		
		// cleanup steps
		// ...
	};
}
```