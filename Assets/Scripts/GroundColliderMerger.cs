using UnityEngine;

public class GroundColliderMerger : MonoBehaviour
{

  public string layerName = "Ground";
  public bool destroyOriginalObjects = true;

  private void Start()
  {
      // ÐÐ°Ñ//Ð¾Ð´Ð¸Ð¼ Ð²ÑÐµ Ð¾Ð±ÑÐµÐºÑÑ Ð½Ð° ÑÐ»Ð¾ÐµÐ½ Ñ ÑÐºÐ°Ð·Ð°Ð½Ð½ÑÐ¼ Ð¸Ð¼ÐµÐ½ÐµÐ¼
      GameObject[] objectsOnLayer = GameObject.FindGameObjectsWithTag(layerName);

      // Ð¡Ð¾Ð·Ð´Ð°ÐµÐ¼ Ð½Ð¾Ð²ÑÐ¹ Ð¿ÑÑÑÐ¾Ð¹ Ð¾Ð±ÑÐµÐºÑ Ð´Ð»Ñ Ð¾Ð±ÑÐµÐ´Ð¸Ð½ÐµÐ½Ð½Ð¾Ð³Ð¾ Ð¼ÐµÑÐ°
      GameObject combinedMeshObject = new GameObject("GroundLayerMesh");
      combinedMeshObject.transform.position = Vector3.zero;
      combinedMeshObject.transform.rotation = Quaternion.identity;

      // Ð¡Ð¾Ð·Ð´Ð°ÐµÐ¼ Ð¼Ð°ÑÑÐ¸Ð²Ð° Ð¼ÐµÑÐµÐ¹, ÐºÐ¾ÑÐ¾ÑÑÐµ Ð¼Ñ Ð±ÑÐ´ÐµÐ¼ Ð¾Ð±ÑÐµÐ´Ð¸Ð½ÑÑÑ
      MeshFilter[] meshFilters = new MeshFilter[objectsOnLayer.Length];
      for (int i = 0; i < objectsOnLayer.Length; i++)
      {
          meshFilters[i] = objectsOnLayer[i].GetComponent<MeshFilter>();
      }

      // Ð¡Ð¾Ð·Ð´Ð°ÐµÐ¼ Ð½Ð¾Ð²ÑÐ¹ Ð¼ÐµÑ Ð¸ Ð¾Ð±ÑÐµÐ´Ð¸Ð½ÑÐµÐ¼ Ð²ÑÐµ Ð¼ÐµÑÐ¸ Ð² Ð¾Ð´Ð¸Ð½
      Mesh combinedMesh = new Mesh();
      CombineInstance[] combine = new CombineInstance[meshFilters.Length];
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

      MeshFilter combinedMeshFilter = combinedMeshObject.AddComponent<MeshFilter>();
      combinedMeshFilter.sharedMesh = combinedMesh;

      MeshRenderer combinedMeshRenderer = combinedMeshObject.AddComponent<MeshRenderer>();
      combinedMeshRenderer.sharedMaterial = objectsOnLayer[0].GetComponent<MeshRenderer>().sharedMaterial;

      MeshCollider combinedMeshCollider = combinedMeshObject.AddComponent<MeshCollider>();
      combinedMeshCollider.sharedMesh = combinedMesh;

      combinedMeshObject.tag = "Ground";
      combinedMeshObject.layer = LayerMask.NameToLayer("Ground");
      combinedMeshCollider.convex = true;
  }
}
