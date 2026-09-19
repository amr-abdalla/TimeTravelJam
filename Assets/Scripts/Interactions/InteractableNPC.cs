public class InteractableNPC : Interactable
{
	public override void Interaction()
	{
		NpcPositionArranger.Instance.SelectSpecific(gameObject);
		base.Interaction();
	}
}
