using Renga;

namespace CreateFloor
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private IImage? _icon;

		public bool Initialize(string pluginFolder)
		{
			Application app = new();
			IUI ui = app.UI;

			string icoPath = pluginFolder + @"\ico.png";
			_icon = ui.CreateImage();
			_icon.LoadFromFile(icoPath);

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();
			IDropDownButton dropDownButton = ui.CreateDropDownButton();
			dropDownButton.ToolTip = "Net8 DropDownButton";

			dropDownButton.AddAction(CreateFloor(ui));

			panelExtension.AddDropDownButton(dropDownButton);

			ui.AddExtensionToPrimaryPanel(panelExtension);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateFloor(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = "Floor";
			action.Icon = _icon;

			ActionEventSource events = new(action);
			events.Triggered += (s, e) =>
			{
				PluginHelpers.CreateFloor();
			};

			_eventSources.Add(events);

			return action;
		}
	}
}
