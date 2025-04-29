using Renga;

namespace SystemEvents
{
	public class Plugin : IPlugin
	{
		public bool Initialize(string pluginFolder)
		{
			return true;
		}

		private void OnProjectSaved(string obj)
		{
		}

		public void Stop()
		{
		}
	}
}