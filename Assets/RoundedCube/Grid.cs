using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour {

	public int xSize, ySize;

	private Mesh mesh;
	private Vector3[] vertices;

	private void Awake () {
		Generate();
	}

	private void Generate () {
		GetComponent<MeshFilter>().mesh = mesh = new Mesh();
		mesh.name = "Procedural Grid";

		vertices = new Vector3[(xSize*2 + 1) * (ySize*2 + 1)];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		for (int i = 0, y = 0; y <= ySize*2; y++) {
			for (int x = 0; x <= xSize*2; x++, i++) {
				vertices[i] = new Vector3((float)x/2, (float)y/2);
				uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
				tangents[i] = tangent;
			}
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.tangents = tangents;

		int[] triangles = new int[xSize*2 * ySize*2 * 6];
		for (int ti = 0, vi = 0, y = 0; y < ySize*2; y++, vi++) {
			for (int x = 0; x < xSize*2; x++, ti += 6, vi++) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + xSize*2 + 1;
				triangles[ti + 5] = vi + xSize*2 + 2;
			}
		}
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null) {
    //        return;
    //    }
    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < vertices.Length; i++) {
    //        Gizmos.DrawSphere(vertices[i], 0.1f);
    //        Debug.Log(vertices[i]);
    //    }
    //}
}