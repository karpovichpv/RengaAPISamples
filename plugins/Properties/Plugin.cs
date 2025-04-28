using Renga;

namespace Properties
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private Application? _app;
		private IUI? _ui;
		private IUIPanelExtension? _panelExtension;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();

			_ui = _app.UI;
			_panelExtension = _ui.CreateUIPanelExtension();


			CreateMessageBox(
				"Add properties to all floors in the model",
				"Adding properties to all floors",
				PluginHelpers.AddPropertyToAllFloors);

			CreateMessageBox(
				"Printing the first floor properties values",
				"Print properties",
				PluginHelpers.PrintPropertyParameters);

			_ui.AddExtensionToPrimaryPanel(_panelExtension);

			return true;
		}

		private void CreateMessageBox(
			string tooltip,
			string title,
			Func<IApplication, string> callBack)
		{
			if (_ui is not null && _app is not null)
			{
				var action = _ui.CreateAction();
				action.ToolTip = tooltip;
				_panelExtension?.AddToolButton(action);

				ActionEventSource events = new(action);
				events.Triggered += (sender, arguments) =>
				{
					_ui.ShowMessageBox(
						MessageIcon.MessageIcon_Info,
						title,
						callBack?.Invoke(_app));
				};
			}
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}
	}
}
