public class InteractableNPC : Interactable
{
	public override void Interaction()
	{
		NpcPositionManager.Instance.SelectNPC(gameObject);
		base.Interaction();
	}
}
