namespace SampleGodotCSharpProject.Game.Component.Element;

public partial class ElementComponent : BaseComponent
{
	[Signal]
	public delegate void IntensityDepletedEventHandler(ElementComponent element);

	[Signal]
	public delegate void IntensityMaxedEventHandler(ElementComponent element);

	protected float Energy;

	public virtual float AddEnergy(float factor, bool emitSignals = true)
	{
		return 0.0f;
	}

	public virtual float SetEnergy(float energy, bool emitSignals = true)
	{
		var oldEnergy = Energy;
		Energy = Mathf.Clamp(energy, 0f, 1f);
		return oldEnergy;
	}

	public float GetEnergy()
	{
		return Energy;
	}
}
