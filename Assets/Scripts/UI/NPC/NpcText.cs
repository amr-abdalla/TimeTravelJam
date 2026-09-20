using TMPro;
using UnityEngine;

public class NpcText : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI textMeshPro;

	public void UpdateText(NpcData npcData)
	{
		textMeshPro.text = TextHelper.Instance.GetNPCText(npcData);
	}
}
