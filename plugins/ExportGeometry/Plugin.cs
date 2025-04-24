using Renga;

namespace ExportGeometry
{
	public class Plugin : IPlugin
	{
		private Application? _app;
		private IUI? _ui;
		private ActionEventSource? _eventSource;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();
			_ui = _app.UI;

			IUIPanelExtension panelExtension = _ui.CreateUIPanelExtension();
			IAction exportObjects = _ui.CreateAction();
			exportObjects.ToolTip = "ExportObjects";
			_eventSource = new ActionEventSource(exportObjects);
			_eventSource.Triggered += OnExportObjectsButtonClicked;
			panelExtension.AddToolButton(exportObjects);

			_ui.AddExtensionToPrimaryPanel(panelExtension);

			return true;
		}

		private void OnExportObjectsButtonClicked(object? sender, EventArgs e)
		{
			_ui?.ShowMessageBox(
				MessageIcon.MessageIcon_Info,
				"Export objects",
				"Exported objects");
		}

		public void Stop()
		{
			_eventSource?.Dispose();
		}
	}
}
