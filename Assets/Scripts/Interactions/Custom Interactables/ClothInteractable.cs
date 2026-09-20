using UnityEngine;

[RequireComponent (typeof(MeshCollider))]
[RequireComponent (typeof(SkinnedMeshRenderer))]
public class ClothInteractable : Interactable
{
	private MeshCollider _meshColl;

	private void Start()
	{
		_meshColl = GetComponent<MeshCollider>();
		SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();

		Mesh mesh = new Mesh();
		renderer.BakeMesh(mesh);
		_meshColl.sharedMesh = mesh;
	}

	public override void Interaction()
	{
		base.Interaction();
	}
}