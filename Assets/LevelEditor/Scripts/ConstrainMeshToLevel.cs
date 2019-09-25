using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ConstrainMeshToLevel : MonoBehaviour
{

	private void Start()
	{
		var filter = GetComponent<MeshFilter>();
		filter.mesh = ConstrainMesh(filter.mesh);
	}


	Mesh ConstrainMesh(Mesh mesh)
	{
		var worldToLocal = GetComponent<MeshRenderer>().worldToLocalMatrix;
		List<Vector3> verts = new List<Vector3>(mesh.vertexCount);
		mesh.GetVertices(verts);
		var tris = mesh.GetTriangles(0);
		Bounds levelBounds = new Bounds();
		levelBounds.SetMinMax(
			worldToLocal.MultiplyPoint(Vector3.zero),
			worldToLocal.MultiplyPoint(new Vector3(LevelGrid.Instance.GetMapSize().x, 512, LevelGrid.Instance.GetMapSize().y)));
		Bounds newMeshBounds = new Bounds();
		foreach (var vert in verts)
		{
			if (levelBounds.Contains(vert))
				newMeshBounds.Encapsulate(vert);
		}
		/*
		for (int i = 0; i < tris.Length; i += 3)
		{
			if (!levelBounds.Contains(verts[tris[i]]) && levelBounds.Contains(verts[tris[i + 1]]))
			{
				var dir = (verts[tris[i + 1]] - verts[tris[i]]).normalized;
				var ray = new Ray(verts[tris[i]], dir);
				float dist;
				if (levelBounds.IntersectRay(ray, out dist))
				{
					verts[tris[i]] += dir * dist;
					newMeshBounds.Encapsulate(verts[tris[i]]);
				}
			}
			else if (!levelBounds.Contains(verts[tris[i]]) && levelBounds.Contains(verts[tris[i + 2]]))
			{
				var dir = (verts[tris[i + 2]] - verts[tris[i]]).normalized;
				var ray = new Ray(verts[tris[i]], dir);
				float dist;
				if (levelBounds.IntersectRay(ray, out dist))
				{
					verts[tris[i]] += dir * dist;
					newMeshBounds.Encapsulate(verts[tris[i]]);
				}
			}
		}*/

		for (int i = 0; i < verts.Count; i++)
		{
			verts[i] = newMeshBounds.ClosestPoint(verts[i]);
		}


		mesh.SetVertices(verts);
		mesh.UploadMeshData(true);
		return mesh;
	}
}
