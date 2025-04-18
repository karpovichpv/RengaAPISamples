

using Renga;
using System.Reflection;
using System.Windows;

namespace SamplePluginNetFramework
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
			string name = Assembly.GetAssembly(GetType()).FullName;
			button.ToolTip = name;
			button.DisplayName = name;

			IImage icon = ui.CreateImage();
			string iconPath = pluginFolder + @"\ico.png";
			icon.LoadFromFile(iconPath);
			button.Icon = icon;

			_followAction = new ActionEventSource(button);
			_followAction.Triggered += (sender, args) =>
			{
				ui.ShowMessageBox(Renga.MessageIcon.MessageIcon_Info, $"{name} plugin", "Hello world!");
				MessageBox.Show("Message box from the WPF!");
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
