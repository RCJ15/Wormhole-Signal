using UnityEngine;
using UnityEditor;

namespace WormholeSignal.Editor
{
    /// <summary>
    /// The editor for the <see cref="Signal"/> <see cref="ScriptableObject"/>.
    /// </summary>
    [CustomEditor(typeof(Signal))]
    public class SignalEditor : UnityEditor.Editor
    {
        /// <summary>
        /// A private static get property that simply returns the result of <see cref="SignalList.Obj"/>.
        /// </summary>
        private static SignalList List => SignalList.Obj;
        private Signal _signal;

        private void OnEnable()
        {
            // Save the target object as type Signal
            _signal = target as Signal;
        }

        public override void OnInspectorGUI()
        {
            AddToList();

            GUID();

            EditorGUILayout.Space(2);

            SignalListEditor.DoSignalList("Global Signal List", SignalList.Obj.SerializedObject);

            EditorGUILayout.Space();

            // Ping the List object if this button is pressed
            if (GUILayout.Button("Find Global Signal List Object"))
            {
                EditorGUIUtility.PingObject(List);
            }
        }

        /// <summary>
        /// A method which is responsible for displaying and setting the GUID value of this <see cref="Signal"/>.
        /// </summary>
        private void GUID()
        {
            // Get the GUID property
            SerializedProperty guidProp = serializedObject.FindProperty("_guid");

            // Set the value of the GUID property if it's empty or null
            if (string.IsNullOrEmpty(guidProp.stringValue))
            {
                // We make sure to set the value to this objects unique GUID by getting the path and then using the path to get the GUID
                guidProp.stringValue = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(target));

                // Apply all changes without undo
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            // Display the GUID in a disabled scope using this Disposable type
            using (new EditorGUI.DisabledScope(true))
            {
                // Force all of the next labels to support rich text
                // Also ensure to store the starting value of this bool so that we can later reset it
                bool startLabelRichText = EditorStyles.label.richText;
                EditorStyles.label.richText = true;

                // Display the GUID label with <b>bold</b> text
                EditorGUILayout.LabelField($"<b>GUID:</b> {guidProp.stringValue}");

                // Reset the richText property back to what we started with
                EditorStyles.label.richText = startLabelRichText;
            }
        }

        /// <summary>
        /// Will check if this <see cref="Signal"/> is inside of the <see cref="SignalList"/>. <para/>
        /// If not, then it will add itself to the list, otherwise nothing will happen.
        /// </summary>
        private void AddToList()
        {
            // Signal is already in the list? No problems here!
            if (List.ContainsSignal(_signal))
            {
                return;
            }

            // Add this signal to the list if not
            List.AddSignal(_signal);
        }
    }
}
