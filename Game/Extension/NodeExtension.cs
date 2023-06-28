namespace Game.Extension;

using Game.Component;

public static class NodeExtension
{
	private static ResourcePreloader _preloader;

	public static T InstantiateFromResources<T>(this Node node) where T : Node
	{
		_preloader ??= node.GetNode<ResourcePreloader>("/root/RsPreloader");
		return _preloader.InstanceSceneOrNull<T>();
	}

	public static Color IntensifyColor(this Node node, Color color, float factor)
	{
		var result = new Color(color);
		result *= factor;
		result.A = 1.0f;
		return result;
	}

	public static T AddResourceDeferred<T>(this Node node) where T : Node
	{
		var child = node.InstantiateFromResources<T>();
		node.CallDeferred("add_child", child);
		return child;
	}

	public static async void AddResourceDeferredWithAction<T>(this Node node, Action<T> action) where T : Node
	{
		var child = node.AddResourceDeferred<T>();
		await child.ToSignal(child, Node.SignalName.TreeEntered);
		action(child);
	}

	public static void AddNodeToQueueFreeComponent(this Node node, Node nodeToAdd)
	{
		var queueFreeComponent = node.GetFirstNodeOfType<QueueFreeComponent>();
		if (queueFreeComponent == null)
		{
			return;
		}

		queueFreeComponent.AddWaitForNodeExit(nodeToAdd);
	}

	public static void AddResourceAndQueueFree<T>(this Node node) where T : Node =>
		node.AddResourceDeferredWithAction<T>(node.AddNodeToQueueFreeComponent);

	/// <summary>
	/// Enables or disables the specified child component type - does nothing if component not found.
	/// </summary>
	/// <typeparam name="T">The type of component to enable or disable. Must be a subclass of BaseComponent.</typeparam>
	/// <param name="node">The node to search for its component.</param>
	/// <param name="enabled">Whether to enable or disable the component.</param>
	public static void EnableComponent<T>(this Node node, bool enabled) where T : BaseComponent =>
		node.GetFirstNodeOfType<T>()?.SetEnabled(enabled);
}
