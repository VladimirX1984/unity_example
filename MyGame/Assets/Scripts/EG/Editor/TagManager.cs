using UnityEditor;

namespace EG.Editor {
public class TagManager : EG.Misc.Singleton<TagManager> {

  public bool ExistTag(string tagName) {
    SerializedProperty tagsProp = _tagManager.FindProperty("tags");
    for (int i = 0; i < tagsProp.arraySize; i++) {
      SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
      if (t.stringValue.Equals(tagName)) {
        return true;
      }
    }
    return false;
  }

  public void UpdateTag(string tagName) {
    SerializedProperty tagsProp = _tagManager.FindProperty("tags");
    bool found = false;
    for (int i = 0; i < tagsProp.arraySize; i++) {
      SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
      if (t.stringValue.Equals(tagName)) {
        found = true;
        break;
      }
    }
    if (!found) {
      tagsProp.InsertArrayElementAtIndex(0);
      SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
      n.stringValue = tagName;
    }
    _tagManager.ApplyModifiedProperties();
  }

  public void UpdateLayer(string layerName, int index) {
    SerializedProperty layersProp = _tagManager.FindProperty("layers");
    SerializedProperty sp = layersProp.GetArrayElementAtIndex(index);
    if (sp != null) {
      sp.stringValue = layerName;
    }
    _tagManager.ApplyModifiedProperties();
  }

  public TagManager() {
    // Open tag manager
    _tagManager = new SerializedObject(
      AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
  }

  private SerializedObject _tagManager;
}
}
