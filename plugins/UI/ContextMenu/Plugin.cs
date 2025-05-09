using Renga;

namespace ContextMenu
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];
		private IUI? _ui;
		private IContextMenu? _contextMenu;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();

			IUI ui = app.UI;
			_ui = ui;

			IContextMenu contextMenu = ui.CreateContextMenu();
			_contextMenu = contextMenu;

			AddNodeItems(ui, contextMenu);

			contextMenu.AddSeparator();

			AddMenuItem("Message from sample", (s, e) =>
			{
				_ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "Message from sample", "Context menu clicked");
			});
			AddMenuItem("Add item", (s, e) =>
			{
				IContextMenuNodeItem nodeItem = _contextMenu.AddNodeItem();
				nodeItem.DisplayName = "New node";

				Update();
			});

			Update();

			return true;
		}

		private static void AddNodeItems(IUI ui, IContextMenu contextMenu)
		{
			IContextMenuNodeItem nodeItem = contextMenu.AddNodeItem();
			nodeItem.DisplayName = "NodeItem";

			IAction action1 = ui.CreateAction();
			action1.DisplayName = "Node1";
			action1.Checkable = true;
			action1.Checked = true;

			IAction action2 = ui.CreateAction();
			action2.DisplayName = "Node2";
			action2.Enabled = false;

			nodeItem.AddActionItem(action1);
			nodeItem.AddActionItem(action2);
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private void AddMenuItem(string displayName, EventHandler handler)
		{
			if (_ui is not null)
			{
				IAction action = _ui.CreateAction();
				action.DisplayName = displayName;
				action.ToolTip = displayName;
				ActionEventSource actionEventSource = new(action);
				actionEventSource.Triggered += handler;
				_eventSources.Add(actionEventSource);

				_contextMenu?.AddActionItem(action);
			}
		}

		private void Update()
		{
			_ui?.AddContextMenu(
				new Guid(),
				_contextMenu,
				ViewType.ViewType_View3D,
				ContextMenuShowCase.ContextMenuShowCase_Selection);
		}
	}
}
