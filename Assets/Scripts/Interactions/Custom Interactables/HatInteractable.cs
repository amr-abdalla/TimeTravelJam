using UnityEngine;

public class HatInteractable : Interactable
{
	[HideInInspector] public DressUpCharacter AssociatedCharacter;

	public override void Interaction()
	{
		base.Interaction();

		if (AssociatedCharacter == null)
			Debug.LogError("Associated character is null on Hat Interactable for some reason", this.gameObject);

		AssociatedCharacter.RemoveHat();
	}
}
