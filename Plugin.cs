

using Renga;
using System.Windows;

namespace RengaSamplePlugin
{
	public class Plugin : IPlugin
	{
		private ActionEventSource _followAction;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Renga.Application();
			IUI ui = app.UI;
			IUIPanelExtension panel = ui.CreateUIPanelExtension();
			IAction button = ui.CreateAction();
			button.ToolTip = "Тестовый плагин №1";
			button.DisplayName = "Тестовый плагин №1";

			IImage icon = ui.CreateImage();
			string iconPath = pluginFolder + @"\logo.png";
			icon.LoadFromFile(iconPath);
			button.Icon = icon;

			_followAction = new ActionEventSource(button);
			_followAction.Triggered += (sender, args) =>
			{
				ui.ShowMessageBox(Renga.MessageIcon.MessageIcon_Info, "Пользовательский плагин", "Привет Renga");
				MessageBox.Show("show message box");
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
