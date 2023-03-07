using UnityEngine;
using UnityEditor;

namespace WormholeSignal.Editor
{
    /// <summary>
    /// The editor for the <see cref="SignalList"/> <see cref="ScriptableObject"/>.
    /// </summary>
    [CustomEditor(typeof(SignalList))]
    public class SignalListEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("The Signal List is a scriptable object which hold a list of every single Signal in the entire project and is what allows you to get Signals via GUID or Name in code.\n\nPress the \"Clean List\" button to remove any null entries in the Signal list.", MessageType.Info);

            EditorGUILayout.Space();

            DoSignalList("Signals", SignalList.SerializedObject);

            EditorGUILayout.Space();

            // Clean the List if this button is pressed
            if (GUILayout.Button("Clean List"))
            {
                SignalList.CleanList(SignalList.SerializedObject);
            }
        }

        public static void DoSignalList(string title, SerializedObject serializedObject)
        {
            // Display signals list in a disabled scope
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("signals"), new GUIContent(title));
            }
        }
    }
}
