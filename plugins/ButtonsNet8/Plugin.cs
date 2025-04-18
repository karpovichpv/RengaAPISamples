using Renga;

namespace ButtonsNet8
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

			// Tool button:
			panelExtension.AddToolButton(
			  CreateAction(ui, "Net8 SimpleButton"));

			// DropDownButton:
			IDropDownButton dropDownButton = ui.CreateDropDownButton();
			dropDownButton.ToolTip = "Net8 DropDownButton";

			dropDownButton.AddAction(
			  CreateAction(ui, "Net8 DropDownButton Action1"));

			dropDownButton.AddAction(
			  CreateAction(ui, "Net8 DropDownButton Action2"));
			dropDownButton.AddSeparator();

			dropDownButton.AddAction(
			  CreateAction(ui, "Net8 DropDownButton Action3"));

			panelExtension.AddDropDownButton(dropDownButton);

			// SplitButton:
			ISplitButton splitButton = ui.CreateSplitButton(
			  CreateAction(ui, "Net8 SplitButton Default Action"));

			splitButton.AddAction(
			  CreateAction(ui, "Net8 SplitButton Action2"));

			splitButton.AddAction(
			  CreateAction(ui, "Net8 SplitButton Action 3"));

			panelExtension.AddSplitButton(splitButton);


			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateAction(IUI ui, string displayName)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = displayName;
			action.Icon = _icon;

			ActionEventSource events = new(action);
			events.Triggered += (s, e) =>
			{
				ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "Plugin message", displayName + " Handler");
			};

			_eventSources.Add(events);

			return action;
		}
	}
}
