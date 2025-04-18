using Renga;
using System.Reflection;

namespace SamplePluginNet8
{
	public class Plugin : IPlugin
	{
		private ActionEventSource? _followAction;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Renga.Application();
			IUI ui = app.UI;
			IUIPanelExtension panel = ui.CreateUIPanelExtension();
			IAction button = ui.CreateAction();
			string? name = GetAssemblyName();

			button.ToolTip = name;
			button.DisplayName = name;

			IImage icon = ui.CreateImage();
			string iconPath = pluginFolder + @"\ico.png";
			icon.LoadFromFile(iconPath);
			button.Icon = icon;

			_followAction = new ActionEventSource(button);
			_followAction.Triggered += (sender, args) =>
			{
				ui.ShowMessageBox(MessageIcon.MessageIcon_Info, $"{name} plugin", "Hello world!");
			};


			panel.AddToolButton(button);
			ui.AddExtensionToPrimaryPanel(panel);

			return true;
		}

		public void Stop()
		{
			_followAction?.Dispose();
		}

		private string? GetAssemblyName()
		{
			Assembly? assembly = Assembly.GetAssembly(GetType());
			string? name = string.Empty;

			if (assembly is not null)
				name = assembly.GetName().Name;
			return name;
		}
	}
}
