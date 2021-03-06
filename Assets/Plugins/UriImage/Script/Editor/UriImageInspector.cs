using SaltyfishKK.UriImage;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(UriImage), true)]
public class UriImageInspector : ImageEditor
{ 
    private SerializedProperty m_UriType;
    private SerializedProperty m_Uri;
    private SerializedProperty m_LoadOnStart;
    private SerializedProperty m_Param;


    protected override void OnEnable()
    {
        m_UriType = serializedObject.FindProperty("m_UriType");
        m_Uri = serializedObject.FindProperty("m_Uri");
        m_LoadOnStart = serializedObject.FindProperty("m_LoadOnStart");
        m_Param = serializedObject.FindProperty("m_Param");
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(m_UriType);
        EditorGUILayout.PropertyField(m_Uri);
        EditorGUILayout.PropertyField(m_LoadOnStart);
        EditorGUILayout.PropertyField(m_Param);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}