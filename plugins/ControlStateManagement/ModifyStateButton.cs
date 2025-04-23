using Renga;

namespace ControlStateManagement
{
	/// <summary>
	/// Кнопка контролируящая свойство Enable у другой кнопки
	/// </summary>
	/// <param name="ui"></param>
	/// <param name="tooltip"></param>
	internal class ModifyStateButton(IUI ui, string tooltip)
		: Button(ui, tooltip)
	{
		private readonly List<IAction> _actions = [];

		public void AddLinkedButton(Button button)
		{
			_actions.Add(button.Action);
		}

		protected override void OnActionEventsTriggered(object? sender, EventArgs e)
		{
			foreach (IAction action in _actions)
				action.Enabled = !action.Enabled;
		}

	}
}
