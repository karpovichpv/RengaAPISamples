using Renga;

namespace ControlStateManagement
{
	public class Plugin : IPlugin
	{
		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			IUI ui = app.UI;

			Button b1 = new(ui, "button #1");
			ModifyStateButton b2 = new(ui, "button #2");

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();
			b1.AddToPanel(panelExtension);
			b2.AddToPanel(panelExtension);
			b2.AddLinkedButton(b1);

			ui.AddExtensionToPrimaryPanel(panelExtension);

			return true;
		}

		public void Stop()
		{

		}
	}
}
