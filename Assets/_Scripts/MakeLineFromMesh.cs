using UnityEngine;
using System.Collections;

public class MakeLineFromMesh : MonoBehaviour {

	private MeshFilter meshFilter;
	private int[] triangles;

	void Start () {
		meshFilter = GetComponent<MeshFilter>();
	}
	
	void Update () {
		//updateMesh();
	}

	public void updateMesh() {
		if(meshFilter.mesh != null) {
			MeshTopology topo = meshFilter.mesh.GetTopology(0);

			if( topo == MeshTopology.Triangles ){
				triangles = meshFilter.mesh.triangles;
				meshFilter.mesh.SetIndices(MakeIndices(), MeshTopology.Lines, 0);

				topo = meshFilter.mesh.GetTopology(0);
			}
		}
	}

	int[] MakeIndices(){
		int[] indices = new int[2 * triangles.Length];
		int i = 0;
		for( int t = 0; t < triangles.Length; t+=3 )
		{
			indices[i++] = triangles[t];		//start
			indices[i++] = triangles[t + 1];	//end
			indices[i++] = triangles[t + 1];	//start
			indices[i++] = triangles[t + 2];	//end
			indices[i++] = triangles[t + 2];	//start
			indices[i++] = triangles[t];		//end
		}
		return indices;
	}
}
