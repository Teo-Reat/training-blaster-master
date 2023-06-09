using UnityEngine;

public class GroundColliderMerger : MonoBehaviour
{
    public string layerName = "Ground";
    public bool destroyOriginalObjects = true;

    private void Start()
    {
        // Находим все объекты на слоен с указанным именем
        var objectsOnLayer = GameObject.FindGameObjectsWithTag(layerName);

        // Создаем новый пустой объект для объединенного меша
        var combinedMeshObject = new GameObject("CombinedMesh");
        combinedMeshObject.transform.position = Vector3.zero;
        combinedMeshObject.transform.rotation = Quaternion.identity;

        // Создаем массива мешей, которые мы будем объединять
        var meshFilters = new MeshFilter[objectsOnLayer.Length];
        for (int i = 0; i < objectsOnLayer.Length; i++)
        {
            meshFilters[i] = objectsOnLayer[i].GetComponent<MeshFilter>();
        }

        // Создаем новый меш и объединяем все меши в один
        var combinedMesh = new Mesh();
        var combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            if (destroyOriginalObjects)
            {
                Destroy(objectsOnLayer[i]);
            }
        }

        combinedMesh.CombineMeshes(combine);

        var combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
        combinedMeshFilter.sharedMesh = combinedMesh;

        var combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
        combinedMeshRenderer.sharedMaterial = objectsOnLayer[0].GetComponent<MeshRenderer>().sharedMaterial;

        var combinedMeshCollider = combinedMeshObject.AddComponent<MeshCollider>();
        combinedMeshCollider.sharedMesh = combinedMesh;

        combinedMeshObject.tag = "Ground";
        combinedMeshObject.layer = LayerMask.NameToLayer("Ground");
        combinedMeshCollider.convex = true;
    }
}