using Renga;

namespace ObjectPlacementModifying
{
	public class Plugin : IPlugin
	{
		private const int _movementValue = 100;
		private readonly List<ActionEventSource> _eventSources = [];
		private IUI? _ui;
		private IContextMenu? _contextMenu;
		private Application? _app;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();

			IUI ui = _app.UI;
			_ui = ui;
			IContextMenu contextMenu = ui.CreateContextMenu();
			_contextMenu = contextMenu;

			IContextMenuNodeItem nodeItem = contextMenu.AddNodeItem();
			nodeItem.DisplayName = "Properties and parameters";

			_ui?.AddContextMenu(
				new Guid(),
				_contextMenu,
				ViewType.ViewType_View3D,
				ContextMenuShowCase.ContextMenuShowCase_Selection);

			return true;
		}


		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}
	}
}
