using System.Linq;
using System.Numerics;
using UnityEngine;

public class GroundColliderMerger2 : MonoBehaviour
{
    [SerializeField] private GameObject cube1;
    [SerializeField] private GameObject cube2;
    public string layerName = "Ground";
    public bool destroyOriginalObjects = true;



    private void Start()
    {
        //var objectsOnLayer = GameObject.FindGameObjectsWithTag(layerName);

        //var combinedMeshObject = new GameObject("GroundLayerMesh");
        //combinedMeshObject.transform.position = Vector3.zero;
        //combinedMeshObject.transform.rotation = Quaternion.identity;

        var verticses1 = cube1.GetComponent<MeshFilter>().mesh.vertices.Select(x => cube1.transform.TransformPoint(x)).ToArray();
        var uvs1 = cube1.GetComponent<MeshFilter>().mesh.uv.Select(x => cube1.transform.TransformPoint(x)).ToArray();
        var triangles1  = cube1.GetComponent<MeshFilter>().mesh.triangles;
        var verticses2 = cube2.GetComponent<MeshFilter>().mesh.vertices.Select(x => cube2.transform.TransformPoint(x)).ToArray();
        var triangles2 = cube2.GetComponent<MeshFilter>().mesh.triangles;
        var uvs2 = cube2.GetComponent<MeshFilter>().mesh.uv.Select(x => cube2.transform.TransformPoint(x)).ToArray();
        //var newMesh = new Mesh();
        //newMesh.vertices = verticses;
        //newMesh.uv = uvs;
        //newMesh.triangles = triangles;

        //var combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
        //combinedMeshFilter.sharedMesh = newMesh;

        ////var combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
        ////combinedMeshRenderer.sharedMaterial = objectsOnLayer[0].GetComponent<MeshRenderer>().sharedMaterial;

        //var combinedMeshCollider = combinedMeshObject.AddComponent<MeshCollider>();
        //combinedMeshCollider.sharedMesh = newMesh;

        //combinedMeshObject.tag = "Ground";
        //combinedMeshObject.layer = LayerMask.NameToLayer("Ground");
        ////combinedMeshCollider.convex = true;
    }
}