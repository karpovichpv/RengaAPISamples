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
			nodeItem.DisplayName = "Placement editing";
			nodeItem.AddActionItem(CreateAction(ui, "X+" + _movementValue, (IPlacement3D placement) =>
			  {
				  placement.Move(new Vector3D { X = _movementValue, Y = 0, Z = 0 });
				  return placement;
			  }));
			nodeItem.AddActionItem(CreateAction(ui, "Y+" + _movementValue, (IPlacement3D placement) =>
			  {
				  placement.Move(new Vector3D { X = 0, Y = _movementValue, Z = 0 });
				  return placement;
			  }));
			nodeItem.AddActionItem(CreateAction(ui, "Z+" + _movementValue, (IPlacement3D placement) =>
			  {
				  placement.Move(new Vector3D { X = 0, Y = 0, Z = _movementValue });
				  return placement;
			  }));

			_ui?.AddContextMenu(
				new Guid(),
				_contextMenu,
				ViewType.ViewType_View3D,
				ContextMenuShowCase.ContextMenuShowCase_Selection);

			return true;
		}

		private IAction CreateAction(IUI ui, string displayName, Func<IPlacement3D, IPlacement3D> func)
		{
			var action = ui.CreateAction();
			action.DisplayName = displayName;

			var events = new Renga.ActionEventSource(action);
			events.Triggered += (s, e) =>
			{
				if (_app is not null)
				{
					int[] selectedObjectsIds = (int[])_app.Selection.GetSelectedObjects();
					if (selectedObjectsIds.Length > 1)
						return;

					int id = selectedObjectsIds.ElementAt(0);
					if (_app.Project.Model.GetObjects().GetById(id) is not ILevelObject levelObject)
						return;

					IOperation op = _app.Project.CreateOperation();
					op.Start();
					IPlacement3D placement = levelObject.GetPlacement();
					placement = func(placement);
					levelObject.SetPlacement(placement);
					op.Apply();
				}
			};

			_eventSources.Add(events);

			return action;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}
	}
}
