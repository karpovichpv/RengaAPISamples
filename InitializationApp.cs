

namespace RengaSamplePlugin
{
	public class InitializationApp : Renga.IPlugin
	{
		private Renga.ActionEventSource _followAction;

		public bool Initialize(string pluginFolder)
		{
			Renga.IApplication app = new Renga.Application();
			Renga.IUI ui = app.UI;
			Renga.IUIPanelExtension panel = ui.CreateUIPanelExtension();
			Renga.IAction button = ui.CreateAction();
			button.ToolTip = "Start test message";
			button.DisplayName = "Start text message box";

			Renga.IImage icon = ui.CreateImage();
			icon.LoadFromFile(pluginFolder + @"\logo.png");
			button.Icon = icon;

			_followAction = new Renga.ActionEventSource(button);
			_followAction.Triggered += (sender, args) =>
			{
				ui.ShowMessageBox(Renga.MessageIcon.MessageIcon_Info, "Пользовательский плагин", "Привет Renga");
			};


			panel.AddToolButton(button);
			ui.AddExtensionToPrimaryPanel(panel);

			return true;
		}

		public void Stop()
		{
			_followAction.Dispose();
		}
	}
}
