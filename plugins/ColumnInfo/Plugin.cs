using Renga;

namespace ColumnInfo
{
	public class Plugin : IPlugin
	{
		readonly List<ActionEventSource> _actionEventSources = [];

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			IUI ui = app.UI;

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();

			panelExtension.AddToolButton(CreateAction(ui));
			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);

			return true;
		}


		public void Stop()
		{
			foreach (ActionEventSource eventSource in _actionEventSources)
				eventSource.Dispose();

			_actionEventSources.Clear();
		}

		private IAction CreateAction(IUI ui)
		{
			IAction action = ui.CreateAction();

			ActionEventSource eventSource = new(action);
			eventSource.Triggered += (obj, args) =>
			{
				ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "sample plugin", "sample plugin");
			};

			_actionEventSources.Add(eventSource);
			return action;
		}
	}
}
